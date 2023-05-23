﻿using Android.App;
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
using static Google.Firestore.V1.StructuredAggregationQuery.Aggregation;
using Javax.Crypto;

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
            DrawCards(MyGame.Round, Me);
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
            // set game player to next player fro debugging
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
            var layoutParams = new RelativeLayout.LayoutParams(500 / 2, 726 / 2);
            layoutParams.SetMargins(left, top, 0, 0);
            var imageView = new ImageView(this) { LayoutParameters = layoutParams };
            imageView.SetImageResource(Resources.GetIdentifier(cardName, "drawable", PackageName));
            rl2.AddView(imageView);
        }

        void DrawCards(Round round, Player player)
        {
            var upsidedown = "upsidedowncard";

            var playerCards = MyGame.Players[player.Id].Cards;
            var card0 = upsidedown;
            var card1 = upsidedown;
            if (MyGame.Round > Round.NotStarted) 
            {
                card0 = playerCards[0].Name;
                card1 = playerCards[1].Name;
            }
            DrawCard(card0, 350, 350);
            DrawCard(card1, 380, 350);

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

            var communityCards = MyGame.CommunityCards;

            var showFlop = round >= Round.Flop;
            var showTurn = round >= Round.Turn;
            var showRiver = round >= Round.River;

            var cardName0 = showFlop ? communityCards[0].Name : upsidedown;
            var cardName1 = showFlop ? communityCards[1].Name : upsidedown;
            var cardName2 = showFlop ? communityCards[2].Name : upsidedown;
            var cardName3 = showTurn ? communityCards[3].Name : upsidedown;
            var cardName4 = showRiver ? communityCards[4].Name : upsidedown;

            DrawCard(cardName0, 550, 350);
            DrawCard(cardName1, 720, 350);
            DrawCard(cardName2, 890, 350);
            DrawCard(cardName3, 1060, 350);
            DrawCard(cardName4, 1230, 350);
        }
    }
}