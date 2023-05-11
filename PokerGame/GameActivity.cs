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

namespace PokerGame
{
    [Activity(Label = "GameActivity",
     TurnScreenOn = false,
     ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class GameActivity : Activity , IOnSuccessListener
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
            container = (FrameLayout)FindViewById(Resource.Id.GameContainer);
            PokerTableView myGame = new PokerTableView(this);
            SetUpStuff();
            container.AddView(myGame);


            Live.db = GetDataBase();
            
            
            if (Intent.GetIntExtra("PlayerId", 1) == 1)
            {
                MyGame = CreateGame();
                UpLoadStartingGameState();
            }
            else
            {
                    
            }
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;
            Card c;
            foreach (var doc in snapshot.Documents)
            {
                c = new Card((CardSuit)(int)doc.Get("P1Card1Suit"), (CardValue)(int)doc.Get("P1Card1Value"));
                Console.WriteLine(c.ToString());
            }
        }

        private void UpLoadStartingGameState()
        {
            DocumentReference docRef = Live.db.Collection("Games").Document("Game1");
            //docRef.Set(MyGame);

            // Create a HashMap to store your data like an object
            // HashMap is a collection of "keys" and "values"
            //HashMap map = new HashMap();
            //// save data
            //// map.Put([field name], content);
            //map.Put("P1Card1Value", (int)MyGame.Players[0].PlayerCards[0].Value);
            //map.Put("P1Card1Suit", (int)MyGame.Players[0].PlayerCards[0].Suit);
            //DocumentReference docRef = Live.db.Collection("Cards").Document("Player1Card1");
            //docRef.Set(map);
            //map.Put("P1Card2Value", (int)MyGame.Players[0].PlayerCards[1].Value);
            //map.Put("P1Card2Suit", (int)MyGame.Players[0].PlayerCards[1].Suit);
            //DocumentReference docRef2 = Live.db.Collection("Cards").Document("Player1Card1");
            //docRef2.Set(map);

            //map.Put("P2Card1Value", (int)MyGame.players[1].PlayerCards[0].Value);
            //map.Put("P2Card1Suit", (int)MyGame.players[1].PlayerCards[0].Suit);
            //map.Put("P2Card2Value", (int)MyGame.players[1].PlayerCards[1].Value);
            //map.Put("P2Card2Suit", (int)MyGame.players[1].PlayerCards[1].Suit);
            //map.Put("GameCard1Value", (int)MyGame.GameCards[0].Value);
            //map.Put("GameCard2Suit", (int)MyGame.GameCards[0].Suit);
            //map.Put("GameCard2Value", (int)MyGame.GameCards[1].Value);
            //map.Put("GameCard2Suit", (int)MyGame.GameCards[1].Value);
            //map.Put("GameCard3Value", (int)MyGame.GameCards[2].Value);
            //map.Put("GameCard3Suit", (int)MyGame.GameCards[2].Suit);
            //map.Put("GameCard4Value", (int)MyGame.GameCards[3].Value);
            //map.Put("GameCard4Suit", (int)MyGame.GameCards[3].Suit);
            //map.Put("GameCard5Value", (int)MyGame.GameCards[4].Value);
            //map.Put("GameCard5Suit", (int)MyGame.GameCards[4].Suit);
            // create an empty document reference for firestore

            //CollectionReference collection = Live.db.Collection("Cards");
            // puts the map info in the document

        }
        //public void DeleteAll()
        //{
        //    Live.db.Collection("Cards").Document("id").Delete();
        //}

        private void LoadGame()
        {
            // generate a query (request) from the database
            Query q = Live.db.Collection("students");
            // perform the request
            q.Get().AddOnSuccessListener(this);
        }

        protected override void OnStart()
        {
            base.OnStart();
            GameLoop();
        }

        public FirebaseFirestore GetDataBase()
        {
            FirebaseFirestore db;
            // info from "google-services.json"
            var options = new FirebaseOptions.Builder()
            .SetProjectId("pokergame2")
            .SetApplicationId("pokergame2")
            .SetApiKey("AIzaSyCASOGnHrcqa5YDEXzY4lPqENz5Fk6CNcc")
            .SetStorageBucket("pokergame2.appspot.com")
            .Build();
            try
            {


            var app = FirebaseApp.InitializeApp(this, options);
            db = FirebaseFirestore.GetInstance(app);
            return db;

            }
            catch
            {
                var app = FirebaseApp.GetApps(this);
                db = FirebaseFirestore.GetInstance(app[0]);
                return db;
            }
        }

        private void SetUpNewGame()
        {
            MyGame = null;
            Player1HasMoney = true;
            Player2HasMoney = true;
            FirstRoundWasPlayed = false;
            SecondRoundWasPlayed = false;
            ThirdRoundWasPlayed = false;
            fourthfRoundWasPlayed = false;
            didPlayerOneWin = false;
            didPlayerTwoWin = false;
            MyGame.Pot = 0;
            MyGame.CurrentRound = 0;
            MyGame.LastBet = 0;

        }

        public void SetUpStuff()
        {
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
        }

        private void AllIn_Click(object sender, EventArgs e)
        {
            MyGame.AllIn();
        }

        private void RaisePot_Click(object sender, EventArgs e)
        {
            MyGame.BetPecentage(100);
        }

        private void Raise50_Click(object sender, EventArgs e)
        {
            MyGame.BetPecentage(50);
        }

        private void Raise25_Click(object sender, EventArgs e)
        {
            MyGame.BetPecentage(25);
        }

        private void Btnraise_Click(object sender, EventArgs e)
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

        private void SubmitRaise_Click(object sender, EventArgs e)
        {
            var sum = Int32.Parse(et.Text);
            MyGame.BetOld(sum);
        }

        private void Btncheck_Click(object sender, EventArgs e)
        {
            MyGame.Check();
        }

        private void Btnfold_Click(object sender, EventArgs e)
        {
            didPlayerTwoWin = true;
        }

        public static Game CreateGame()
        {
            var game = new Game();
            game.AddPlayer(new Player(2000, 1, true));
            game.AddPlayer(new Player(2000, 2, true));
            game.DealCardsToGameCards(5);
            game.DealFirstRound();
            return game;
        }

        public static void AddPlayer()
        {
        }

        public void GameLoop()
        {
            DrawCards(MyGame.CurrentRound, MyGame.CurrentPlayer);
            textview1.Text = MyGame.Players[0].Pot.ToString();
            textview2.Text = MyGame.Players[1].Pot.ToString();
            textview3.Text = MyGame.Pot.ToString();
            if (MyGame.CurrentRound > 4)
            {
                Player[] winners = MyGame.CheckWinners();
                didPlayerOneWin = winners.Contains(p1);
                didPlayerTwoWin = winners.Contains(p2);
            }
            if (didPlayerOneWin && didPlayerTwoWin)
            {
                textview5.Visibility = ViewStates.Visible;
                textview4.Visibility = ViewStates.Visible;
                SetUpNewGame();
            }

            else if (didPlayerTwoWin)
            {
                textview4.Visibility = ViewStates.Visible;
                SetUpNewGame();
            }

            else if (didPlayerOneWin)
            {
                textview5.Visibility = ViewStates.Visible;
                SetUpNewGame();
            }

            MyGame.SetNextPlayerTurn();
        }

        private void DrawCards(int round, Player player)
        {
            int imgkey1, imgkey2, imgkey3, imgkey4, imgkey5, imgkey6, imgkey7;
            if (round == 1)
            {
                this.img1 = new ImageView(this);
                this.img2 = new ImageView(this);
                RelativeLayout.LayoutParams ivparams = new RelativeLayout.LayoutParams(200, 200);
                RelativeLayout.LayoutParams ivparams2 = new RelativeLayout.LayoutParams(200, 200);
                ivparams.SetMargins(350, 350, 0, 0);
                ivparams2.SetMargins(370, 350, 0, 0);
                this.img1.LayoutParameters = ivparams;
                this.img2.LayoutParameters = ivparams2;
                this.rl2.AddView(this.img1);
                this.rl2.AddView(this.img2);
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

                imgkey1 = Resources.GetIdentifier("" + str, "drawable", this.PackageName);
                imgkey2 = Resources.GetIdentifier("" + str2, "drawable", this.PackageName);
                img1.SetImageResource(imgkey1);
                img2.SetImageResource(imgkey2);
            }
            else if (round == 2)
            {
                this.img1 = new ImageView(this);
                this.img2 = new ImageView(this);
                this.img3 = new ImageView(this);
                this.img4 = new ImageView(this);
                this.img5 = new ImageView(this);
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
                this.img1.LayoutParameters = ivparams;
                this.img2.LayoutParameters = ivparams2;
                this.img3.LayoutParameters = ivparams3;
                this.img4.LayoutParameters = ivparams4;
                this.img5.LayoutParameters = ivparams5;
                this.rl2.AddView(this.img1);
                this.rl2.AddView(this.img2);
                this.rl2.AddView(this.img3);
                this.rl2.AddView(this.img4);
                this.rl2.AddView(this.img5);
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
                this.img1 = new ImageView(this);
                this.img2 = new ImageView(this);
                this.img3 = new ImageView(this);
                this.img4 = new ImageView(this);
                this.img5 = new ImageView(this);
                this.img6 = new ImageView(this);
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
                this.img1.LayoutParameters = ivparams;
                this.img2.LayoutParameters = ivparams2;
                this.img3.LayoutParameters = ivparams3;
                this.img4.LayoutParameters = ivparams4;
                this.img5.LayoutParameters = ivparams5;
                this.img6.LayoutParameters = ivparams6;
                this.rl2.AddView(this.img1);
                this.rl2.AddView(this.img2);
                this.rl2.AddView(this.img3);
                this.rl2.AddView(this.img4);
                this.rl2.AddView(this.img5);
                this.rl2.AddView(this.img6);
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
                this.img1 = new ImageView(this);
                this.img2 = new ImageView(this);
                this.img3 = new ImageView(this);
                this.img4 = new ImageView(this);
                this.img5 = new ImageView(this);
                this.img6 = new ImageView(this);
                this.img7 = new ImageView(this);
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
                this.img1.LayoutParameters = ivparams;
                this.img2.LayoutParameters = ivparams2;
                this.img3.LayoutParameters = ivparams3;
                this.img4.LayoutParameters = ivparams4;
                this.img5.LayoutParameters = ivparams5;
                this.img6.LayoutParameters = ivparams6;
                this.img7.LayoutParameters = ivparams7;
                this.rl2.AddView(this.img1);
                this.rl2.AddView(this.img2);
                this.rl2.AddView(this.img3);
                this.rl2.AddView(this.img4);
                this.rl2.AddView(this.img5);
                this.rl2.AddView(this.img6);
                this.rl2.AddView(this.img7);
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

        private void Btn1_Click(object sender, EventArgs e)
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
        private void Btnhome_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(MainActivity));
            StartActivity(Intent);
        }

        private void Btngame_Click(object sender, EventArgs e)
        {
            Intent l = new Intent(this, typeof(GameActivity));
            StartActivity(Intent);
        }
    }
}