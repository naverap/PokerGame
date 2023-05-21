//using Android.App;
//using Android.OS;
//using Android.Views;
//using Android.Widget;
//using PokerGame;

//namespace YourPackageName
//{
//    [Activity(Label = "SignInActivity")]
//    public class SignInActivity : Activity
//    {
//        private EditText usernameEditText;
//        private EditText passwordEditText;
//        private Button signInButton;
//        private TextView signUpTextView;

//        protected override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            SetContentView(Resource.Layout.activity_sign_in);

//            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
//            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
//            signInButton = FindViewById<Button>(Resource.Id.signinButton);
//            signUpTextView = FindViewById<TextView>(Resource.Id.signupTextView);

//            signInButton.Click += SignInButton_Click;
//            signUpTextView.Click += SignUpTextView_Click;
//        }

//        private void SignInButton_Click(object sender, System.EventArgs e)
//        {
//            SignIn();
//        }

//        private void SignUpTextView_Click(object sender, System.EventArgs e)
//        {
//            // Start the SignUpActivity
//            StartActivity(typeof(SignUpActivity));
//        }

//        private void SignIn()
//        {
//            string username = usernameEditText.Text;
//            string password = passwordEditText.Text;

//            // Perform sign-in logic here
//            // Validate username and password

//            if (IsValidSignIn(username, password))
//            {
//                // Show a toast message indicating successful sign-in
//                Toast.MakeText(this, "Sign in successful!", ToastLength.Short).Show();

//                // Start the MainActivity or perform any desired action
//                StartActivity(typeof(MainActivity));
//                Finish();
//            }
//            else
//            {
//                // Show a toast message indicating invalid credentials
//                Toast.MakeText(this, "Invalid username or password!", ToastLength.Short).Show();
//            }
//        }

//        private bool IsValidSignIn(string username, string password)
//        {
//            // Implement your own sign-in validation logic here
//            // For demonstration purposes, assume sign-in is successful if the username and password are both "admin"
//            return (username == "admin" && password == "admin");
//        }
//    }
//}