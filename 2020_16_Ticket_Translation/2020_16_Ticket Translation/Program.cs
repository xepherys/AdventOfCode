using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace _2020_16_Ticket_Translation
{
    class Program
    {

        // Part 2: 609507353 (too low)

        public static List<Rule> rules = new List<Rule>();
        public static Ticket myTicket;
        public static List<Ticket> tickets = new List<Ticket>();
        public static int[] myTicketPositions;
        public static string positionsToFind = "departure";

        static void Main(string[] args)
        {
            ImportData();

            Console.WriteLine("Imported Rules:");
            foreach (Rule rule in rules)
                Console.WriteLine("   " + rule);

            Console.WriteLine();

            //foreach (Ticket t in tickets)
            //    Console.WriteLine(t);
            

            Console.WriteLine($"Read error rate: {GetErrorRate()}\n");

            int tix = tickets.Count();

            for (int i = tix - 1; i >= 0; i--)
            {
                if (!tickets[i].Valid)
                    tickets.Remove(tickets[i]);
            }

            Console.WriteLine($"Removed {tix - tickets.Count()} invalid tickets.\n{tickets.Count()} tickets remaining.\n");

            Console.WriteLine("Initializing possible positions...");
            int ruleCount = rules.Count();
            foreach (Rule r in rules)
                r.InitPossiblePositions(ruleCount);

            Console.WriteLine($"All rules initialized with {ruleCount} possible positions.\n");

            Console.WriteLine($"Calculating rule positions...\n");
            FindRulePositions();

            foreach (Rule rule in rules.OrderBy(o => o.Position))
                Console.WriteLine("    " + rule);

            Console.WriteLine($"Finding all positions that include keyword {positionsToFind}...\n");
            var l = rules.Where(w => w.Name.Contains(positionsToFind)).ToList();
            myTicketPositions = new int[l.Count()];

            for (int i = 0; i < l.Count(); i++)
                myTicketPositions[i] = l[i].Position;

            Console.Write("Reading positions [");
            foreach (var v in myTicketPositions)
                Console.Write($"{v} ");
            Console.Write("[ from my ticket...\n\n");


            Console.WriteLine($"Product of all {positionsToFind}-related values is: {parseMyTicket(myTicketPositions)}\n");

            Console.Read();
        }

        static long parseMyTicket(int[] pos)
        {
            long _ret = 1;

            foreach (int p in pos)
            {
                _ret *= myTicket.Values[p - 1];
            }

            return _ret;
        }

        static void FindRulePositions()
        {
            int index = 0;

            while (rules.Any(a => a.PossiblePositions.Count() > 1))
            {
                Ticket t = tickets[index++];
                for (int i = 0; i < t.Values.Length; i++)
                {
                    int value = t.Values[i];

                    foreach (Rule rule in rules)
                    {
                        bool b = true;

                        foreach (Range range in rule.Ranges)
                        {
                            b = range.Includes(value);
                            if (b)
                                break;
                        }

                        if (b == false)
                        {
                            rule.PossiblePositions.Remove(i + 1);
                        }
                    }
                }

                if (index >= tickets.Count())
                    index = 0;

                foreach (Rule rule in rules.Where(w => w.PossiblePositions.Count() == 1))
                {
                    rule.Position = rule.PossiblePositions[0];
                    foreach (Rule innerRule in rules.Where(w => w != rule))
                    {
                        if (innerRule.PossiblePositions.Contains(rule.Position))
                            innerRule.PossiblePositions.Remove(rule.Position);
                    }
                }
            }

            foreach (Rule rule in rules.Where(w => w.Position == -1))
            {
                if (rule.PossiblePositions.Count() == 1)
                    rule.Position = rule.PossiblePositions[0];
            }
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day16_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != "")
                {
                    rules.Add(new Rule(s));
                }

                s = reader.ReadLine();
                s = reader.ReadLine();

                myTicket = new Ticket(rules, s);

                s = reader.ReadLine();
                s = reader.ReadLine();

                while ((s = reader.ReadLine()) != null)
                {
                    tickets.Add(new Ticket(rules, s));
                }
            }
        }

        static int GetErrorRate()
        {
            int _ret = 0;

            foreach (Ticket t in tickets)
                _ret += t.ErrorValue;

            return _ret;
        }
    }

    public class Rule
    {
        public string Name;
        public List<Range> Ranges = new List<Range>();
        public int Position = -1;
        public List<int> PossiblePositions = new List<int>();

        public Rule() { }

        public Rule(string s)
        {
            string[] import = s.Split(": ");
            this.Name = import[0];

            string[] rangeVals = import[1].Split(" or ");
            this.Ranges.Add(new Range(rangeVals[0]));
            this.Ranges.Add(new Range(rangeVals[1]));
        }

        public void InitPossiblePositions(int totalPositions)
        {
            for (int i = totalPositions; i > 0; i--)
                PossiblePositions.Add(i);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"({this.Position}) - {this.Name}: ");
            foreach (Range r in this.Ranges)
                sb.Append($"{r} ");

            return sb.ToString();
        }
    }

    public struct Range
    {
        public int Min;
        public int Max;

        public Range(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        public Range(string s)
        {
            string[] import = s.Split('-');
            this.Min = Convert.ToInt32(import[0]);
            this.Max = Convert.ToInt32(import[1]);
        }

        public override string ToString()
        {
            return $"{this.Min}-{this.Max}";
        }

        public bool Includes(int i)
        {
            return (i >= this.Min && i <= this.Max);
        }
    }

    public class Ticket
    {
        public bool Valid;
        public int[] Values;
        public int ErrorValue = 0;

        public Ticket(List<Rule> rules, string s)
        {
            this.Values = Array.ConvertAll(s.Split(','), x => Convert.ToInt32(x));
            this.Valid = CheckValid(rules);
        }

        private bool CheckValid(List<Rule> rules)
        {
            bool _ret = true;

            foreach (int i in this.Values)
            {
                bool b = true;

                foreach (Rule r in rules)
                {
                    foreach (Range range in r.Ranges)
                    {
                        b = range.Includes(i);

                        if (b == true)
                            break;
                    }

                    if (b == true)
                        break;
                }

                if (b == false)
                {
                    ErrorValue += i;
                    return false;
                }
            }

            return _ret;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"Valid? {this.Valid} ");
            foreach (int i in Values)
                sb.Append($"{i}, ");

            sb.Remove(sb.Length - 2, 2);

            return sb.ToString();
        }
    }
}
