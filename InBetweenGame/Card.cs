using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InBetweenGame
{
    public class Card
    {
        public char Rank { get; set; }
        public char Suit { get; set; }

        public Card(char rank, char suit)
        {
            Rank = rank;
            Suit = suit;
        }

        public override string ToString()
        {
            return $"{Rank}-{Suit}";
        }
    }

    public class Deck
    {
        private List<Card> cards;
        private int currentIndex;
        private Random rng;

        private static readonly char[] Ranks = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
        private static readonly char[] Suits = { 'H', 'D', 'C', 'S' };

        public int CardsLeft => cards.Count - currentIndex;

        public Deck(Random random)
        {
            rng = random;
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            cards = new List<Card>();
            foreach (char suit in Suits)
            {
                foreach (char rank in Ranks)
                {
                    cards.Add(new Card(rank, suit));
                }
            }
            currentIndex = 0;
        }

        public void Shuffle()
        {
            currentIndex = 0;

            // Fisher-Yates shuffle algorithm
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                Card temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card GetNextCard()
        {
            if (CardsLeft == 0)
            {
                // Reshuffle when deck is empty
                InitializeDeck();
                Shuffle();
            }

            Card card = cards[currentIndex];
            currentIndex++;
            return card;
        }

        public void Reset()
        {
            InitializeDeck();
        }

        /// <summary>
        /// Gets the current deck state as a string for saving
        /// </summary>
        public string GetDeckState()
        {
            var cardStrings = cards.Select(c => c.ToString()).ToArray();
            return $"{currentIndex}|{string.Join(",", cardStrings)}";
        }

        /// <summary>
        /// Restores the deck state from a saved string
        /// </summary>
        public void SetDeckState(string state)
        {
            try
            {
                var parts = state.Split('|');
                if (parts.Length != 2)
                {
                    InitializeDeck();
                    Shuffle();
                    return;
                }

                if (!int.TryParse(parts[0], out int index))
                {
                    InitializeDeck();
                    Shuffle();
                    return;
                }

                var cardStrings = parts[1].Split(',');
                cards = new List<Card>();

                foreach (var cardStr in cardStrings)
                {
                    if (cardStr.Length >= 3)
                    {
                        char rank = cardStr[0];
                        char suit = cardStr[2];
                        cards.Add(new Card(rank, suit));
                    }
                }

                if (cards.Count != 52)
                {
                    InitializeDeck();
                    Shuffle();
                    return;
                }

                currentIndex = index;
            }
            catch
            {
                InitializeDeck();
                Shuffle();
            }
        }
    }
}