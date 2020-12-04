using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdventOfCode2018.Core;
using Xepherys.AlphabetProviders;

namespace AdventOfCode2018
{
    class Day7
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        // Day7a acts also as a method of providing data to Day7b and thus has a return value.
        public static Day7aPackage Day7a()
        {
            List<string> steps = new List<string>();
            string line;
            string order = String.Empty;
            EnglishAlphabetProvider provider = new EnglishAlphabetProvider();
            int onStepNumber = 0;
            HashSet<Tuple<char, char>> hash = new HashSet<Tuple<char, char>>();
            HashSet<Tuple<char, char>> hashreturn = new HashSet<Tuple<char, char>>();
            HashSet<char> chash = new HashSet<char>();
            List<char> result;
            Stopwatch sw = new Stopwatch();

            try
            {
                foreach (char c in provider.GetAlphabet())
                {
                    order += c.ToString();
                    chash.Add(c);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }

            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day7.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    steps.Add(line);
                }
            }
            sw.Start();
            while (onStepNumber < steps.Count)
            {
                line = steps[onStepNumber];
                char letter;
                char before;
                ReadDay7Step(line, out letter, out before);

                hash.Add(Tuple.Create(letter, before));
                hashreturn.Add(Tuple.Create(letter, before));

                onStepNumber++;
            }

            result = Sorting.TopologicalSort(
                chash,
                hash,
                true);

            return new Day7aPackage(hashreturn, chash, result);
        }

        public static void Day7b()
        {
            EnglishAlphabetProvider provider = new EnglishAlphabetProvider();
            Day7aPackage d7ap = Day7a();
            Day7WorkManager _mgr = Day7WorkManager.Instance;
            HashSet<Tuple<char, char>> hash = d7ap.hash;
            HashSet<char> chash = d7ap.chash;
            List<List<char>> result;
            List<char> done = new List<char>();
            int second = 0;
            int maxWorkers = 5;
            int timebase = 60;
            int totalTime = 0;

            /*
            result = Sorting.TopologicalSortGroups(
                chash,
                hash,
                true);

            foreach (var v in result)
            {
                char c = Convert.ToChar(0);

                foreach (var _c in v)
                {
                    if (_c > c)
                        c = _c;
                }

                totalTime += 60 + provider.CharValue(c);
            }
            */

            Dictionary<char, string> dependencies = new Dictionary<char, string>();

            foreach (var v in chash)
            {
                dependencies.Add(v, String.Empty);
            }

            foreach (var v in hash)
            {
                dependencies[v.Item2] += v.Item1.ToString();
            }

            Dictionary<int, List<Day7Worker>> workList;

            StringBuilder sb = new StringBuilder();

            foreach (var v in dependencies)
            {
                sb.Append(v.Key.ToString() + "   " + v.Value + Environment.NewLine);
            }
            Form messagedep = new GridDisplayDay3a(sb.ToString(), 0);
            messagedep.Show();


            _mgr.Setup(maxWorkers, timebase);
            _mgr.Dependencies = dependencies;
            _mgr.Chash = chash;
            totalTime = _mgr.RunWork(out workList);

            sb = new StringBuilder();

            sb.Append("Second |");

            for (int i = 1; i <= _mgr.MaxWorkers; i++)
            {
                sb.Append(" Worker " + i + " |");
            }

            int headerLength = sb.Length;
            sb.Append(Environment.NewLine);

            sb.Append(new string('-', headerLength));
            sb.Append(Environment.NewLine);

            foreach (var v in workList)
            {
                sb.Append(v.Key.ToString().PadLeft(6));

                foreach (var vv in v.Value)
                {
                    sb.Append(" | ");
                    sb.Append(vv.WorkingOnChar.ToString().PadCenter(8));
                }

                sb.Append(" |");

                sb.Append(Environment.NewLine);
            }

            //Incorrect Answer: 1070 (too high)
            //Incorrect Answer: 1041 (too high)
            //Incorrect Answer: 1027 (too low)
            //MessageBox.Show(totalTime.ToString());
            Form message = new GridDisplayDay3a(sb.ToString(), totalTime);
            message.Show();

            /*
            DataTable dt = new DataTable("Assembly Order and Time");
            DataColumn c;
            DataRow r;

            c = new DataColumn();
            c.DataType = System.Type.GetType("System.Int32");
            c.ColumnName = "Second";
            c.ReadOnly = false;
            c.Unique = false;
            dt.Columns.Add(c);

            for (int i = 1; i <= maxWorkers; i++)
            {
                c = new DataColumn();
                c.DataType = System.Type.GetType("System.Char");
                c.ColumnName = "Worker " + i;
                c.ReadOnly = false;
                c.Unique = false;
                dt.Columns.Add(c);
            }

            DataSet dataSet = new DataSet();
            dataSet.Tables.Add(dt);

            HashSet<char> S = new HashSet<char>(chash.Where(n => hash.All(e => e.Item2.Equals(n) == false)));

            Day7Worker[] workers = new Day7Worker[maxWorkers];

            for (int i = 1; i <= maxWorkers; i++)
            {
                workers[i] = new Day7Worker(i);
            }

            while (done.Count < 26)
            {
                ProcessWorkers(workers, ref S, timebase);
            }
            */
        }


        #region Support Methods
        static void ReadDay7Step(string line, out char letter, out char before)
        {
            string[] s = line.Split(' ');
            int t = s.Count();
            bool first = true;
            letter = ' ';
            before = ' ';

            for (int i = 0; i < t; i++)
            {
                if (s[i].Length == 1)
                {
                    if (first)
                    {
                        letter = Convert.ToChar(s[i]);
                        first = false;
                    }

                    else
                    {
                        before = Convert.ToChar(s[i]);
                    }
                }
            }
        }

        static void ProcessWorkers(Day7Worker[] workers, ref HashSet<char> S, int timebase)
        {
            IAlphabetProvider provider = new EnglishAlphabetProvider();

            int freeWorkers = 0;

            foreach (Day7Worker worker in workers)
            {
                if (!worker.Working)
                    freeWorkers++;
            }

            if (freeWorkers > 0)
            {
                int w = Math.Abs(freeWorkers - workers.Length - 1);
                freeWorkers--;
                workers[w].WorkingOnChar = S.OrderBy(s => s).First();
                S.Remove(workers[w].WorkingOnChar);
                workers[w].TimeLeft = timebase + provider.CharValue(workers[w].WorkingOnChar);
            }
        }
        #endregion
    }


    public class Day7aPackage
    {
        public HashSet<Tuple<char, char>> hash { get; set; }
        public HashSet<char> chash { get; set; }
        public List<char> result { get; set; }

        public Day7aPackage(HashSet<Tuple<char, char>> _hash, HashSet<char> _chash, List<char> _result)
        {
            hash = _hash;
            chash = _chash;
            result = _result;
        }
    }
}
