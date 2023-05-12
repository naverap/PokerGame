
using PokerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLib
{
    public class HandFullHouse : Hand
    {
        public HandFullHouse(Card[] cards)
        {
            Cards = cards;
            Stregth = 6;
            Name = "FullHouse";
            IsValid = Verify;
        }
        public bool Verify => Cards.GroupBy(h => h.Value)
       .Where(g => g.Count() == 3)
       .Count() > 0 &&
        Cards.GroupBy(l => l.Value)
       .Where(m => m.Count() == 2).Count() > 0;
            

    }
    }
