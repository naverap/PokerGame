using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Firebase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using static Java.Util.Jar.Attributes;
using Java.Util;
using PokerLib;
using Android.Gms.Tasks;
using System.Text.Json;
using System.Threading;
using Android.Views.Animations;

namespace PokerGame
{
    [Activity(Label = "GameActivity",
     TurnScreenOn = false,
     ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class GameActivity : Activity, IOnSuccessListener
    {
        Button raise25, raise50, raisePot, AllIn, submitRaise;
        Dialog d, RaiseDialog;
        FrameLayout container;
        RelativeLayout rl2;
        EditText et;
        public static bool Player1HasMoney = true, Player2HasMoney = true, FirstRoundWasPlayed = false, SecondRoundWasPlayed = false, ThirdRoundWasPlayed = false, fourthfRoundWasPlayed = false, didPlayerOneWin = false, didPlayerTwoWin = false;
        public static Game MyGame;
        public static string GameId;
        Player CurrentPlayer => MyGame.CurrentPlayer;
        Player p1 => MyGame.Players[0];
        Player p2 => MyGame.Players[1];
        const string playerName = "nave";
        System.Threading.Timer getGameUpdatesTimer;
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        Animation CardFlip;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Game);

            rl2 = (RelativeLayout)FindViewById(Resource.Id.relativeLayout2);
            //textview1 = (TextView)FindViewById(Resource.Id.tv1);
            //textview2 = (TextView)FindViewById(Resource.Id.tv2);
            //textview3 = (TextView)FindViewById(Resource.Id.tv3);
            //textview4 = (TextView)FindViewById(Resource.Id.tv4);
            //textview5 = (TextView)FindViewById(Resource.Id.tv5);
            //textview4.Visibility = ViewStates.Invisible;
            //textview5.Visibility = ViewStates.Invisible; ;

            var btnmenu = (Button)FindViewById(Resource.Id.menubtn2);
            btnmenu.Click += Btnmenu_Click;
            var btnfold = (Button)FindViewById(Resource.Id.foldbtn);
            btnfold.Click += Btnfold_Click;
            var btncheck = (Button)FindViewById(Resource.Id.checkbtn);
            btncheck.Click += Btncheck_Click;
            var btnraise = (Button)FindViewById(Resource.Id.raisebtn);
            btnraise.Click += Btnraise_Click;

            container = (FrameLayout)FindViewById(Resource.Id.GameContainer);
            container.AddView(new PokerTableView(this));

            Repository.Init(this);
            Repository.GamesCollection.Get().AddOnSuccessListener(this);
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            if (result is QuerySnapshot query)
                OnGetGames(query);
            else if (result is DocumentSnapshot document)
                OnGetGame(document);
        }

        void OnGetGames(QuerySnapshot query)
        {
            var games = query.Documents.Select(doc => Repository.GetGame(doc));

            MyGame = games.FirstOrDefault(g => g.HasPlayer(playerName));

            if (MyGame == null)
            {
                // join an available game or create a new game
                MyGame = games.FirstOrDefault(g => g.CanJoin) ?? new Game();
                Player me = new Player(playerName, 1000, false);
                MyGame.AddPlayer(me);
                Repository.UploadGame(MyGame);
            }

            Repository.gameId ??= MyGame.Id.ToString();
            getGameUpdatesTimer ??= new System.Threading.Timer(OnGetGameUpdatesTimer, autoResetEvent, 2000, 10000);

        }

        void OnGetGame(DocumentSnapshot document)
        {
            MyGame = Repository.GetGame(document);
            DrawCards(MyGame.CurrentRound, MyGame.CurrentPlayer);
        }

        public void OnGetGameUpdatesTimer(Object stateInfo)
        {
            Repository.GameDocument.Get().AddOnSuccessListener(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            //GameLoop();
        }

        void AllIn_Click(object sender, EventArgs e)
        {
            MyGame.AllIn();
            Repository.UploadGame(MyGame);
        }

        void RaisePot_Click(object sender, EventArgs e)
        {
            MyGame.BetPecentage(100);
        }

        void Raise50_Click(object sender, EventArgs e)
        {
            MyGame.BetPecentage(50);
        }

        void Raise25_Click(object sender, EventArgs e)
        {
            MyGame.BetPecentage(25);
        }

        void Btnraise_Click(object sender, EventArgs e)
        {
            RaiseDialog = new Dialog(this);
            RaiseDialog.SetContentView(Resource.Layout.Raise);
            RaiseDialog.SetTitle("Raise");

            raise25 = (Button)RaiseDialog.FindViewById(Resource.Id.quarterpotbtn);
            raise50 = (Button)RaiseDialog.FindViewById(Resource.Id.halfpotbtn);
            raisePot = (Button)RaiseDialog.FindViewById(Resource.Id.potbtn);
            AllIn = (Button)RaiseDialog.FindViewById(Resource.Id.allinbtn);
            submitRaise = (Button)RaiseDialog.FindViewById(Resource.Id.submitRaisebtn);
            et = (EditText)RaiseDialog.FindViewById(Resource.Id.etraise);

            raise25.Click += Raise25_Click;
            raise50.Click += Raise50_Click;
            raisePot.Click += RaisePot_Click;
            AllIn.Click += AllIn_Click;
            submitRaise.Click += SubmitRaise_Click;

            RaiseDialog.SetCancelable(true);
            RaiseDialog.Show();
        }

        void SubmitRaise_Click(object sender, EventArgs e)
        {
            var sum = Int32.Parse(et.Text);
            MyGame.BetOld(sum);
        }

        void Btncheck_Click(object sender, EventArgs e)
        {
            MyGame.Check();
        }

        void Btnfold_Click(object sender, EventArgs e)
        {
            didPlayerTwoWin = true;
        }

        public void GameLoop()
        {
            if (MyGame == null) return;
            //DrawCards(MyGame.CurrentRound, MyGame.CurrentPlayer);

            //textview1.Text = MyGame.Players[0].Pot.ToString();
            //textview2.Text = MyGame.Players[1].Pot.ToString();
            //textview3.Text = MyGame.Pot.ToString();
            ////if (MyGame.CurrentRound > 4)
            ////{
            ////    Player[] winners = MyGame.CheckWinners();
            ////    didPlayerOneWin = winners.Contains(p1);
            ////    didPlayerTwoWin = winners.Contains(p2);
            ////}
            //if (didPlayerOneWin && didPlayerTwoWin)
            //{
            //    textview5.Visibility = ViewStates.Visible;
            //    textview4.Visibility = ViewStates.Visible;
            //    SetUpNewGame();
            //}

            //else if (didPlayerTwoWin)
            //{
            //    textview4.Visibility = ViewStates.Visible;
            //    SetUpNewGame();
            //}

            //else if (didPlayerOneWin)
            //{
            //    textview5.Visibility = ViewStates.Visible;
            //    SetUpNewGame();
            //}

            //MyGame.SetNextPlayerTurn();
        }

        void Btnmenu_Click(object sender, EventArgs e)
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.MenuPopUp);
            d.SetTitle("MENU");
            d.SetCancelable(true);

            var btngame = (Button)d.FindViewById(Resource.Id.gamebtn);
            btngame.Click += Btngame_Click;
            var btnhome = (Button)d.FindViewById(Resource.Id.homebtn);
            btnhome.Click += Btnhome_Click;
            var btnsettings = (Button)d.FindViewById(Resource.Id.settingbtn);

            d.Show();
        }

