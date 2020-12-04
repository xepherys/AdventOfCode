using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AoC;

/*
Asteroid with the greatest visibility is (17, 22) with eyes on 288 other asteroids.
Answer found in 452ms (4527383 ticks).
Part 2 answer: 616, found in 0ms.
*/

namespace Day10_Monitoring_Station
{
    class Program
    {
        const char asteroid = '#';
        const char openspace = '.';
        static List<Asteroid> asteroids;

        static void Main(string[] args)
        {
            asteroids = new List<Asteroid>();
            FetchInput();
            Stopwatch sw = new Stopwatch();

            sw.Start();
            foreach (Asteroid a in asteroids)
                countVisible(a);
            

            Asteroid best = asteroids.OrderByDescending(o => o.Count).First();
            sw.Stop();

            Console.WriteLine("Asteroid with the greatest visibility is {0} with eyes on {1} other asteroids.", best.ToString(), best.visible.Count());
            Console.WriteLine("Answer found in {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);

            sw.Reset();
            sw.Start();
            int goal = 200;
            while (best.visible.Count < goal)
            {
                goal -= best.visible.Count;
                foreach (Asteroid a in best.visible)
                    asteroids.Remove(a);
                countVisible(best);
            }

            foreach (Asteroid a in best.visible)
            {
                a.angle = -Math.Atan2(a.p.X - best.p.X, a.p.Y - best.p.Y);
            }
            best.visible.Sort((a, b) => a.angle.CompareTo(b.angle));
            Asteroid target = best.visible[goal - 1];
            sw.Stop();

            Console.WriteLine("Part 2 answer: {0}, found in {1}ms.", target.p.X * 100 + target.p.Y, sw.ElapsedMilliseconds);

            Console.ReadLine();
            Environment.Exit(0);
        }

        public static void countVisible(Asteroid from)
        {
            from.visible = new List<Asteroid>();
            foreach (Asteroid a in asteroids)
                if (a != from && lineOfSight(from, a))
                {
                    from.visible.Add(a);
                    from.Count++;
                }
        }

        public static bool lineOfSight(Asteroid from, Asteroid to)
        {
            int dX = abs(to.p.X - from.p.X);
            int dY = abs(to.p.Y - from.p.Y);
            foreach (Asteroid a in asteroids)
            {
                if (a != from && a != to)
                {
                    int dXa = a.p.X - from.p.X;
                    int dYa = a.p.Y - from.p.Y;
                    if (abs(dXa) <= dX && abs(dYa) <= dY && eqAngle(from, to, a))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool eqAngle(Asteroid from, Asteroid a, Asteroid b)
        {
            
            if (sign(a.p.X - from.p.X) != sign(b.p.X - from.p.X)) return false;
            if (sign(a.p.Y - from.p.Y) != sign(b.p.Y - from.p.Y)) return false;
            
            //if (a.p.X == from.p.X && b.p.X == from.p.X) return true;
            //if (a.p.Y == from.p.Y && b.p.Y == from.p.Y) return true;
            
            return ((a.p.X - from.p.X) * (b.p.Y - from.p.Y) == (b.p.X - from.p.X) * (a.p.Y - from.p.Y));
        }

        public static int sign(int x)
        {
            return (x > 0) ? 1 : (x < 0) ? -1 : 0;
        }

        public static int abs(int input)
        {
            return Math.Abs(input);
        }

        public static void FetchInput()
        {
            List<string> lines = new List<string>();

            using (Stream stream = File.OpenRead(@"..\..\..\Day10_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                int y = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    for (int x = 0; x < line.Length; x++)
                    {
                        if (line[x] == asteroid)
                            asteroids.Add(new Asteroid(x, y));
                    }
                    y++;
                }
            }
        }
    }

    class Asteroid
    {
        public Point p;
        public int Count;
        public List<Asteroid> visible = null;
        public double angle = 0;

        public Asteroid(int x, int y)
        {
            p = new Point(x, y);
        }

        public override string ToString()
        {
            return "(" + p.X + ", " + p.Y + ")";
        }

    }
}
