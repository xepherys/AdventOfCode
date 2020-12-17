using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace _2020_14_Docking_Data
{
    class Program
    {
        // Part 2: 2722954324463544 (too high)

        public static Dictionary<int, long> memory = new Dictionary<int, long>();
        public static Dictionary<int, List<long>> memory2 = new Dictionary<int, List<long>>();
        public static string mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
        public static int part = 1;

        static void Main(string[] args)
        {
            ParseData();
            long answer = GetAnswer();

            Console.WriteLine(String.Format("Part 1 answer: {0}", answer));
            Console.Read();


            memory.Clear();
            mask = "XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX";
            part++;
            ParseData();
            answer = GetAnswer2();

            foreach (KeyValuePair<int, List<long>> kvp in memory2)
                foreach (long l in kvp.Value)
                    Console.WriteLine(l);

            Console.WriteLine(String.Format("Part 2 answer: {0}", answer));
            Console.Read();
            Environment.Exit(0);
        }

        static void ParseData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day14_InputSample2.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null)
                {
                    if (part == 1)
                    {
                        if (s.Substring(0, 3) == "mas")
                            ParseMask(s);

                        else if (s.Substring(0, 3) == "mem")
                            ParseValue(s);
                    }

                    else if (part == 2)
                    {
                        if (s.Substring(0, 3) == "mas")
                            ParseMask2(s);

                        else if (s.Substring(0, 3) == "mem")
                            ParseValue2(s);
                    }
                }

            }
        }

        static long GetAnswer()
        {
            long _ret = 0;

            foreach (KeyValuePair<int, long> kvp in memory)
                _ret += kvp.Value;

            return _ret;
        }

        static long GetAnswer2()
        {
            long _ret = 0;

            foreach (KeyValuePair<int, List<long>> kvp in memory2)
            {
                foreach (long l in kvp.Value)
                {
                    _ret += l;
                }
            }

            return _ret;
        }

        static void ParseMask(string s)
        {
            mask = s.Substring(7, 36);
        }

        static void ParseValue(string s)
        {
            int memoryAddress;
            long value;

            string exp = @"mem[[](\d+)[]] = (\d+)";
            Regex r = new Regex(exp, RegexOptions.IgnoreCase);

            Match m = r.Match(s);
            memoryAddress = Int32.Parse(m.Groups[1].ToString());
            value = Int64.Parse(m.Groups[2].ToString());

            string val = Convert.ToString(value, 2);
            val = val.PadLeft(36, '0');

            StringBuilder sb = new StringBuilder(val);

            for (int i = 0; i < 36; i++)
            {
                if (mask[i] == '0')
                    sb[i] = '0';

                if (mask[i] == '1')
                    sb[i] = '1';
            }

            long longVal = Convert.ToInt64(sb.ToString(), 2);

            memory[memoryAddress] = longVal;
        }

        static void ParseMask2(string s)
        {
            mask = s.Substring(7, 36);
        }

        static void ParseValue2(string s)
        {
            int memoryAddress;
            List<long> values = new List<long>();
            List<string> binaryStrings = new List<string>();

            string exp = @"mem[[](\d+)[]] = (\d+)";
            Regex r = new Regex(exp, RegexOptions.IgnoreCase);

            Match m = r.Match(s);
            memoryAddress = Int32.Parse(m.Groups[1].ToString());
            long value = Int64.Parse(m.Groups[2].ToString());

            string val = Convert.ToString(value, 2);
            //val = val.PadLeft(36, '0');

            string imask = mask.Substring(mask.Length - val.Length, val.Length);

            int[] xIndices = imask.IndicesOf('X');

            StringBuilder sb = new StringBuilder(val);

            for (int i = 0; i < val.Length; i++)
            {
                if (mask[i] == '1')
                    sb[i] = '1';
            }

            int combinations = (int)Math.Pow(2, xIndices.Length);
            Console.WriteLine("Number of combinations: " + combinations);

            for (int i = 0; i < combinations; i++)
            {
                string ss = Convert.ToString(i, 2).PadLeft(xIndices.Length, '0');
                //Console.WriteLine(s);
                StringBuilder sb2 = new StringBuilder(sb.ToString());

                for (int j = 0; j < xIndices.Length; j++)
                {
                    //Console.WriteLine($"i: {i} j: {j} i + 1 & j: {i + 1 & j}");
                    if (ss[j] == '1')
                    {
                        sb2[xIndices[j]] = '1';
                    }

                    else
                    {
                        sb2[xIndices[j]] = '0';
                    }
                }

                if (!binaryStrings.Contains(sb2.ToString()))
                    binaryStrings.Add(sb2.ToString());

                //if (!values.Contains(Convert.ToInt64(sb2.ToString(), 2)))
                //	values.Add(Convert.ToInt64(sb2.ToString(), 2));
            }

        memory2[memoryAddress] = values;
        }
    }


    public static class StringExtnesions
    {
        public static int[] IndicesOf(this string source, char value)
        {
            List<int> _retList = new List<int>();

            for (int i = 0; i < source.Length; i++)
            {
                if (source[i] == value)
                    _retList.Add(i);
            }

            return (_retList.Count() > 0) ? _retList.ToArray() : null;
        }
    }
}