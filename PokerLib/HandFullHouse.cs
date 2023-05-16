using System.Linq;

namespace PokerLib;

public class HandFullHouse : Hand
{
    public HandFullHouse(Card[] cards)
    {
        Cards = cards;
        Stregth = 6;
        Name = "FullHouse";
        IsValid = Verify;
    }

    public override bool Verify => 
        Cards.GroupBy(h => h.Value).Where(g => g.Count() == 3).Count() > 0 && 
        Cards.GroupBy(l => l.Value).Where(m => m.Count() == 2).Count() > 0;
}
