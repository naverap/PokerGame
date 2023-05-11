using System.Linq;

namespace PokerLib;

public class HandPair : Hand
{
    public HandPair(Card[] cards)
    {
        Cards = cards;
        Stregth = 1;
        Name = "Pair";
        IsValid = Verify;
    }

    public override bool Verify => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 2)
        .Count() == 1;
}
