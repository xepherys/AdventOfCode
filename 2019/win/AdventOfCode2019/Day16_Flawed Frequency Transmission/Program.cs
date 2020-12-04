using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AoC.Collections;
using AoC.Extensions;

/*
Part A:
Output after 100 phases is 82525123.
Completed in 289 ms (2890166 ticks).

Part B:
Offset: 2720837

Output after 100 phases is 49476260.
Completed in 2980 ms (29804504 ticks).
*/

namespace Day16_Flawed_Frequency_Transmission
{
    class Program
    {
        static int[] input;
        static List<int> pattern;
        static List<int> tempOutput;
        static int φ;
        static readonly RollingList<int> basePattern = new RollingList<int> { 0, 1, 0, -1 };
        static bool partB = false;
        static int offset = 0;

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            int maxφ = 100;
            FetchInput();

            sw.Start();
            Console.WriteLine("Part A:");
                
            for (φ = 1; φ <= maxφ; φ++)
            {
                FetchOutputPattern(φ);
            }
                
            sw.Stop();
            Console.WriteLine("Output after {0} phases is {1}.", maxφ, input.ArrToString(offset, 8, false));
            Console.WriteLine("Completed in {0} ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Reset();

            sw.Start();
            Console.WriteLine();
            Console.WriteLine("Part B:");

            partB = true;
            FetchInput();

            offset = (input[0] * 1000000) + (input[1] * 100000) + (input[2] * 10000) + (input[3] * 1000) + (input[4] * 100) + (input[5] * 10) + (input[6] * 1);
            offset = offset - input.Length;
            Console.WriteLine("Offset: {0}", offset);

            for (φ = 1; φ <= maxφ; φ++)
            {
                CalcPartB();
            }

            Console.WriteLine();
            Console.WriteLine("Output after {0} phases is {1}.", maxφ, input.ArrToString(offset, 8, false));
            Console.WriteLine("Completed in {0} ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.ReadLine();
            Environment.Exit(0);
        }

        static void CalcPartB()
        {
            for (int i = input.Length - 2; i >= 0; i--)
            {
                int k = 0;
                for (int j = i; j < i + 2; j++)
                {
                    k = (k + input[j]) % 10;
                }

                input[i] = k;
            }
        }

        public static void FetchInput()
        {
            #if DEBUG
            Stopwatch sw = new Stopwatch();
            #endif
            string inputString = String.Empty;
            string line;

            using (Stream stream = File.OpenRead(@"..\..\..\Day16_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    inputString += line;
                }
            }

            if (partB)
            {
                #if DEBUG
                sw.Start();
                #endif
                offset = Convert.ToInt32(inputString.Substring(0, 7));
                inputString = String.Concat(Enumerable.Repeat(inputString, 5000));
                #if DEBUG
                sw.Stop();
                Console.WriteLine("Multiplying inputString took {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
                sw.Reset();
                #endif
            }

            input = new int[inputString.Length];

            #if DEBUG
            sw.Start();
            #endif
            for (int i = 0; i < inputString.Length; i++)
            {
                Int32.TryParse(inputString[i].ToString(), out input[i]);
            }
            #if DEBUG
            sw.Stop();
            Console.WriteLine("Parsing input to int[] took {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            sw.Reset();
            #endif
        }

        public static void FetchOutputPattern(int loop)
        {
            tempOutput = new List<int>();

            for (int innerLoop = 0; innerLoop < input.Length; innerLoop++)
            {
                GetNewPattern(innerLoop + 1);
                tempOutput.Add(GetValue(input, pattern));
            }

            tempOutput.CopyTo(input);
        }

        public static int GetValue(int[] b, List<int> p)
        {
            int _ret = 0;

            for (int i = 0; i < b.Length; i++)
            {
                _ret += b[i] * p[i];
            }

            string s = _ret.ToString();
            _ret = Convert.ToInt32(s.Substring(s.Length - 1, 1));

            return _ret;
        }

        public static void GetNewPattern(int val)
        {
            basePattern.Reset();
            pattern = new List<int>();
            int j = 0;

            while (pattern.Count <= input.Length)
            {
                int add = basePattern.Next();
                for (int i = 0; i < val; i++)
                {
                    pattern.Add(add);
                    if (pattern.Count > input.Length)
                        break;
                }
                j++;
            }

            pattern.Remove(0);
        }
    }
}
