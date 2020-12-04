using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day13
    {
        public static void Day13a()
        {
            Day13Manager _mgr = new Day13Manager();
            Day13Form _frm = new Day13Form(ref _mgr);
            _frm.RunInNewThread(false);
            //_frm.Show();

            #region Support Methods

            #endregion
        }
    }


    public class Day13Manager
    {
        #region Fields
        int stepNum { get; set; }
        int Height { get; set; }
        int Width { get; set; }
        char[,] map;
        //char[,] cartMap;
        List<Day13Cart> carts = new List<Day13Cart>();
        string ResourceFilename = "AdventOfCode2018._data.AdventOfCode_Day13_sample.txt";
        bool running = true;
        bool crashed = false;
        #endregion

        #region Properties
        public List<Day13Cart> Carts
        {
            get
            {
                return this.carts;
            }

            set
            {
                this.carts = value;
            }
        }

        public char[,] Map
        {
            get
            {
                return this.map;
            }

            private set
            {
                this.map = value;
            }
        }

        public int StepNum
        {
            get
            {
                return this.stepNum;
            }

            private set
            {
                this.stepNum = value;
            }
        }
        #endregion

        public void RunWork(string _filename)
        {
            this.ResourceFilename = _filename;
            GetMap();
            FetchCarts();
        }

        public string Update(int movement = 1)
        {
            string _ret = null;

            foreach (Day13Cart cart in this.Carts.OrderBy(o => o.Position.Y).ThenBy(o => o.Position.X).ToList())
            {
                if (this.Carts.Count == 1)
                    return "Last cart: (" + cart.Position.X + ", " + cart.Position.Y + ").";

                if (this.Carts.Exists(e => e.ID == cart.ID))  // Only execute if the cart has not been removed due to a collision.
                    _ret = cart.Update(movement);

                if (cart.Position.X == 53 && cart.Position.Y == 36)
                    MessageBox.Show("Step: " + this.StepNum + Environment.NewLine +
                                    "Cart: " + cart.ID + Environment.NewLine);
                //cart.Update(movement);
                if (!String.IsNullOrEmpty(_ret))
                    MessageBox.Show(_ret);
            }

            if (movement == 1)
                this.StepNum++;
            else if (movement == -1)
                this.StepNum--;
            return null; // make _ret for 13a
        }

        public void GetMap()
        {
            System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

            int height = 0;
            int width = 0;

            using (Stream stream = thisExe.GetManifestResourceStream(this.ResourceFilename))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    width = line.Length;
                    height++;
                }
            }

            this.Height = height;
            this.Width = width;
            char[,] _map = new char[width, height];

            using (Stream stream = thisExe.GetManifestResourceStream(this.ResourceFilename))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                int lineIterator = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    int charIterator = 0;
                    foreach (char c in line)
                    {
                        _map[charIterator, lineIterator] = c;
                        charIterator++;
                    }

                    lineIterator++;
                }
            }

            this.Map = _map;
        }

        public void FetchCarts()
        {
            for (int y = 0; y < this.Height; y++)
            {
                for (int x = 0; x < this.Width; x++)
                {
                    char c = this.Map[x, y];

                    if (c == '<' || c == '>')
                    {
                        this.Carts.Add(new AdventOfCode2018.Day13Cart(x, y, c, '-', this));
                    }

                    else if (c == '^' || c == 'v')
                    {
                        this.Carts.Add(new AdventOfCode2018.Day13Cart(x, y, c, '|', this));
                    }
                }
            }
        }
    }

    public class Day13Cart
    {
        #region Fields
        string id = String.Empty;
        public Core.Point Position { get; set; }
        char currentChar;
        char previousTrackChar;
        Day13Manager Mgr;
        Turn turn = Turn.LEFT;
        #endregion

        #region Properties
        public string ID
        {
            get
            {
                return this.id;
            }

            private set
            {
                if (this.id == String.Empty)
                {
                    this.id = value;
                }
            }
        }
        char CurrentChar
        {
            get
            {
                return this.currentChar;
            }

            set
            {
                if (value == '<' || value == '>' || value == '^' || value == 'v' || value == 'X')
                {
                    this.currentChar = value;
                }

                else
                    throw new ArgumentOutOfRangeException("Valid values are only [< | > | ^ | v | X]. Character sent: '" + value.ToString() + "'. (Day13Cart.CurrentChar)");
            }
        }

        char PreviousTrackChar
        {
            get
            {
                return this.previousTrackChar;
            }

            set
            {
                if (value == '|' || value == '-' || value == '\\' || value == '/' || value == '+')
                    previousTrackChar = value;
                else
                    throw new ArgumentOutOfRangeException(@"Valid characters are only [| \ / - +]. Character sent: '" + value.ToString() + "'. (Day13Cart.PreviousTrackChar)");
            }
        }
        #endregion

        #region Constructors
        public Day13Cart(int x, int y, char _char, char _trackChar, Day13Manager _mgr)
        {
            this.Position = new Core.Point(x, y);
            this.currentChar = _char;
            this.Mgr = _mgr;
            this.ID = Guid.NewGuid().ToString();
            this.PreviousTrackChar = _trackChar;
        }
        #endregion

        #region Methods
        public string Update(int movement = 1)
        {
            int x = this.Position.X;
            int y = this.Position.Y;
            int? newX = null;
            int? newY = null;

            if (!(movement == 1 || movement == -1))
                throw new ArgumentOutOfRangeException("Day13Cart.Update() can only accept a movement of 1 (forward) or -1 (backward).");

            if (this.CurrentChar == '>')
            {
                this.Mgr.Map[x, y] = this.PreviousTrackChar;
                if (movement == 1)
                    newX = x + 1;
                else
                    newX = x - 1;
                newY = y;
            }

            else if (this.CurrentChar == '<')
            {
                this.Mgr.Map[x, y] = this.PreviousTrackChar;
                if (movement == 1)
                    newX = x - 1;
                else
                    newX = x + 1;
                newY = y;
            }

            else if (this.CurrentChar == '^')
            {
                this.Mgr.Map[x, y] = this.PreviousTrackChar;
                newX = x;
                if (movement == 1)
                    newY = y - 1;
                else
                    newY = y + 1;
            }

            else if (this.CurrentChar == 'v')
            {
                this.Mgr.Map[x, y] = this.PreviousTrackChar;
                newX = x;
                if (movement == 1)
                    newY = y + 1;
                else
                    newY = y - 1;
            }

            else if (this.CurrentChar == 'X')
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Crash Occured at (" + x + ", " + y + "). (Spot 1)");
                this.CurrentChar = 'X';
                this.Mgr.Map[x, y] = this.CurrentChar;

                return sb.ToString();
            }

            else
            {
                throw new ArgumentOutOfRangeException("Day13Cart.Update(): Pre-update state of the cart direction is invalid.");
            }

            if (newX == null || newY == null)
            {
                throw new ArgumentNullException("Day13Cart.Update():  New X or Y value is null.");
            }

            char c = this.Mgr.Map[(int)newX, (int)newY];

            if (c == 'v' || c == '^' || c == '<' || c == '>' || c == 'X')
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("Crash Occured at (" + newX + ", " + newY + ").  (Spot 2)");
                //this.CurrentChar = 'X'; //Uncomment for 13a
                Day13Cart otherCart = this.Mgr.Carts.Single(s => s.Position.X == (int)newX && s.Position.Y == (int)newY); //Comment out for 13a
                this.Mgr.Map[x, y] = this.PreviousTrackChar;
                this.Mgr.Map[(int)newX, (int)newY] = otherCart.PreviousTrackChar; //Comment out for 13a
                this.Mgr.Carts.Remove(otherCart); //Comment out for 13a
                this.Mgr.Carts.Remove(this); //Comment out for 13a
                //this.Mgr.Map[(int)newX, (int)newY] = this.CurrentChar; //Uncomment for 13a

                return sb.ToString();
            }

            else if (c == '-')
            {
                if (this.CurrentChar == '>' || this.CurrentChar == '<')
                {
                    this.CurrentChar = this.CurrentChar;
                    this.Mgr.Map[(int)newX, (int)newY] = this.CurrentChar;
                    this.PreviousTrackChar = '-';
                }

                else
                {
                    throw new ArgumentOutOfRangeException("Day13Cart.Update(): '-' track should only be accessible by a > or < cart.");
                }
            }

            else if (c == '|')
            {
                if (this.CurrentChar == '^' || this.CurrentChar == 'v')
                {
                    this.CurrentChar = this.CurrentChar;
                    this.Mgr.Map[(int)newX, (int)newY] = this.CurrentChar;
                    this.PreviousTrackChar = '|';
                }

                else
                {
                    throw new ArgumentOutOfRangeException("Day13Cart.Update(): '|' track should only be accessible by a v or ^ cart.");
                }
            }

            else if (c == '\\')
            {
                switch (this.CurrentChar)
                {
                    case 'v':
                        if (movement == 1)
                            this.CurrentChar = '>';
                        else
                            this.CurrentChar = '<';
                        break;
                    case '<':
                        if (movement == 1)
                            this.CurrentChar = '^';
                        else
                            this.CurrentChar = 'v';
                        break;
                    case '^':
                        if (movement == 1)
                            this.CurrentChar = '<';
                        else
                            this.CurrentChar = '>';
                        break;
                    case '>':
                        if (movement == 1)
                            this.CurrentChar = 'v';
                        else
                            this.CurrentChar = '^';
                        break;
                }

                this.Mgr.Map[(int)newX, (int)newY] = this.CurrentChar;
                this.PreviousTrackChar = '\\';
                //throw new ArgumentOutOfRangeException("Day13Cart.Update(): '\\' track should only be accessible by a v or < cart.");
            }

            else if (c == '/')
            {
                switch (this.CurrentChar)
                {
                    case 'v':
                        if (movement == 1)
                            this.CurrentChar = '<';
                        else
                            this.CurrentChar = '>';
                        break;
                    case '<':
                        if (movement == 1)
                            this.CurrentChar = 'v';
                        else
                            this.CurrentChar = '^';
                        break;
                    case '^':
                        if (movement == 1)
                            this.CurrentChar = '>';
                        else
                            this.CurrentChar = '<';
                        break;
                    case '>':
                        if (movement == 1)
                            this.CurrentChar = '^';
                        else
                            this.CurrentChar = 'v';
                        break;
                }

                this.Mgr.Map[(int)newX, (int)newY] = this.CurrentChar;
                this.PreviousTrackChar = '/';
            }

            else if (c == '+')
            {
                if (this.CurrentChar == '>')
                {
                    switch (this.turn)
                    {
                        case Turn.LEFT:
                            this.CurrentChar = '^';
                            this.turn = Turn.STRAIGHT;
                            break;
                        case Turn.STRAIGHT:
                            this.turn = Turn.RIGHT;
                            break;
                        case Turn.RIGHT:
                            this.CurrentChar = 'v';
                            this.turn = Turn.LEFT;
                            break;
                    }
                }

                else if (this.CurrentChar == '<')
                {
                    switch (this.turn)
                    {
                        case Turn.LEFT:
                            this.CurrentChar = 'v';
                            this.turn = Turn.STRAIGHT;
                            break;
                        case Turn.STRAIGHT:
                            this.turn = Turn.RIGHT;
                            break;
                        case Turn.RIGHT:
                            this.CurrentChar = '^';
                            this.turn = Turn.LEFT;
                            break;
                    }
                }

                else if (this.CurrentChar == '^')
                {
                    switch (this.turn)
                    {
                        case Turn.LEFT:
                            this.CurrentChar = '<';
                            this.turn = Turn.STRAIGHT;
                            break;
                        case Turn.STRAIGHT:
                            this.turn = Turn.RIGHT;
                            break;
                        case Turn.RIGHT:
                            this.CurrentChar = '>';
                            this.turn = Turn.LEFT;
                            break;
                    }
                }

                else if (this.CurrentChar == 'v')
                {
                    switch (this.turn)
                    {
                        case Turn.LEFT:
                            this.CurrentChar = '>';
                            this.turn = Turn.STRAIGHT;
                            break;
                        case Turn.STRAIGHT:
                            this.turn = Turn.RIGHT;
                            break;
                        case Turn.RIGHT:
                            this.CurrentChar = '<';
                            this.turn = Turn.LEFT;
                            break;
                    }
                }

                this.Mgr.Map[(int)newX, (int)newY] = this.CurrentChar;
                this.PreviousTrackChar = '+';
            }

            else
            {
                throw new ArgumentOutOfRangeException("Day13Cart.Update(): Track state is invalid.");
            }

            this.Position = new Core.Point((int)newX, (int)newY);

            /*
            if (CheckForCollision())
            {

            }
            */

            return null;
        }
        #endregion

        #region Helper Methods
        Turn NextTurn()
        {
            switch (this.turn)
            {
                case Turn.RIGHT:
                    return Turn.LEFT;
                case Turn.LEFT:
                    return Turn.STRAIGHT;
                case Turn.STRAIGHT:
                    return Turn.RIGHT;
                case Turn.NONE:
                default:
                    return Turn.NONE;
            }
        }

        bool CheckForCollision()
        {
            bool _ret = false;

            foreach (Day13Cart cart in this.Mgr.Carts.Where(w => w.ID != this.ID))
            {
                if (cart.Position == this.Position)
                {
                    //CRASH
                    this.CurrentChar = 'X';
                    MessageBox.Show("Crash occured at: " + this.Position.ToString() + " on turn: " + this.Mgr.StepNum);
                }
            }

            return _ret;
        }
        #endregion

        #region Enums
        enum Turn
        {
            NONE,
            LEFT,
            STRAIGHT,
            RIGHT
        }
        #endregion
    }
}