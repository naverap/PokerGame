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

namespace PokerGame
{
    [Activity(Label = "GameActivity",
     TurnScreenOn = false,
     ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class GameActivity : Activity, IOnSuccessListener
    {
        Button btn1, btnhome, btngame, btnsettings, btnfold, btncheck, btnraise, raise25, raise50, raisePot, AllIn, submitRaise;
        Dialog d, RaiseDialog;
        FrameLayout container;
        ImageView img1, img2, img3, img4, img5, img6, img7;
        TextView textview1, textview2, textview3, textview4, textview5;
        RelativeLayout rl2;
        EditText et;
        public static bool Player1HasMoney = true, Player2HasMoney = true, FirstRoundWasPlayed = false, SecondRoundWasPlayed = false, ThirdRoundWasPlayed = false, fourthfRoundWasPlayed = false, didPlayerOneWin = false, didPlayerTwoWin = false;
        public static Game MyGame;
        Player CurrentPlayer => MyGame.CurrentPlayer;
        Player p1 => MyGame.Players[0];
        Player p2 => MyGame.Players[1];

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.Game);

            rl2 = (RelativeLayout)FindViewById(Resource.Id.relativeLayout2);
            textview1 = (TextView)FindViewById(Resource.Id.tv1);
            btnfold = (Button)FindViewById(Resource.Id.foldbtn);
            btncheck = (Button)FindViewById(Resource.Id.checkbtn);
            btnraise = (Button)FindViewById(Resource.Id.raisebtn);
            textview2 = (TextView)FindViewById(Resource.Id.tv2);
            textview3 = (TextView)FindViewById(Resource.Id.tv3);
            btn1 = (Button)FindViewById(Resource.Id.menubtn2);
            textview4 = (TextView)FindViewById(Resource.Id.tv4);
            textview5 = (TextView)FindViewById(Resource.Id.tv5);
            textview4.Visibility = ViewStates.Invisible;
            textview5.Visibility = ViewStates.Invisible; ;
            btn1.Click += Btn1_Click;
            btnfold.Click += Btnfold_Click;
            btncheck.Click += Btncheck_Click;
            btnraise.Click += Btnraise_Click;
            container = (FrameLayout)FindViewById(Resource.Id.GameContainer);

            container.AddView(new PokerTableView(this));

            Repository.Init(this);

            Repository.GamesCollection.Get().AddOnSuccessListener(this);

            //MyGame = Game.CreateGame(1, 1000);
            //Repository.UploadGame(MyGame);

            //if (Intent.GetIntExtra("PlayerId", 1) == 1)
            //{
            //    MyGame = CreateGame();
            //}
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            foreach (var document in snapshot.Documents)
            {
                var data = document.Get("data").ToString();
                var game = JsonSerializer.Deserialize<Game>(data);
                if (game.CanJoin)
                {
                    MyGame = game;
                    break;
                }
            }
            if (MyGame == null)
            {
                MyGame = Game.CreateGame();
                Repository.UploadGame(MyGame);
            }

            Player me = new Player(1000, false);
            MyGame.AddPlayer(me);

            DrawCards(MyGame.CurrentRound, MyGame.CurrentPlayer);
        }

        protected override void OnStart()
        {
            base.OnStart();
            //GameLoop();
        }

        void AllIn_Click(object sender, EventArgs e)
        {
            MyGame.AllIn();
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
            if (MyGame == null ) return;
            DrawCards(MyGame.CurrentRound, MyGame.CurrentPlayer);

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

        void Btn1_Click(object sender, EventArgs e)
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.MenuPopUp);
            d.SetTitle("MENU");
            d.SetCancelable(true);

            btngame = (Button)d.FindViewById(Resource.Id.gamebtn);
            btnhome = (Button)d.FindViewById(Resource.Id.homebtn);
            btnsettings = (Button)d.FindViewById(Resource.Id.settingbtn);

            btngame.Click += Btngame_Click;
            btnhome.Click += Btnhome_Click;
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

        void DrawCards(int round, Player player)
        {
            int imgkey1, imgkey2, imgkey3, imgkey4, imgkey5, imgkey6, imgkey7;
            if (round == 1)
            {
                img1 = new ImageView(this);
                img2 = new ImageView(this);
                RelativeLayout.LayoutParams ivparams = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams2 = new RelativeLayout.LayoutParams(200, 200);
                ivparams.SetMargins(350, 350, 0, 0);
                ivparams2.SetMargins(370, 350, 0, 0);
                img1.LayoutParameters = ivparams;
                img2.LayoutParameters = ivparams2;
                rl2.AddView(img1);
                rl2.AddView(img2);
                var card1 = MyGame.Players[player.Id].Cards[0].Name;
                var card2 = MyGame.Players[player.Id].Cards[1].Name;
                imgkey1 = Resources.GetIdentifier(card1, "drawable", PackageName);
                imgkey2 = Resources.GetIdentifier(card2, "drawable", PackageName);
                img1.SetImageResource(imgkey1);
                img2.SetImageResource(imgkey2);
            }
            else if (round == 2)
            {
                img1 = new ImageView(this);
                img2 = new ImageView(this);
                img3 = new ImageView(this);
                img4 = new ImageView(this);
                img5 = new ImageView(this);
                RelativeLayout.LayoutParams ivparams = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams2 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams3 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams4 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams5 = new RelativeLayout.LayoutParams(200, 200);
                ivparams.SetMargins(350, 350, 0, 0);
                ivparams2.SetMargins(370, 350, 0, 0);
                ivparams3.SetMargins(550, 400, 0, 0);
                ivparams4.SetMargins(720, 400, 0, 0);
                ivparams5.SetMargins(890, 400, 0, 0);
                img1.LayoutParameters = ivparams;
                img2.LayoutParameters = ivparams2;
                img3.LayoutParameters = ivparams3;
                img4.LayoutParameters = ivparams4;
                img5.LayoutParameters = ivparams5;
                this.rl2.AddView(img1);
                this.rl2.AddView(img2);
                this.rl2.AddView(img3);
                this.rl2.AddView(img4);
                this.rl2.AddView(img5);
                string str = "";
                string str2 = "";
                if (player.Id == p1.Id)
                {
                    str = MyGame.Players[0].Cards[0].Name;
                    str = MyGame.Players[0].Cards[1].Name;
                }
                else
                {
                    str = MyGame.Players[1].Cards[0].Name;
                    str2 = MyGame.Players[1].Cards[1].Name;
                }
                string str3 = MyGame.TableCards[0].Name;
                string str4 = MyGame.TableCards[1].Name;
                string str5 = MyGame.TableCards[2].Name;
                imgkey1 = Resources.GetIdentifier("" + str, "drawable", this.PackageName);
                imgkey2 = Resources.GetIdentifier("" + str2, "drawable", this.PackageName);
                imgkey3 = Resources.GetIdentifier("" + str3, "drawable", this.PackageName);
                imgkey4 = Resources.GetIdentifier("" + str4, "drawable", this.PackageName);
                imgkey5 = Resources.GetIdentifier("" + str5, "drawable", this.PackageName);
                img1.SetImageResource(imgkey1);
                img2.SetImageResource(imgkey2);
                img3.SetImageResource(imgkey3);
                img4.SetImageResource(imgkey4);
                img5.SetImageResource(imgkey5);
            }
            else if (round == 3)
            {
                img1 = new ImageView(this);
                img2 = new ImageView(this);
                img3 = new ImageView(this);
                img4 = new ImageView(this);
                img5 = new ImageView(this);
                img6 = new ImageView(this);
                RelativeLayout.LayoutParams ivparams = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams2 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams3 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams4 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams5 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams6 = new RelativeLayout.LayoutParams(200, 200);
                ivparams.SetMargins(350, 350, 0, 0);
                ivparams2.SetMargins(370, 350, 0, 0);
                ivparams3.SetMargins(550, 400, 0, 0);
                ivparams4.SetMargins(720, 400, 0, 0);
                ivparams5.SetMargins(890, 400, 0, 0);
                ivparams6.SetMargins(1060, 400, 0, 0);
                img1.LayoutParameters = ivparams;
                img2.LayoutParameters = ivparams2;
                img3.LayoutParameters = ivparams3;
                img4.LayoutParameters = ivparams4;
                img5.LayoutParameters = ivparams5;
                img6.LayoutParameters = ivparams6;
                this.rl2.AddView(img1);
                this.rl2.AddView(img2);
                this.rl2.AddView(img3);
                this.rl2.AddView(img4);
                this.rl2.AddView(img5);
                this.rl2.AddView(img6);
                string str = "";
                string str2 = "";
                if (player.Id == p1.Id)
                {
                    str = MyGame.Players[0].Cards[0].Name;
                    str = MyGame.Players[0].Cards[1].Name;
                }
                else
                {
                    str = MyGame.Players[1].Cards[0].Name;
                    str2 = MyGame.Players[1].Cards[1].Name;
                }
                string str3 = MyGame.TableCards[0].Name;
                string str4 = MyGame.TableCards[1].Name;
                string str5 = MyGame.TableCards[2].Name;
                string str6 = MyGame.TableCards[3].Name;
                imgkey1 = Resources.GetIdentifier("" + str, "drawable", this.PackageName);
                imgkey2 = Resources.GetIdentifier("" + str2, "drawable", this.PackageName);
                imgkey3 = Resources.GetIdentifier("" + str3, "drawable", this.PackageName);
                imgkey4 = Resources.GetIdentifier("" + str4, "drawable", this.PackageName);
                imgkey5 = Resources.GetIdentifier("" + str5, "drawable", this.PackageName);
                imgkey6 = Resources.GetIdentifier("" + str6, "drawable", this.PackageName);
                img1.SetImageResource(imgkey1);
                img2.SetImageResource(imgkey2);
                img3.SetImageResource(imgkey3);
                img4.SetImageResource(imgkey4);
                img5.SetImageResource(imgkey5);
                img6.SetImageResource(imgkey6);
            }
            else
            {
                img1 = new ImageView(this);
                img2 = new ImageView(this);
                img3 = new ImageView(this);
                img4 = new ImageView(this);
                img5 = new ImageView(this);
                img6 = new ImageView(this);
                img7 = new ImageView(this);
                RelativeLayout.LayoutParams ivparams = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams2 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams3 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams4 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams5 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams6 = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams7 = new RelativeLayout.LayoutParams(200, 200);
                ivparams.SetMargins(350, 350, 0, 0);
                ivparams2.SetMargins(370, 350, 0, 0);
                ivparams3.SetMargins(550, 400, 0, 0);
                ivparams4.SetMargins(720, 400, 0, 0);
                ivparams5.SetMargins(890, 400, 0, 0);
                ivparams6.SetMargins(1060, 400, 0, 0);
                ivparams7.SetMargins(1230, 400, 0, 0);
                img1.LayoutParameters = ivparams;
                img2.LayoutParameters = ivparams2;
                img3.LayoutParameters = ivparams3;
                img4.LayoutParameters = ivparams4;
                img5.LayoutParameters = ivparams5;
                img6.LayoutParameters = ivparams6;
                img7.LayoutParameters = ivparams7;
                this.rl2.AddView(img1);
                this.rl2.AddView(img2);
                this.rl2.AddView(img3);
                this.rl2.AddView(img4);
                this.rl2.AddView(img5);
                this.rl2.AddView(img6);
                this.rl2.AddView(img7);
                string str = "";
                string str2 = "";
                if (player.Id == p1.Id)
                {
                    str = MyGame.Players[0].Cards[0].Name;
                    str = MyGame.Players[0].Cards[1].Name;
                }
                else
                {
                    str = MyGame.Players[1].Cards[0].Name;
                    str2 = MyGame.Players[1].Cards[1].Name;
                }
                string str3 = MyGame.TableCards[0].Name;
                string str4 = MyGame.TableCards[1].Name;
                string str5 = MyGame.TableCards[2].Name;
                string str6 = MyGame.TableCards[3].Name;
                string str7 = MyGame.TableCards[4].Name;
                imgkey1 = Resources.GetIdentifier("" + str, "drawable", this.PackageName);
                imgkey2 = Resources.GetIdentifier("" + str2, "drawable", this.PackageName);
                imgkey3 = Resources.GetIdentifier("" + str3, "drawable", this.PackageName);
                imgkey4 = Resources.GetIdentifier("" + str4, "drawable", this.PackageName);
                imgkey5 = Resources.GetIdentifier("" + str5, "drawable", this.PackageName);
                imgkey6 = Resources.GetIdentifier("" + str6, "drawable", this.PackageName);
                imgkey7 = Resources.GetIdentifier("" + str7, "drawable", this.PackageName);
                img1.SetImageResource(imgkey1);
                img2.SetImageResource(imgkey2);
                img3.SetImageResource(imgkey3);
                img4.SetImageResource(imgkey4);
                img5.SetImageResource(imgkey5);
                img6.SetImageResource(imgkey6);
                img7.SetImageResource(imgkey7);
            }
        }
    }
}