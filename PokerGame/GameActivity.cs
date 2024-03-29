﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Firebase.Firestore;
using System;
using System.Linq;
using PokerLib;
using Android.Gms.Tasks;
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

        string playerName;
        Player Me => MyGame.GetPlayerByName(playerName);

        System.Threading.Timer getGameUpdatesTimer;
        AutoResetEvent autoResetEvent = new AutoResetEvent(false);
        Animation CardFlip;

        const string upsidedown = "upsidedowncard";
        const int margin = 20;
        const int cardWidth = 180;
        const int cardHeight = 261;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Game);

            playerName = Intent.GetStringExtra("PlayerName");

            rl2 = (RelativeLayout)FindViewById(Resource.Id.relativeLayout2);
            var btnmenu = (Button)FindViewById(Resource.Id.menubtn2);
            btnmenu.Click += Btnmenu_Click;
            var btnfold = (Button)FindViewById(Resource.Id.foldbtn);
            btnfold.Click += Btnfold_Click;
            var btncheck = (Button)FindViewById(Resource.Id.checkbtn);
            btncheck.Click += Btncheck_Click;
            var btnraise = (Button)FindViewById(Resource.Id.raisebtn);
            btnraise.Click += Btnraise_Click;
            var btnPlay = (Button)FindViewById(Resource.Id.playbtn);
            btnPlay.Click += BtnPlay_Click;

            container = (FrameLayout)FindViewById(Resource.Id.GameContainer);
            container.AddView(new PokerTableView(this));
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

            Repository.gameId = MyGame.Id.ToString();
            getGameUpdatesTimer ??= new Timer(OnGetGameUpdatesTimer, autoResetEvent, 200, 1000);

            var intent = new Intent(this, typeof(NotifyService));
            intent.PutExtra("PlayerName", playerName);
            StartService(intent);
        }

        void OnGetGame(DocumentSnapshot document)
        {
            MyGame = Repository.GetGame(document);
            DrawGameCards(MyGame.Round);
            DrawPlayers(MyGame.Round, Me);
        }

        public void OnGetGameUpdatesTimer(Object stateInfo)
        {
            Repository.GameDocument.Get().AddOnSuccessListener(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            Repository.Init(this);
            Repository.GamesCollection.Get().AddOnSuccessListener(this);
        }

        protected override void OnStop()
        {
            base.OnStop();
            getGameUpdatesTimer.Dispose();
            getGameUpdatesTimer = null;
        }

        void BtnPlay_Click(object sender, EventArgs e)
        {
            if (MyGame.Round == Round.NotStarted)
            {
                MyGame.Round++;
                MyGame.PlayBlinds();
                Repository.UploadGame(MyGame);
                return;
            }
            if (MyGame.Round == Round.Ended)
            {
                MyGame.StartNew();
                Repository.UploadGame(MyGame);
                return;
            }
            // set game player to next player for debugging
            var id = MyGame.GetNextPlayerIndex(Me.Id);
            playerName = MyGame.Players[id].Name;
        }

        void AllIn_Click(object sender, EventArgs e)
        {
            var bet = Me.Pot;
            et.Text = bet.ToString();
        }

        void RaisePot_Click(object sender, EventArgs e)
        {
            var bet = MyGame.Pot;
            et.Text = bet.ToString();
        }

        void Raise50_Click(object sender, EventArgs e)
        {
            var bet = MyGame.Pot / 2;
            et.Text = bet.ToString();
        }

        void Raise25_Click(object sender, EventArgs e)
        {
            var bet = MyGame.Pot / 4;
            et.Text = bet.ToString();
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
            var betType = MyGame.LastBet > 0 ? BetType.Raise : BetType.Bet;
            var sum = int.Parse(et.Text);
            var isValid = MyGame.Bet(Me, betType, sum);
            if (isValid)
            {
                Repository.UploadGame(MyGame);
                RaiseDialog.Hide();
            }
            else
            {
                Toast.MakeText(this, "Bet is not valid", ToastLength.Long).Show();
            }
        }

        void Btncheck_Click(object sender, EventArgs e)
        {
            var betType = MyGame.LastBet > 0 ? BetType.Call : BetType.Check;
            var isValid = MyGame.Bet(Me, betType);
            if (isValid)
            {
                Repository.UploadGame(MyGame);
            }
            else
            {
                Toast.MakeText(this, "Bet is not valid", ToastLength.Long).Show();
            }
        }

        void Btnfold_Click(object sender, EventArgs e)
        {
            var isValid = MyGame.Bet(Me, BetType.Fold);
            if (isValid)
            {
                Repository.UploadGame(MyGame);
            }
            else
            {
                Toast.MakeText(this, "Bet is not valid", ToastLength.Long).Show();
            }
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
            var btnsignup = (Button)d.FindViewById(Resource.Id.signupbtn);
            btnsignup.Click += Btnsignup_Click;
            d.Show();
        }

        private void Btnsignup_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(SignUpActivity));
            StartActivity(Intent);
        }

        void Btnhome_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(MainActivity));
            StartActivity(Intent);
        }

        void Btngame_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(GameActivity));
            StartActivity(Intent);
        }

        void DrawCard(string cardName, int left, int top)
        {
            // card image size is 500x726 pixels
            var layoutParams = new RelativeLayout.LayoutParams(cardWidth, cardHeight);
            layoutParams.SetMargins(left, top, 0, 0);
            var imageView = new ImageView(this) { LayoutParameters = layoutParams };
            imageView.SetImageResource(Resources.GetIdentifier(cardName, "drawable", PackageName));
            rl2.AddView(imageView);
        }

        void DrawPlayers(Round round, Player currentPlayer)
        {
            var w = rl2.Width;
            var h = rl2.Height;

            var centerX = w / 2;
            var centerY = h / 2;

            var cardsWidth = cardWidth * 5 + margin * 4;

            var startX = centerX - cardsWidth / 2;
            var startY = centerY - cardHeight / 2;

            var ex = startX + cardsWidth + 4 * margin;
            var wx = startX - cardWidth - 6 * margin;
            var nx = centerX - (cardWidth + 2 * margin) / 2;
            var sx = centerX - (cardWidth + 2 * margin) / 2;

            var ey = startY;
            var wy = startY;
            var ny = startY - 2 * margin - cardHeight;
            var sy = startY + 2 * margin + cardHeight;

            var textViews = new[]
            {
                Resource.Id.tvPlayer0,
                Resource.Id.tvPlayer1,
                Resource.Id.tvPlayer2,
                Resource.Id.tvPlayer3
            };
            var cardsPositions = new[]
            {
                (sx, sy),
                (wx, wy),
                (nx, ny),
                (ex, ey)
            };

            var playerIdToDraw = currentPlayer.Id;
            for (int i = 0; i < 4; i++)
            {
                var textView = (TextView)FindViewById(textViews[i]);
                textView.Text = "";
                if (playerIdToDraw < MyGame.Players.Count)
                {
                    var player = MyGame.Players[playerIdToDraw];
                    // draw stats
                    var isCurrent = MyGame.CurrentPlayerIndex == playerIdToDraw;
                    var gameEnded = MyGame.Round == Round.Ended;
                    textView.Text = player.GetStatus(isCurrent, gameEnded);
                    // draw cards
                    var cardsPosition = cardsPositions[i];
                    var showCards = player.Id == currentPlayer.Id
                        ? MyGame.Round > Round.NotStarted
                        : MyGame.Round == Round.Ended && !player.HasFolded;
                    DrawPlayerCards(player, showCards, cardsPosition.Item1, cardsPosition.Item2);
                }
                playerIdToDraw++;
                if (playerIdToDraw >= 4)
                    playerIdToDraw = 0;
            }
        }

        void DrawPlayerCards(Player player, bool showCards, int left, int top)
        {
            var card0 = showCards ? player.Cards[0].Name : upsidedown;
            var card1 = showCards ? player.Cards[1].Name : upsidedown;
            DrawCard(card0, left, top);
            DrawCard(card1, left + margin * 2, top);
        }

        void DrawGameCards(Round round)
        {
            var w = rl2.Width;
            var h = rl2.Height;

            var centerX = w / 2;
            var centerY = h / 2;

            var cardsWidth = cardWidth * 5 + margin * 4;

            var startX = centerX - cardsWidth / 2;
            var startY = centerY - cardHeight / 2;

            var communityCards = MyGame.CommunityCards;

            var showFlop = round >= Round.Flop;
            var showTurn = round >= Round.Turn;
            var showRiver = round >= Round.River;

            var cardName0 = showFlop ? communityCards[0].Name : upsidedown;
            var cardName1 = showFlop ? communityCards[1].Name : upsidedown;
            var cardName2 = showFlop ? communityCards[2].Name : upsidedown;
            var cardName3 = showTurn ? communityCards[3].Name : upsidedown;
            var cardName4 = showRiver ? communityCards[4].Name : upsidedown;

            DrawCard(cardName0, startX, startY);
            startX += cardWidth + margin;
            DrawCard(cardName1, startX, startY);
            startX += cardWidth + margin;
            DrawCard(cardName2, startX, startY);
            startX += cardWidth + margin;
            DrawCard(cardName3, startX, startY);
            startX += cardWidth + margin;
            DrawCard(cardName4, startX, startY);
        }
    }
}
