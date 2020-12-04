using System;
using System.Diagnostics;

/*
Part 1:
Parallel:
Total number of valid values in range 183564 - 657474 is: 1610
Found in 387ms (3872009 ticks).

Part 2:
Total number of valid values in range 183564 - 657474 is: 1104
Found in 1154ms (11547021 ticks).

Parallel:
Total number of valid values in range 183564 - 657474 is: 1104
Found in 866ms (8663799 ticks).

Part 1 Violations testing:
Total number of valid values in range 183564 - 657474 is: 1610

Rule Violations:
Rule 1 - Not 6 digits: 0
Rule 2 - Not in range: 0
Rule 3 - Not decreasing: 451019
Rule 4 - No doubles: 27

Found in 503ms (5030338 ticks).

Reorder:
Total number of valid values in range 183564 - 657474 is: 1610

Rule Violations:
Rule 1 - Not 6 digits: 0
Rule 2 - Not in range: 0
Rule 3 - Not decreasing: 446620
Rule 4 - No doubles: 28

Found in 402ms (4021999 ticks).

Total number of valid values in range 183564 - 657474 is: 1610
Found in 304ms (3040450 ticks).

*/

namespace Day4
{
    class Program
    {
        static bool part2;
        static int[] range;
        static int goodValues;
        static int[] violations;

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            goodValues = 0;
            part2 = false;
            violations = new int[4];

            range = new int[] { 183564, 657474 };

            sw.Start();
            
            for (int i = range[0]; i <= range[1]; i++)
            {
                ParseRules(i);
            }
            
            sw.Stop();

            Console.WriteLine("Total number of valid values in range {0} - {1} is: {2}", range[0], range[1], goodValues);
            Console.WriteLine("Found in {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);

            Console.ReadLine();
            Environment.Exit(0);
        }

        static void ParseRules(int valueToTest)
        {
            //Going from left to right, the digits never decrease; they only ever increase or stay the same (like 111123 or 135679).
            if (!CheckForDecrement(valueToTest))
            {
                return;
            }

            //Rule 1: It is a six-digit number.
            if (valueToTest.ToString().Length != 6)
            {
                return;
            }

            //The value is within the range given in your puzzle input.
            if (valueToTest < range[0] || valueToTest > range[1])
            {
                return;
            }

            //Two adjacent digits are the same (like 22 in 122345).
            if (!part2)
            {
                if (!CheckForDouble(valueToTest))
                {
                    return;
                }
            }

            else if (part2)
            {
                if (!CheckForDoublePart2(valueToTest))
                {
                    return;
                }
            }

            

            goodValues++;
        }

        static bool CheckForDouble(int valueToTest)
        {
            string s = valueToTest.ToString();

            for (int i = 0; i < valueToTest.ToString().Length - 1; i++)
            {
                if (s.Substring(i, 1) == s.Substring(i + 1, 1))
                    return true;
            }
            
            return false;
        }

        static bool CheckForDoublePart2(int valueToTest)
        {
            string[] s = ParseAndRemoveThreePlus(valueToTest);

            if (!String.IsNullOrEmpty(s[0]) && CheckForDouble(Convert.ToInt32(s[0])))
                return true;

            else if (!String.IsNullOrEmpty(s[1]) && CheckForDouble(Convert.ToInt32(s[1])))
                return true;

            return false;
        }

        static bool CheckForDecrement(int valueToTest)
        {
            string strValue = valueToTest.ToString();
            string[] strArrValues = new string[6];

            for (int i = 0; i < strValue.Length; i++)
            {
                strArrValues[i] = strValue[i].ToString();
            }

            int[] intArrValues = Array.ConvertAll(strArrValues, int.Parse);

            for (int i = 0; i < intArrValues.Length - 1; i++)
            {
                if (intArrValues[i + 1] < intArrValues[i])
                    return false;
            }

            return true;
        }

        static string[] ParseAndRemoveThreePlus(int valueToParse)
        {
            int len = valueToParse.ToString().Length;
            char[] chars = new char[len];
            bool[] remove = new bool[len];
            string s = valueToParse.ToString();
            string[] _ret = new string[2];   // [0] is before split, [1] is after split.  If 3+ occurs at the beginning, only [1] value will exist.  If 3+ occurs at the end, only [0] value will exist.

            for (int i = 0; i < len - 2; i++)
            {
                if ((s.Substring(i, 1) == s.Substring(i + 1, 1)) && (s.Substring(i + 1, 1) == s.Substring(i + 2, 1)))
                {
                    remove[i] = true;
                    remove[i + 1] = true;
                    remove[i + 2] = true;
                }
            }

            for (int i = 0; i < remove.Length; i++)
            {
                if (!remove[i])
                    chars[i] = s[i];
            }

            int counter = 0;
            string working = "";

            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] != '\0')
                    working += chars[i];

                else if (chars[i] == '\0' && working == "")
                    continue;

                else if (chars[i] == '\0' && !String.IsNullOrEmpty(working))
                {
                    _ret[counter] = working;
                    working = "";
                    counter++;
                }
            }

            _ret[counter] = working;

            return _ret;
        }
    }
}
