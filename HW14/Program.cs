using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW14
{
    class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
        }
    }

    class Player
    {
        public string Name { get; set; }
        public List<Card> Hand { get; set; }

        public Player(string name)
        {
            Name = name;
            Hand = new List<Card>();
        }

        public void DisplayHand()
        {
            Console.WriteLine($"{Name}'s hand:");
            foreach (var card in Hand)
            {
                Console.WriteLine($"{card.Rank} of {card.Suit}");
            }
            Console.WriteLine();
        }
    }

    class Game<T>
        where T : IComparable<T>
    {
        private List<Player> players;
        private List<Card> deck;

        public Game(List<string> playerNames)
        {
            if (playerNames.Count < 2)
            {
                throw new ArgumentException("At least 2 players are required for the game.");
            }

            players = playerNames.Select(name => new Player(name)).ToList();
            InitializeDeck();
            ShuffleDeck();
            DealCards();
        }

        private void InitializeDeck()
        {
            deck = new List<Card>();
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    deck.Add(new Card(suit, rank));
                }
            }
        }

        private void ShuffleDeck()
        {
            Random random = new Random();
            deck = deck.OrderBy(card => random.Next()).ToList();
        }

        private void DealCards()
        {
            int numPlayers = players.Count;
            int cardsPerPlayer = deck.Count / numPlayers;

            for (int i = 0; i < numPlayers; i++)
            {
                players[i].Hand.AddRange(deck.GetRange(i * cardsPerPlayer, cardsPerPlayer));
            }
        }

        public void Play()
        {
            while (players.All(player => player.Hand.Any()))
            {
                PlayRound();
            }

            Player winner = players.OrderByDescending(player => player.Hand.Count).First();
            Console.WriteLine($"{winner.Name} wins the game!");
        }

        private void PlayRound()
        {
            List<Card> cardsInPlay = players.Select(player => player.Hand.First()).ToList();
            int maxCardIndex = cardsInPlay.IndexOf(cardsInPlay.Max());

            Player roundWinner = players[maxCardIndex];
            players.ForEach(player => player.Hand.RemoveAt(0));
            roundWinner.Hand.AddRange(cardsInPlay);

            Console.WriteLine($"{roundWinner.Name} wins the round!");
        }
    }

    class Program
    {
        static void Main()
        {
            List<string> playerNames = new List<string> { "Player1", "Player2" };
            Game<string> cardGame = new Game<string>(playerNames);
            cardGame.Play();
        }
    }
}
