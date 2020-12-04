using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day12
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        public static void Day12a(long numGenerations = 1)
        {
            string initialState = String.Empty;
            List<string> steps = new List<string>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day12.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                steps = StreamFunctions.EnumerateLines(reader).ToList();
            }

            if (steps[0].StartsWith("initial state: "))
            {
                initialState = steps[0].Remove(0, 15);
                steps.Remove(steps[0]);
            }

            if (steps[0] == "")
            {
                steps.Remove(steps[0]);
            }

            // TestStuff
            /*
            initialState = "#..#.#..##......###...###";
            steps.Clear();
            steps = new List<string>( new string[] { "...## => #" , 
                                                     "..#.. => #" ,
                                                     ".#... => #" ,
                                                     ".#.#. => #" ,
                                                     ".#.## => #" ,
                                                     ".##.. => #" ,
                                                     ".#### => #" ,
                                                     "#.#.# => #" ,
                                                     "#.### => #" ,
                                                     "##.#. => #" ,
                                                     "##.## => #" ,
                                                     "###.. => #" ,
                                                     "###.# => #" ,
                                                     "####. => #" } );
                                                     */
            Day12Manager _mgr = new Day12Manager(initialState, steps, numGenerations);
            _mgr.RunWork();


            // 3547 is too low


            #region Support Methods

            #endregion
        }

    #region Support Methods

    #endregion
    }

    class Day12Manager
    {
        #region Fields
        string initialState = String.Empty;
        List<Day12StepGroup> steps = new List<Day12StepGroup>();
        long currentStep = 0;
        long totalSteps = -99; //50000000000;  //20;
        List<Day12Spot> stepData;  // int is the index of the character
        List<List<Day12Spot>> stepValues = new List<List<Day12Spot>>();
        bool VeryLargeStepCount = false;
        int maxX = 0;
        List<Day12GenerationComparer> comparer = new List<Day12GenerationComparer>();
        #endregion

        #region Constructors
        public Day12Manager(string _initialState, List<string> _steps, long _totalSteps)
        {
            initialState = _initialState;
            foreach (string s in _steps)
            {
                Day12StepGroup g = new Day12StepGroup(s);
                steps.Add(g);
            }

            totalSteps = _totalSteps;
            if (totalSteps > 10000)
                VeryLargeStepCount = true;
        }
        #endregion

        #region Methods
        public void RunWork()
        {
            #region Step 0 - Initialization
            stepData = new List<Day12Spot>();
            int index = 0;
            bool stopRunning = false;

            foreach (char c in initialState)
            {
                stepData.Add(new Day12Spot(index, c));
                index++;
            }


            stepValues.Add(stepData);
            comparer.Add(new Day12GenerationComparer(stepData));
            this.maxX = Math.Max(stepData.Last().X, maxX);
            currentStep++;
            #endregion

            while (currentStep <= totalSteps && !stopRunning)
            {
                stepData = new List<Day12Spot>();
                List<Day12Spot> previousStep = new List<Day12Spot>();

                if (!VeryLargeStepCount)
                    previousStep = stepValues[Convert.ToInt32(currentStep) - 1];

                else
                    previousStep = stepValues[0];

                stepData = CompareAll(previousStep);

                if (VeryLargeStepCount)
                    stepValues.Clear();
                stepValues.Add(stepData);
                comparer.Add(new Day12GenerationComparer(stepData));
                this.maxX = Math.Max(stepData.Last().X, maxX);
                currentStep++;

                stopRunning = CheckForPattern(ref comparer);
            }

            long endValue = Tally(stepValues.Last());

            if (stopRunning)
                endValue = PatternTally(stepValues.Last());

            //MessageBox.Show(endValue.ToString());

            StringBuilder sb = new StringBuilder();

            int min = 0;
            int max = 0;

            if (!VeryLargeStepCount)
            {
                foreach (List<Day12Spot> spotList in stepValues)
                {
                    foreach (Day12Spot spot in spotList)
                    {
                        if (spot.X - 1 < min)
                            min = spot.X - 1;
                    }

                    foreach (Day12Spot spot in spotList)
                    {
                        if (spot.X + 2 - min > max)
                            max = spot.X + 2 - min;
                    }
                }

                foreach (List<Day12Spot> spotList in stepValues)
                {
                    string line = "";
                    int prefix = (spotList.First().X - min);
                    if (prefix > 0)
                        line = line.PadLeft(prefix, '.');

                    foreach (Day12Spot spot in spotList)
                    {
                        line += spot.C.ToString();
                    }

                    line = line.PadRight(max, '.');

                    line += Environment.NewLine;
                    sb.Append(line);
                }
            }

            Day10Form _frm = new Day10Form(sb.ToString(), endValue, 12);
            _frm.Show();
        }

        bool CheckForPattern(ref List<Day12GenerationComparer> comparer)
        {
            bool _ret = false;

            if (comparer.Count == 5)
            {
                if (comparer[0].FirstToLast.SequenceEqual(comparer[1].FirstToLast) && comparer[0].FirstToLast.SequenceEqual(comparer[2].FirstToLast) &&
                    comparer[0].FirstToLast.SequenceEqual(comparer[3].FirstToLast) && comparer[0].FirstToLast.SequenceEqual(comparer[4].FirstToLast))
                {
                    _ret = true;
                }

                else
                {
                    comparer.Remove(comparer[0]);
                }
            }

            return _ret;
        }

        long PatternTally(List<Day12Spot> spots)
        {
            long _ret = 0;
            int count = 0;

            foreach (Day12Spot spot in spots)
            {
                if (spot.C == '#')
                {
                    _ret += spot.X;
                    count++;
                }
            }

            long countDiff = (this.totalSteps - this.currentStep + 1) * count;

            _ret += countDiff;

            return _ret;
        }

        long Tally(List<Day12Spot> spots)
        {
            long _ret = 0;

            foreach (Day12Spot spot in spots)
            {
                if (spot.C == '#')
                    _ret += spot.X;
            }

            return _ret;
        }

        List<Day12Spot> CompareAll(List<Day12Spot> previous)
        {
            List<Day12Spot> _ret = new List<Day12Spot>();
            Day12Spot s = new Day12Spot();

            int baseIndex = previous.First(f => f.C == '#').X - 4;

            for (int i = baseIndex; i <= previous.Count + this.maxX; i++)
            {
                Day12Group g = new Day12Group(previous, i);
                s = new Day12Spot(i, GroupCompare(g));

                _ret.Add(s);
            }

            //int last = _ret.Last(l => l.C == '#').X;
            //int first = _ret.First(f => f.C == '#').X;

            while (_ret.Last().C == '.')
                _ret.Remove(_ret.Last());

            while (_ret.First().C == '.')
                _ret.Remove(_ret.First());

            return _ret;
        }


        char GroupCompare(Day12Group g)
        {
            char _ret = '.';

            foreach (Day12StepGroup d12sg in this.steps)
            {
                if (g.Group.SequenceEqual(d12sg.Group))
                {
                    _ret = d12sg.Result;
                }
            }

            return _ret;
        }
        #endregion
    }

    class Day12Spot
    {
        #region Fields
        int x;
        char c;
        string validChars = "#.";
        #endregion

        #region Properties
        public int X
        {
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }

        public char C
        {
            get
            {
                return this.c;
            }

            set
            {
                if (validChars.Contains(value))
                    this.c = value;
                else
                    throw new FormatException("Invalid value, only ( # | . ) allowed [Day12Spot].");
            }
        }

        public string ValidChars
        {
            get
            {
                return this.validChars;
            }

            set
            {
                this.validChars = value;
            }
        }
        #endregion

        #region Constructors
        public Day12Spot()
        {
        }

        public Day12Spot(int x, char c)
        {
            this.X = x;
            this.C = c;
        }
        #endregion
    }

    class Day12Group
    {
        #region Fields
        char[] group = new char[5];
        int? index = null;  // Index of middle character, or C in (LLCRR) [left, left, center, right, right]
        #endregion

        #region Properties
        public char[] Group
        {
            get
            {
                return this.group;
            }

            set
            {
                if (value.Length == 5)
                    this.group = value;
                else
                    throw new FormatException("Char[] array must contain exactly five characters [Day12Group].");
            }
        }

        public int Index
        {
            get
            {
                if (this.index == null)
                    throw new Exception("Index not set for this object. [Day12Group]");
                else
                    return Convert.ToInt32(this.index);
            }

            set
            {
                this.index = value;
            }
        }
        #endregion

        #region Constructors
        public Day12Group()
        { }

        public Day12Group(List<Day12Spot> list, int _index)
        {
            char L2;
            char L1;
            char C;
            char R1;
            char R2;

            if (list.Exists(e => e.X == _index - 2))
                L2 = list.Single(s => s.X == _index - 2).C;
            else
                L2 = '.';

            if (list.Exists(e => e.X == _index - 1))
                L1 = list.Single(s => s.X == _index - 1).C;
            else
                L1 = '.';

            if (list.Exists(e => e.X == _index))
                C = list.Single(s => s.X == _index).C;
            else
                C = '.';

            if (list.Exists(e => e.X == _index + 1))
                R1 = list.Single(s => s.X == _index + 1).C;
            else
                R1 = '.';

            if (list.Exists(e => e.X == _index + 2))
                R2 = list.Single(s => s.X == _index + 2).C;
            else
                R2 = '.';


            this.Group = new char[] { L2, L1, C, R1, R2 };
            this.Index = _index;
        }
        #endregion
    }

    class Day12StepGroup
    {
        #region Fields
        char[] group = new char[5];
        char result;  //###.# => #
        #endregion

        #region Properties
        public char[] Group
        {
            get
            {
                return this.group;
            }

            set
            {
                if (value.Length == 5)
                    this.Group = value;
                else
                    throw new FormatException("Char[] array must contain exactly five characters [Day12Group].");
            }
        }

        public char Result
        {
            get
            {
                return this.result;
            }
        }
        #endregion

        #region Constructors
        public Day12StepGroup(string s)
        {
            string[] parser = s.Split(' ');
            group = parser[0].ToArray();
            result = parser[2].First();
        }
        #endregion
    }

    class Day12GenerationComparer
    {
        #region Fields
        public int StartIndex { get; set; }
        public string FirstToLast { get; set; }
        #endregion

        #region Constructors
        public Day12GenerationComparer()
        { }

        public Day12GenerationComparer(List<Day12Spot> _l)
        {
            this.StartIndex = _l.First(f => f.C == '#').X;
            //int EndIndex = _l.Last(l => l.C == '#').X;
            string pattern = "";

            if (this.StartIndex >= 0)
            {
                for (int i = this.StartIndex; i < _l.Count - 1; i++)
                {
                    pattern += _l[i].C;
                }
            }

            this.FirstToLast = pattern;
        }
        #endregion
    }
}