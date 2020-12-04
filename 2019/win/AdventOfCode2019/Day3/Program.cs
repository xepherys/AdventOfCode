using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using XephLibs.Mathematics;
/*
    Lowest Manhattan distance intersection between Wire 1 and Wire 2 is: 266
    Lowest latency intersection between Wire 1 and Wire 2 is: 19242
*/

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            string[] wires = new string[2];
            int count = 0;

            Console.WriteLine("Initialized and starting process at {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine(Environment.NewLine);

            using (Stream stream = File.OpenRead(@"..\..\..\Day03_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    wires[count] = s;
                    count++;
                }
            }
            Console.WriteLine("Finished parsing files at {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine(Environment.NewLine);

            Motion[] wire1Path = GetPath(wires[0]);
            Motion[] wire2Path = GetPath(wires[1]);

            Console.WriteLine("Finished getting paths at {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine(Environment.NewLine);

            List<Point> wire1AllPoints = ExplodePath(wire1Path);
            List<Point> wire2AllPoints = ExplodePath(wire2Path);

            Console.WriteLine("Finished exploding paths at {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine(Environment.NewLine);

            List<Point> intersections = FindIntersections(wire1AllPoints, wire2AllPoints);
            List<int> manhattanDistances = FindManhattanDistances(intersections);
            List<int> latencySteps = FindLatencySteps(intersections, wire1AllPoints, wire2AllPoints);

            Console.WriteLine("Completed processing at {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine(Environment.NewLine);

            Console.WriteLine("Lowest Manhattan distance intersection between Wire 1 and Wire 2 is: {0}", manhattanDistances.Where(w => w > 0).Min());
            Console.WriteLine("Lowest latency intersection between Wire 1 and Wire 2 is: {0}", latencySteps.Where(w => w > 0).Min());

            /*
            Thread newThread = new Thread(delegate ()
                {
                    TestForm _frm = new TestForm();
                    _frm.Show();
                    System.Windows.Threading.Dispatcher.Run();
                }
            );

            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();

            while (newThread.)
            { }
            */

            VectorPair[] wire1Vectors = GetAllWireVectorPairs(wire1Path);
            VectorPair[] wire2Vectors = GetAllWireVectorPairs(wire2Path);

            

            Console.WriteLine("Press any key to terminate...");
            Console.Read();
            Environment.Exit(0);
        }

        public static VectorPair[] GetAllWireVectorPairs(Motion[] path)
        {
            VectorPair[] _ret = new VectorPair[(int)Math.Floor((double)path.Length / 2)];

            for (int i = 0; i < _ret.Length; i++)
            {
                VectorPair vp = new VectorPair(path[i], path[i + 1]);
                _ret[i] = vp;
            }

            return _ret;
        }

        public static Motion[] GetPath(string wireInput)
        {
            string[] input = wireInput.Split(',').ToArray();
            Motion[] _ret = new Motion[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                _ret[i] = new Motion(input[i]);
            }

            return _ret;
        }

        public static List<int> FindLatencySteps(List<Point> points, List<Point> a, List<Point> b)
        {
            List<int> _ret = new List<int>();

            foreach (Point p in points)
            {
                int aL = a.IndexOf(p);
                int bL = b.IndexOf(p);
                _ret.Add(aL + bL);
            }

            return _ret;
        }

        public static List<int> FindManhattanDistances(List<Point> points)
        {
            List<int> _ret = new List<int>();

            foreach (var v in points)
            {
                _ret.Add(Math.Abs(v.X) + Math.Abs(v.Y));
            }

            return _ret;
        }

        public static List<Point> FindIntersections(List<Point> a, List<Point> b)
        {
            return a.Intersect(b, new PointComparer()).ToList<Point>();
        }

        public static List<Point> FindIntersections(Point[][] pArr)
        {
            List<Point> _ret = new List<Point>();

            foreach (var v in pArr[0].Intersect(pArr[1]))
                _ret.Add(v);

            return _ret;
        }

        
        public static List<Point> ExplodePath(Motion[] motions)
        {
            List<Point> _ret = new List<Point>();
            _ret.Add(new Point(0, 0));

            for (int i = 0; i < motions.Length; i++)
            {
                for (int spaces = 1; spaces <= motions[i].Spaces; spaces++)
                {
                    if (motions[i].Dir == Directions.UP)
                    {
                        _ret.Add(new Point(_ret.Last().X, _ret.Last().Y + 1));
                    }

                    else if (motions[i].Dir == Directions.RIGHT)
                    {
                        _ret.Add(new Point(_ret.Last().X + 1, _ret.Last().Y));
                    }

                    else if (motions[i].Dir == Directions.DOWN)
                    {
                        _ret.Add(new Point(_ret.Last().X, _ret.Last().Y - 1));
                    }

                    else if (motions[i].Dir == Directions.LEFT)
                    {
                        _ret.Add(new Point(_ret.Last().X - 1, _ret.Last().Y));
                    }
                }
            }

            return _ret;
        }
    }

    public class Motion
    {
        public Directions Dir;
        public int Spaces;

        public Motion(string input)
        {
            char dir = input[0];
            this.Spaces = Convert.ToInt32(input.Substring(1));

            if (dir == 'U')
                this.Dir = Directions.UP;
            else if (dir == 'R')
                this.Dir = Directions.RIGHT;
            else if (dir == 'D')
                this.Dir = Directions.DOWN;
            else if (dir == 'L')
                this.Dir = Directions.LEFT;
        }
    }

    public class VectorPair
    {
        public Directions[] Dir = new Directions[2];
        public int[] Spaces = new int[2];
        public double LinearMotion = 0;
        public int[] Weight = new int[2];

        public VectorPair(Motion m1, Motion m2)
        {
            this.Dir[0] = m1.Dir;
            this.Dir[1] = m2.Dir;
            this.Spaces[0] = m1.Spaces;
            this.Spaces[1] = m2.Spaces;
            this.LinearMotion = Math.Sqrt(Math.Pow(this.Spaces[0], 2) + Math.Pow(this.Spaces[1], 2));
            this.Weight[0] = (int)((decimal)this.Spaces[0] / (decimal)(this.Spaces[0] + this.Spaces[1]) * 100);
            this.Weight[1] = 100 - this.Weight[0];
        }

        public void DrawBezier()
        {
            Point p0 = new Point(0, 0);
            Point p1 = new Point(0, 0).AddPoint(this.Dir[0], this.Spaces[0]).AddPoint(this.Dir[1], this.Spaces[1]);
        }
    }

    public static class PointExtensions
    {
        public static Point AddPoint(this Point source, Directions dir, int spaces)
        {
            if (dir == Directions.UP)
                return new Point(0, spaces);

            if (dir == Directions.RIGHT)
                return new Point(spaces, 0);

            if (dir == Directions.DOWN)
                return new Point(0, -spaces);

            if (dir == Directions.LEFT)
                return new Point(-spaces, 0);

            return new Point(0, 0);
        }
    }

    public enum Directions
    {
        UP      =   1,
        NORTH   =   1,
        RIGHT   =   2,
        EAST    =   2,
        DOWN    =   3,
        SOUTH   =   3,
        LEFT    =   4,
        WEST    =   4
    }
}
