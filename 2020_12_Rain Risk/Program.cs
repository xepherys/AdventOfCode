using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
                ship.ParseNavigationItem(s);
        }
    }

    public class Ship
    {
        #region Fields
        int facing = 90;
        Point2 location = new Point2(0, 0);
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
        public Ship() { }
        #endregion

        #region Methods
        public void ParseNavigationItem(string s)
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
    }
}