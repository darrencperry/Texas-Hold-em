using poker.view;
using System.Collections.Generic;
using UnityEngine;

namespace poker
{
    public abstract class CardHolder : MonoBehaviour
    {
        protected List<Card> _cards = new List<Card>();
        [SerializeField]
        private Transform _cardContainer;

        protected abstract void OnAddCard();
        protected abstract void OnRemoveAllCards();

        private void Awake()
        {
            if (_cardContainer == null) _cardContainer = transform;
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);
            GameObject cardGO = Instantiate(Resources.Load<GameObject>("Prefabs/Card"));
            cardGO.GetComponent<CardView>().SetSuitAndValue(card);
            cardGO.transform.SetParent(_cardContainer);
            OnAddCard();
        }

        public void RemoveAllCards()
        {
            _cards.Clear();
            Util.RemoveChildrenFromTransform(_cardContainer);
            OnRemoveAllCards();
        }

        public Card[] GetCards()
        {
            return _cards.ToArray();
        }

        public CardView[] GetCardViews()
        {
            return _cardContainer.GetComponentsInChildren<CardView>();
        }

    }
}