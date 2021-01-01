using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020_22_Crab_Combat
{
    class Program
    {
        public static string importPath = @"..\..\..\Day22_Input.txt";
        public static Game game = Game.Instance;

        static void Main(string[] args)
        {
            ImportData();
            //game.ShowPlayerHands();

            game.PlayGame();

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(importPath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                string player = String.Empty;
                List<string> players = new List<string>();
                int cardCount = 0;
                bool playerRead = false;

                while ((s = reader.ReadLine()) != null)
                {
                    if ((s != ""))
                    {
                        if (!playerRead)
                        {
                            playerRead = true;
                        }

                        else
                        {
                            player += $"{s}, ";
                            cardCount++;
                        }
                    }

                    else
                    {
                        playerRead = false;
                        player = player.Remove(player.Length - 2, 2);
                        players.Add(player);
                        player = String.Empty;
                    }
                }

                playerRead = false;
                player = player.Remove(player.Length - 2, 2);
                players.Add(player);
                player = String.Empty;

                for (int i = 0; i < players.Count; i++)
                {
                    Player p = new Player(i + 1, players[i].Split(", ", StringSplitOptions.RemoveEmptyEntries).ToList());
                    game.AddPlayer(p);
                }
            }
        }
    }

    public class Game
    {
        #region Fields
        private static readonly Lazy<Game> lazy = new Lazy<Game>(() => new Game());
        private static List<Player> players = new List<Player>();
        private static int turn = 0;
        private static int totalCardsInDeck = 0;
        #endregion

        #region Properties
        public int Turn
        {
            get
            {
                return turn;
            }
        }
        #endregion

        #region Constructors
        private Game()
        {
        }

        public static Game Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        #region Methods
        public void AddPlayer(Player p)
        {
            players.Add(p);
        }

        public void PlayGame()
        {
            foreach (Player p in players)
                totalCardsInDeck += p.Deck.Count;

            while (!CheckForGameOver())
            {
                TakeTurn();
            }

            GameResults();
        }

        private bool CheckForGameOver()
        {
            if (players.Any(a => a.Deck.Count == totalCardsInDeck))
                return true;

            return false;
        }

        private void TakeTurn()
        {
            turn++;

            Console.WriteLine($"-- Round {turn} --");
            foreach (Player p in players)
                Console.WriteLine($"Player {p.Number}'s deck: {p.Deck.ToArray().ArrToString()}");

            int[] cards = new int[players.Count];

            for (int i = 0; i < players.Count; i++)
                cards[i] = players[i].Deck.Dequeue();

            for (int i = 0; i < players.Count; i++)
                Console.WriteLine($"Player {i + 1} plays: {cards[i]}");

            int indexOfHandWinner = Array.IndexOf(cards, cards.Max());

            Console.WriteLine($"Player {indexOfHandWinner + 1} wins the round!");

            Player winner = players.Single(s => s.Number == indexOfHandWinner + 1);

            foreach (int i in cards.OrderByDescending(o => o))
                winner.Deck.Enqueue(i);

            Console.WriteLine();
        }

        private void GameResults()
        {
            Console.WriteLine();
            Console.WriteLine("== Post-game results ==");
            foreach (Player p in players)
                Console.WriteLine($"Player {p.Number}'s deck: {p.Deck.ToArray().ArrToString()}");

            Player winner = players.Single(s => s.Deck.Count == totalCardsInDeck);

            Console.WriteLine($"Player {winner.Number} wins with {winner.GetPoints()} points.");
        }

        public void ShowPlayerHands()
        {
            foreach (Player p in players)
            {
                Console.WriteLine($"Player {p.Number}:");

                foreach (int card in p.Deck)
                    Console.WriteLine($"{card}");

                Console.WriteLine();
            }
        }
        #endregion
    }

    public class Player
    {
        public int Number;
        public Queue<int> Deck;
        public long Score;

        public Player(int number, List<string> deck)
        {
            this.Number = number;

            BuildDeck(deck);
        }

        private void BuildDeck(List<string> deck)
        {
            Deck = new Queue<int>();

            for (int i = 0; i < deck.Count; i++)
                this.Deck.Enqueue(Convert.ToInt32(deck[i]));
        }

        public long GetPoints()
        {
            long _ret = 0;

            for (int i = this.Deck.Count; i > 0; i--)
                _ret += i * this.Deck.Dequeue();

            return _ret;
        }
    }

    public static class Extensions
    {
        public static string ArrToString<T>(this T[] source)
        {
            string _ret = String.Empty;

            if (source.Length > 0)
            {
                for (int i = 0; i < source.Length; i++)
                    _ret += $"{source[i].ToString()}, ";

                _ret = _ret.Remove(_ret.Length - 2, 2);
            }

            return _ret;
        }
    }
}
