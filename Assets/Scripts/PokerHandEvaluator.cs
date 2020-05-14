using System;
using System.Collections.Generic;
using System.Linq;
using Debug = UnityEngine.Debug;

namespace poker
{
    public class PokerHandEvaluator
    {
        private static int _straightCount;
        private static int _straightCountMax;
        private static CardValue _straightCountMaxCardValue;
        private static Dictionary<CardValue, int> _sets;

        private static Rank _rank;

        private static List<Card> _cards;
        private static List<Card> _bestHandCards;

        public static RankedHand EvaluateHand(Card[] cards)
        {
            _straightCount = 1;
            _straightCountMax = 0;
            _rank = Rank.HighCard;
            _straightCountMaxCardValue = (CardValue)0;

            _sets = new Dictionary<CardValue, int>();

            _cards = new List<Card>(cards);
            List<Card> flushCards = new List<Card>();
            List<Card> straightCards = new List<Card>();
            _bestHandCards = new List<Card>();
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
                    (_cards[i].Value == CardValue.King && _cards[i + 1].Value == CardValue.Ace) || // this is king and next is ace
                    (_straightCountMaxCardValue == CardValue.Five && _straightCount == 4 && _cards[i + 1].Value == CardValue.Ace) // highest so far is 5 and straight count is currently 4 and next is Ace
                    )
                {
                    _straightCount++;
                    if (_straightCount > _straightCountMax)
                    {
                        _straightCountMax = _straightCount;
                        straightCards.Add(_cards[i]);
                        while (straightCards.Count > 5) straightCards.RemoveAt(0);
                    }
                    _straightCountMaxCardValue = _cards[i + 1].Value;
                }
                else if (_cards[i].Value != _cards[i + 1].Value) // account for sets within the straight
                {
                    if (_straightCount != 5) straightCards.Clear();
                    _straightCount = 1;
                    _straightCountMaxCardValue = _cards[i + 1].Value;
                }
            }

            List<Card> straightFlushCards = new List<Card>();
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                flushCards = _cards.Where(o => o.Suit == suit).OrderBy(p => p.Value).ToList();

