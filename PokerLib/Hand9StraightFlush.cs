using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Hand9StraightFlush : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    public Hand9StraightFlush(IList<Card> cards)
    {
        Name = "straight flush";
        var flush = Hand6Flush.GetFlush(cards);
        IsValid = flush != null;
        if (!IsValid) return;

        Cards = Hand5Straight.GetStraight(flush);
        IsValid = Cards != null;
        if (!IsValid) return;

        Score = Cards.First().IntValue * Hand.GetMultiplier(9);

        if (Cards.First().Value == CardValue.Ace)
            Name = "royal flush";
    }
}
