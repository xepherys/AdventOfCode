using System;
using System.IO;

namespace _2020_13_Shuttle_Search
{
    class Program
    {
        public static int timeOfArrival;
        public static int[] busses;

        static void Main(string[] args)
        {
            ImportDataPart1();

            Console.WriteLine($"Time of arrival: {timeOfArrival}");
            for (int i = 0; i < busses.Length; i++)
                Console.WriteLine($"Bus: {busses[i]}");

            long answer = ParsePart1();

            Console.WriteLine();
            Console.WriteLine($"Answer (p1): {answer}");

            answer = ParsePart2();

            Console.WriteLine();
            Console.WriteLine($"Answer (p2): {answer}");


            Console.Read();
            Environment.Exit(0);
        }

        static void ImportDataPart1()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day13_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                timeOfArrival = Int32.Parse(reader.ReadLine());
                string s = reader.ReadLine();

                string[] sArr = s.Split(",", StringSplitOptions.RemoveEmptyEntries);
                busses = ParseBusses(sArr);
            }
        }

        static long ParsePart1()
        {
            int[] r = new int[busses.Length];
            int index = -1;
            int minTime = Int32.MaxValue;

            for (int i = 0; i < busses.Length; i++)
            {
                int t = (busses[i] > 0) ? (int)Math.Ceiling((decimal)timeOfArrival / (decimal)busses[i]) : 0;
                r[i] = t * busses[i];

                if (r[i] < minTime && r[i] > 0)
                {
                    index = i;
                    minTime = r[i];
                }
            }

            return (r[index] - timeOfArrival) * busses[index];
        }

        static long ParsePart2()
        {
            // (x + times[0]) % times[1] == 0 for all buses. Find x.
            // Chinese Remainder Theorem
            long[,] times = new long[busses.Length, 2];

            long product = 1;
            for (int i = 0; i < busses.Length; i++)
            {
                if (busses[i] != 0)
                {
                    product *= busses[i];
                    times[i, 0] = i;
                    times[i, 1] = busses[i];
                }
            }

            long total = 0;

            for (int i = 0; i < busses.Length; i++)
            {
                if (times[i, 1] != 0)
                {
                    long factor = 0;

                    long term = (product / times[i, 1]);

                    while (((term * factor + times[i, 0]) % times[i, 1]) != 0)
                    {
                        factor += 1;
                    }

                    total = total + term * factor;
                }
            }

            return total % product;
        }

        static int[] ParseBusses(string[] val)
        {
            int[] _ret = new int[val.Length];

            for (int i = 0; i < val.Length; i++)
            {
                if (val[i] == "x")
                    _ret[i] = 0;

                else
                    _ret[i] = Int32.Parse(val[i]);
            }

            return _ret;
        }
    }
}
