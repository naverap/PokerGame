using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLib
{
    public class HandHighCard : Hand
    {
        public HandHighCard(Card[] cards)
        {
            Cards = cards;
            Stregth = 0;
            Name = "HighCard";
            IsValid = Verify;
            //TieBreaker = FindTieBreaker();
        }

 
        public override bool Verify => true;

        //private Card FindTieBreaker()
        //{
        //    var temp = Cards.OrderByDescending(h => h.Value);
        //    return temp.FirstOrDefault();
        //}

    }
}
