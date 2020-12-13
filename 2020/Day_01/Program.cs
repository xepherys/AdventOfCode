using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace _2020_01_Report_Repair
{
    class Program
    {
        static List<int> values = new List<int>();
        static int targetSum = 2020;

        static void Main(string[] args)
        {
            using (Stream stream = File.OpenRead(@"..\..\..\..\Day1_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    int.TryParse(line, out int i);
                    if (i >= 0)
                        values.Add(i);
                }
            }

            Console.WriteLine(String.Format("Read in {0} values.", values.Count));

            FindTwo();
            FindThree();
            Console.Read();
            Environment.Exit(0);
        }

        static void FindTwo()
        {
            foreach (int i in values)
                foreach (int j in values.Where(w => w != i))
                    if (i + j == targetSum)
                    {
                        Console.WriteLine(String.Format("Part 1: {0} and {1} give {2}", i, j, i * j));
                        return;
                    }
        }

        static void FindThree()
        {
            foreach (int i in values)
                foreach (int j in values.Where(w => w != i))
                    foreach (int k in values.Where(w => w != i && w != j))
                        if (i + j + k == targetSum)
                        {
                            Console.WriteLine(String.Format("Part 2: {0} and {1} and {2} give {3}", i, j, k, i * j * k));
                            return;
                        }
        }
    }
}