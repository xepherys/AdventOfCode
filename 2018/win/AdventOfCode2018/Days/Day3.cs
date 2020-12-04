using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AdventOfCode2018
{
    class Day3
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        static public void Day3a()
        {
            int maxX = 0;
            int maxY = 0;
            int countOver1 = 0;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            List<Day3aObject> elfClaims = new List<Day3aObject>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day3.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                elfClaims = EnumerateClaims(reader).ToList();
            }

            foreach (Day3aObject d3ao in elfClaims)
            {
                if (d3ao.topLeft.X + d3ao.xLength > maxX)
                    maxX = d3ao.topLeft.X + d3ao.xLength;

                if (d3ao.topLeft.Y + d3ao.yHeight > maxY)
                    maxY = d3ao.topLeft.Y + d3ao.yHeight;
            }

            int[,] grid = new int[maxX, maxY];

            FillGrid(ref grid, elfClaims);

            foreach (int i in grid)
            {
                if (i > 1)
                    countOver1++;
            }
            sw.Stop();

            //BuildGridDisplay(grid);

            MessageBox.Show("Total overlapping squares: " + countOver1 + "\n\n" + "Found in: " + sw.ElapsedMilliseconds + " ms (" + sw.ElapsedTicks + " ticks).");
            sw.Reset();
        }

        static public void Day3b()
        {
            int maxX = 0;
            int maxY = 0;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            List<Day3aObject> elfClaims = new List<Day3aObject>();
            Dictionary<Point, List<Day3aObject>> claimIntersections = new Dictionary<Point, List<Day3aObject>>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day3.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                elfClaims = EnumerateClaims(reader).ToList();
            }

            foreach (Day3aObject d3ao in elfClaims)
            {
                if (d3ao.topLeft.X + d3ao.xLength > maxX)
                    maxX = d3ao.topLeft.X + d3ao.xLength;

                if (d3ao.topLeft.Y + d3ao.yHeight > maxY)
                    maxY = d3ao.topLeft.Y + d3ao.yHeight;
            }

            //FillClaimIntersections(ref claimIntersections, elfClaims);

            int[,] grid = new int[maxX, maxY];

            FillGrid(ref grid, elfClaims);

            Day3aObject properClaim = TestGrid(grid, elfClaims);
            sw.Stop();

            MessageBox.Show("Claim with no overlaps: " + properClaim.id + "\n\n" + "Found in: " + sw.ElapsedMilliseconds + " ms (" + sw.ElapsedTicks + " ticks).");
            sw.Reset();
        }


        #region Support Methods
        static Day3aObject TestGrid(int[,] grid, List<Day3aObject> elfClaims)
        {
            Day3aObject _ret = null;
            foreach (Day3aObject d3ao in elfClaims)
            {
                for (int x = d3ao.topLeft.X; x < d3ao.topLeft.X + d3ao.xLength; x++)
                {
                    for (int y = d3ao.topLeft.Y; y < d3ao.topLeft.Y + d3ao.yHeight; y++)
                    {
                        if (grid[x, y] > 1)
                            d3ao.overlaps = true;
                    }
                }
            }

            _ret = elfClaims.Single(c => c.overlaps == false);

            return _ret;
        }

        static IEnumerable<Day3aObject> EnumerateClaims(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                Day3aObject d3ao = new Day3aObject();

                string[] lineread = line.Split(new char[0]);

                d3ao.id = Convert.ToInt32(lineread[0].Replace("#", ""));

                string[] coords = lineread[2].Replace(":", "").Split(',');
                d3ao.topLeft = new Point(int.Parse(coords[0]), int.Parse(coords[1]));

                string[] sizes = lineread[3].Split('x');
                d3ao.xLength = int.Parse(sizes[0]);
                d3ao.yHeight = int.Parse(sizes[1]);

                yield return d3ao;
            }
        }

        static void FillGrid(ref int[,] grid, List<Day3aObject> elfClaims)
        {
            foreach (Day3aObject d3ao in elfClaims)
            {
                for (int x = d3ao.topLeft.X; x < d3ao.topLeft.X + d3ao.xLength; x++)
                {
                    for (int y = d3ao.topLeft.Y; y < d3ao.topLeft.Y + d3ao.yHeight; y++)
                    {
                        grid[x, y]++;

                        if (grid[x, y] > 9)
                            grid[x, y] = 9;
                    }
                }
            }
        }

        static void FillClaimIntersections(ref Dictionary<Point, List<Day3aObject>> claimIntersections, List<Day3aObject> elfClaims)
        {
            foreach (Day3aObject d3ao in elfClaims)
            {
                for (int x = d3ao.topLeft.X; x < d3ao.topLeft.X + d3ao.xLength; x++)
                {
                    for (int y = d3ao.topLeft.Y; y < d3ao.topLeft.Y + d3ao.yHeight; y++)
                    {
                        Point _pt = new Point(x, y);
                        if (!claimIntersections.ContainsKey(_pt))
                            claimIntersections.Add(_pt, new List<Day3aObject>());

                        claimIntersections[_pt].Add(d3ao);
                    }
                }
            }
        }

        static void BuildGridDisplay(int[,] grid)
        {
            int countOver1 = 0;

            string s = String.Empty;

            for (int x = 0; x < grid.GetUpperBound(0); x++)
            {
                for (int y = 0; y < grid.GetUpperBound(1); y++)
                {
                    s += grid[x, y] + " ";
                }

                s += Environment.NewLine;
            }

            foreach (int i in grid)
            {
                if (i > 1)
                    countOver1++;
            }

            //MessageBox.Show(s);
            GridDisplayDay3a gridDisplay = new GridDisplayDay3a(s, countOver1);
            gridDisplay.Show();
        }

        static Day3aObject FindProperClaim(Dictionary<Point, List<Day3aObject>> claimIntersections)
        {
            Day3aObject _ret = null;

            List<List<Day3aObject>> sample = new List<List<Day3aObject>>();

            foreach (List<Day3aObject> _list in claimIntersections.Values)
            {
                sample.Add(_list);
            }

            var test = sample.Where(c => c.Count() == 1).Distinct() as List<Day3aObject>;

            foreach (Day3aObject d3ao in test)
            {
                //List<Point> sampling = new List<Point>();

                //sampling = claimIntersections.Values.
            }

            return _ret;
        }
        #endregion
    }

    /// <summary>
    /// Collection class to contain objects related to Day3 puzzles
    /// </summary>
    public class Day3aObject
    {
        public int id { get; set; }
        public Point topLeft { get; set; }
        public int xLength { get; set; }
        public int yHeight { get; set; }
        public bool overlaps { get; set; }
    }
}