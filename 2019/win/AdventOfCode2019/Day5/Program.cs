using System;
using System.IO;

using AoC.IntCode;

/*
Part 1: 352119 is too low.


Part 2: 11193703 is too low.
        11159911 is too low.
*/

namespace Day5
{
    class Program
    {
        static IntCodeVM vm;
        static long[] program;

        static void Main(string[] args)
        {
            program = FetchInput();
            vm = new IntCodeVM(1, (long[])program.Clone(), new long[] { 1 });
            #if DEBUG
            vm.Verbose = true;
            #endif
            vm.Run();

            Console.WriteLine("Part 1: {0}", vm.Output);

            vm = new IntCodeVM(1, (long[])program.Clone(), new long[] { 5 });
            #if DEBUG
            vm.Verbose = true;
            #endif
            vm.Run();

            Console.WriteLine("Part 2: {0}", vm.Output);


            Console.ReadLine();
            Environment.Exit(0);
        }

        public static long[] FetchInput()
        {
            string[] strArr;

            using (Stream stream = File.OpenRead(@"..\..\..\Day05_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strArr = reader.ReadLine().Split(',');
            }

            return Array.ConvertAll(strArr, s => long.Parse(s));
        }
    }
}
