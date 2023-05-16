namespace PokerTests;

[TestClass]
public class CardTests
{
    [DataTestMethod]
    [DataRow(CardSuit.Clubs, CardValue.Two, 0)]
    [DataRow(CardSuit.Clubs, CardValue.Ace, 12)]
    [DataRow(CardSuit.Diamonds, CardValue.Two, 13)]
    [DataRow(CardSuit.Spades, CardValue.Ace, 51)]
    public void CardId(CardSuit suit, CardValue value, int id)
    {
        var card = new Card(suit, value);
        Assert.AreEqual(id, card.Id);
        card = new Card(id);
        Assert.AreEqual(suit, card.Suit);
        Assert.AreEqual(value, card.Value);
    }

    [DataTestMethod]
    [DataRow(CardSuit.Hearts, CardValue.Five, "five_of_hearts")]
    [DataRow(CardSuit.Clubs, CardValue.Ace, "ace_of_clubs")]
    public void CardName(CardSuit suit, CardValue value, string name)
    {
        var card = new Card(suit, value);
        Assert.AreEqual(name, card.Name);
    }

    [DataTestMethod]
    [DataRow(CardSuit.Hearts, CardValue.Five, "5♡")]
    [DataRow(CardSuit.Clubs, CardValue.Ace, "A♣")]
    public void CardDisplayName(CardSuit suit, CardValue value, string name)
    {
        var card = new Card(suit, value);
        Assert.AreEqual(name, card.DisplayName);
    }

    [DataTestMethod]
    [DataRow("five_of_hearts", "five_of_hearts")]
    [DataRow("5♡", "five_of_hearts")]
    [DataRow("ace_of_clubs", "ace_of_clubs")]
    [DataRow("A♣", "ace_of_clubs")]
    public void CardParseName(string name, string expectedName)
    {
        var card = Card.Parse(name);
        Assert.AreEqual(expectedName, card.Name);
    }
}