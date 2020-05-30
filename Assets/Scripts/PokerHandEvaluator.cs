using System;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace poker
{
    public class PokerHandEvaluator
    {
        private int _straightCount;
        private int _straightCountMax;
        private CardValue _straightCountMaxCardValue;
        private int _straightFlushCount;
        private int _straightFlushCountMax;
        private CardValue _straightFlushCountMaxCardValue;
        private int _numberOfPairs;
        private int _numberOfTripples;
        private Rank _calculatedRank;
        private Dictionary<CardValue, int> _sets;
        private List<Card> _cards;
        private List<Card> _bestHandCards;
        private List<Card> _straightFlushCards;
        private List<Card> _flushCards;
        private List<Card> _straightCards;
        private List<Card[]> _pairs;
        private List<Card[]> _tripples;
        private Card[] _bestPair;
        private Card[] _bestTripple;

        public RankedHand EvaluateHand(Card[] cards)
        {
            _straightCount = 1;
            _straightCountMax = 0;
            _straightCountMaxCardValue = (CardValue)0;
            _straightFlushCount = 1;
            _straightFlushCountMax = 0;
            _straightFlushCountMaxCardValue = (CardValue)0;
            _numberOfPairs = 0;
            _numberOfTripples = 0;
            _calculatedRank = Rank.HighCard;
            _sets = new Dictionary<CardValue, int>();
            _cards = new List<Card>(cards);
            _bestHandCards = new List<Card>();
            _straightFlushCards = new List<Card>();
            _flushCards = new List<Card>();
            _straightCards = new List<Card>();
            _pairs = new List<Card[]>();
            _tripples = new List<Card[]>();
            _bestPair = null;
            _bestTripple = null;

            // lowest to highest card value
            _cards = _cards.OrderBy(o => o.Value).ToList();

            for (int i = 0; i < _cards.Count - 1; i++)
            {
                // sets
                if (_cards[i].Value == _cards[i + 1].Value)
                {
                    if (!_sets.ContainsKey(_cards[i].Value))
                        _sets[_cards[i].Value] = 1;

                    _sets[_cards[i].Value]++;
                }

                // straights
                if ((int)_cards[i].Value == (int)_cards[i + 1].Value - 1 || // next card is next highest in value
                    (_cards[i].Value == CardValue.King && _cards[i + 1].Value == CardValue.Ace) // this is king and next is ace
                    )
                {
                    _straightCount++;
                    if (_straightCount > _straightCountMax)
                    {
                        _straightCountMax = _straightCount;
                        _straightCards.Add(_cards[i]);
                        if(i == _cards.Count - 1) _straightCards.Add(_cards[i+1]);
                        _straightCountMaxCardValue = _cards[i + 1].Value;
                    }
                }
                else if (_cards[i].Value != _cards[i + 1].Value) // account for sets within the straight
                {
                    if (_straightCount != 5) _straightCards.Clear();
                    _straightCount = 1;
                }

                // highest so far is 5 (2,3,4,5) and straight count is currently 4 and next is Ace)
                if (_straightCountMaxCardValue == CardValue.Five && _straightCountMax == 4 && _cards[i + 1].Value == CardValue.Ace) 
                {
                    _straightCount = _straightCountMax = 5;
                    _straightCards.Clear();
                    _straightCards.Add(_cards.Find(o => o.Value == CardValue.Two));
                    _straightCards.Add(_cards.Find(o => o.Value == CardValue.Three));
                    _straightCards.Add(_cards.Find(o => o.Value == CardValue.Four));
                    _straightCards.Add(_cards.Find(o => o.Value == CardValue.Five));
                    _straightCards.Add(_cards[i+1]);
                }

            }

            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                _flushCards = _cards.Where(o => o.Suit == suit).OrderBy(p => p.Value).ToList();

                if (_flushCards.Count >= 5)
                {
                    for (int i = 0; i < _flushCards.Count - 1; i++)
                    {
                        if ((int)_flushCards[i].Value == (int)_flushCards[i + 1].Value - 1 ||
                            (_flushCards[i].Value == CardValue.King && _flushCards[i + 1].Value == CardValue.Ace)
                            
                            )
                        {
                            _straightFlushCount++;

                            _straightFlushCards.Add(_cards[i]);
                            while (_straightFlushCards.Count > 5) _straightFlushCards.RemoveAt(0);
                            if (_straightFlushCount > _straightFlushCountMax) _straightFlushCountMax = _straightFlushCount;
                            _straightFlushCountMaxCardValue = _flushCards[i + 1].Value;
                        }

                        if (_straightFlushCountMaxCardValue == CardValue.Five && _straightFlushCountMax == 4 && _flushCards[i + 1].Value == CardValue.Ace)
                        {

                            _straightFlushCount = _straightFlushCountMax = 5;
                            _straightFlushCards.Add(_cards[i]);
                        }
                    }

                    if (_straightFlushCountMax >= 5)
                    {
                        if (_straightFlushCountMaxCardValue == CardValue.Ace)
                        {
                            // Royal Flush
                            _bestHandCards = _straightFlushCards.Skip(_straightFlushCards.Count - 5).ToList();
                            _calculatedRank = Rank.RoyalFlush;
                        }
                        else
                        {
                            // Straight Flush
                            _bestHandCards = _straightFlushCards.Skip(_straightFlushCards.Count - 5).ToList();
                            _calculatedRank = Rank.StraightFlush;
                        }
                    }
                    else
                    {
                        // Flush
                        if ((int)_calculatedRank < (int)Rank.Flush)
                        {
                            _bestHandCards = _flushCards.Skip(_flushCards.Count - 5).ToList();
                            _calculatedRank = Rank.Flush;
                        }
                    }
                }
            }

            // Straight
            if (_straightCountMax >= 5)
            {
                if ((int)_calculatedRank < (int)Rank.Straight)
                {
                    _bestHandCards = _straightCards.Skip(_straightCards.Count - 5).ToList();
                    _calculatedRank = Rank.Straight;
                }
            }

            // Sets
            foreach (KeyValuePair<CardValue, int> set in _sets)
            {
                if (set.Value == 4)
                {
                    // four of a kind
                    if ((int)_calculatedRank < (int)Rank.FourOfAKind)
                    {
                        _bestHandCards = _cards.Where(o=>o.Value == set.Key).ToList();
                        _calculatedRank = Rank.FourOfAKind;
                    }
                }
                else if (set.Value == 3)
                {
                    // three of a kind
                    _numberOfTripples++;
                    _tripples.Add(_cards.Where(o => o.Value == set.Key).ToArray());
                    if ((int)_calculatedRank < (int)Rank.ThreeOfAKind)
                    {
                        _bestHandCards = _cards.Where(o => o.Value == set.Key).ToList();
                        _calculatedRank = Rank.ThreeOfAKind;
                    }
                }
                else if (set.Value == 2)
                {
                    // pair
                    _numberOfPairs++;
                    _pairs.Add(_cards.Where(o => o.Value == set.Key).ToArray());
                    if ((int)_calculatedRank < (int)Rank.Pair)
                    {
                        _bestHandCards = _cards.Where(o => o.Value == set.Key).ToList();
                        _calculatedRank = Rank.Pair;
                    }
                }
            }

            if (_pairs.Count == 1) _bestPair = _pairs[0];
            else
            {
                foreach (Card[] pair in _pairs)
                {
                    if (_bestPair == null || pair[0].Value > _bestPair[0].Value) _bestPair = pair;
                }
            }

            if (_tripples.Count == 1) _bestTripple = _tripples[0];
            else if(_tripples.Count != 0)
            {
                foreach (Card[] tripple in _tripples)
                {
                    if (_bestTripple == null || tripple[0].Value > _bestTripple[0].Value) _bestTripple = tripple;
                }
                _bestHandCards = _bestTripple.ToList();
            }

            // full house
            if (_numberOfTripples >= 1 && _numberOfPairs >= 1)
            {
                
                if ((int)_calculatedRank < (int)Rank.FullHouse)
                {
                    _bestHandCards = _bestPair.Concat(_bestTripple).ToList();
                    _calculatedRank = Rank.FullHouse;
                }
            }

            // two pair
            if (_numberOfPairs >= 2)
            {
                Card[] secondBestPair = null;
                foreach (Card[] pair in _pairs)
                {
                    if (secondBestPair == null && pair != _bestPair) secondBestPair = pair;
                    else if (pair[0].Value > secondBestPair[0].Value && pair != _bestPair) secondBestPair = pair;
                }
                if ((int)_calculatedRank < (int)Rank.TwoPair)
                {
                    _bestHandCards = _bestPair.Concat(secondBestPair).ToList();
                    _calculatedRank = Rank.TwoPair;
                }
            }

            // high card
            if ((int)_calculatedRank <= (int)Rank.HighCard)
            {
                _bestHandCards = new List<Card>() { _cards.Last() };
                _calculatedRank = Rank.HighCard;
            }

            // get the kickers
            List<Card> otherCards = new List<Card>();
            foreach(Card card in _cards)
                if (!_bestHandCards.Contains(card)) otherCards.Add(card);
            otherCards = otherCards.OrderByDescending(o => o.Value).ToList();
            while (otherCards.Count > 5 - _bestHandCards.Count) otherCards.RemoveAt(otherCards.Count-1);

            Card[] returnCards = _bestHandCards.OrderBy(o=>o.Value).ToArray();
            Card[] returnKickers = otherCards.ToArray();

            _sets               .Clear();
            _cards              .Clear();
            _bestHandCards      .Clear();
            _straightFlushCards .Clear();
            _flushCards         .Clear();
            _straightCards      .Clear();
            _pairs              .Clear();
            _tripples           .Clear();

            return new RankedHand(_calculatedRank, returnCards, returnKickers);
        }

        public int[] EvaluateWinningHands(Card[][] hands)
        {
            List<RankedHand> rankedHands = new List<RankedHand>();
            foreach(Card[] hand in hands)
            {
                RankedHand rankedHand = EvaluateHand(hand);
                rankedHands.Add(rankedHand);
            }

            List<int> scores = rankedHands.Select(o => (int)o.Rank).ToList();

            // sort by rank first and then by up to 5 cards
            List<RankedHand> sortedRankedHands = rankedHands.OrderByDescending(o => o.Rank)
                .ThenByDescending(p => p.Hand[0].Value)
                .ThenByDescending(p => p.Hand[1].Value)
                .ThenByDescending(p => p.Hand[2].Value)
                .ThenByDescending(p => p.Hand[3].Value)
                .ThenByDescending(p => p.Hand[4].Value)
                .ToList();

            int[] sortedIndex = new int[hands.Length];

            // tied hands
            int rank = 0;
            int[] ranks = new int[hands.Length];
            for (int i = 0; i < hands.Length; i++)
            {
                ranks[i] = rank;
                if (i > hands.Length-2 ||
                    sortedRankedHands[i].Rank != sortedRankedHands[i + 1].Rank ||
                    sortedRankedHands[i].Hand[0].Value != sortedRankedHands[i + 1].Hand[0].Value ||
                    sortedRankedHands[i].Hand[1].Value != sortedRankedHands[i + 1].Hand[1].Value ||
                    sortedRankedHands[i].Hand[2].Value != sortedRankedHands[i + 1].Hand[2].Value ||
                    sortedRankedHands[i].Hand[3].Value != sortedRankedHands[i + 1].Hand[3].Value ||
                    sortedRankedHands[i].Hand[4].Value != sortedRankedHands[i + 1].Hand[4].Value
                    )
                    rank++;
            }

            for (int i = 0; i < hands.Length; i++)
            {
                sortedIndex[i] = ranks[sortedRankedHands.IndexOf(rankedHands[i])];
            }

            return sortedIndex;
        }
    }
}