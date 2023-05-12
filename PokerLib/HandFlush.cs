using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLib
{
    public class HandFlush : Hand
    {
        public HandFlush(Card[] cards)
        {
            Cards = cards;
            Stregth = 5;
            Name = "Flush";
            IsValid = Verify;

        }

        public bool Verify => Cards.GroupBy(h => h.Suit)
       .Where(g => g.Count() >= 5)
       .Any();
    }
}
