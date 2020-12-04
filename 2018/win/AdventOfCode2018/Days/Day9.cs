using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day9
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        public static void Day9a(bool isDay9b = false)
        {
            int numPlayers = 0;
            int numMarbles = 0;
            Day9GameManager _mgr;
            //452 players; last marble is worth 71250 points
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day9.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string[] s = reader.ReadLine().Split(' ');
                numPlayers = Convert.ToInt32(s[0]);
                numMarbles = Convert.ToInt32(s[6]) + 1;
            }

            if (isDay9b)
                numMarbles *= 100;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            _mgr = new Day9GameManager(numPlayers, numMarbles);

            Queue<Day9Player> endGame = _mgr.RunGame();

            Day9Player winner = endGame.OrderByDescending(o => o.Score).First();
            sw.Stop();

            StringBuilder sb = new StringBuilder();

            sb.Append("The winning player is Player " + winner.PlayerNumber + " with a score of " + winner.Score.ToString() + ".");
            sb.Append(Environment.NewLine);
            sb.Append("Run in " + sw.ElapsedMilliseconds + "ms, or " + sw.ElapsedTicks + " ticks.");

            MessageBox.Show(sb.ToString());
        }

        public static void Day9b()
        {
            Day9a(true);
        }


        #region Support Methods

        #endregion
    }

    class Day9DoublyLinkedList
    {
        public static void RunWork(bool isB = false)
        {
            //Setup
            int lastscore = 71250, numplayers = 452, player = 0;

            DoublyLinkedList l = new DoublyLinkedList();
            Stopwatch sw = new Stopwatch();

            if (isB)
            {
                lastscore *= 100;
            }

            long[] players = new long[numplayers];

            sw.Start();
            for (int i = 3; i <= lastscore; i++)
            {
                if (i % 23 != 0)
                {
                    l.inc();
                    l.insert(i);
                }
                else
                {
                    players[player] += i;
                    for (int j = 0; j < 7; j++)
                    {
                        l.dec();
                    }
                    players[player] += l.getCurrent();
                    l.delete();
                }
                player = (player + 1) % numplayers;
            }

            Array.Sort(players);
            sw.Stop();

            StringBuilder sb = new StringBuilder();
            sb.Append(players[numplayers - 1]);
            sb.Append(Environment.NewLine);
            sb.Append("Run in " + sw.ElapsedMilliseconds + "ms, or " + sw.ElapsedTicks + " ticks.");

            MessageBox.Show(sb.ToString());
        }
    }

    class Day9Player
    {
        #region Fields
        int playerNumber = 0;
        int score = 0;
        #endregion

        #region Properties
        public int PlayerNumber
        {
            get
            {
                return this.playerNumber;
            }

            set
            {
                if (this.playerNumber == 0)
                    this.playerNumber = value;
            }
        }

        public int Score
        {
            get
            {
                return this.score;
            }

            set
            {
                this.score = value;
            }
        }
        #endregion

        #region Constructors
        public Day9Player(int i)
        {
            this.PlayerNumber = i;
        }
        #endregion
    }

    class Day9Marble
    {
        #region Fields
        int value = 0;
        bool currentMarble = false;
        #endregion

        #region Properties
        public int Value
        {
            get
            {
                return this.value;
            }
        }

        public bool CurrentMarble
        {
            get
            {
                return this.currentMarble;
            }

            set
            {
                this.currentMarble = value;
            }
        }
        #endregion

        #region Constructors
        public Day9Marble(int i)
        {
            this.value = i;
        }
        #endregion
    }

    class Day9GameManager
    {
        #region Fields
        int numPlayers = 0;
        int numMarbles = 0;
        int currentTurn = 0;
        Queue<Day9Player> playerList = new Queue<Day9Player>();
        List<Day9Marble> marbleList = new List<Day9Marble>();
        #endregion

        #region Constructors
        public Day9GameManager(int players, int marbles)
        {
            numPlayers = players;
            numMarbles = marbles;
        }
        #endregion

        #region Methods
        public string ReportCurrent()
        {
            StringBuilder _ret = new StringBuilder();

            _ret.Append("Number of Players: " + numPlayers + "     " + "Current Turn: " + currentTurn + Environment.NewLine + Environment.NewLine);
            foreach (Day9Player p in playerList.OrderBy(o => o.PlayerNumber))
            {
                _ret.Append("Player" + p.PlayerNumber.ToString().PadLeft(4) + ": " + p.Score.ToString().PadLeft(7) + Environment.NewLine);
            }

            return _ret.ToString();
        }

        private Day9Marble DrawMarble(int i)
        {
            return new Day9Marble(i);
        }

        public Queue<Day9Player> RunGame()
        {
            Day9Marble currentMarble;
            Day9Player currentPlayer;
            int currentMarblePosition = 0;

            for (int i = 1; i <= numPlayers; i++)
            {
                Day9Player p = new Day9Player(i);
                playerList.Enqueue(p);
            }

            while (currentTurn < numMarbles)
            {
                currentMarble = DrawMarble(this.currentTurn);
                currentPlayer = playerList.AssignCircular();

                if (currentTurn != 0 && currentTurn % 23 == 0)
                {
                    currentPlayer.Score += currentTurn;
                    currentMarblePosition = RotateBack7(currentMarblePosition);
                    Day9Marble fetchMarbleForScoring = marbleList[currentMarblePosition];
                    currentPlayer.Score += fetchMarbleForScoring.Value;
                    marbleList.RemoveAt(currentMarblePosition);
                }

                else
                {
                    currentMarblePosition = RotateBetween1And2(currentMarblePosition);
                    marbleList.Insert(currentMarblePosition, currentMarble);
                }


                currentTurn++;
            }

            return playerList;
        }

        private int RotateBetween1And2(int currentMarblePostiion)
        {
            int _ret = 0;
            int totalLength = marbleList.Count;

            if (totalLength <= 1)
                _ret = totalLength;

            else
            {
                _ret = currentMarblePostiion + 2;
                if (_ret > totalLength)
                    _ret -= totalLength;
                if (_ret == 0)
                    _ret = 1;
            }

            return _ret;
        }

        private int RotateBack7(int currentMarblePostiion)
        {
            int _ret = 0;
            int totalLength = marbleList.Count;

            _ret = currentMarblePostiion - 7;
            if (_ret < 0)
                _ret += totalLength;
            if (_ret == 0)
                _ret = totalLength;

            return _ret;
        }
        #endregion
    }
}