using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace AdventOfCode2018
{
    public partial class Day14Form : Form
    {
        List<int> recipeScores = new List<int>();
        int numberOfRecipesToMake = 0;
        string runBCheck = "";
        Stopwatch sw = new Stopwatch();

        public Day14Form()
        {
            InitializeComponent();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            string[] split = txtStartScores.Text.Split(',');
            foreach (string s in split)
            {
                recipeScores.Add(Convert.ToInt32(s));
            }

            numberOfRecipesToMake = (int)numInputValue.Value;

            RunWork(cbB.Checked);

            // B incorrect guess: 141032488
            //                    141032487
        }




        private void RunWork(bool runB = false)
        {
            Day14CookingElf elf1 = new Day14CookingElf(0);
            Day14CookingElf elf2 = new Day14CookingElf(1);

            runBCheck = numberOfRecipesToMake.ToString();
            int runBCheckChars = runBCheck.Length;

            Queue<int> cache = new Queue<int>(runBCheckChars);

            string initial = "(" + recipeScores[elf1.Index] + ") " + "[" + recipeScores[elf2.Index] + "] " + Environment.NewLine;
            //txtScores.Text += initial;

            bool keepRunning = true;

            sw.Start();
            while (keepRunning)
            {
                int value = recipeScores[elf1.Index] + recipeScores[elf2.Index];

                foreach (char c in value.ToString())
                {
                    recipeScores.Add(Convert.ToInt32(c.ToString()));
                    if (cache.Count == runBCheckChars)
                        cache.Dequeue();

                    cache.Enqueue(Convert.ToInt32(c.ToString()));
                    keepRunning = CheckMatch(cache);
                    if (!keepRunning)
                        break;
                }

                elf1.Step(recipeScores);
                elf2.Step(recipeScores);

                string stepString = "";

                int indexer = 0;

                if (numberOfRecipesToMake < 2500)
                {
                    foreach (int i in recipeScores)
                    {
                        if (indexer == elf1.Index)
                            stepString += "(" + i.ToString() + ") ";
                        else if (indexer == elf2.Index)
                            stepString += "[" + i.ToString() + "] ";
                        else
                            stepString += " " + i.ToString() + "  ";

                        indexer++;
                    }
                }
                

                stepString += Environment.NewLine;

                if (numberOfRecipesToMake < 2500)
                {
                    txtScores.Text += stepString;
                }

                if (!runB)
                {
                    keepRunning = recipeScores.Count < (numberOfRecipesToMake + 10);
                }
            }
            sw.Stop();

            MessageBox.Show(sw.ElapsedMilliseconds + "ms elapsed for main work(). [" + sw.ElapsedTicks + " ticks]");
            string s = "";

            if (runB && !keepRunning)
            {
                s += (recipeScores.Count - runBCheck.Length).ToString();
            }

            else
            {
                for (int i = numberOfRecipesToMake; i <= numberOfRecipesToMake + 9; i++)
                {
                    s += recipeScores[i].ToString();
                }
            }

            txtScores.Text += Environment.NewLine + Environment.NewLine + s;
        }

        bool CheckMatch(Queue<int> q)
        {
            string comparer = "";

            foreach (var v in q)
            {
                comparer += v.ToString();
            }

            return !(comparer.Equals(runBCheck));
        }
    }

    public class Day14CookingElf
    {
        public int Index { get; set; }

        public Day14CookingElf(int idx)
        {
            this.Index = idx;
        }

        public void Step(List<int> recipeScores)
        {
            int indexValue = this.Index + (1 + recipeScores[this.Index]);
            while (indexValue >= recipeScores.Count)
                indexValue -= recipeScores.Count;

            this.Index = indexValue;
        }
    }

    public class Day14RecipeValues
    {
        #region Fields
        Dictionary<int, int[]> values = new Dictionary<int, int[]>();
        int Index = 1;
        int arraySize = 100000;
        #endregion
        
        #region Constructors
        public Day14RecipeValues(int i)
        {
            arraySize = i;
        }
        #endregion

        #region Methods
        public int Fetch(long l)
        {
            int indexer = 0;
            int place = 0;

            indexer = (int)(l / this.arraySize);
            place = (int)(l % this.arraySize);

            return values[indexer][place];
        }

        public void Put(int i)
        {
            int indexer = 0;
            int place = 0;

            indexer = (int)(i / this.arraySize);
            place = (int)(i % this.arraySize);

            if (!values.ContainsKey(indexer))
            {
                values[indexer] = new int[arraySize];
            }
        }
        #endregion
    }
}
