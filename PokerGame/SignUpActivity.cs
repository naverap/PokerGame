using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using PokerLib;
using Firebase.Firestore.Auth;
using PokerGame;


namespace PokerGame
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity
    {
        private EditText usernameEditText;
        private EditText gmailEditText;
        private EditText passwordEditText;
        private EditText reEnterPasswordEditText;
        private Button signUpButton;
        private TextView signInTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SignUp);

            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
            gmailEditText = FindViewById<EditText>(Resource.Id.GmailEditText);
            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
            reEnterPasswordEditText = FindViewById<EditText>(Resource.Id.reEnterPasswordEditText);
            signUpButton = FindViewById<Button>(Resource.Id.signinButton);
            signInTextView = FindViewById<TextView>(Resource.Id.signinTextView);

            signUpButton.Click += SignUpButton_Click;
            signInTextView.Click += SignInTextView_Click;
        }

        private void SignUpButton_Click(object sender, System.EventArgs e)
        {
            SignUp();
        }

        private void SignInTextView_Click(object sender, System.EventArgs e)
        {
            Finish();
        }

        private void SignUp()
        {
            Repository.Init(this);
            string username = usernameEditText.Text;
            string gmail = gmailEditText.Text;
            string password = passwordEditText.Text;
            string reEnterPassword = reEnterPasswordEditText.Text;

            MyUser user = new MyUser(username, password, gmail);
            Repository.UploadUser(user);
            MainActivity.IsSignedIn = true;
            FinishActivity(-1);

            //Intent l = new Intent(this, typeof(MainActivity));


            // Perform sign-up logic here
            // Validate input and save user data
          //  if (IsSignUpValid())
          //  {
                //StartActivity(Intent);
           // }
           // else Toast.MakeText(this, "your details are invalide", ToastLength.Long).Show();
            

            
        }

        private bool IsSignUpValid()
        {
            if (string.IsNullOrEmpty(usernameEditText.Text)) { return false; }
            if (string.IsNullOrEmpty(passwordEditText.Text)) { return false; }
            if (string.IsNullOrEmpty(gmailEditText.Text)) { return false; }
            if (passwordEditText != reEnterPasswordEditText) { return false; }
            return true;
        }
    }
}