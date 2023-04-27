//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace PokerGame
//{
//    [Activity(Label = "SignUpActivity")]
//    public class SignUpActivity : Activity
//    {
//        EditText usernameEditText;
//        EditText gmailEditText;
//        EditText passwordEditText;
//        EditText reEnterPasswordEditText;
//        Button signUpButton;
//        TextView forgotPasswordTextView;
//        TextView signInTextView;

//        protected override void OnCreate(Bundle savedInstanceState)
//        {
//            base.OnCreate(savedInstanceState);
//            SetContentView(Resource.Layout.Signup);

//            usernameEditText = FindViewById<EditText>(Resource.Id.usernameEditText);
//            gmailEditText = FindViewById<EditText>(Resource.Id.GmailEditText);
//            passwordEditText = FindViewById<EditText>(Resource.Id.passwordEditText);
//            reEnterPasswordEditText = FindViewById<EditText>(Resource.Id.reEnterPasswordEditText);
//            signUpButton = FindViewById<Button>(Resource.Id.signinButton);
//            forgotPasswordTextView = FindViewById<TextView>(Resource.Id.forgotPasswordTextView);
//            signInTextView = FindViewById<TextView>(Resource.Id.signinTextView);

//            signUpButton.Click += SignUpButton_Click;
//            forgotPasswordTextView.Click += ForgotPasswordTextView_Click;
//            signInTextView.Click += SignInTextView_Click;
//        }

//        private void SignUpButton_Click(object sender, System.EventArgs e)
//        {
//            string username = usernameEditText.Text;
//            string email = gmailEditText.Text;
//            string password = passwordEditText.Text;
//            string reEnterPassword = reEnterPasswordEditText.Text;

//            // Do your sign-up logic here

//            Toast.MakeText(this, "Sign up button clicked", ToastLength.Short).Show();
//        }

//        private void ForgotPasswordTextView_Click(object sender, System.EventArgs e)
//        {
//            // Handle forgot password click here
//            Toast.MakeText(this, "Forgot password clicked", ToastLength.Short).Show();
//        }

//        private void SignInTextView_Click(object sender, System.EventArgs e)
//        {
//            // Handle sign-in click here
//            Toast.MakeText(this, "Sign in clicked", ToastLength.Short).Show();
//        }
//    }
//}