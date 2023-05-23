using System.Collections.Generic;
using System.Linq;

namespace PokerLib;

public class Hand5Straight : IHand
{
    public IList<Card> Cards { get; private set; }
    public string Name { get; private set; }
    public long Score { get; private set; }
    public bool IsValid { get; private set; }

    public Hand5Straight(IList<Card> cards)
    {
        Name = "straight";
        Cards = GetStraight(cards);
        IsValid = Cards != null;
        if (!IsValid) return;
        Score = Cards.First().IntValue * Hand.GetMultiplier(5);
    }

    public static IList<Card> GetStraight(IList<Card> cards)
    {
        if (cards.ContainsValue(CardValue.Five) &&
            cards.ContainsValue(CardValue.Four) &&
            cards.ContainsValue(CardValue.Three) &&
            cards.ContainsValue(CardValue.Two) &&
            cards.ContainsValue(CardValue.Ace))
        {
            return new List<Card>
            {
                cards.GetFirstOrDefaultByValue(CardValue.Five),
                cards.GetFirstOrDefaultByValue(CardValue.Four),
                cards.GetFirstOrDefaultByValue(CardValue.Three),
                cards.GetFirstOrDefaultByValue(CardValue.Two),
                cards.GetFirstOrDefaultByValue(CardValue.Ace)
            };
        }

        var ordered = cards
            .GroupBy(h => h.Value)
            .OrderByDescending(g => g.Key)
            .ToArray();

        if (ordered.Length < 5) return null;

        var distinct = ordered.Select(g => g.First()).ToList();

        for (var i = 0; i < distinct.Count - 4; i++)
        {
            var first = distinct[i].IntValue;
            var last = distinct[i + 4].IntValue;
            if (first - last == 4)
                return distinct.Skip(i).Take(5).ToList();
        }
        return null;
    }
}
