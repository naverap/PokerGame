using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace PokerGame
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true,
        TurnScreenOn = false, ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class MainActivity : AppCompatActivity
    {
        Button btn1, btn2, btnhome, btngame, btnsignup;
        Dialog d;
        BroadcastReceiver flightModeReceiver = new FlightModeReceiver();
        public static bool IsSignedIn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource

            SetContentView(Resource.Layout.activity_main);
            btn1 = (Button)FindViewById(Resource.Id.playbtn);
            btn1.Click += Btn1_Click;
            btn2 = (Button)FindViewById(Resource.Id.menubtn);
            btn2.Click += Btn2_Click;
        }

        protected override void OnResume()
        {
            base.OnResume();
            RegisterReceiver(flightModeReceiver, new IntentFilter("android.intent.action.AIRPLANE_MODE"));
        }

        protected override void OnPause()
        {
            base.OnPause();
            UnregisterReceiver(flightModeReceiver);
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            CreateMenu();
        }

        private void CreateMenu()
        {
            d = new Dialog(this);
            d.SetContentView(Resource.Layout.MenuPopUp);
            d.SetTitle("MENU");
            d.SetCancelable(true);

            btngame = (Button)d.FindViewById(Resource.Id.gamebtn);
            btnhome = (Button)d.FindViewById(Resource.Id.homebtn);
            btnsignup = (Button)d.FindViewById(Resource.Id.signupbtn);

            btngame.Click += Btngame_Click;
            btnhome.Click += Btnhome_Click;
            btnsignup.Click += Btnsignup_Click;
            d.Show();
        }

        private void Btnsignup_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(SignUpActivity));
            StartActivityForResult(t, -1);
        }

        private void Btnhome_Click(object sender, EventArgs e)
        {
            Intent t = new Intent(this, typeof(MainActivity));
            StartActivity(t);
        }

        private void Btngame_Click(object sender, EventArgs e)
        {
            StartGameActivity();
        }

        private void Btn1_Click(object sender, EventArgs e)
        {
            StartGameActivity();
        }

        private void StartGameActivity()
        {
            var editPlayerName = (EditText)FindViewById(Resource.Id.playerNameEditText);
            var playerName = editPlayerName.Text;

            Intent d = new Intent(this, typeof(GameActivity));
            d.PutExtra("PlayerName", playerName);
            StartActivity(d);

        }
    }
}