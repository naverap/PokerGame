using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Hand7FullHouse : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    public Hand7FullHouse(IList<Card> cards)
    {
        Name = "full house";
        var groups = cards
            .GroupBy(h => h.Value)
            .Where(g => g.Count() == 2 || g.Count() == 3)
            .OrderByDescending(g => g.Count())
            .ThenByDescending(g => g.First().Value);
        IsValid = groups.Count() > 1 && groups.First().Count() == 3;
        if (!IsValid) return;

        var three = groups.First().ToList();
        var pair = groups.Skip(1).First().Take(2).ToList();

        Cards = three.Concat(pair).ToList();

        var multiplier = Hand.GetMultiplier(7);
        Score = three.First().IntValue * multiplier + pair.First().IntValue * multiplier / 0x10;
    }
}
