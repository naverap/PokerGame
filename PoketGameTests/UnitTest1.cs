using PokerLib;

namespace PoketGameTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Pair()
        {
            var hand = new Hand(new Card[]
            {
                new Card(Card.SuitEnum.Spades, Card.ValueEnum.Two),
                new Card(Card.SuitEnum.Hearts, Card.ValueEnum.Two),
                new Card(Card.SuitEnum.Hearts, Card.ValueEnum.Three),
                new Card(Card.SuitEnum.Spades, Card.ValueEnum.Five),
                new Card(Card.SuitEnum.Diamonds, Card.ValueEnum.Seven),
                new Card(Card.SuitEnum.Clubs, Card.ValueEnum.Ten),
                new Card(Card.SuitEnum.Spades, Card.ValueEnum.King)
            });
            Assert.IsTrue(hand.IsPair);
        }
    }
}