using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*
Best pre-mod part 1:
Parallel:
Total number of valid values in range 183564 - 657474 is: 1610
Found in 387ms (3872009 ticks).


Part 1:
Non-parallel
Good values: 1610
Found in 165ms (1652795 ticks).

Parallel:
Good values: 1610
Found in 138ms (1386691 ticks).

Mod2, but not range limited:
Good values: 4795
Found in 0ms (26 ticks).


*/

namespace Day4Mod
{
    class Program
    {
        static int goodValues;
        static int[] range;
        static Object lockObj;

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            goodValues = 0;
            lockObj = "1";
            int min = 100;
            int max = Int32.MaxValue;

            range = new int[] { 183564, 657474 };

            sw.Start();

            Parallel.For(range[0], range[1] + 1, i =>
                {
                    string s = i.ToString();

                    if (s.Length < 6)
                        return;

                    byte[] valByte = Encoding.ASCII.GetBytes(s);

                    if (valByte[0] > valByte[1] || valByte[1] > valByte[2] || valByte[2] > valByte[3] || valByte[3] > valByte[4] || valByte[4] > valByte[5])
                        return;

                    if (valByte[0] == valByte[1] || valByte[1] == valByte[2] || valByte[2] == valByte[3] || valByte[3] == valByte[4] || valByte[4] == valByte[5])
                    {
                        if (i >= range[0] && i <= range[1])
                            lock (lockObj)
                                goodValues++;
                    }
                }
            );
            sw.Stop();

            Console.WriteLine("Good values: {0}", goodValues);
            Console.WriteLine("Found in {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);

            Console.ReadLine();
            Environment.Exit(0);
        }


    }
}