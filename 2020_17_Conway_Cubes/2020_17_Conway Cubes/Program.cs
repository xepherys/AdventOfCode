using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XephLibs.Graphing.Points;

namespace _2020_17_Conway_Cubes
{
    class Program
    {
        public static List<Day17Point> points = new List<Day17Point>();
        public readonly static int turns = 6;

        static void Main(string[] args)
        {
            ImportData();

            Console.WriteLine("Initial Setup:");
            Console.WriteLine($"Active points: {CountActive()} (out of {points.Count()} points).\n");

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < turns; i++)
            {
                ProcessTurn();

                Console.WriteLine($"Turn {i + 1}:");
                Console.WriteLine($"Active points: {CountActive()} (out of {points.Count()} points).");
                Console.WriteLine($"This turn took {sw.ElapsedMilliseconds}ms to run.\n");
            }
            sw.Stop();

            Console.WriteLine($"Part 1 took {sw.ElapsedMilliseconds}ms to run.");

            /*
            points.Clear();
            sw.Reset();

            ImportData2();

            sw.Start();
            for (int i = 0; i < turns; i++)
            {
                ProcessTurn2();

                Console.WriteLine($"Turn {i + 1}:");
                Console.WriteLine($"Active points: {CountActive()} (out of {points.Count()} points).");
                Console.WriteLine($"This turn took {sw.ElapsedMilliseconds}ms to run.\n");
            }
            sw.Stop();
            Console.WriteLine($"Part 2 took {sw.ElapsedMilliseconds}ms to run.");
            */
            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day17_InputSample.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                List<string> lines = new List<string>();

                while ((s = reader.ReadLine()) != null)
                {
                    lines.Add(s);
                }

                for (int x = 0; x < lines[0].Length; x++)
                {
                    for (int y = 0; y < lines.Count(); y++)
                    {
                        Point<int> p = new Point<int>(x, y, 0);
                        bool a = false;
                        if (lines[y][x] == '#')
                            a = true;

                        points.Add(new Day17Point(p, a));
                    }
                }

