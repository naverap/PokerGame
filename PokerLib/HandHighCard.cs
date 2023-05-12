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
        }
        public override bool Verify => true;
    }
}
