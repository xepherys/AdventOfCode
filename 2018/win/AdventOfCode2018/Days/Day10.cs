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
    public class Day10
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();


        // Day10b is a natural solution of Day10a in this setup.  Actually, I'm not sure how you could solve a without also already knowing b.  Hmmm...
        public static string Day10a(int updates = 1, int by = 1, bool invert = false)
        {
            string[] lines;
            List<Day10PointOfLight> points = new List<Day10PointOfLight>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day10.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                lines = StreamFunctions.EnumerateLines(reader).ToArray();
            }

            foreach (string line in lines)
            {
                Day10PointOfLight p = new Day10PointOfLight(line);
                points.Add(p);
            }

            Day10PointManager _mgr = new Day10PointManager(points);

            Stopwatch sw = new Stopwatch();

            points = _mgr.RunWork(updates, by);

            char[,] starArray = _mgr.GetStarArray();

            StringBuilder sb = new StringBuilder();

            if (invert)
            {
                for (int x = 0; x < starArray.GetLength(0); x++)
                {
                    for (int y = 0; y < starArray.GetLength(1); y++)
                    {
                        if (starArray[x, y] == '*')
                            sb.Append("*");
                        else
                            sb.Append(" ");
                    }
                    sb.Append(Environment.NewLine);
                }
            }

            else
            {
                for (int y = 0; y < starArray.GetLength(1); y++)
                {
                    for (int x = 0; x < starArray.GetLength(0); x++)
                    {
                        if (starArray[x, y] == '*')
                            sb.Append("*");
                        else
                            sb.Append(" ");
                    }
                    sb.Append(Environment.NewLine);
                }
            }
            //_mgr.GetScatterData();

            return sb.ToString();

            #region Support Methods

            #endregion
        }
    }

    class Day10PointManager
    {
        #region Fields
        int step = 0;
        List<Day10PointOfLight> points = new List<Day10PointOfLight>();
        #endregion

        #region Properties
        public int Step
        {
            get
            {
                return this.step;
            }

            set
            {
                this.step = value;
            }
        }

        public List<Day10PointOfLight> Points
        {
            get
            {
                return this.points;
            }

            set
            {
                this.points = value;
            }
        }
        #endregion

        #region Constructors
        public Day10PointManager()
        { }

        public Day10PointManager(List<Day10PointOfLight> _points)
        {
            points = _points;
        }
        #endregion

        #region Methods
        public List<Day10PointOfLight> RunWork(int updates, int by)
        {
            for (int i = 0; i < updates; i++)
            {
                step++;
                foreach (Day10PointOfLight p in points)
                {
                    p.Update(by);
                }
            }

            return points;
        }

        public void GetScatterData()
        {
            Day10PointOfLight MaxXY;
            Day10PointOfLight MinXY;
            Day10PointOfLight MaxXMinY;
            Day10PointOfLight MinXMaxY;
            Day10Coord average;

            MaxXY = points.OrderBy(o1 => o1.X).ThenBy(o2 => o2.Y).First();
            MinXY = points.OrderByDescending(o1 => o1.X).ThenByDescending(o2 => o2.Y).First();
            MaxXMinY = points.OrderBy(o1 => o1.X).ThenByDescending(o2 => o2.Y).First();
            MinXMaxY = points.OrderByDescending(o1 => o1.X).ThenBy(o2 => o2.Y).First();

            average = Day10PointOfLight.Average(new Day10PointOfLight[] { MaxXY, MinXY, MaxXMinY, MinXMaxY });

            StringBuilder sb = new StringBuilder();

            sb.Append("Average of max points: " + average.X.ToString() + ", " + average.Y.ToString() + Environment.NewLine + Environment.NewLine);
            sb.Append("   MaxXY: " + MaxXY.X    + ", " + MaxXY.Y    + Environment.NewLine);
            sb.Append("   MinXY: " + MinXY.X    + ", " + MinXY.Y    + Environment.NewLine);
            sb.Append("MaxXMinY: " + MaxXMinY.X + ", " + MaxXMinY.Y + Environment.NewLine);
            sb.Append("MinXMaxY: " + MinXMaxY.X + ", " + MinXMaxY.Y + Environment.NewLine);

            MessageBox.Show(sb.ToString());
        }

        public char[,] GetStarArray()
        {
            int maxX = points.OrderByDescending(o1 => o1.X).First().X;
            int minX = points.OrderBy(o1 => o1.X).First().X;
            int maxY = points.OrderByDescending(o1 => o1.Y).First().Y;
            int minY = points.OrderBy(o1 => o1.Y).First().Y;

            int width = Math.Abs(maxX) + Math.Abs(minX) + 1;
            int height = Math.Abs(maxY) + Math.Abs(minY) + 1;

            char[,] grid = new char[width, height];
            foreach (var v in points)
            {
                grid[v.X + Math.Abs(minX), v.Y + Math.Abs(minY)] = '*';
            }

            return grid;
        }
        #endregion
    }

    class Day10PointOfLight
    {
        #region Fields
        Day10Coord originalPos;
        Day10Coord currPos;
        Day10Coord velocity;
        #endregion

        #region Properties
        public int X
        {
            get
            {
                return this.currPos.X;
            }
        }

        public int Y
        {
            get
            {
                return this.currPos.Y;
            }
        }
        #endregion

        #region Constructors
        public Day10PointOfLight(Day10Coord o, Day10Coord v)
        {
            originalPos = o;
            currPos = o;
            velocity = v;
        }

        public Day10PointOfLight(string s)
        {
            //position=< 21188,  31669> velocity=<-2, -3>
            s = s.Remove(0, 10);
            s = s.Remove(s.IndexOf('v'), 10);
            s = s.Replace(">", "");
            s = s.Replace(",", "");
            s = s.Replace("  ", " ");
            while (s[0] == ' ')
                s = s.Remove(0, 1);
            string[] val = s.Split(' ');
            Day10Coord _origin = new Day10Coord(Convert.ToInt32(val[0]), Convert.ToInt32(val[1]));
            Day10Coord _velocity = new Day10Coord(Convert.ToInt32(val[2]), Convert.ToInt32(val[3]));

            originalPos = _origin;
            currPos = _origin;
            velocity = _velocity;
        }
        #endregion

        #region Methods
        public void Update(int iterations = 1)
        {
            if (iterations > 0)
            {
                //for (int i = 0; i < iterations; i++)
                //{
                    currPos += (velocity * iterations);
                //}
            }

            else if (iterations < 0)
            {
                //for (int i = 0; i > iterations; i--)
                //{
                    currPos -= (velocity * iterations);
                //}
            }

            else
            {
                throw new ArgumentException("Update cannot be 0.");
            }
        }

        public static Day10Coord Average(Day10PointOfLight[] _arr)
        {
            int x = 0;
            int y = 0;

            foreach (var v in _arr)
            {
                x += v.X;
                y += v.Y;
            }

            return new Day10Coord(x / _arr.Length, y / _arr.Length);
        }
        #endregion
    }

    class Day10Coord
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Day10Coord(int x, int y)
        {
            X = x;
            Y = y;
        }


        public static Day10Coord operator +(Day10Coord p1, Day10Coord p2)
        {
            int x = p1.X + p2.X;
            int y = p1.Y + p2.Y;
            return new Day10Coord(x, y);
        }

        public static Day10Coord operator -(Day10Coord p1, Day10Coord p2)
        {
            int x = p1.X - p2.X;
            int y = p1.Y - p2.Y;
            return new Day10Coord(x, y);
        }

        public static Day10Coord operator *(Day10Coord p1, int i)
        {
            int x = p1.X * i;
            int y = p1.Y * i;
            return new Day10Coord(x, y);
        }
    }
}