using System;

namespace PokerLib;

public struct Card
{
    public CardSuit Suit { get; set; }
    public CardValue Value { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }

    static readonly char[] SuitChars = new[] { '♣', '♢', '♡', '♠' };

    public Card(CardSuit suit, CardValue value)
    {
        Suit = suit;
        Value = value;
        Name = $"{Value}_of_{Suit}".ToLowerInvariant();
        DisplayName = GetDisplayName();
    }

    string GetDisplayName()
    {
        var suitChar = SuitChars[(int)Suit];
        var valueName = Value <= CardValue.Ten
            ? ((int)Value).ToString()
            : Value.ToString().Substring(0, 1);
        return valueName + suitChar;
    }

    public static Card Parse(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));
        if (name.Length > 3)
        {
            var parts = name.Split('_');
            Enum.TryParse(parts[0], true, out CardValue value);
            Enum.TryParse(parts[2], true, out CardSuit suit);
            return new Card(suit, value);
        }
        else
        {
            var valueName = name.Substring(0, name.Length - 1);
            var suitChar = name[name.Length - 1];
            var suit = (CardSuit)Array.IndexOf(SuitChars, suitChar);
            var value = valueName switch
            {
                "A" => CardValue.Ace,
                "K" => CardValue.King,
                "Q" => CardValue.Queen,
                "J" => CardValue.Jack,
                _ => (CardValue)int.Parse(valueName)
            };
            return new Card(suit, value);
        }
    }
}