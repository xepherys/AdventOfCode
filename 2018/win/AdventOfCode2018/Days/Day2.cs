using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day2
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        static public void Day2a()
        {
            string[] lines;

            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day2.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                lines = StreamFunctions.EnumerateLines(reader).ToArray();
            }

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

            MessageBox.Show("3-sets: " + count3.ToString() + "\n" + "2-sets: " + count2.ToString() + "\n\n" +
                              "Checksum: " + (count3 * count2).ToString());
        }

        static public void Day2b()
        {
            string[] lines;
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day2.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                lines = StreamFunctions.EnumerateLines(reader).ToArray();
            }

            Stopwatch sw = new Stopwatch();
            string match1 = "";
            string match2 = "";
            int placevalue = 0;
            int iteration = 0;
            bool found = false;
            //lines = lines.OrderBy(a => a).ToArray();
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

            string answer = match1.Remove(placevalue, 1);

            MessageBox.Show("Matched lines are: " + match1 + " & " + match2 + "\n\n" +
                            "Answer is: " + answer + "\n" + "Found in " + sw.ElapsedMilliseconds +
                            " ms. (" + sw.ElapsedTicks + " ticks)" + "\n" + "Found in " + iteration + " iterations.");
        }

        // Day 2bThreaded is an attempt to thread the work and see if it would run faster.  It does, but only marginally since the data set is relatively small.
        static public void Day2bThreaded()
        {
            string[] lines;
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day2.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                lines = StreamFunctions.EnumerateLines(reader).ToArray();
            }

            Stopwatch sw = new Stopwatch();
            string match1 = "";
            string match2 = "";
            int placevalue = 0;
            int iteration = 0;
            bool found = false;
            //lines = lines.OrderBy(a => a).ToArray();
            ConcurrentQueue<string> comparelines = new ConcurrentQueue<string>(lines);

            while (!found)
            {
                sw.Start();

                Parallel.ForEach(comparelines, (i, loopState) =>
                {
                    string s;
                    comparelines.TryDequeue(out s);
                    string delta;

                    foreach (string comparer in comparelines)
                    {
                        iteration++;
                        delta = "";
                        if (match1 == "")
                        {
                            for (int _i = 0; _i < 26; _i++)
                            {
                                if (comparer[_i] != s[_i])
                                {
                                    delta += comparer[_i];
                                    placevalue = _i;
                                }
                                if (delta.Length > 1)
                                    break;
                            }

                            if (delta.Length == 1)
                            {
                                match1 = s;
                                match2 = comparer;
                                found = true;
                                loopState.Stop();
                            }
                        }

                        if (found)
                            loopState.Stop();
                    }
                    if (found)
                        loopState.Stop();

                });

                sw.Stop();
            }

            string answer = match1.Remove(placevalue, 1);

            MessageBox.Show("Matched lines are: " + match1 + " & " + match2 + "\n\n" +
                            "Answer is: " + answer + "\n" + "Found in " + sw.ElapsedMilliseconds +
                            " ms. (" + sw.ElapsedTicks + " ticks)" + "\n" + "Found in " + iteration + " iterations.");
        }


        #region Support Methods

        #endregion
    }
}