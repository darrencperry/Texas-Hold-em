using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using poker;
using System.Linq;
using System;

namespace Tests
{
    public class poker_hand_evaluator
    {
        private PokerHandEvaluator _pokerHandEvaluator = new PokerHandEvaluator();
        // Royal FLush
        [Test]
        public void poker_hand_evaluator_test_royal_flush()
        {
            RankedHand rankedHand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });

            Assert.AreEqual(Rank.RoyalFlush, rankedHand1.Rank);

            Assert.AreEqual(Rank.RoyalFlush, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Clubs),
            }).Rank);
        }

        // Straight Flush
        [Test]
        public void poker_hand_evaluator_test_straight_flush()
        {
            Assert.AreEqual(Rank.StraightFlush, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            }).Rank);
            Assert.AreEqual(Rank.StraightFlush, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Diamonds),
            }).Rank);
            Assert.AreEqual(Rank.StraightFlush, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Five, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Diamonds),
            }).Rank);
        }

        // Four of a Kind
        [Test]
        public void poker_hand_evaluator_test_four_of_a_kind()
        {
            RankedHand hand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.FourOfAKind, hand1.Rank);
            Assert.AreEqual(new Card(CardValue.King, CardSuit.Clubs).ToString(), new List<Card>(hand1.Kickers).First().ToString());
            Assert.AreEqual(1, hand1.Kickers.Length);

            RankedHand hand2 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.Five, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Six, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.FourOfAKind, hand2.Rank);
            Assert.AreEqual(CardValue.Six, new List<Card>(hand2.Kickers).First().Value);
            Assert.AreEqual(4, hand2.RankCards.Length);
            Assert.AreEqual(1, hand2.Kickers.Length);
        }

        // Full House
        [Test]
        public void poker_hand_evaluator_test_full_house()
        {
            RankedHand hand = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.FullHouse, hand.Rank);
            Assert.AreEqual(5, hand.RankCards.Length);
            Assert.AreEqual(5, hand.Hand.Length);
        }

        // Flush
        [Test]
        public void poker_hand_evaluator_test_flush()
        {
            Assert.AreEqual(Rank.Flush, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            }).Rank);

            // has a straight as well but should return flush
            Assert.AreEqual(Rank.Flush, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Six, CardSuit.Diamonds),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            }).Rank);
        }

        // Straight
        [Test]
        public void poker_hand_evaluator_test_straight()
        {
            Assert.AreEqual(Rank.Straight, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Hearts),
                new Card(CardValue.Queen, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            }).Rank);

            Assert.AreEqual(Rank.Straight, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Hearts),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Diamonds),
            }).Rank);

            Assert.AreEqual(Rank.Straight, _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Hearts),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Diamonds),
            }).Rank);
            
            RankedHand hand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Hearts),
                new Card(CardValue.Queen, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Diamonds),
            });
            
            Assert.AreEqual(Rank.Straight, hand1.Rank);
            Assert.AreEqual(5, hand1.RankCards.Length);


            RankedHand hand2 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Hearts),
                new Card(CardValue.Two, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.Straight, hand2.Rank);
            Assert.AreEqual(5, hand2.RankCards.Length);

            RankedHand hand3 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Hearts),
                new Card(CardValue.Two, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.Straight, hand3.Rank);
            Assert.AreEqual(5, hand3.RankCards.Length);
        }

        // Threee of a kind
        [Test]
        public void poker_hand_evaluator_test_three_of_a_kind()
        {
            RankedHand hand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.ThreeOfAKind, hand1.Rank);
            Assert.AreEqual(new Card(CardValue.Jack, CardSuit.Clubs).ToString(), new List<Card>(hand1.Kickers).First().ToString());
            Assert.AreEqual(2, hand1.Kickers.Length);

            RankedHand hand2 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Seven, CardSuit.Clubs),
                new Card(CardValue.Seven, CardSuit.Hearts),
                new Card(CardValue.Seven, CardSuit.Diamonds),
                new Card(CardValue.Five, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.ThreeOfAKind, hand2.Rank);
            Assert.AreEqual(new Card(CardValue.Five, CardSuit.Clubs).ToString(), new List<Card>(hand2.Kickers).First().ToString());
            Assert.AreEqual(2, hand2.Kickers.Length);
        }

        // Two pair
        [Test]
        public void poker_hand_evaluator_test_two_pair()
        {
            RankedHand hand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Two, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.TwoPair, hand1.Rank);
            Assert.AreEqual(new Card(CardValue.Jack, CardSuit.Clubs).ToString(), new List<Card>(hand1.Kickers).First().ToString());
            Assert.AreEqual(1, hand1.Kickers.Length);

            RankedHand hand2 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Seven, CardSuit.Hearts),
                new Card(CardValue.Seven, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.TwoPair, hand2.Rank);
            Assert.AreEqual(CardValue.Seven, new List<Card>(hand2.Kickers).First().Value);
            Assert.AreEqual(1, hand2.Kickers.Length);
        }

        // Pair
        [Test]
        public void poker_hand_evaluator_test_pair()
        {
            RankedHand hand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.Pair, hand1.Rank);
            Assert.AreEqual(new Card(CardValue.Ace, CardSuit.Diamonds).ToString(), new List<Card>(hand1.Kickers).First().ToString());
            Assert.AreEqual(3, hand1.Kickers.Length);
        }

        // High Card
        [Test]
        public void poker_hand_evaluator_test_high_card()
        {
            RankedHand hand1 = _pokerHandEvaluator.EvaluateHand(new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Five, CardSuit.Diamonds),
            });
            Assert.AreEqual(Rank.HighCard, hand1.Rank);
            Assert.AreEqual(new Card(CardValue.Jack, CardSuit.Clubs).ToString(), hand1.Kickers[0].ToString());
            Assert.AreEqual(new Card(CardValue.Ten, CardSuit.Clubs).ToString(), hand1.Kickers[1].ToString());
            Assert.AreEqual(new Card(CardValue.Eight, CardSuit.Clubs).ToString(), hand1.Kickers[2].ToString());
            Assert.AreEqual(new Card(CardValue.Five, CardSuit.Diamonds).ToString(), hand1.Kickers[3].ToString());
            Assert.AreEqual(4, hand1.Kickers.Length);
        }


        // Winning Hands
        [Test]
        public void poker_hand_evaluator_test_winning_hand_high_card_v_high_card()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Ten, CardSuit.Spades)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_high_card_v_high_card_one_kicker()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Ten, CardSuit.Spades)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_high_card_v_high_card_two_kickers()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.King, CardSuit.Spades),
                new Card(CardValue.Ten, CardSuit.Spades)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_high_card_v_high_card_three_kickers()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Seven, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.King, CardSuit.Spades),
                new Card(CardValue.Ten, CardSuit.Spades)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Ten, CardSuit.Hearts)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3 });
            int[] expected = { 2, 0, 1 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_pair_vs_pair_b()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Ten, CardSuit.Spades)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Hearts),
                new Card(CardValue.Ace, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_highcard_v_pair()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Ten, CardSuit.Spades)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_highcard_v_pair_v_set()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Jack, CardSuit.Clubs)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Queen, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3 });
            int[] expected = { 2, 0, 1 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_highcard_v_pair_v_set_v_four()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.King, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Jack, CardSuit.Clubs)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Queen, CardSuit.Clubs)
            };
            Card[] hand4 = new Card[]
            {
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Two, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Diamonds),
                new Card(CardValue.Two, CardSuit.Spades),
                new Card(CardValue.Ace, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3, hand4 });
            int[] expected = { 3, 1, 2, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_pair_v_pair_v_pair_v_pair()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Two, CardSuit.Clubs)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Queen, CardSuit.Clubs)
            };
            Card[] hand4 = new Card[]
            {
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Two, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Diamonds),
                new Card(CardValue.Five, CardSuit.Spades),
                new Card(CardValue.Ace, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3, hand4 });
            int[] expected = { 2, 0, 1, 3 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_pair_v_pair_v_pair_v_set()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Clubs)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Two, CardSuit.Clubs)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Diamonds),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Queen, CardSuit.Clubs)
            };
            Card[] hand4 = new Card[]
            {
                new Card(CardValue.Two, CardSuit.Hearts),
                new Card(CardValue.Two, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Diamonds),
                new Card(CardValue.Five, CardSuit.Spades),
                new Card(CardValue.Ace, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3, hand4 });
            int[] expected = { 3, 1, 2, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_pair_v_pair_tied()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Two, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 0, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }


        [Test]
        public void poker_hand_evaluator_test_winning_hand_pair_v_pair_tied_v_high()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Three, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Hearts)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Three, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Diamonds),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Two, CardSuit.Clubs)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Ace, CardSuit.Hearts),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Ten, CardSuit.Diamonds),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Three, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3 });
            int[] expected = { 0, 0, 1 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_full_house_tied()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Eight, CardSuit.Diamonds)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Eight, CardSuit.Diamonds)
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Eight, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Eight, CardSuit.Diamonds)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3 });
            int[] expected = { 0, 0, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_straight_v_straight_v_straight()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Ten, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Seven, CardSuit.Diamonds)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Hearts),
                new Card(CardValue.Ten, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Queen, CardSuit.Diamonds)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_straight_v_flush_v_flush()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.Seven, CardSuit.Clubs)
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Clubs),
                new Card(CardValue.Two, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Clubs)
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2 });
            int[] expected = { 1, 0 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_set_v_set_v_full_house()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Two, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Diamonds),
                new Card(CardValue.Four, CardSuit.Spades),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Ace, CardSuit.Spades),
                new Card(CardValue.Nine, CardSuit.Spades),
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Two, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Diamonds),
                new Card(CardValue.Four, CardSuit.Spades),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.King, CardSuit.Hearts),
                new Card(CardValue.Six, CardSuit.Clubs),
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Two, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Clubs),
                new Card(CardValue.Four, CardSuit.Diamonds),
                new Card(CardValue.Four, CardSuit.Spades),
                new Card(CardValue.King, CardSuit.Clubs),
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Ace, CardSuit.Hearts),
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3 });
            int[] expected = { 1, 0, 1 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }

        [Test]
        public void poker_hand_evaluator_test_winning_hand_straight_v_three_v_two_v_pair()
        {
            Card[] hand1 = new Card[]
            {
                new Card(CardValue.Five, CardSuit.Spades),
                new Card(CardValue.Eight, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Six, CardSuit.Diamonds),
                new Card(CardValue.Seven, CardSuit.Spades),
            };
            Card[] hand2 = new Card[]
            {
                new Card(CardValue.Six, CardSuit.Spades),
                new Card(CardValue.Six, CardSuit.Hearts),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Six, CardSuit.Diamonds),
                new Card(CardValue.Seven, CardSuit.Spades),
            };
            Card[] hand3 = new Card[]
            {
                new Card(CardValue.Jack, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Spades),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Six, CardSuit.Diamonds),
                new Card(CardValue.Seven, CardSuit.Spades),
            };
            Card[] hand4 = new Card[]
            {
                new Card(CardValue.Five, CardSuit.Diamonds),
                new Card(CardValue.Six, CardSuit.Clubs),
                new Card(CardValue.Nine, CardSuit.Spades),
                new Card(CardValue.Four, CardSuit.Hearts),
                new Card(CardValue.Jack, CardSuit.Clubs),
                new Card(CardValue.Six, CardSuit.Diamonds),
                new Card(CardValue.Seven, CardSuit.Spades),
            };

            int[] handOrder = _pokerHandEvaluator.EvaluateWinningHands(new Card[][] { hand1, hand2, hand3, hand4 });
            int[] expected = { 0,1,2,3 };
            for (int i = 0; i < handOrder.Length; i++)
            {
                Assert.AreEqual(handOrder[i], expected[i]);
            }
        }
    }
}
