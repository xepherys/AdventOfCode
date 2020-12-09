using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020_09_Encoding_Error
{
    class Program
    {
        public static Queue<long> data = new Queue<long>();
        public static List<long> workingSet = new List<long>();
        public static int preamble = 25;
        public static long foundValue = -1;
        public static List<long> testPool;

        static void Main(string[] args)
        {
            ImportData();
            foundValue = ParseData1();

            Console.WriteLine($"Part1: {foundValue}");
            Console.Read();

            data.Clear();
            ImportData(foundValue);
            foundValue = ParseData2();

            Console.WriteLine($"Part2: {foundValue}");
            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData(long limiter = -1)
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day9_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null && s != limiter.ToString())
                {
                    data.Enqueue(Int64.Parse(s));
                }
            }
        }

        static long ParseData1()
        {
            long _ret = -1;
            bool matched = true;
            long valueToTest = -1;

            for (int i = 0; i < preamble; i++)
                workingSet.Add(data.Dequeue());

            while (matched)
            {
                valueToTest = data.Peek();
                matched = TryValues(workingSet, valueToTest);

                workingSet.RemoveAt(0);
                workingSet.Add(data.Dequeue());
            }

            _ret = valueToTest;

            return _ret;
        }

        static long ParseData2()
        {
            long _ret = -1;
            long test;
            testPool = data.ToList();
            List<long> runningValues;

            while (testPool.Count > 1)
            {
                runningValues = new List<long>();

                test = 0;
                int i;
                for (i = 0; i < testPool.Count; i++)
                {
                    test += testPool[i];
                    runningValues.Add(testPool[i]);
                    if (test == foundValue)
                        break;
                }

                if (test == foundValue)
                {
                    _ret = runningValues.Min() + runningValues.Max();
                    break;
                }

                else
                {
                    testPool.RemoveAt(0);
                }
            }

            return _ret;
        }

        static bool TryValues(List<long> list, long test)
        {
            bool _ret = false;

            foreach (Tuple<long, long> t in GetPairs())
            {
                if (t.Item1 + t.Item2 == test)
                {
                    _ret = true;
                    break;
                }
            }

            return _ret;
        }

        static List<Tuple<long, long>> GetPairs()
        {
            List<Tuple<long, long>> _ret = new List<Tuple<long, long>>();
            for (int i = 0; i < preamble - 1; i++)
                for (int j = i; j < preamble; j++)
                    _ret.Add(new Tuple<long, long>(workingSet[i], workingSet[j]));

            return _ret;
        }
    }
}
