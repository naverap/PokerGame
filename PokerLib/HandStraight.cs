using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLib
{
    public class HandStraight : Hand
    {
        public HandStraight(Card[] cards)
        {
            Cards = cards;
            Stregth = 4;
            Name = "Straight";
            IsValid = Verify;

        }

        public bool Verify => Hand.ContainsStraight(this);
    }
}
