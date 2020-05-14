using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace poker
{
    public class Deck
    {
        private static Random rng = new Random();
        private List<Card> _cards;
        private int _currentCardIndex;

        public int NumberOfCards => _cards.Count;

        public Deck()
        {
            _cards = new List<Card>();
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    _cards.Add(new Card(value, suit));
                }
            }
        }

        public void Reset()
        {
            _currentCardIndex = 0;
        }

        public void Shuffle()
        {
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }
        }

        public Card Draw()
        {
            if (_currentCardIndex < _cards.Count)
                return _cards[_currentCardIndex++];
            else
                return null;
        }
    }
}