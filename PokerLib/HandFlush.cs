using System.Linq;

namespace PokerLib;

public class HandFlush : Hand
{
    public HandFlush(Card[] cards)
    {
        Cards = cards;
        Stregth = 5;
        Name = "Flush";
        IsValid = Verify;

    }

    public override bool Verify => Cards.GroupBy(h => h.Suit)
        .Where(g => g.Count() >= 5)
        .Any();
}
