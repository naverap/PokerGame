using System.Collections.Generic;
using System.Linq;

namespace PokerLib
{
    public static class HandsExtensions
    {
        public static bool ContainsValue(this IList<Card> cards, CardValue value)
            => cards.Any(c => c.Value == value);

        public static Card GetFirstOrDefaultByValue(this IList<Card> cards, CardValue value)
            => cards.FirstOrDefault(c => c.Value == value);

        public static long GetHighCardsScore(this IList<Card> cards)
        {
            var hexScore = string.Join("", cards.Select(c => c.HexValue));
            return long.Parse(hexScore, System.Globalization.NumberStyles.HexNumber);
        }
    }
}
