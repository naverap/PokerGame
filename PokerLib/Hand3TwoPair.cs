using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Hand3TwoPair : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    public Hand3TwoPair(IList<Card> cards)
    {
        Name = "two pair";
        var pairs = cards
            .GroupBy(h => h.Value)
            .Where(g => g.Count() == 2);
        IsValid = pairs.Count() > 1;
        if (!IsValid) return;
        var highestPairsValues = pairs
            .OrderByDescending(g => g.Key)
            .Take(2)
            .Select(g => g.Key)
            .ToList();
        var highestNonPair = cards
            .Where(c => !highestPairsValues.Contains(c.Value))
            .OrderByDescending(c => c.Value)
            .First();
        var highestNonPairAsList = new List<Card> { highestNonPair };
        Cards = cards
            .Where(c => highestPairsValues.Contains(c.Value))
            .OrderByDescending(c => c.Value)
            .Concat(highestNonPairAsList)
            .ToList();
        Score = Cards[0].IntValue * 0x1000000 + Cards[2].IntValue * 0x100000 + highestNonPair.IntValue;
    }
}
