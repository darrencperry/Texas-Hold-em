using System;
using UnityEngine;
using UnityEngine.UI;

namespace poker.view
{
    [RequireComponent(typeof(Image))]
    public class CardView : MonoBehaviour
    {
        public Card Card { get; private set; }
        private Image _imageCached;
        private Image _image { get { if (_imageCached == null) _imageCached = GetComponent<Image>(); return _imageCached; } }

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

        public void SetSuitAndValue(Card card)
        {
            Card = card;
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (Card == null) return;

            int val = (int)Card.Value;
            int suit = (int)Card.Suit;
            int numVals = Enum.GetValues(typeof(CardValue)).Length;
            int index = val + (suit * numVals);
                
            _image.sprite = Resources.LoadAll<Sprite>("Images/Deck")[index];
        }
        override public string ToString()
        {
            return $"CardView: {Card}";
        }
    }
}