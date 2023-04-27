using System;
using System.Collections.Generic;

namespace PokerLib
{
    public class Card
    {

        public enum ValueEnum
        {
            Two = 2,
            Three,
            Four,
            Five,
            Six,
            Seven,
            Eight,
            Nine,
            Ten,
            Jack,
            Queen,
            King,
            Ace

        }

        public enum SuitEnum
        {
            Spades,
            Diamonds,
            Hearts,
            Clubs
        }



        public SuitEnum Suit { get; set; }
        public ValueEnum Value { get; set; }
        public string imageFile { get; set; }

        public Card(SuitEnum mysuit, ValueEnum myValue)
        {
            Suit = mysuit;
            Value = myValue;
            imageFile = cardImages[new Tuple<SuitEnum, ValueEnum>(Suit, Value)];
        }
        public Card()
        {

        }

        private static Dictionary<Tuple<SuitEnum, ValueEnum>, string> cardImages = new Dictionary<Tuple<SuitEnum, ValueEnum>, string>
{
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Two), "two_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Three), "three_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Four), "four_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Five), "five_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Six), "six_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Seven), "seven_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Eight), "eight_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Nine), "nine_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Ten), "ten_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Jack), "jack_of_clubs2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Queen), "queen_of_clubs2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.King), "king_of_clubs2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Clubs, ValueEnum.Ace), "ace_of_clubs" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Two), "two_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Three), "three_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Four), "four_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Five), "five_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Six), "six_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Seven), "seven_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Eight), "eight_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Nine), "nine_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Ten), "ten_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Jack), "jack_of_diamonds2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Queen), "queen_of_diamonds2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.King), "king_of_diamonds2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Diamonds, ValueEnum.Ace), "ace_of_diamonds" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Two), "two_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Three), "three_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Four), "four_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Five), "five_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Six), "six_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Seven), "seven_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Eight), "eight_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Nine), "nine_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Ten), "ten_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Jack), "jack_of_hearts2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Queen), "queen_of_hearts2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.King), "king_of_hearts2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Hearts, ValueEnum.Ace), "ace_of_hearts" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Two), "two_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Three), "three_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Four), "four_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Five), "five_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Six), "six_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Seven), "seven_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Eight), "eight_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Nine), "nine_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Ten), "ten_of_spades" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Jack), "jack_of_spades2." },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Queen), "queen_of_spades2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.King), "king_of_spades2" },
    { new Tuple<SuitEnum, ValueEnum>(SuitEnum.Spades, ValueEnum.Ace), "ace_of_spades" },

        };

    }
}