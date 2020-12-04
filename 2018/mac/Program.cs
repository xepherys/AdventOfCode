using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace AdventOfCode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to execute Day 2b.");
            Console.ReadKey();
            Day2b();
        }

        static public void Day1()
        {
            int value = 0;
            bool foundFirstDouble = false;
            int iteration = 0;
            Dictionary<int, int> frequency = new Dictionary<int, int>();
            string[] lines = File.ReadAllLines("./_data/AdventOfCode_Day1.txt");

            if (frequency.ContainsKey(value))
                frequency[value]++;
            else
                frequency.Add(value, 1);

            while (foundFirstDouble == false)
            {
                foreach (string s in lines)
                {
                    iteration++;
                    string ss = s.Trim();
                    string _operator = ss.Substring(0, 1);
                    int _value = Convert.ToInt32(ss.Substring(1, ss.Length - 1));

                    if (_operator == "-")
                    {
                        value = value - _value;
                    }
                    else if (_operator == "+")
                    {
                        value = value + _value;
                    }

                    if (frequency.ContainsKey(value))
                        frequency[value]++;
                    else
                        frequency.Add(value, 1);

                    if (frequency[value] > 1 && foundFirstDouble == false)
                    {
                        Console.WriteLine("First value to hit a frequency of two is: " + value.ToString() + " at iteration: " + iteration.ToString());
                        foundFirstDouble = true;
                        break;
                    }
                }
            }

            Console.Write("Total value of file: " + value.ToString() + " after " + iteration.ToString() + " iterations.");
        }

        static public void Day2a()
        {
            string[] lines = File.ReadAllLines("./_data/AdventOfCode_Day2.txt");

            int count2 = 0;
            int count3 = 0;


            foreach (string s in lines)
            {
                bool got2s = false;
                bool got3s = false;

                char[] carr = s.ToCharArray();
                Dictionary<char, int> counts = new Dictionary<char, int>();

                foreach (char c in carr)
                {
                    if (counts.ContainsKey(c))
                        counts[c]++;
                    else
                        counts.Add(c, 1);
                }

                foreach (KeyValuePair<char, int> kvp in counts)
                {
                    if (kvp.Value == 3 && !got3s)
                    {
                        count3++;
                        got3s = true;
                    }
                    else if (kvp.Value == 2 && !got2s)
                    {
                        count2++;
                        got2s = true;
                    }
                }
            }

            Console.WriteLine("3-sets: " + count3.ToString() + "\n" + "2-sets: " + count2.ToString() + "\n\n" +
                              "Checksum: " + (count3 * count2).ToString());
        }
    
        static public void Day2b()
        {
            Stopwatch sw = new Stopwatch();
            string[] lines = File.ReadAllLines("/Users/xepherys/Desktop/AdventOfCode_Day2.txt");
            string match1 = "";
            string match2 = "";
            int placevalue = 0;
            int iteration = 0;
            bool found = false;
            lines = lines.OrderBy(a => a).ToArray();
            List<string> comparelines = new List<string>(lines);

            while (iteration < lines.Count() && !found)
            {
                sw.Start();
                foreach (string s in lines)
                {
                    comparelines.Remove(s);
                    string delta;

                    foreach (string comparer in comparelines)
                    {
                        iteration++;
                        delta = "";
                        if (match1 == "")
                        {
                            for (int i = 0; i < 26; i++)
                            {
                                if (comparer[i] != s[i])
                                {
                                    delta += comparer[i];
                                    placevalue = i;
                                }
                                if (delta.Length > 1)
                                    break;
                            }

                            if (delta.Length == 1)
                            {
                                match1 = s;
                                match2 = comparer;
                                found = true;
                                break;
                            }
                        }

                        if (found)
                            break;
                    }
                    if (found)
                        break;
                }
                sw.Stop();
            }

            Console.WriteLine("Matched lines are: " + match1 + " & " + match2);
            string answer = match1.Remove(placevalue, 1);
            Console.WriteLine("Answer is: " + answer);
            Console.WriteLine("Found in " + sw.ElapsedMilliseconds + " ms. (" + sw.ElapsedTicks + " ticks)" + "\n" + "Found in " + iteration + " iterations.");
        }
    } 
}