using poker.view;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace poker
{
    public class TexasHoldEm : MonoBehaviour
    {
        private PokerHandEvaluator _pokerHandEvaluator;
        private Deck _deck;
        [SerializeField]
        private List<PlayerView> _players;
        [SerializeField]
        private TableView _table;
        [SerializeField]
        private Transform _playerContainer;
        private int _maxPlayers;

        private void Awake()
        {
            _pokerHandEvaluator = new PokerHandEvaluator();
            _deck = new Deck();
            _players = new List<PlayerView>();
            _maxPlayers = (_deck.NumberOfCards - 5) / 2;
        }
        public void AddPlayer() {
            if (_players.Count == 0 || _players[0].GetCards().Length == 0 && _table.GetCards().Length == 0)
                Debug.Log($"Add Player: {TryAddPlayer()}");
            else
                Debug.Log("No adding players in the middle of a round");
        }
        public void Deal()
        {
            if (_players.Count < 2)
            {
                Debug.LogError("2 Players Minimum");
                return;
            }
            int numTableCards = _table.GetCards().Length;

            if (_players[0].GetCards().Length == 0)
            {
                BeginRound();
                RankPlayerHands();
            }
            else if (numTableCards == 0)
            {
                Flop();
                RankPlayerHands();
                CalculatePlayerPositions();
            }
            else if (numTableCards == 3)
            {
                Turn();
                RankPlayerHands();
                CalculatePlayerPositions();
            }
            else if (numTableCards == 4)
            {
                River();
                RankPlayerHands();
                CalculatePlayerPositions();
                HighlightWinningHand();
            }
            else
            {
                Reset();
                return;
            }

        }

        private void HighlightWinningHand()
        {
            foreach (PlayerView player in _players)
            {
                if (player.Position == 0)
                {
                    foreach (CardView tableCard in player.GetCardViews().Concat(_table.GetCardViews()))
                    {
                        foreach (Card winningHandCard in player.RankedHand.RankCards)
                        {
                            if (tableCard.Card.ToString() == winningHandCard.ToString()) tableCard.HighlightGreen = true;
                        }
                        foreach (Card winningHandCard in player.RankedHand.Kickers)
                        {
                            if (tableCard.Card.ToString() == winningHandCard.ToString()) tableCard.HighlightBlue = true;
                        }
                    }
                }
            }
        }

        private void RankPlayerHands()
        {
            foreach (PlayerView player in _players)
            {
                //player.HideRank();
                player.RankedHand = _pokerHandEvaluator.EvaluateHand(_table.GetCards().Concat(player.GetCards()).ToArray());
            }
        }

        private bool TryAddPlayer()
        {
            if (_players.Count < _maxPlayers)
            {
                GameObject playerGO = Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
                _players.Add(playerGO.GetComponent<PlayerView>());
                playerGO.transform.SetParent(_playerContainer);
                return true;
            }
            else return false;
        }

        private void BeginRound()
        {
            _deck.Shuffle();

            foreach (PlayerView player in _players)
            {
                for (int i = 0; i < 2; i++)
                    player.AddCard(_deck.Draw());
            }
        }

        private void Flop()
        {
            for (int i = 0; i < 3; i++)
                _table.AddCard(_deck.Draw());
        }

        private void Turn()
        {
            _table.AddCard(_deck.Draw());
        }

        private void River()
        {
            _table.AddCard(_deck.Draw());

        }

        private void CalculatePlayerPositions()
        {
            int j = 0;
            foreach (int i in _pokerHandEvaluator.EvaluateWinningHands(_players.Select(o => o.RankedHand.Hand).ToArray()))
            {
                _players[j].Position = i;
                j++;
            }
        }

        private void Reset()
        {
            _table.RemoveAllCards();
            foreach (PlayerView player in _players)
                player.RemoveAllCards();
            _deck.Reset();
        }

    }
}