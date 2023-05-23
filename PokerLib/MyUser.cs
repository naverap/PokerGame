using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLib
{
    public class MyUser
    {
        public MyUser(string name, string password, string email)
        {
            Name = name;
            Password = password;
            Email = email;
        }


        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
    }
}
