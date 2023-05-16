using System.Linq;

namespace PokerLib;

public class HandFourOfAKind : Hand
{
    public HandFourOfAKind(Card[] cards)
    {
        Cards = cards;
        Stregth = 7;
        Name = "FourOfAKind";
        IsValid = Verify;

    }
    public override bool Verify => Cards.GroupBy(h => h.Value)
        .Where(g => g.Count() == 4)
        .Any();
}