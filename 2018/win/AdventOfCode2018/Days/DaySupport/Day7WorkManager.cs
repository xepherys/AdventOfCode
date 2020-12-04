using System;
using System.Collections.Generic;
using System.Linq;
using Xepherys.AlphabetProviders;

namespace AdventOfCode2018
{
    public class Day7WorkManager
    {
        #region Fields
        private static readonly Lazy<Day7WorkManager> lazy = new Lazy<Day7WorkManager>(() => new Day7WorkManager());
        private int second = 0;
        private int maxWorkers = 1;
        private int timebase = 0;
        HashSet<char> chash;
        protected Dictionary<int, List<Day7Worker>> workList = new Dictionary<int, List<Day7Worker>>();
        private AlphabetProvider alphaProvider = new EnglishAlphabetProvider();
        private List<Day7Worker> workers = new List<Day7Worker>();
        private Dictionary<char, string> dependencies = new Dictionary<char, string>();
        #endregion

        #region Properties
        private int Second
        {
            get
            {
                return this.second;
            }

            set
            {
                this.second = value;
            }
        }

        public int MaxWorkers
        {
            get
            {
                return this.maxWorkers;
            }

            set
            {
                this.maxWorkers = value;
            }
        }

        public int Timebase
        {
            get
            {
                return this.timebase;
            }

            set
            {
                this.timebase = value;
            }
        }

        public Dictionary<char, string> Dependencies
        {
            get
            {
                return this.dependencies;
            }

            set
            {
                this.dependencies = value;
            }
        }

        public HashSet<char> Chash
        {
            get
            {
                return this.chash;
            }

            set
            {
                this.chash = value;
            }
        }

        public AlphabetProvider AlphaProvider
        {
            get
            {
                return alphaProvider;
            }

            set
            {
                this.alphaProvider = value;
            }
        }
        #endregion

        #region Constructors
        private Day7WorkManager()
        {
            
        }

        public static Day7WorkManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        #region Methods
        public void Setup(int workercount, int time)
        {
            this.MaxWorkers = workercount;
            this.Timebase = time;
            CreateWorkerPool();
        }

        public int RunWork(out Dictionary<int, List<Day7Worker>> workList)
        {
            workList = this.workList;
            while (Chash.Count > 0)
            {
                foreach (var worker in workers)
                {
                    bool isCompleted = false;

                    char work = '*';

                    if (worker.Working)
                    {
                        worker.TimeLeft--;

                        if (worker.TimeLeft == 0)
                            isCompleted = true;
                    }

                    if (isCompleted)
                    {
                        worker.Working = false;
                        Chash.Remove(worker.WorkingOnChar);
                        foreach (var v in Dependencies.Where(w => w.Value.Contains(worker.WorkingOnChar)).ToList())
                            Dependencies[v.Key] = Dependencies[v.Key].Remove(Dependencies[v.Key].IndexOf(worker.WorkingOnChar), 1);
                        worker.WorkingOnChar = '*';
                        isCompleted = false;
                    }


                    if (!worker.Working)
                    {
                        try
                        {
                            work = Dependencies.OrderByDescending(o => o.Key).First(f => String.IsNullOrEmpty(f.Value)).Key;
                        }

                        catch { }

                        if (work != '*')
                        {
                            Dependencies.Remove(work);
                            worker.Working = true;
                            worker.WorkingOnChar = work;
                            worker.TimeLeft = timebase + this.AlphaProvider.CharValue(work);
                        }
                    }

                    /*
                    else
                    {
                        worker.TimeLeft--;

                        if (worker.TimeLeft == 0)
                            isCompleted = true;
                    }

                    if (isCompleted)
                    {
                        worker.Working = false;
                        Chash.Remove(worker.WorkingOnChar);
                        foreach (var v in Dependencies.Where(w => w.Value.Contains(worker.WorkingOnChar)).ToList())
                            Dependencies[v.Key] = Dependencies[v.Key].Remove(Dependencies[v.Key].IndexOf(worker.WorkingOnChar), 1);
                        worker.WorkingOnChar = '*';
                        isCompleted = false;
                    }
                    */
                }

                List<Day7Worker> _tmplist = new List<Day7Worker>();
                foreach (var worker in workers)
                {
                    Day7Worker _tmpworker = new Day7Worker(worker);
                    _tmplist.Add(_tmpworker);
                }

                workList.Add(this.Second, _tmplist);

                this.Second++;
            }

            return this.Second;
        }

        private void CreateWorkerPool()
        {
            for (int i = 1; i <= maxWorkers; i++)
                workers.Add(new Day7Worker(i));
        }
        #endregion
    }

    public class Day7Worker
    {
        private int id;
        private bool working;
        private char workingOnChar = '*';
        private int timeLeft = -99;

        public int ID
        {
            get
            {
                return this.id;
            }

            private set
            {
                this.id = value;
            }
        }

        public bool Working
        {
            get
            {
                return this.working;
            }

            set
            {
                this.working = value;
            }
        }

        public char WorkingOnChar
        {
            get
            {
                return this.workingOnChar;
            }

            set
            {
                this.workingOnChar = value;
            }
        }

        public int TimeLeft
        {
            get
            {
                return this.timeLeft;
            }

            set
            {
                this.timeLeft = value;
            }
        }

        public void Tick()
        {
            this.timeLeft--;
        }

        public Day7Worker(int i)
        {
            this.id = i;
        }

        public Day7Worker(Day7Worker _tmp)
        {
            this.ID = _tmp.ID;
            this.Working = _tmp.Working;
            this.WorkingOnChar = _tmp.WorkingOnChar;
            this.TimeLeft = _tmp.TimeLeft;
        }
    }
}