using Common;

namespace Year2023;

internal class Day07 : Day
{
    protected override int TestSolutionOne { get; set; } = 6440;
    protected override int TestSolutionTwo { get; set; } = 5905;

    protected override long SolveOne(string input)
    {
        return Solve(input, 1);
    }

    protected override long SolveTwo(string input)
    {
        return Solve(input, 2);
    }

    private static int Solve(string input, int part)
    {
        var output = 0;
        var hands = new List<Hand>();

        using var reader = new StreamReader(input);
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine() ?? throw new NullReferenceException();
            var split = line.Split(' ');
            hands.Add(new(split[0], int.Parse(split[1]), part));
        }

        hands.Sort();

        for (int i = 0; i < hands.Count; i++)
        {
            output += hands[i].Bid * (i + 1);
        }

        return output;
    }

    private class Hand : IComparable<Hand>
    {
        private string Cards { get; set; }
        private HandType Type { get; set; }
        public int Bid { get; set; }
        private int Part { get; set; }

        public Hand(string cards, int bid, int part)
        {
            Cards = cards;
            Bid = bid;
            Part = part;

            var cardsSorted = cards.ToList();
            cardsSorted.Sort();
            cardsSorted = cardsSorted.OrderByDescending(ch => cards.Count(letter => letter == ch)).ToList();
            if (Part == 2) cardsSorted = cardsSorted.OrderByDescending(ch => ch != 'J').ToList();

            if (cardsSorted[0] == cardsSorted[4])       Type = HandType.FiveCards;
            else if (cardsSorted[0] == cardsSorted[3])  Type = HandType.FourCards;
            else if (cardsSorted[0] == cardsSorted[2]
                  && cardsSorted[3] == cardsSorted[4])  Type = HandType.FullHouse;
            else if (cardsSorted[0] == cardsSorted[2])  Type = HandType.ThreeCards;
            else if (cardsSorted[0] == cardsSorted[1]
                  && cardsSorted[2] == cardsSorted[3])  Type = HandType.TwoPairs;
            else if (cardsSorted[0] == cardsSorted[1])  Type = HandType.OnePair;
            else                                        Type = HandType.HighCard;

            if (Part == 2) Upgrade(Cards.Count(card => card == 'J'));
        }

        public int CompareTo(Hand? other)
        {
            if (other is null) return 1;

            if (this.Type != other.Type) return this.Type > other.Type ? 1 : -1;

            for (int i = 0; i < this.Cards.Length; i++)
            {
                if (this.Cards[i] != other.Cards[i])
                {
                    if (Part == 1)
                    {
                        return ValuesOne.IndexOf(this.Cards[i]) > ValuesOne.IndexOf(other.Cards[i]) ? 1 : -1;
                    }
                    else
                    {
                        return ValuesTwo.IndexOf(this.Cards[i]) > ValuesTwo.IndexOf(other.Cards[i]) ? 1 : -1;
                    }
                }
            }

            return 0;
        }

        public override string ToString()
        {
            return $"{Cards} - {Type}";
        }

        private static List<char> ValuesOne { get; set; } = new List<char>()
        {
            '2','3','4','5','6','7','8','9','T','J','Q','K','A'
        };

        private static List<char> ValuesTwo { get; set; } = new List<char>()
        {
            'J','2','3','4','5','6','7','8','9','T','Q','K','A'
        };

        private enum HandType
        {
            HighCard,
            OnePair,
            TwoPairs,
            ThreeCards,
            FullHouse,
            FourCards,
            FiveCards
        }
        private void Upgrade(int times)
        {
            for (int i = 0; i < times; i++)
            {
                switch (this.Type)
                {
                    case HandType.HighCard:
                        Type = HandType.OnePair; break;
                    case HandType.OnePair:
                        Type = HandType.ThreeCards; break;
                    case HandType.TwoPairs:
                        Type = HandType.FullHouse; break;
                    case HandType.ThreeCards:
                        Type = HandType.FourCards; break;
                    case HandType.FullHouse:
                        Type = HandType.FourCards; break;
                    case HandType.FourCards:
                        Type = HandType.FiveCards; break;
                    default:
                        break;
                }
            }
        }
    }
}
