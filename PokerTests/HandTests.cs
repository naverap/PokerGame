namespace PokerTests;

[TestClass]
public class HandTests
{
    [TestMethod]
    public void HighCard()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Two),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.Five),
            new Card(CardSuit.Diamonds, CardValue.Seven),
            new Card(CardSuit.Clubs, CardValue.Ten),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand1HighCard));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xDCA75, hand.Score);
    }

    [TestMethod]
    public void OnePairOfThrees()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.Five),
            new Card(CardSuit.Diamonds, CardValue.Seven),
            new Card(CardSuit.Clubs, CardValue.Ten),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand2OnePair));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0x300DCA, hand.Score);
    }

    [TestMethod]
    public void ThreePairsAsTwoPair()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Spades, CardValue.Ten),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Diamonds, CardValue.King),
            new Card(CardSuit.Clubs, CardValue.Ten),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand3TwoPair));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xDA0000C, hand.Score);
    }

    [TestMethod]
    public void ThreeOfAKind()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.Five),
            new Card(CardSuit.Diamonds, CardValue.Seven),
            new Card(CardSuit.Clubs, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand4ThreeOfAKind));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0x300000DC, hand.Score);
    }

    [TestMethod]
    public void FullHouse331()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.King),
            new Card(CardSuit.Diamonds, CardValue.King),
            new Card(CardSuit.Clubs, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand7FullHouse));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xD3000000000, hand.Score);
    }


    [TestMethod]
    public void FullHouse322()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.Five),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.Five),
            new Card(CardSuit.Diamonds, CardValue.King),
            new Card(CardSuit.Clubs, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand7FullHouse));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0x3D000000000, hand.Score);
    }

    [TestMethod]
    public void FourOfAKind()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Three),
            new Card(CardSuit.Diamonds, CardValue.Three),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.Five),
            new Card(CardSuit.Diamonds, CardValue.Seven),
            new Card(CardSuit.Clubs, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand8FourOfAKind));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0x30000000000D, hand.Score);
    }

    [TestMethod]
    public void GetStraightFiveToAce()
    {
        var cards = new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Ace),
            new Card(CardSuit.Diamonds, CardValue.Two),
            new Card(CardSuit.Hearts, CardValue.Three),
            new Card(CardSuit.Spades, CardValue.Four),
            new Card(CardSuit.Diamonds, CardValue.Four),
            new Card(CardSuit.Clubs, CardValue.Five),
            new Card(CardSuit.Spades, CardValue.King)
        };

        var straight = Hand5Straight.GetStraight(cards);
        Assert.IsNotNull(straight);
        Assert.AreEqual(5, straight.Count);
        Assert.AreEqual(CardValue.Five, straight[0].Value);
        Assert.AreEqual(CardValue.Four, straight[1].Value);
        Assert.AreEqual(CardValue.Three, straight[2].Value);
        Assert.AreEqual(CardValue.Two, straight[3].Value);
        Assert.AreEqual(CardValue.Ace, straight[4].Value);
    }

    [TestMethod]
    public void GetStraight()
    {
        var cards = new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Six),
            new Card(CardSuit.Diamonds, CardValue.Seven),
            new Card(CardSuit.Hearts, CardValue.Eight),
            new Card(CardSuit.Spades, CardValue.Nine),
            new Card(CardSuit.Diamonds, CardValue.Ten),
            new Card(CardSuit.Clubs, CardValue.Five),
            new Card(CardSuit.Spades, CardValue.King)
        };

        var straight = Hand5Straight.GetStraight(cards);
        Assert.IsNotNull(straight);
        Assert.AreEqual(5, straight.Count);
        Assert.AreEqual(CardValue.Ten, straight[0].Value);
        Assert.AreEqual(CardValue.Nine, straight[1].Value);
        Assert.AreEqual(CardValue.Eight, straight[2].Value);
        Assert.AreEqual(CardValue.Seven, straight[3].Value);
        Assert.AreEqual(CardValue.Six, straight[4].Value);
    }

    [TestMethod]
    public void Straight()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Spades, CardValue.Six),
            new Card(CardSuit.Diamonds, CardValue.Seven),
            new Card(CardSuit.Hearts, CardValue.Eight),
            new Card(CardSuit.Spades, CardValue.Nine),
            new Card(CardSuit.Diamonds, CardValue.Ten),
            new Card(CardSuit.Clubs, CardValue.Five),
            new Card(CardSuit.Spades, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand5Straight));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xA00000000, hand.Score);
    }

    [TestMethod]
    public void Flush()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Hearts, CardValue.Six),
            new Card(CardSuit.Diamonds, CardValue.Two),
            new Card(CardSuit.Hearts, CardValue.Two),
            new Card(CardSuit.Hearts, CardValue.Nine),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Five),
            new Card(CardSuit.Hearts, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand6Flush));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xD000000000, hand.Score);
    }

    [TestMethod]
    public void StraightFlush()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Hearts, CardValue.Jack),
            new Card(CardSuit.Diamonds, CardValue.Two),
            new Card(CardSuit.Hearts, CardValue.Eight),
            new Card(CardSuit.Hearts, CardValue.Nine),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Ten),
            new Card(CardSuit.Hearts, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand9StraightFlush));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xD000000000000, hand.Score);
        Assert.AreEqual("straight flush", hand.Name);
    }

    [TestMethod]
    public void RoyalFlush()
    {
        var hand = Hand.CreateHand(new List<Card>
        {
            new Card(CardSuit.Hearts, CardValue.Jack),
            new Card(CardSuit.Diamonds, CardValue.Two),
            new Card(CardSuit.Hearts, CardValue.Eight),
            new Card(CardSuit.Hearts, CardValue.Ace),
            new Card(CardSuit.Hearts, CardValue.Queen),
            new Card(CardSuit.Hearts, CardValue.Ten),
            new Card(CardSuit.Hearts, CardValue.King)
        });

        Assert.IsInstanceOfType(hand, typeof(Hand9StraightFlush));
        Assert.IsTrue(hand.IsValid);
        Assert.AreEqual(0xE000000000000, hand.Score);
        Assert.AreEqual("royal flush", hand.Name);
    }
}