using Android.App;
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

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Game);

            playerName = Intent.GetStringExtra("PlayerName");

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
            getGameUpdatesTimer ??= new System.Threading.Timer(OnGetGameUpdatesTimer, autoResetEvent, 200, 1000);

        }

        void OnGetGame(DocumentSnapshot document)
        {
            MyGame = Repository.GetGame(document);
            DrawPlayerCards(MyGame.Round, Me);
            DrawGameCards(MyGame.Round);
            DrawPlayerStats(Me);
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
                Repository.UploadGame(MyGame);
                return;
            }
            // set game player to next player for debugging
            var id = Me.Id;
            id++;
            if (id >= MyGame.Players.Count)
                id = 0;
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
            var layoutParams = new RelativeLayout.LayoutParams(180 , 261);
            layoutParams.SetMargins(left, top, 0, 0);
            var imageView = new ImageView(this) { LayoutParameters = layoutParams };
            imageView.SetImageResource(Resources.GetIdentifier(cardName, "drawable", PackageName));
            rl2.AddView(imageView);
        }

        void DrawPlayerCards(Round round, Player player)
        {
            var w = rl2.Width;
            var h = rl2.Height;

            var centerX = w / 2;
            var centerY = h / 2;

            var margin = 20;
            var cardWidth = 180;
            var cardHeight = 261;

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

            var upsidedown = "upsidedowncard";

            var playerCards = MyGame.Players[player.Id].Cards;
            var card0 = upsidedown;
            var card1 = upsidedown;
            var card2 = upsidedown;
            var card3 = upsidedown;
            var card4 = upsidedown;
            var card5 = upsidedown;
            var card6 = upsidedown;
            var card7 = upsidedown;
            if (MyGame.Round > Round.NotStarted)
            {
                card0 = playerCards[0].Name;
                card1 = playerCards[1].Name;
            }
            DrawCard(card0, sx, sy);
            DrawCard(card1, sx + 2*margin, sy);



            //Draw other Players' Cards
            if (MyGame.Players.Count > 1)
            {
                DrawCard(card2, nx, ny);
                DrawCard(card3, nx+2*margin, ny);
            }
            if (MyGame.Players.Count > 2)
            {
                DrawCard(card4, ex, ey);
                DrawCard(card5, ex + margin*2, ey);
            }
            if (MyGame.Players.Count > 3)
            {
                DrawCard(card6, wx, wy);
                DrawCard(card7, wx + margin*2, wy);
            }
            

        }
        void DrawGameCards(Round round)
        {
            var w = rl2.Width;
            var h = rl2.Height;

            var centerX = w / 2;
            var centerY = h / 2;

            var margin = 20;
            var cardWidth = 180;
            var cardHeight = 261;

            var cardsWidth = cardWidth * 5 + margin * 4;

            var startX = centerX - cardsWidth / 2;
            var startY = centerY - cardHeight / 2;

            var upsidedown = "upsidedowncard";
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

        void DrawPlayerStats(Player player)
        {
            var textViews = new int[]
            {
                Resource.Id.tvPlayer0,
                Resource.Id.tvPlayer1,
                Resource.Id.tvPlayer2,
                Resource.Id.tvPlayer3
            };

            var playerIdToDraw = player.Id;
            for (int i = 0; i < 4; i++)
            {
                var tvResourceId = textViews[i];
                var textView = (TextView)FindViewById(tvResourceId);
                textView.Text = "";
                if (playerIdToDraw < MyGame.Players.Count)
                {
                    var playerToDraw = MyGame.Players[playerIdToDraw];
                    textView.Text = playerToDraw.GetStatus();
                    if (MyGame.CurrentPlayerIndex == playerIdToDraw)
                    {
                        textView.Text += "\nIs Current";
                    }
                }
                playerIdToDraw++;
                if (playerIdToDraw >= 4)
                    playerIdToDraw = 0;
            }


        }
    }
            
    }
