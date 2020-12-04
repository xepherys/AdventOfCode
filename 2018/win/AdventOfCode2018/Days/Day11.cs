using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day11
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        public static void Day11a(bool day11b = false)
        {
            int serialNumber;
            List<Day10PointOfLight> points = new List<Day10PointOfLight>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day11.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                //serialNumber = Convert.ToInt32(reader.ReadLine());
                serialNumber = 18;
            }

            Day11Manager _mgr = new Day11Manager(serialNumber, 300, 300);
            _mgr.RunWork(day11b);

            #region Support Methods

            #endregion
        }
    }

    public class Day11Manager
    {
        #region Fields
        int serialNumber;
        int[,] grid;
        int[,] extendedGrid;
        int powerLevel;
        int height;
        int width;
        #endregion

        #region Properties
        public int SerialNumber
        {
            get
            {
                return this.serialNumber;
            }

            set
            {
                this.SerialNumber = value;
            }
        }

        public int[,] Grid
        {
            get
            {
                return this.grid;
            }

            set
            {
                this.grid = value;
            }
        }

        public int[,] ExtendedGrid
        {
            get
            {
                return this.extendedGrid;
            }

            set
            {
                this.extendedGrid = value;
            }
        }
        #endregion

        #region Constructors
        public Day11Manager(int _serial, int height, int width)
        {
            Grid = new int[width, height];
            ExtendedGrid = new int[width, height];
            serialNumber = _serial;
        }
        #endregion
        public int GridVal(int x, int y)
        {
            return this.Grid[x - 1, y - 1];
        }

        public void GridVal(int x, int y, int val)
        {
            this.Grid[x - 1, y - 1] = val;
        }

        public void RunWork(bool day11b)
        {
            string bestCoord = String.Empty;
            SetIndividualValues();
            if (!day11b)
                bestCoord = RunThroughGroups(3, 3);
            else
                bestCoord = RunThroughGroups();

            MessageBox.Show(bestCoord);
        }

        void SetIndividualValues()
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    //int power = GetPowerLevelCell(x, y);
                    this.Grid[x, y] = GetPowerLevelCell(x, y);
                }
            }
        }

        int GetPowerLevelCell(int x, int y)
        {
            int power = (x + 1) + 10;
            power *= (y + 1);
            power += this.SerialNumber;
            power *= ((x + 1) + 10);

            if (power.ToString().Length > 2)
            {
                int index = power.ToString().Length - 3;
                string s = power.ToString().Substring(index, 1);
                power = Convert.ToInt32(s);
            }
            else
            {
                power = 0;
            }

            power -= 5;

            return power;
        }

        string RunThroughGroups(int height, int width)
        {
            int currHighPower = -99;
            int currHighX = -99;
            int currHighY = -99;
            for (int y = 0; y < this.Grid.GetLength(1) - 2; y++)
            {
                for (int x = 0; x < this.Grid.GetLength(0) - 2; x++)
                {
                    int groupPower = 0;

                    for (int y2 = y; y2 < y + height; y2++)
                    {
                        for (int x2 = x; x2 < x + width; x2++)
                        {
                            groupPower += this.Grid[x2, y2];
                        }
                    }

                    this.ExtendedGrid[x, y] = groupPower;
                    if (groupPower > currHighPower)
                    {
                        currHighPower = groupPower;
                        currHighX = x;
                        currHighY = y;
                    }
                }
            }

            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < this.ExtendedGrid.GetLength(1); y++)
            {
                for (int x = 0; x < this.ExtendedGrid.GetLength(0); x++)
                {
                    sb.Append(ExtendedGrid[x, y].ToString().PadLeft(3));
                }
                sb.Append(Environment.NewLine);
            }

            //Day10Form _frm = new Day10Form(sb.ToString());
            //_frm.Show();

            //int maxPowerInGrid = this.ExtendedGrid.Where(w => w).Single();
            return "Highest Power Grouping (top-left x, y): (" + (currHighX + 1) + ", " + (currHighY + 1) + ") with a power value of: " + currHighPower;
        }

        string RunThroughGroups()
        {
            int currSize = 1;

            int currHighPower   = -99;
            int currHighX       = -99;
            int currHighY       = -99;
            int currHighSize    = -99;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            //while (currSize < Math.Min(this.Grid.GetLength(0), this.Grid.GetLength(1)))
            Parallel.For(0, Math.Min(this.Grid.GetLength(0), this.Grid.GetLength(1)) - 1, i =>
            {
                for (int y = 0; y < this.Grid.GetLength(1) - i; y++)
                {
                    for (int x = 0; x < this.Grid.GetLength(0) - i; x++)
                    {
                        int groupPower = 0;

                        for (int y2 = y; y2 <= y + i; y2++)
                        {
                            for (int x2 = x; x2 <= x + i; x2++)
                            {
                                groupPower += this.Grid[x2, y2];
                            }
                        }

                        this.ExtendedGrid[x, y] = groupPower;
                        if (groupPower > currHighPower)
                        {
                            currHighPower = groupPower;
                            currHighX = x;
                            currHighY = y;
                            currHighSize = i;
                        }
                    }
                }
                //currSize++;
            });
            sw.Stop();
            return "Highest Power Grouping (top-left x, y, size): (" + (currHighX + 1) + ", " + (currHighY + 1) + ", " + currHighSize + 
                   ") with a power value of: " + currHighPower + Environment.NewLine + "Total time: " + sw.ElapsedMilliseconds + "ms, " +
                   sw.ElapsedTicks + " ticks.";
        }
    }
}