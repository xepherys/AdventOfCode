using System;
using System.Collections.Generic;
using System.IO;

namespace _2020_03_Toboggan_Trajectory
{
    // Part 2:  428316104  is too low

    class Program
    {
        static int[] slope = { 3, 1 };
        static List<int[]> slopes = new List<int[]> { new int[] { 1, 1 }, new int[] { 3, 1 }, new int[] { 5, 1 }, new int[] { 7, 1 }, new int[] { 1, 2 } };
        static char[,] map;
        static int width;
        static int height;
        static int chunks;

        static void Main(string[] args)
        {
            ParseInput();
            //DisplayMap();

            bool part1Complete = Plot();
            //DisplayMap();
            Console.WriteLine("Part 1: ");
            Console.WriteLine($"Reached bottom? {part1Complete}");
            int part1Trees = CountTrees();
            Console.WriteLine($"Trees encountered? {part1Trees}");
            Console.WriteLine(Environment.NewLine);


            Console.WriteLine("Part 2: ");
            List<int> trees = new List<int>();
            foreach (int[] v in slopes)
            {
                slope = v;
                Console.WriteLine($"{v[0]}, {v[1]}: ");
                ParseInput();
                Plot();
                int t = CountTrees();
                Console.WriteLine($"Trees encountered? {t}");
                trees.Add(t);
            }

            long treeProduct = trees.Product();

            Console.WriteLine($"Product of trees: {treeProduct}");

            Console.Read();
            Environment.Exit(0);
        }

        static void ParseInput()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\..\Day3_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                List<string> lines = new List<string>();
                int lineCount = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                    lineCount++;
                }

                height = lineCount;
                width = lines[0].Length;
                chunks = (height * slope[0]) / width + 1;

                map = new char[width * chunks, height];

                for (int c = 0; c < chunks; c++)
                {
                    for (int w = 0; w < width; w++)
                    {
                        for (int h = 0; h < height; h++)
                        {
                            map[w + (c * width), h] = lines[h][w];
                        }
                    }
                }
            }
        }

        static void DisplayMap()
        {
            for (int y = 0; y < map.GetLength(1); y++)
            {
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    Console.Write(map[x, y]);
                }
                Console.Write(Environment.NewLine);
            }
        }

        static bool Plot()
        {
            int x = slope[0];
            int y = slope[1];

            while (x < width * chunks && y < height)
            {
                if (map[x, y] == '.')
                    map[x, y] = 'O';
                else if (map[x, y] == '#')
                    map[x, y] = 'X';

                x += slope[0];
                y += slope[1];
            }

            if (y == height)
                return true;

            return false;
        }

        static int CountTrees()
        {
            int _ret = 0;

            for (int x = 0; x < map.GetLength(0); x++)
                for (int y = 0; y < map.GetLength(1); y++)
                    if (map[x, y] == 'X')
                        _ret++;

            return _ret;
        }
    }

    public static class Extensions
    {
        public static long Product(this List<int> source)
        {
            long _ret = source[0];

            for (int i = 1; i < source.Count; i++)
                _ret *= source[i];

            return _ret;
        }

        public static long Product(this List<long> source)
        {
            long _ret = source[0];

            for (int i = 1; i < source.Count; i++)
                _ret *= source[i];

            return _ret;
        }
    }
}
