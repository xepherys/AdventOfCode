using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    public class Day1
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        // Day1a is setup for Day1b, as I hadn't yet decided to keep separate functions for each.
        static public void Day1a()
        {
            string[] lines;

            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day1.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                lines = StreamFunctions.EnumerateLines(reader).ToArray();
            }

            int value = 0;
            bool foundFirstDouble = false;
            int iteration = 0;
            Dictionary<int, int> frequency = new Dictionary<int, int>();

            //string[] lines = File.ReadAllLines("./_data/AdventOfCode_Day1.txt");

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
                        MessageBox.Show("First value to hit a frequency of two is: " + value.ToString() + " at iteration: " + iteration.ToString());
                        foundFirstDouble = true;
                        break;
                    }
                }
            }

            MessageBox.Show("Total value of file: " + value.ToString() + " after " + iteration.ToString() + " iterations.");
        }


        #region Support Methods
        
        #endregion
    }
}