using System.Diagnostics;
using System.Linq;

namespace poker
{
    public class RankedHand
    {
        public Rank Rank;
        public Card[] RankCards;
        public Card[] Kickers;

        public RankedHand(Rank rank, Card[] cards, Card[] kickers = null)
        {
            Rank = rank;
            RankCards = cards;
            Kickers = kickers;
        }

        public Card HighCard { get { return RankCards.OrderBy(o => o.Value).Last(); } }

        public Card[] Hand { get { return RankCards.Concat(Kickers).ToArray(); } }
    }
}