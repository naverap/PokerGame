
using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public abstract class HandNOfAKind : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    protected HandNOfAKind(IList<Card> cards, int n, int rank, string name)
    {
        Name = name;
        var nGroups = cards
            .GroupBy(h => h.Value)
            .Where(g => g.Count() == n);
        IsValid = nGroups.Count() == 1;
        if (!IsValid) return;
        var nValuedCards = nGroups.Single().ToList();
        var nValue = nValuedCards.First().Value;
        var highestNonN = cards
            .Where(c => c.Value != nValue)
            .OrderByDescending(c => c.Value)
            .Take(5 - n)
            .ToList();
        Cards = nValuedCards.Concat(highestNonN).ToList();
        var scoreMultiplier = Hand.GetMultiplier(rank);
        Score = nValuedCards.First().IntValue * scoreMultiplier + highestNonN.GetHighCardsScore();
    }
}