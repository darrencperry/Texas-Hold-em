namespace poker
{
    public class Card
    {
        public CardValue Value;
        public CardSuit Suit;

        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }

        override public string ToString()
        {
            return $"{Value} of {Suit}";
        }
    }
}