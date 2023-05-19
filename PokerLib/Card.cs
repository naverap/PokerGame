using System;
using System.Text.Json.Serialization;

namespace PokerLib;

public struct Card
{
    public int Id { get; set; }

    [JsonIgnore]
    public readonly CardSuit Suit => (CardSuit)(Id / 13);

    [JsonIgnore]
    public readonly CardValue Value => (CardValue)(Id % 13 + 2);

    [JsonIgnore]
    public readonly string Name => $"{Value}_of_{Suit}".ToLowerInvariant();

    [JsonIgnore]
    public readonly string DisplayName => GetDisplayName();

    public Card(CardSuit suit, CardValue value)
    {
        Id = (int)suit * 13 + (int)value - 2;
    }

    public Card(int id)
    {
        Id = id;
    }

    static readonly char[] SuitChars = new[] { '♣', '♢', '♡', '♠' };

    readonly string GetDisplayName()
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