                foreach (Day17Point p in points)
                {
                    for (int y = p.Point.y - 1; y <= p.Point.y + 1; y++)
                        for (int x = p.Point.x - 1; x <= p.Point.x + 1; x++)
                        {
                            try
                            {
                                Day17Point dp = points.Single(s => s.Point.x == x && s.Point.y == y);
                                p.Neighbors.Add(dp);
                            }

                            catch { }
                        }
                }
            }
        }

        static void ImportData2()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day17_InputSample.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                List<string> lines = new List<string>();

                while ((s = reader.ReadLine()) != null)
                {
                    lines.Add(s);
                }

                for (int x = 0; x < lines[0].Length; x++)
                {
                    for (int y = 0; y < lines.Count(); y++)
                    {
                        Point<int> p = new Point<int>(x, y, 0, 0);
                        bool a = false;
                        if (lines[y][x] == '#')
                            a = true;

                        points.Add(new Day17Point(p, a));
                    }
                }

                foreach (Day17Point p in points)
                {
                    for (int y = p.Point.y - 1; y <= p.Point.y + 1; y++)
                        for (int x = p.Point.x - 1; x <= p.Point.x + 1; x++)
                        {
                            Day17Point dp = points.Single(s => s.Point.x == x && s.Point.y == y);
                            p.Neighbors.Add(dp);
                        }
                }
            }
        }

        static int CountActive()
        {
            int _ret = 0;

            foreach (Day17Point d17p in points)
                if (d17p.Active)
                    _ret++;

            return _ret;
        }

        static void ProcessTurn()
        {
            List<Day17Point> newPoints = new List<Day17Point>();
            Stopwatch sw = new Stopwatch();

            sw.Start();

            Parallel.ForEach(points, p =>
            {
                if (p.Neighbors.Count() < (int)Math.Pow(3, p.Point.Dimensions()) - 1)
                {
                    for (int z = p.Point.z - 1; z <= p.Point.z + 1; z++)
                        for (int y = p.Point.y - 1; y <= p.Point.y + 1; y++)
                            for (int x = p.Point.x - 1; x <= p.Point.x + 1; x++)
                            {
                                if (x == p.Point.x && y == p.Point.y && z == p.Point.z)
                                {
                                }

                                else
                                {
                                    Day17Point np = new Day17Point(false, x, y, z);

                                    if (!points.Contains(np) && !newPoints.Contains(np))
                                        newPoints.Add(np);

                                    if (!p.Neighbors.Contains(np))
                                        p.Neighbors.Add(np);

                                    if (!np.Neighbors.Contains(p))
                                        np.Neighbors.Add(p);
                                }
                            }
                }
            });

            points.AddRange(newPoints.Distinct().ToList());
            Console.Write($"{sw.ElapsedMilliseconds}ms   ");
            sw.Reset();
            sw.Start();

            Parallel.ForEach(points, p =>
            {
                int activeNeighbors = 0;

                if (p.Neighbors.Count() < (int)Math.Pow(3, p.Point.Dimensions()) - 1)
                {
                    for (int z = p.Point.z - 1; z <= p.Point.z + 1; z++)
                        for (int y = p.Point.y - 1; y <= p.Point.y + 1; y++)
                            for (int x = p.Point.x - 1; x <= p.Point.x + 1; x++)
                            {
                                if (x == p.Point.x && y == p.Point.y && z == p.Point.z)
                                {
                                }

                                else
                                {
                                    try
                                    {
                                        if (points.Single(s => s.Point.x == x && s.Point.y == y && s.Point.z == z).Active)
                                            activeNeighbors++;
                                    }

                                    catch { }
                                }
                            }
                }

                else foreach (Day17Point dp in p.Neighbors)
                {
                    if (dp.Active)
                        activeNeighbors++;
                }

                if (p.Active && (activeNeighbors < 2 || activeNeighbors > 3))
                    p.Swap = true;

                if (!p.Active && activeNeighbors == 3)
                    p.Swap = true;
            });
            Console.Write($"{sw.ElapsedMilliseconds}ms   ");
            sw.Reset();
            sw.Start();

            Parallel.ForEach(points, p => p.Update());
            Console.Write($"{sw.ElapsedMilliseconds}ms\n");
            sw.Stop();
        }

        static void ProcessTurn2()
        {
            List<Day17Point> newPoints = new List<Day17Point>();
            Stopwatch sw = new Stopwatch();

            sw.Start();

            Parallel.ForEach(points, p =>
            {
                for (int z = p.Point.z - 1; z <= p.Point.z + 1; z++)
                    for (int y = p.Point.y - 1; y <= p.Point.y + 1; y++)
                        for (int x = p.Point.x - 1; x <= p.Point.x + 1; x++)
                            for (int w = p.Point.w - 1; w <= p.Point.w + 1; w++)
                            {
                                if (w == p.Point.w && x == p.Point.x && y == p.Point.y && z == p.Point.z)
                                {
                                }

                                else
                                {
                                    Day17Point np = new Day17Point(false, x, y, z, w);

                                    if (!points.Contains(np) && !newPoints.Contains(np))
                                        newPoints.Add(np);
                                }
                            }
            });

            points.AddRange(newPoints.Distinct().ToList());
            Console.Write($"{sw.ElapsedMilliseconds}ms   ");
            sw.Reset();
            sw.Start();

            Parallel.ForEach(points, p =>
            {
                int activeNeighbors = 0;
                for (int z = p.Point.z - 1; z <= p.Point.z + 1; z++)
                    for (int y = p.Point.y - 1; y <= p.Point.y + 1; y++)
                        for (int x = p.Point.x - 1; x <= p.Point.x + 1; x++)
                            for (int w = p.Point.w - 1; w <= p.Point.w + 1; w++)
                            {
                                if (w == p.Point.w && x == p.Point.x && y == p.Point.y && z == p.Point.z)
                                {
                                }

                                else
                                {
                                    try
                                    {
                                        if (points.Single(s => s.Point.x == x && s.Point.y == y && s.Point.z == z && s.Point.w == w).Active)
                                            activeNeighbors++;
                                    }

                                    catch { }
                                }
                            }

                if (p.Active && (activeNeighbors < 2 || activeNeighbors > 3))
                    p.Swap = true;

                if (!p.Active && activeNeighbors == 3)
                    p.Swap = true;
            });
            Console.Write($"{sw.ElapsedMilliseconds}ms   ");
            sw.Reset();
            sw.Start();

            Parallel.ForEach(points, p => p.Update());
            Console.Write($"{sw.ElapsedMilliseconds}ms\n");
            sw.Stop();
        }
    }

    public class Day17Point : IEquatable<Day17Point>
    {
        public Point<int> Point;
        public List<Day17Point> Neighbors = new List<Day17Point>();
        public bool Active;
        public bool Swap = false;

        public Day17Point(Point<int> p, bool a)
        {
            this.Point = p;
            this.Active = a;
        }

        public Day17Point(bool a, params int[] coords)
        {
            this.Point = new Point<int>(coords);
            this.Active = a;
        }

        public void Update()
        {
            if (Swap)
            {
                Active = !Active;
                Swap = false;
            }
        }

        public override string ToString()
        {
            return $"{(Active ? '✓' : 'X')} {this.Point.ToString()}";
        }

        public bool Equals(Day17Point other)
        {
            return (this.Point == other.Point);
        }

        public override int GetHashCode()
        {
            return $"[{this.Point.x}, {this.Point.y}, {this.Point.z}, {this.Point.w}]".GetHashCode();
        }
    }

    public static class ConcurrentBagExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> source, IEnumerable<T> toAdd)
        {
            foreach (T element in toAdd)
            {
                source.Add(element);
            }
        }
    }
}
