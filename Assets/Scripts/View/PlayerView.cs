using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace poker
{
    public class PlayerView : CardHolder
    {
        [SerializeField]
        private Text rankDisplay;
        [SerializeField]
        private Text positionDisplay;

        RankedHand _rankedHand;
        public RankedHand RankedHand
        {
            set
            {
                _rankedHand = value;
                rankDisplay.text = Enum.GetName(typeof(Rank), value.Rank);
            }
            get
            {
                return _rankedHand;
            }
        }
        private int _position;
        public int Position
        {
            set
            {
                _position = value;
                positionDisplay.text = $"Position: {value}";
            }
            get
            {
                return _position;
            }
        }

        public void ToggleDisplayRank()
        {
            if (rankDisplay.color == Color.grey) HideRank();
            else ShowRank();
        }
        public void ShowRank()
        {
            rankDisplay.color = Color.grey;
        }
        public void HideRank()
        {
            rankDisplay.color = Color.clear;
        }

        protected override void OnAddCard()
        {
            ShowRank();
        }

        protected override void OnRemoveAllCards()
        {
            HideRank();
            positionDisplay.text = "";
        }
    }
}