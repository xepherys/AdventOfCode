using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using AdventOfCode2018.Core;
using Xepherys.AlphabetProviders;

namespace AdventOfCode2018
{
    class Day6
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        public static void Day6a()
        {
            Stopwatch sw = new Stopwatch();
            List<Core.Point> destinations = new List<Core.Point>();
            int maxX = 0;
            int maxY = 0;
            string line;
            string[] lineSplit;
            IAlphabetProvider provider = null;
            string alpha = String.Empty;
            Dictionary<char, int[,]> overlays = new Dictionary<char, int[,]>();
            //Dictionary<int[,], Day6Spot> heatmap = new Dictionary<int[,], Day6Spot>();
            char[,] heatmap = null;
            int[,] heatmapvalues = null;
            List<Day6Spot> d6s = new List<Day6Spot>();


            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day6.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lineSplit = line.RemoveWhitespace().Split(',');
                    Core.Point point = new Core.Point(Convert.ToInt32(lineSplit[0]), Convert.ToInt32(lineSplit[1]));
                    destinations.Add(point);
                }
            }

            if (destinations.Count <= 26)
            {
                provider = new EnglishAlphabetProvider();
            }

            else if (destinations.Count <= 61)
            {
                provider = new LargerUniqueAlphabetProvider();
            }

            else
            {
                throw new Exception("Are you serious?  Too many elements!  ?: " + destinations.Count);
            }

            foreach (char c in provider.GetAlphabet(destinations.Count))
            {
                alpha += c;
            }

            maxX = (destinations.OrderBy(d => d.X).LastOrDefault()).X + 1;
            maxY = (destinations.OrderBy(d => d.Y).LastOrDefault()).Y + 1;

            int[,] coordMap = new int[maxX, maxY];

            Parallel.ForEach(destinations, p =>
            {
                char c;
                lock (alpha)
                {
                    c = alpha[0];
                    alpha = alpha.Remove(alpha.IndexOf(c), 1);
                }
                overlays.Add(c, GetRotaryValues(coordMap, p));
            });

            /*
            for (int i = 0; i < coordMap.GetLength(0); i++)
                for (int ii = 0; ii < coordMap.GetLength(1); ii++)
                {
                    heatmap.Add(new int[i, ii]);
                }
            */

            heatmap = new char[maxX, maxY];
            heatmapvalues = new int[maxX, maxY];

            for (int i = 0; i < coordMap.GetLength(0); i++)
                for (int ii = 0; ii < coordMap.GetLength(1); ii++)
                {
                    heatmapvalues[i, ii] = 99;
                }

            foreach (KeyValuePair<char, int[,]> kvp in overlays)
            {
                for (int i = 0; i < heatmap.GetLength(0); i++)
                    for (int ii = 0; ii < heatmap.GetLength(1); ii++)
                    {
                        if (kvp.Value[i, ii] < heatmapvalues[i, ii])
                        {
                            heatmap[i, ii] = kvp.Key;
                            heatmapvalues[i, ii] = kvp.Value[i, ii];
                        }

                        else if (kvp.Value[i, ii] == heatmapvalues[i, ii])
                        {
                            heatmap[i, ii] = '*';
                        }
                    }
            }

            /*
            StringBuilder sb = new StringBuilder();
            StringBuilder sb2 = new StringBuilder();

            for (int i = 0; i < heatmap.GetLength(0); i++)
            {
                for (int ii = 0; ii < heatmap.GetLength(1); ii++)
                {
                    sb.Append(heatmap[i, ii].ToString().PadLeft(2));
                    sb2.Append(heatmapvalues[i, ii].ToString().PadLeft(2));
                }

                sb.Append(Environment.NewLine);
                sb2.Append(Environment.NewLine);
            }

            File.WriteAllText("heatmap.txt", sb.ToString());
            File.WriteAllText("heatmapvalues.txt", sb2.ToString());
            */

            foreach (var v in heatmap)
            {
                Day6Spot d6;

                d6 = d6s.FirstOrDefault(x => x.c == v);

                if (d6 != null)
                    d6.count++;
                else
                    d6s.Add(new Day6Spot(v, 1));
            }

            /*
            sb = new StringBuilder();

            foreach (var v in d6s)
            {
                sb.Append("Char: " + v.c + "   Count:" + v.count.ToString().PadLeft(6));
                sb.Append(Environment.NewLine);
            }

            File.WriteAllText("countsPerChar.txt", sb.ToString());
            */
        }

        public static void Day6b()
        {
            string line;
            string[] lineSplit;
            List<Core.Point> destinations = new List<Core.Point>();

            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day6.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    lineSplit = line.RemoveWhitespace().Split(',');
                    Core.Point point = new Core.Point(Convert.ToInt32(lineSplit[0]), Convert.ToInt32(lineSplit[1]));
                    destinations.Add(point);
                }
            }

            int maxX = (destinations.OrderBy(d => d.X).LastOrDefault()).X + 1;
            int maxY = (destinations.OrderBy(d => d.Y).LastOrDefault()).Y + 1;

            char[,] validPartOfZone = new char[maxX, maxY];
            int goodPointCount = 0;

            for (int y = 0; y < maxY; y++)
            {
                for (int x = 0; x < maxX; x++)
                {
                    validPartOfZone[x, y] = CoordinateGeometry.CheckManhattanDistances(x, y, destinations, 10000);
                    if (validPartOfZone[x, y] == 'X')
                        goodPointCount++;
                }
            }

            /*
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < validPartOfZone.GetLength(0); i++)
            {
                for (int ii = 0; ii < validPartOfZone.GetLength(1); ii++)
                {
                    sb.Append(validPartOfZone[i, ii].ToString().PadLeft(2));
                }

                sb.Append(Environment.NewLine);
            }

            File.WriteAllText("validzone.txt", sb.ToString());
            */

            MessageBox.Show(goodPointCount.ToString());
        }


        #region Support Methods
        static int[,] GetRotaryValues(int[,] coordMap, Core.Point p)
        {
            int x = 0;
            int y = 0;
            int d = 1;
            decimal m = 0.5m;
            int[,] distance = new int[coordMap.GetLength(0), coordMap.GetLength(1)];

            while (Inbounds(x, y, coordMap))
            {
                //int step = (int)Math.Ceiling((m));
                while (x * d < m)
                {
                    if (x + p.X >= 0 && y + p.Y >= 0 && (x + p.X) < coordMap.GetLength(0) && (y + p.Y) < coordMap.GetLength(1))
                    {
                        distance[x + p.X, y + p.Y] = Math.Min(Math.Abs(p.X - (p.X + x)) + Math.Abs(p.Y - (p.Y + y)), 99);
                    }

                    x = x + d;
                }
                while (y * d < m)
                {
                    if (x + p.X >= 0 && y + p.Y >= 0 && (x + p.X) < coordMap.GetLength(0) && (y + p.Y) < coordMap.GetLength(1))
                    {
                        distance[x + p.X, y + p.Y] = Math.Min(Math.Abs(p.X - (p.X + x)) + Math.Abs(p.Y - (p.Y + y)), 99);
                    }

                    y = y + d;
                }

                d = -1 * d;
                m = m + 0.5m;
            }

            return distance;
        }

        static bool Inbounds(int x, int y, int[,] coordMap)
        {
            bool _ret = false;

            int minX = -(coordMap.GetUpperBound(0));
            int minY = -(coordMap.GetUpperBound(1));
            int maxX = (coordMap.GetUpperBound(0));
            int maxY = (coordMap.GetUpperBound(1));

            if (x >= minX && x <= maxX && y >= minY && y <= maxY)
                _ret = true;

            return _ret;
        }
        #endregion
    }

    public class Day6Spot
    {
        public char c { get; set; }
        public int count { get; set; }

        public Day6Spot(char _c, int _count)
        {
            c = _c;
            count = _count;
        }
    }
}