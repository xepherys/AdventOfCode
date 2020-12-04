using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AdventOfCode2018
{
    class Day4
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        // Day 4a currently does the work for both steps.  I was iterating through data when I completed 'a' and realized 'b' could be done manually quicker than changing code (the values were already there).
        static public void Day4a()
        {
            List<Day4aSchedule> schedule = new List<Day4aSchedule>();
            List<Day4aGuard> guards = new List<Day4aGuard>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2018._data.AdventOfCode_Day4.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                schedule = EnumerateSchedule(reader).OrderBy(a => a.timestamp).ToList();
            }

            string dayInfo = "";

            foreach (Day4aSchedule d4as in schedule)
            {
                dayInfo += d4as.timestamp.ToString("yyyy-MM-dd hh:mm");
                dayInfo += " ";
                dayInfo += d4as.entry;
                dayInfo += Environment.NewLine;
            }

            GridDisplayDay3a window = new GridDisplayDay3a(dayInfo, 0);
            if (window.ShowDialog() == DialogResult.OK) { }

            string id = "";
            foreach (Day4aSchedule d4as in schedule)
            {
                if (d4as.entry.StartsWith("Guard #"))
                {
                    string[] s = d4as.entry.Split(' ');
                    string _id = s[1].Remove(0, 1);

                    if (_id != id)
                        id = _id;
                }

                d4as.id = id;
            }

            dayInfo = "";

            foreach (Day4aSchedule d4as in schedule)
            {
                dayInfo += d4as.timestamp.ToString("yyyy-MM-dd hh:mm");
                dayInfo += " ";
                dayInfo += d4as.id;
                dayInfo += " ";
                dayInfo += d4as.entry;
                dayInfo += Environment.NewLine;
            }

            GridDisplayDay3a window2 = new GridDisplayDay3a(dayInfo, 0);
            if (window2.ShowDialog() == DialogResult.OK) { }

            ParseGuards(ref guards, schedule);

            dayInfo = "";
            foreach (Day4aGuard guard in guards)
            {
                dayInfo += guard.id.PadLeft(6);
                dayInfo += " Total Sleeps: " + guard.numSleeps;
                dayInfo += " Total Minutes Sleeping: " + guard.minutesSleeping;
                dayInfo += Environment.NewLine;
                for (int i = 0; i < 60; i++)
                {
                    dayInfo += guard[i].ToString().PadLeft(3);
                }
                dayInfo += Environment.NewLine;
                dayInfo += Environment.NewLine;
            }

            GridDisplayDay3a window3 = new GridDisplayDay3a(dayInfo, 0);
            if (window3.ShowDialog() == DialogResult.OK) { }
        }



        #region Support Methods
        static IEnumerable<Day4aSchedule> EnumerateSchedule(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                Day4aSchedule d4as = new Day4aSchedule();

                line = line.Replace("] ", "]*");

                string[] read = line.Split('*');

                string date = read[0];
                date = date.Remove(date.IndexOf(']'), 1);
                date = date.Remove(date.IndexOf('['), 1);
                d4as.timestamp = Convert.ToDateTime(date);
                d4as.entry = read[1];

                yield return d4as;
            }
        }

        static void ParseGuards(ref List<Day4aGuard> guards, List<Day4aSchedule> schedule)
        {
            GuardState state = GuardState.NONE;
            int minutes = 0;
            int entryMinute = 0;
            int exitMinute = 0;
            Day4aGuard _guard;
            string _id = "";
            foreach (Day4aSchedule d4as in schedule)
            {
                if (d4as.id != _id)
                    _id = d4as.id;

                if (guards.Where(g => g.id == _id).Count() == 0)
                {
                    _guard = new Day4aGuard();
                    _guard.id = _id;
                    guards.Add(_guard);
                }

                if (d4as.entry.Contains("begins shift") && d4as.timestamp.Minute > 30)
                {
                    entryMinute = 0;
                    exitMinute = 0;
                    minutes = 0;
                }

                else if (d4as.entry.Contains("begins shift"))
                {
                    entryMinute = 0;
                }

                /*
                else if (d4as.entry.Contains("falls asleep") && d4as.timestamp.Minute > 30)
                {
                    entryMinute = (60 - d4as.timestamp.Minute) * -1;
                }
                */

                else if (d4as.entry.Contains("falls asleep"))// && d4as.timestamp.Minute > 30)
                {
                    entryMinute = d4as.timestamp.Minute;
                }

                else if (d4as.entry.Contains("wakes up"))
                {
                    exitMinute = d4as.timestamp.Minute;
                    minutes = exitMinute - entryMinute;
                    for (int i = entryMinute; i < exitMinute; i++)
                    {
                        if (entryMinute < 0)
                        {
                        }
                        guards.Single(g => g.id == _id)[i]++;
                    }
                    guards.Single(g => g.id == _id).minutesSleeping += minutes;
                    guards.Single(g => g.id == _id).numSleeps++;

                }
            }
        }
        #endregion
    }

    /// <summary>
    /// Collection class to contain the sorted schedule
    /// </summary>
    public class Day4aSchedule
    {
        public DateTime timestamp { get; set; }
        public string id { get; set; }
        public string entry { get; set; }
    }
}