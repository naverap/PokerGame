//using Android.App;
//using Android.OS;
//using Android.Views;
//using Android.Widget;

//namespace YourPackageName
//{
//    [Activity(Label = "SignUpActivity")]
//    public class SignUpActivity : Activity
//    {
//        private EditText usernameEditText;
//        private EditText gmailEditText;
//        private EditText passwordEditText;
//        private EditText reEnterPasswordEditText;
//        private Button signUpButton;
//        private TextView signInTextView;

//        protected override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            SetContentView(Resource.Layout.activity_sign_up);

//            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
//            gmailEditText = FindViewById<EditText>(Resource.Id.GmailEditText);
//            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
//            reEnterPasswordEditText = FindViewById<EditText>(Resource.Id.reEnterPasswordEditText);
//            signUpButton = FindViewById<Button>(Resource.Id.signinButton);
//            signInTextView = FindViewById<TextView>(Resource.Id.signinTextView);

//            signUpButton.Click += SignUpButton_Click;
//            signInTextView.Click += SignInTextView_Click;
//        }

//        private void SignUpButton_Click(object sender, System.EventArgs e)
//        {
//            SignUp();
//        }

//        private void SignInTextView_Click(object sender, System.EventArgs e)
//        {
//            Finish();
//        }

//        private void SignUp()
//        {
//            string username = usernameEditText.Text;
//            string gmail = gmailEditText.Text;
//            string password = passwordEditText.Text;
//            string reEnterPassword = reEnterPasswordEditText.Text;

//            // Perform sign-up logic here
//            // Validate input and save user data

//            // Show a toast message indicating successful sign-up
//            Toast.MakeText(this, "Sign up successful!", ToastLength.Short).Show();

//            // Finish the activity and go back to sign-in page
//            Finish();
//        }
//    }
//}