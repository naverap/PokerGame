using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Hand1HighCard : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    public Hand1HighCard(IList<Card> cards)
    {
        Name = "high card";
        IsValid = true;
        Cards = cards
            .OrderByDescending(c => c.Value)
            .Take(5)
            .ToList();
        Score = Cards.GetHighCardsScore();
    }
}
