using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace _2020_18_Operation_Order
{
    class Program
    {
        public static List<string> problems = new List<string>();
        public static long answer = 0;

        static void Main(string[] args)
        {
            ImportData();

            /*
            string[] samples = new string[] { "1 + 2 * 3 + 4 * 5 + 6",
                                              "1 + (2 * 3) + (4 * (5 + 6))",
                                              "2 * 3 + (4 * 5)",
                                              "5 + (8 * 3 + 9 + 3 * 4 * 3)",
                                              "5 * 9 * (7 * 3 * 3 + 9 * 3 + (8 + 6 * 4))",
                                              "((2 + 4 * 9) * (6 + 9 * 8 + 6) + 6) + 2 + 4 * 2" };

            foreach (string s in samples)
            {
                Console.WriteLine(s);
                Console.WriteLine(EvaluateProblem(s) + "\n");
            }
            */

            foreach (string s in problems)
            {
                Console.WriteLine(s);
                long a = EvaluateProblem(s);
                Console.WriteLine(a + "\n");
                answer += a;
            }

            Console.WriteLine($"\n\nSum of all problems: {answer}");

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day18_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null)
                {
                    problems.Add(s);
                }
            }
        }

        static long EvaluateProblem(string s)
        {
            string problem = s;

            int start = -1;
            int stop = -1;

            while (problem.Contains('('))
            {
                if (start == -1)
                    start = problem.IndexOf('(');

                for (int i = start + 1; i < problem.Length; i++)
                {
                    if (problem[i] == '(')
                    {
                        start = i;
                        break;
                    }

                    else if (problem[i] == ')')
                    {
                        stop = i;
                        break;
                    }
                }

                if (start > -1 && stop > start)
                {
                    string rep = problem.Substring(start + 1, stop - start - 1);
                    long val = Solve(rep);
                    problem = problem.Replace("(" + rep + ")", val.ToString());

                    start = -1;
                    stop = -1;
                    Console.WriteLine(problem);
                }
            }

            return Solve(problem);
        }

        static long Solve(string s)
        {
            long _ret = 0;

            string[] p = s.Split(' ');

            _ret = Convert.ToInt32(p[0]);

            for (int i = 1; i < p.Length; i++)
            {
                if (p[i] == "+")
                    _ret += Convert.ToInt32(p[++i]);

                else if (p[i] == "*")
                    _ret *= Convert.ToInt32(p[++i]);
            }

            return _ret;
        }
    }
}
