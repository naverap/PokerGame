namespace PokerTests;

[TestClass]
public class DeckTests
{
    [TestMethod]
    public void Create()
    {
        var deck = new Deck();
        Assert.AreEqual(52, deck.Count);
        Assert.AreEqual(0, deck[0].Id);
        Assert.AreEqual(1, deck[1].Id);
        Assert.AreEqual(51, deck[51].Id);
    }

    [TestMethod]
    public void Shuffle()
    {
        var deck = new Deck();
        deck.Shuffle();
        Assert.AreEqual(52, deck.Count);
        Assert.IsFalse(deck[0].Id == 0 && deck[1].Id == 1);
    }

    [TestMethod]
    public void TakeCard()
    {
        var deck = new Deck();
        var card = deck.Pop();
        Assert.AreEqual(51, deck.Count);
        Assert.AreEqual(0, card.Id);
        Assert.AreEqual(1, deck[0].Id);
    }

    [TestMethod]
    public void TakeCards()
    {
        var deck = new Deck();
        var cards = deck.Pop(5);
        Assert.AreEqual(5, cards.Count);
        Assert.AreEqual(47, deck.Count);
        Assert.AreEqual(0, cards[0].Id);
        Assert.AreEqual(4, cards[4].Id);
        Assert.AreEqual(5, deck[0].Id);
    }
}