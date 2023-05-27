using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;
using PokerLib;
using Firebase.Firestore.Auth;
using PokerGame;
using System.Text.RegularExpressions;
using System.Globalization;
using Android.Gms.Tasks;
using Firebase.Firestore;

namespace PokerGame
{
    [Activity(Label = "SignUpActivity")]
    public class SignUpActivity : Activity, IOnSuccessListener
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
            Intent intent = new Intent(this,typeof(SignInActivity));
            StartActivity(intent);
        }

        private void SignUp()
        {
            Repository.Init(this);
            string username = usernameEditText.Text;
            string gmail = gmailEditText.Text;
            string password = passwordEditText.Text;
            string reEnterPassword = reEnterPasswordEditText.Text;
            Intent i = new Intent(this,typeof(MainActivity));

            // Perform sign-up logic here
            // Validate input and save user data
            if (IsSignUpValid())
            {
                MyUser user = new MyUser(username, password, gmail);
                Repository.UploadUser(user);
                MainActivity.IsSignedIn = true;
                StartActivity(i);
            }
            else Toast.MakeText(this, "your details are invalide", ToastLength.Long).Show();   
        }

        private bool IsSignUpValid()
        {
            if (string.IsNullOrEmpty(usernameEditText.Text)) { return false; }
            if (string.IsNullOrEmpty(passwordEditText.Text)) { return false; }
            if (!IsValidEmail(gmailEditText.Text)) { return false; }
            if (passwordEditText.Text != reEnterPasswordEditText.Text) { return false; }
            return true;
        }

        public void OnGetUser(Object stateInfo)
        {
            Repository.UserDocument.Get().AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result)
        {
        //    if (result is QuerySnapshot query)
        //        OnGetGames(query);
        //    else if (result is DocumentSnapshot document)
        //        OnGetGame(document);
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

      
    }
}