        void Btnhome_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(MainActivity));
            StartActivity(Intent);
        }

        void Btngame_Click(object sender, EventArgs e)
        {
            Intent l = new Intent(this, typeof(GameActivity));
            StartActivity(Intent);
        }

        void DrawCard(string cardName, int left, int top)
        {
            var layoutParams = new RelativeLayout.LayoutParams(200, 200);
            layoutParams.SetMargins(left, top, 0, 0);
            var imageView = new ImageView(this) { LayoutParameters = layoutParams };
            imageView.SetImageResource(Resources.GetIdentifier(cardName, "drawable", PackageName));
            rl2.AddView(imageView);
        }

        void DrawCards(int round, Player player)
        {
            var playerCards = MyGame.Players[player.Id].Cards;
            DrawCard(playerCards[0].Name, 350, 350);
            DrawCard(playerCards[1].Name, 370, 350);

            var upsidedown = "upsidedowncard";
            var communityCards = MyGame.CommunityCards;
            var cardName0 = round >= 2 ? communityCards[0].Name : upsidedown;
            var cardName1 = round >= 2 ? communityCards[1].Name : upsidedown;
            var cardName2 = round >= 2 ? communityCards[2].Name : upsidedown;
            var cardName3 = round >= 3 ? communityCards[3].Name : upsidedown;
            var cardName4 = round >= 4 ? communityCards[4].Name : upsidedown;
            DrawCard(cardName0, 550, 400);
            DrawCard(cardName1, 720, 400);
            DrawCard(cardName2, 890, 400);
            DrawCard(cardName3, 1060, 400);
            DrawCard(cardName4, 1230, 400);
        }
    }
}