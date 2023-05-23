using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Hand6Flush : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    public Hand6Flush(IList<Card> cards)
    {
        Name = "flush";
        Cards = GetFlush(cards);
        IsValid = Cards != null;
        if (!IsValid) return;
        Score = Cards.First().IntValue * Hand.GetMultiplier(6);
    }

    public static IList<Card> GetFlush(IList<Card> cards)
    {
        return cards
            .GroupBy(h => h.Suit)
            .SingleOrDefault(g => g.Count() >= 5)?
            .OrderByDescending(h => h.Value)
            .Take(5)
            .ToList();
    }
}
