using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Day23
{
    class Program
    {
        //  716 - too low
        //  717 - too low (making sure it wasn't an OB1)
        //  977 - too high (fixed typo)
        //  935 - correct (forgot Math.Abs())
        static Stopwatch sw = new Stopwatch();
        static int numberOfNodes = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Working on A...");
            Console.WriteLine();
            List<Node> nodes = new List<Node>();
            ConcurrentBag<Node> nodeBag = new ConcurrentBag<Node>();
            //List<Node> include = new List<Node>();

            sw.Start();
            try
            {
                using (Stream stream = File.OpenRead(@"AdventOfCode_Day23.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        Node n = new Node();

                        //pos=<76140848,-3604484,65709148>,
                        //r=91714805
                        string[] s = line.Split(' ');
                        n.Radius = Convert.ToInt32(s[1].Remove(0, 2));
                        s[0] = s[0].Remove(0, 5);
                        s[0] = s[0].Remove(s[0].Length - 2, 2);
                        string[] coords = s[0].Split(',');
                        n.X = Convert.ToInt32(coords[0]);
                        n.Y = Convert.ToInt32(coords[1]);
                        n.Z = Convert.ToInt32(coords[2]);

                        nodes.Add(n);
                    }
                }
            }

            catch { throw; }

            Node max = nodes.OrderByDescending(o => o.Radius).First();

            Parallel.ForEach(nodes, n =>
            {
                if (Math.Abs(max.X - n.X) + Math.Abs(max.Y - n.Y) + Math.Abs(max.Z - n.Z) <= max.Radius)
                {
                    Interlocked.Increment(ref Program.numberOfNodes);
                    //include.Add(n);
                }
            }
            );

            sw.Stop();
            Console.WriteLine("Node in range of most other nodes has: ");
            Console.WriteLine(numberOfNodes.ToString());
            Console.WriteLine();
            Console.WriteLine("Found in " + sw.ElapsedMilliseconds + "ms.");
            Console.WriteLine();
            Console.WriteLine("Working on B...");
            Console.WriteLine();

            sw.Reset();
            sw.Start();
            /*
            int minX = 999999999;
            int maxX = -999999999;
            int minY = 999999999;
            int maxY = -999999999;
            int minZ = 999999999;
            int maxZ = -999999999;

            Parallel.ForEach(nodes, n =>
            {
                if (n.X < minX)
                    minX = n.X;
                if (n.X > maxX)
                    maxX = n.X;
                if (n.Y < minY)
                    minY = n.Y;
                if (n.Y > maxY)
                    maxY = n.Y;
                if (n.Z < minZ)
                    minZ = n.Z;
                if (n.Z > maxZ)
                    maxZ = n.Z;
            }
            );

            Console.WriteLine("MinX: " + minX);
            Console.WriteLine("MaxX: " + maxX);
            Console.WriteLine("MinY: " + minY);
            Console.WriteLine("MaxY: " + maxY);
            Console.WriteLine("MinZ: " + minZ);
            Console.WriteLine("MaxZ: " + maxZ);
            */

            Parallel.ForEach(nodes, n =>
            {
                int _minX = n.X - n.Radius;
                int _maxX = n.X + n.Radius;
                int _minY = n.Y - n.Radius;
                int _maxY = n.Y + n.Radius;
                int _minZ = n.Z - n.Radius;
                int _maxZ = n.Z + n.Radius;

                for (int x = _minX; x <= _maxX; x++)
                {
                    for (int y = _minY; y <= _maxY; y++)
                    {
                        for (int z = _minZ; z <= _maxZ; z++)
                        {
                            if (Math.Abs(x - n.X) + Math.Abs(y - n.Y) + Math.Abs(z - n.Z) <= n.Radius)
                            {
                                try
                                {
                                    nodeBag.Add(new Node(x, y, z));
                                }

                                catch
                                {
                                    throw;
                                }
                            }
                        }
                    }
                }
            }
            );

            Console.ReadKey();
        }
    }

    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int Radius { get; set;}

        public Node()
        { }
        public Node(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
            Radius = 1;
        }
    }
}
