using System;
using System.Diagnostics;
using System.IO;

using AoC.IntCode;

/*
Benchmarking

Output: 84513
Completed in 4610ms (46106755 ticks).
loops: 371206
*/

namespace Day9_Sensor_Boost
{
    class Program
    {
        static IntCodeVM vm;
        static long[] program;
        static Stopwatch sw;

        static void Main(string[] args)
        {
            sw = new Stopwatch();
            program = FetchInput();

            sw.Start();
            vm = new IntCodeVM(1, (long[])program.Clone(), new long[] { 2 });
            #if DEBUG
            //vm.Verbose = true;
            #endif
            vm.Run();
            sw.Stop();

            Console.WriteLine("Output: {0}", vm.Output);
            Console.WriteLine("Completed in {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine("loops: {0}", vm.loopCount);
            
            Console.ReadLine();
            Environment.Exit(0);
        }

        public static long[] FetchInput()
        {
            string[] strArr;

            using (Stream stream = File.OpenRead(@"..\..\..\Day09_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strArr = reader.ReadLine().Split(',');
            }

            return Array.ConvertAll(strArr, s => long.Parse(s));
        }
    }
}
