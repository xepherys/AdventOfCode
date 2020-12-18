using System;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Linq;

namespace _2020_15_Rambunctious_Recitation
{
    class Program
    {
        static long[] dataArraySample = new long[] { 0, 3, 6 };
        static long[] dataArraySample2 = new long[] { 3, 1, 2 };
        static long[] dataArray = new long[] { 0, 8, 15, 2, 12, 1, 4 };
        static int targetValue = 30000000;//2020;

        static void Main(string[] args)
        {
            Dictionary<int, long> game = new Dictionary<int, long>();
            for (int i = 1; i <= dataArray.Length; i++)
            {
                game[i] = dataArray[i - 1];
            }

            int turnNumber = game.Count();

            //Stopwatch sw = new Stopwatch();

            //sw.Start();
            while (++turnNumber <= targetValue)
            {
                long val = game[turnNumber - 1];

                if (game.Where(w => w.Value == val).Count() > 1)
                {
                    List<int> addresses = game.KeysOf(val).OrderBy(o => o).ToList();
                    int[] add = new int[2];

                    add[0] = addresses.Max();
                    addresses.Remove(addresses.Max());
                    add[1] = addresses.Max();
                    addresses.Remove(addresses.Max());

                    foreach (var v in addresses)
                        game.Remove(v);

                    game[turnNumber] = (add[0] - add[1]);
                }

                else
                    game[turnNumber] = 0;
            }
            //sw.Stop();

            Console.WriteLine(game[targetValue]);
            //Console.WriteLine($"Found in {sw.Elapsed.TotalMilliseconds}ms ({sw.ElapsedTicks} ticks).");
            Console.Read();
        }
    }

    public static class ListExtensions
    {
        public static List<int> IndicesOf<T>(this List<T> source, T value)
        {
            List<int> _retList = new List<int>();

            for (int i = 0; i < source.Count(); i++)
            {
                if (source[i].Equals(value))
                {
                    _retList.Add(i);
                }
            }

            return (_retList.Count() > 0) ? _retList : null;
        }
    }

    public static class DictionaryExtensions
    {
        public static List<U> KeysOf<U, T>(this Dictionary<U, T> source, T value)
        {
            List<U> _retList = new List<U>();

            foreach (KeyValuePair<U, T> kvp in source)
            {
                if (kvp.Value.Equals(value))
                {
                    _retList.Add(kvp.Key);
                }
            }

            return (_retList.Count() > 0) ? _retList : null;
        }
    }
}
