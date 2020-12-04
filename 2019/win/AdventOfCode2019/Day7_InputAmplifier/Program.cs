using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

using AoC.Extensions;
using AoC.IntCode;


namespace Day7_InputAmplifier
{
    class Program
    {
        #region Setup
        static int workers;
        static IntCodeVM vm;
        static long[] program;
        static long[] ampValuesPart1 = { 0, 1, 2, 3, 4 };
        static long[] ampValuesPart2 = { 5, 6, 7, 8, 9 };
        static long[][] ampCodes;
        static List<long[]> valLists = new List<long[]>();
        static bool part1 = false;
        static bool part2 = false;
        #endregion

        static void Main(string[] args)
        {
            workers = 5;
            program = FetchInput();
            part1 = false;
            part2 = true;

            long highestValue = 0;
            long[] highestValueArray = null;

            if (part1)
            {
                ampCodes = GetAllAmpCodePositions(ampValuesPart1);

                foreach (long[] iArr in ampCodes.Where(w => w != null))
                {
                    vm = new IntCodeVM(workers, (long[])program.Clone());
                    vm.PhaseSettings = iArr;
                    #if DEBUG
                    vm.Verbose = true;
                    #endif
                    vm.Run();

                    if (vm.Output > highestValue)
                    {
                        highestValue = vm.Output;
                        highestValueArray = (long[])iArr.Clone();
                    }
                }

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Part 1: highest value is: {0} from Amp Code {{{1}}}", highestValue, highestValueArray.ArrToString());
                Console.ReadLine();
            }
            highestValue = 0;
            highestValueArray = null;

            if (part2)
            {
                ampCodes = GetAllAmpCodePositions(ampValuesPart2);

                foreach (long[] iArr in ampCodes.Where(w => w != null))
                {
                    //int[] iArr = new int[] { 9, 8, 7, 6, 5 };
                    Console.WriteLine("Beginning part 2 work with Amp Code set {{{0}}}.", iArr.ArrToString());

                    vm = new IntCodeVM(workers, (long[])program.Clone());
                    vm.PhaseSettings = iArr;
                    vm.SpecialInstructions = "loop";
                    vm.Debug = true;
                    #if DEBUG
                    vm.Verbose = true;
                    #endif
                    vm.Run();

                    Console.WriteLine("Part 2 work with Amp Code set {{{0}}} gave result: {1}.", iArr.ArrToString(), vm.Output);

                    if (vm.Output > highestValue)
                    {
                        highestValue = vm.Output;
                        highestValueArray = (long[])iArr.Clone();
                    }
                }

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Part 2: highest value is: {0} from Amp Code {{{1}}}", highestValue, highestValueArray.ArrToString());
                Console.ReadLine();
            }

            Environment.Exit(0);

        }

        public static long[][] GetAllAmpCodePositions(long[] input)
        {
            valLists.Clear();

            GetPer(input);

            return valLists.ToArray();
        }

        private static void Swap(ref long a, ref long b)
        {
            if (a == b) return;

            var temp = a;
            a = b;
            b = temp;
        }

        public static void GetPer(long[] list)
        {
            int x = list.Length - 1;
            GetPer(list, 0, x);
        }

        private static void GetPer(long[] list, long k, long m)
        {
            if (k == m)
            {
                valLists.Add((long[])list.Clone());
            }
            else
                for (long i = k; i <= m; i++)
                {
                    Swap(ref list[k], ref list[i]);
                    GetPer(list, k + 1, m);
                    Swap(ref list[k], ref list[i]);
                }
        }
        
        /// <summary>
        /// Permutation counts are based on the number of possible values, and the number of values per
        /// permutation.
        /// 
        /// The formula for the count is:
        /// 
        ///               n!
        /// P(n, r) = ----------
        ///            (n - r)!
        /// 
        /// </summary>
        /// <example>
        /// For (n, r) where n == 10 and r == 4, 
        /// 
        ///               10!          3628800
        /// P(n, r) = -----------  =  ---------  =  5040 possible permutations of 0-9 when each permutation contains 4 unique digits
        ///            (10 - 4)!         720
        /// 
        /// </example>
        /// <param name="valueOptions"></param>
        /// <param name="valueCountPerPermutation"></param>
        /// <returns></returns>
        public static int PermutationCount(int n, int r)
        {
            return Factorial(n) / Factorial(n - r);
        }

        public static long PermutationCount(long n, long r)
        {
            return Factorial(n) / Factorial(n - r);
        }

        public static BigInteger PermutationCount(BigInteger n, BigInteger r)
        {
            return Factorial(n) / Factorial(n - r);
        }

        public static int Factorial(int n)
        {
            int res = 1;
            while (n != 1)
            {
                res = res * n;
                n = n - 1;
            }
            return res;
        }

        public static long Factorial(long n)
        {
            long res = 1;
            while (n != 1)
            {
                res = res * n;
                n = n - 1;
            }
            return res;
        }

        public static BigInteger Factorial(BigInteger n)
        {
            BigInteger res = 1;
            while (n != 1)
            {
                res = res * n;
                n = n - 1;
            }
            return res;
        }

        public static long[] FetchInput()
        {
            string[] strArr;

            using (Stream stream = File.OpenRead(@"..\..\..\Day07_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                strArr = reader.ReadLine().Split(',');
            }

            return Array.ConvertAll(strArr, s => long.Parse(s));
        }

        public static void ParseInstruction(string instr, out string op, out string param)
        {
            op = "";
            param = "";

            if (instr.Length == 1)
                op = instr;

            else if (instr.Length == 2)
                op = instr;

            else
            {
                op = instr.Substring(instr.Length - 2, 2);
                param = ReverseStringDirect(instr.Substring(0, instr.Length - 2));
            }


            if (op[0] == '0')           // Clean up leading zeroes for single digit opcode values
                op = op.Remove(0, 1);

            param = param.PadRight(6, '0');
        }

        public static string ReverseStringDirect(string s)
        {
            char[] array = new char[s.Length];
            int forward = 0;
            for (int i = s.Length - 1; i >= 0; i--)
            {
                array[forward++] = s[i];
            }
            return new string(array);
        }
    }
}
