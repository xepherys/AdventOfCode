using System;
using System.Diagnostics;
using System.IO;

using XephLibs.AdventOfCode.IntCode;

/*
Part 1 result: 3765464
Part 2 values: 76, 10
Completed in 133 ms (1337175 ticks).
*/

namespace Day2
{
    class Program
    {
        #region Setup
        static IntCodeVM vm;
        static long[] program;
        #endregion

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            long result = 0;
            long maxValue = 100;
            program = FetchInput();
            long noun = 1;
            long verb = 1;
            long target = 19690720;
            
            {
                long[] prog = (long[])program.Clone();
                prog[1] = 12;
                prog[2] = 2;
                vm = new IntCodeVM(1, prog);
                #if DEBUG
                //vm.Verbose = true;
                #endif
                vm.SpecialInstructions = "Day2";
                vm.Run();

                result = vm.Output;
            }

            Console.WriteLine("Part 1 result: {0}", result);
            
            while (result != target)
            {
                long[] prog = (long[])program.Clone();
                prog[1] = noun;
                prog[2] = verb;
                vm = new IntCodeVM(1, prog);
                #if DEBUG
                //vm.Verbose = true;
                #endif
                vm.SpecialInstructions = "Day2";
                vm.Run();
                result = vm.Output;

                if (result == target)
                    break;

                if (verb < maxValue)
                    verb++;
                else if (noun < maxValue)
                {
                    noun++;
                    verb = 0;
                }
                else
                {
                    Console.WriteLine("No combination resulted in {0} with maxValue of {1}.", target, maxValue);
                }
            }
            
            Console.WriteLine("Part 2 values: {0}, {1}", noun, verb);
            sw.Stop();

            Console.WriteLine("Completed in {0} ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.Read();
            Environment.Exit(0);
        }

        public static long[] FetchInput()
        {
            string[] strArr;

            using (Stream stream = File.OpenRead(@"..\..\..\Day02_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strArr = reader.ReadLine().Split(',');
            }

            return Array.ConvertAll(strArr, s => long.Parse(s));
        }
    }
}
