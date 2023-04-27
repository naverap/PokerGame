using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class User
    {
        public string UserName { get; set; }
        public string Gmail { get; set; }
        public string Password { get; set; }


        public User(string username, string gmail,string password) 
        {
            UserName= username;
            Gmail= gmail;
            Password= password;
        }
    }
}