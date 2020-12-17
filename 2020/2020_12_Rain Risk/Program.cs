using System;
using System.Collections.Generic;
using System.IO;

using XephLibs.Graphing.Distance;
using XephLibs.Graphing.Points;

namespace _2020_12_Rain_Risk
{
    class Program
    {
        public static List<string> navigation = new List<string>();
        public static Ship ship = new Ship();

        static void Main(string[] args)
        {
            ImportData();
            RunNavigationScript();
            int answer = ManhattenDistance.CheckManhattanDistance(new Point2(0, 0), ship.Location);
            Console.WriteLine($"The ship traveled {answer} units.");

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day12_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null)
                {
                    navigation.Add(s);
                }
            }
        }

        static void RunNavigationScript()
        {
            foreach (string s in navigation)
                ship.ParseNavigationItemPart2(s);
            //ship.ParseNavigationItemPart1(s);
        }
    }

    public class Ship
    {
        #region Fields
        int facing = 90;
        Point2 location = new Point2(0, 0);
        Waypoint wp = new Waypoint();
        #endregion

        #region Properties
        public int Facing
        {
            get
            {
                return this.facing;
            }
        }

        public Point2 Location
        {
            get
            {
                return this.location;
            }
        }
        #endregion

        #region Constructors
        public Ship()
        {
            Console.WriteLine($"INIT\nShip: [{this.location.x}, {this.location.y}]   Waypoint: [{wp.Pos.x}, {wp.Pos.y}]");
        }
        #endregion

        #region Methods
        public void ParseNavigationItemPart1(string s)
        {
            // N, S, E, W, L, R, F

            char inst = s[0];
            int val = Int32.Parse(s.Substring(1, s.Length - 1));

            if (inst == 'F')
            {
                switch (this.facing)
                {
                    case 90:
                        inst = 'E';
                        break;
                    case 270:
                        inst = 'W';
                        break;
                    case 0:
                        inst = 'N';
                        break;
                    case 180:
                        inst = 'S';
                        break;
                }
            }

            switch (inst)
            {
                case 'N':
                    this.location.y -= val;
                    break;

                case 'S':
                    this.location.y += val;
                    break;

                case 'E':
                    this.location.x += val;
                    break;

                case 'W':
                    this.location.x -= val;
                    break;

                case 'L':
                    GetNewFacing(-val);
                    break;

                case 'R':
                    GetNewFacing(val);
                    break;
            }
        }

        public void ParseNavigationItemPart2(string s)
        {
            // N, S, E, W, L, R, F

            char inst = s[0];
            int val = Int32.Parse(s.Substring(1, s.Length - 1));

            switch (inst)
            {
                case 'N':
                    this.wp.Pos.y -= val;
                    break;

                case 'S':
                    this.wp.Pos.y += val;
                    break;

                case 'E':
                    this.wp.Pos.x += val;
                    break;

                case 'W':
                    this.wp.Pos.x -= val;
                    break;

                case 'L':
                    this.wp.Rotate(-val);
                    break;

                case 'R':
                    this.wp.Rotate(val);
                    break;

                case 'F':
                    int x = wp.Pos.x * val;
                    int y = wp.Pos.y * val;
                    this.location.x += x;
                    this.location.y += y;
                    break;
            }

            // Debug
            //Console.WriteLine($"Ship: [{this.location.x}, {this.location.y}]   Waypoint: [{wp.Pos.x}, {wp.Pos.y}]");
            //Console.Read();
        }

        public void GetNewFacing(int v)
        {
            this.facing += v;

            while (this.facing > 270)
            {
                this.facing -= 360;
            }

            while (this.facing < 0)
            {
                this.facing += 360;
            }
        }
        #endregion

        #region Internal Classes
        public class Waypoint
        {
            #region Fields
            Point2 pos;
            #endregion

            #region Properties
            public Point2 Pos
            {
                get
                {
                    return this.pos;
                }

                set
                {
                    this.pos = value;
                }
            }
            #endregion

            #region Constructors
            public Waypoint()
            {
                this.pos = new Point2(10, -1);
            }
            #endregion

            #region Methods
            public void Rotate(int val)
            {
                while (val > 360)
                    val -= 360;

                while (val < 0)
                    val += 360;

                int newX = this.pos.x;
                int newY = this.pos.y;

                switch (val)
                {
                    case 90:
                        newX = -this.pos.y;
                        newY = this.pos.x;
                        break;
                    case 180:
                        newX = -this.pos.x;
                        newY = -this.pos.y;
                        break;
                    case 270:
                        newX = this.pos.y;
                        newY = -this.pos.x;
                        break;
                }

                this.pos.x = newX;
                this.pos.y = newY;
            }
            #endregion
        }
        #endregion
    }
}