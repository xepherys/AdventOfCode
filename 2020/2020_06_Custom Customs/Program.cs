using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using XephLibs.Base.ExtensionMethods;

namespace _2020_06_Custom_Customs
{
    class Program
    {
        // Part 1: 6237 is too low.

        public static List<string> answers = new List<string>();

        static void Main(string[] args)
        {
            ImportData();

            // Part 1
            //Console.WriteLine($"Sum of distinct answers across groups: {ParseData()}");
            // Part 2
            Console.WriteLine($"Sum of absolute answers across groups: {ParseData()}");
            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day6_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                StringBuilder sb = new StringBuilder();
                while ((s = reader.ReadLine()) != null)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        // Part 1
                        //sb.Append(s);

                        // Part 2
                        sb.Append(s + " ");
                    }

                    else
                    {
                        answers.Add(sb.ToString());
                        sb.Clear();
                    }
                }

                answers.Add(sb.ToString());
                sb.Clear();
            }
        }

        static int ParseData()
        {
            int _ret = 0;

            foreach (string s in answers)
            {
                //Console.WriteLine($"{s.Distinct().Count()}: {new string(s.Distinct().OrderBy(d => d).ToArray())}    Running Total: {_ret + s.Distinct().Count()}");

                // Part 1
                //_ret += s.Distinct().Count();

                // Part 2
                int groupMemberCount = s.Split(" ", StringSplitOptions.RemoveEmptyEntries).Length;
                foreach (char c in s.RemoveWhitespace().Distinct())
                    if (s.Count(co => (co == c)) == groupMemberCount)
                        _ret++;
            }

            return _ret;
        }
    }

    
}