                if (flushCards.Count >= 5)
                {
                    int straightFlushCount = 1;
                    CardValue straightFlushCountMaxCardValue = 0;
                    int straightFlushCountMax = 0;

                    for (int i = 0; i < flushCards.Count - 1; i++)
                    {
                        if ((int)flushCards[i].Value == (int)flushCards[i + 1].Value - 1 ||
                            (flushCards[i].Value == CardValue.King && flushCards[i + 1].Value == CardValue.Ace) ||
                            (straightFlushCountMaxCardValue == CardValue.Five && straightFlushCount == 4 && flushCards[i + 1].Value == CardValue.Ace)
                            )
                        {
                            straightFlushCount++;

                            straightFlushCards.Add(_cards[i]);
                            while (straightFlushCards.Count > 5) straightFlushCards.RemoveAt(0);
                            if (straightFlushCount > straightFlushCountMax) straightFlushCountMax = straightFlushCount;
                            straightFlushCountMaxCardValue = flushCards[i + 1].Value;
                        }
                    }

                    if (straightFlushCountMax >= 5)
                    {
                        if (straightFlushCountMaxCardValue == CardValue.Ace)
                        {
                            // Royal Flush
                            _bestHandCards = straightFlushCards.ToList();
                            _rank = Rank.RoyalFlush;
                        }
                        else
                        {
                            // Straight Flush
                            _bestHandCards = straightFlushCards.ToList();
                            _rank = Rank.StraightFlush;
                        }
                    }
                    else
                    {
                        // Flush
                        if ((int)_rank < (int)Rank.Flush)
                        {
                            _bestHandCards = flushCards.Skip(flushCards.Count - 5).ToList();
                            _rank = Rank.Flush;
                        }
                    }
                }
            }

            // Straights
            if (_straightCountMax >= 5)
            {
                // Straight
                if ((int)_rank < (int)Rank.Straight)
                {
                    _bestHandCards = straightCards.Skip(straightCards.Count - 5).ToList();
                    _rank = Rank.Straight;
                }
            }

            // Sets
            int numberOfPairs = 0;
            int numberOfTripples = 0;
            List<Card[]> pairs = new List<Card[]>();
            List<Card[]> tripples = new List<Card[]>();
            Card[] bestPair = null;
            Card[] bestTripple = null;

            foreach (KeyValuePair<CardValue, int> set in _sets)
            {
                if (set.Value == 4)
                {
                    // four of a kind
                    if ((int)_rank < (int)Rank.FourOfAKind)
                    {
                        _bestHandCards = _cards.Where(o=>o.Value == set.Key).ToList();
                        _rank = Rank.FourOfAKind;
                    }
                }
                else if (set.Value == 3)
                {
                    // three of a kind
                    numberOfTripples++;
                    tripples.Add(_cards.Where(o => o.Value == set.Key).ToArray());
                    if ((int)_rank < (int)Rank.ThreeOfAKind)
                    {
                        _bestHandCards = _cards.Where(o => o.Value == set.Key).ToList();
                        _rank = Rank.ThreeOfAKind;
                    }
                }
                else if (set.Value == 2)
                {
                    // pair
                    numberOfPairs++;
                    pairs.Add(_cards.Where(o => o.Value == set.Key).ToArray());
                    if ((int)_rank < (int)Rank.Pair)
                    {
                        _bestHandCards = _cards.Where(o => o.Value == set.Key).ToList();
                        _rank = Rank.Pair;
                    }
                }
            }

            if (pairs.Count == 1) bestPair = pairs[0];
            else
            {
                foreach (Card[] pair in pairs)
                {
                    if (bestPair == null || pair[0].Value > bestPair[0].Value) bestPair = pair;
                }
            }
            if (tripples.Count == 1) bestTripple = tripples[0];
            else
            {
                foreach (Card[] tripple in tripples)
                {
                    if (bestTripple == null || tripple[0].Value > bestTripple[0].Value) bestTripple = tripple;
                }
            }

            // full house
            if (numberOfTripples >= 1 && numberOfPairs >= 1)
            {
                
                if ((int)_rank < (int)Rank.FullHouse)
                {
                    _bestHandCards = bestPair.Concat(bestTripple).ToList();
                    _rank = Rank.FullHouse;
                }
            }
            // two pair

            if (numberOfPairs >= 2)
            {
                Card[] secondBestPair = null;
                foreach (Card[] pair in pairs)
                {
                    if (secondBestPair == null && pair != bestPair) secondBestPair = pair;
                    else if (pair[0].Value > secondBestPair[0].Value && pair != bestPair) secondBestPair = pair;
                }
                if ((int)_rank < (int)Rank.TwoPair)
                {
                    _bestHandCards = bestPair.Concat(secondBestPair).ToList();
                    _rank = Rank.TwoPair;
                }
            }

            // high card
            if ((int)_rank <= (int)Rank.HighCard)
            {
                _bestHandCards = new List<Card>() { _cards.Last() };
                _rank = Rank.HighCard;
            }

            // get the kickers
            List<Card> otherCards = new List<Card>();
            foreach(Card card in _cards)
                if (!_bestHandCards.Contains(card)) otherCards.Add(card);
            otherCards = otherCards.OrderByDescending(o => o.Value).ToList();
            while (otherCards.Count > 5 - _bestHandCards.Count) otherCards.RemoveAt(otherCards.Count-1);

            Card[] returnCards = _bestHandCards.OrderBy(o=>o.Value).ToArray();
            Card[] returnKickers = otherCards.ToArray();
            
            _sets.Clear();
            _cards.Clear();
            _bestHandCards.Clear();

            return new RankedHand(_rank, returnCards, returnKickers);

        }

        public static int[] EvaluateWinningHands(Card[][] hands)
        {
            List<RankedHand> rankedHands = new List<RankedHand>();
            foreach(Card[] hand in hands)
            {
                RankedHand rankedHand = EvaluateHand(hand);
                Debug.Log(rankedHand.Rank);
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