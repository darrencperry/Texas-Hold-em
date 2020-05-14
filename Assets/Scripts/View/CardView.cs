using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace poker.view
{
    [RequireComponent(typeof(Image))]
    public class CardView : MonoBehaviour
    {
        [SerializeField]
        private CardValue _value;
        [SerializeField]
        private CardSuit _suit;

        private Image _imageCached;
        private Image _image { get { if (_imageCached == null) _imageCached = GetComponent<Image>(); return _imageCached; } }

        private void OnValidate() => UpdateImage();
        private void OnEnable() => UpdateImage();

        public bool HighlightBlue
        {
            set
            {
                _imageCached.color = value ? Color.cyan : Color.white;
            }
        }
        public bool HighlightGreen
        {
            set
            {
                _imageCached.color = value ? Color.green : Color.white;
            }
        }
        public void SetValue(CardValue value)
        {
            _value = value;
            UpdateImage();
        }
        public void SetSuit(CardSuit suit)
        {
            _suit = suit;
            UpdateImage();
        }

        public void SetSuitAndValue(Card card)
        {
            _value = card.Value;
            _suit = card.Suit;
            UpdateImage();
        }

        private void UpdateImage()
        {
            int val = (int)_value;
            int suit = (int)_suit;
            int numVals = Enum.GetValues(typeof(CardValue)).Length;
            int index = val + (suit * numVals);
                
            _image.sprite = Resources.LoadAll<Sprite>("Images/Deck")[index];
        }
        override public string ToString()
        {
            return $"{_value} of {_suit}";
        }
    }
}