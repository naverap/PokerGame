using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PokerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerGame
{
    public class Player
    {
        public int Id { get; set; }
        public int Pot { get; set; }
        public int CurrentBet { get; set; }
        public bool HasFolded { get; set; }
        public Card[] PlayerCards;

        public Player(int playerPot, int playerId, bool isPlaying)
        {
            Id = playerId;
            Pot = playerPot;
            CurrentBet = 0;
            HasFolded = isPlaying;
            PlayerCards = new Card[2];
        }

        public void DealNewCards(Card[] cardss)
        {
            for (int i = 0; i < cardss.Length; i++)
            {
                PlayerCards[i] = cardss[i];
            }
        }
    }
}