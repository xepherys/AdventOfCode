using System;
using System.IO;

using XephLibs.AdventOfCode.IntCode;
using XephLibs.Base;

namespace Day11_Space_Police
{
    class Program
    {
        static IntCodeVM vm;
        static long[] program;

        static void Main(string[] args)
        {
            program = FetchInput();

            vm = new IntCodeVM(1, (long[])program.Clone(), new long[] { 0 });
            vm.SpecialInstructions = "PainterBot";
            vm.Debug = true;
            #if DEBUG
            //vm.Verbose = true;
            #endif
            vm.Run();

            Console.WriteLine("Output: {0}", vm.Output);
        }

        public static long[] FetchInput()
        {
            string[] strArr;

            using (Stream stream = File.OpenRead(@"..\..\..\Day11_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strArr = reader.ReadLine().Split(',');
            }

            return Array.ConvertAll(strArr, s => long.Parse(s));
        }
    }
}
