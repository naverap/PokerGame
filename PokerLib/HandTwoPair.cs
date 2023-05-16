﻿using System.Linq;

namespace PokerLib;

public class HandTwoPair : Hand
{
    public HandTwoPair(Card[] cards)
    {
        Cards = cards;
        Stregth = 2;
        Name = "TwoPair";
        IsValid = Verify;
    }

    public override bool Verify => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 2)
        .Count() > 1; 
}