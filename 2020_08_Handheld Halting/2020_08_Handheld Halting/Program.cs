using System;
using System.Collections.Generic;
using System.IO;

using AoC;

namespace _2020_08_Handheld_Halting
{
    class Program
    {
        public static GameConsole c = new GameConsole();

        static void Main(string[] args)
        {
            ImportData();

            int changed = 0;
            c.ops.FixAndRun(out changed);

            Console.WriteLine(c.ops.Accumulator + "   Change at idx: " + changed);
            Console.WriteLine(c.ops.Ops[changed]);
            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            List<string> ops = new List<string>();

            using (Stream stream = File.OpenRead(@"..\..\..\Day8_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    ops.Add(s);
                }
            }

            c.ops.Init(ops);
        }
    }
}
