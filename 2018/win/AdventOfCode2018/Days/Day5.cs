using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xepherys.AlphabetProviders;

namespace AdventOfCode2018
{
    class Day5
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        // Day5a acts also as a method of providing data to Day5b and thus has a return value.
        static public List<char> Day5a(List<char> line = null, bool suppressMessageBox = false)
        {
            bool workToDo = true;
            int polymerPairsRemovedMasterCount = 0;
            int iterations = 0;
            Stopwatch sw = new Stopwatch();

            sw.Start();
            if (line == null || line.Count() == 0)
            {
                using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day5.txt"))
                using (StreamReader reader = new StreamReader(stream))
                {
                    line = reader.ReadToEnd().ToList();
                }
            }

            bool polymersNotFoundOnce = false;

            while (workToDo)
            {
                iterations++;
                int polymerPairsRemoved = 0;

                for (int i = line.Count - 1; i >= 1; i--)
                {
                    if (line[i].ToString().Equals(line[i - 1].ToString(), StringComparison.InvariantCultureIgnoreCase))
                    {
                        if ((Char.IsLower(line[i]) && Char.IsUpper(line[i - 1])) || (Char.IsUpper(line[i]) && Char.IsLower(line[i - 1])))
                        {
                            line.RemoveRange(i - 1, 2);
                            polymerPairsRemoved++;
                            polymersNotFoundOnce = false;
                            i -= 2;
                        }
                    }
                }

                polymerPairsRemovedMasterCount += polymerPairsRemoved;

                if (polymerPairsRemoved == 0)
                {
                    if (polymersNotFoundOnce)
                    {
                        workToDo = false;
                    }

                    else
                    {
                        polymersNotFoundOnce = true;
                    }
                }
            }
            sw.Stop();

            if (!suppressMessageBox)
                MessageBox.Show("Total letters left: " + line.Count() + "\n" + "Time Processing: " + sw.ElapsedMilliseconds + "ms (" + sw.ElapsedTicks +
                                " ticks)." + "\n" + "Iterations required: " + iterations);

            return line;
        }


        // 32436ms before threading
        // 17138ms with Parallel.ForEach
        public static void Day5b()
        {
            Stopwatch sw = new Stopwatch();
            List<char> line;
            Dictionary<char, List<char>> results = new Dictionary<char, List<char>>();
            IAlphabetProvider provider = new EnglishAlphabetProvider();

            sw.Start();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day5.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                line = reader.ReadToEnd().ToList();
            }

            /*
            foreach (char c in provider.GetAlphabet())
            {
                List<char> templine = new List<char>(line);
                templine.RemoveAll(l => l == c);
                templine.RemoveAll(l => l == Char.ToLower(c));
                templine = Day5a(templine, true);
                results.Add(c, templine);
            }
            */

            Parallel.ForEach(provider.GetAlphabet(), c =>
            {
               List<char> templine = new List<char>(line);
               templine.RemoveAll(l => l == c);
               templine.RemoveAll(l => l == Char.ToLower(c));
               templine = Day5a(templine, true);
               results.Add(c, templine);
            });
            sw.Stop();

            StringBuilder sb = new StringBuilder();
            sb.Append("Day5b completed in " + sw.ElapsedMilliseconds + "ms (" + sw.ElapsedTicks + " ticks).").Append(Environment.NewLine).Append(Environment.NewLine);
            foreach (var kvp in results.OrderBy(v => v.Value.Count()))
            {
                sb.Append(kvp.Value.Count()).Append(" for letter ").Append(kvp.Key.ToString()).Append(Environment.NewLine);
            }

            MessageBox.Show(sb.ToString());
        }


        #region Support Methods

        #endregion
    }
}