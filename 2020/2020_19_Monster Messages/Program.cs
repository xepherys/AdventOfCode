using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace _2020_19_Monster_Messages
{
    class Program
    {
        public static RuleCollection _rules = RuleCollection.Instance;
        public static List<string> messages = new List<string>();
        public static int matchedMessageCount = 0;
        public static string pattern0;
        public static bool part2 = false;

        static void Main(string[] args)
        {
            Console.WriteLine("Importing Data...\n");
            ImportData();

            Console.WriteLine("Initializing rule set...\n");
            _rules.Initialize();

            if (part2)
                UpdateRules8And11();

            Console.WriteLine("Calculating match patterns...\n");
            _rules.CalculateStrings(_rules.Rule0);
            pattern0 = "^" + _rules.Rule0.MatchPattern + "$";

            Console.WriteLine("Parsing messages...\n");
            Console.WriteLine($"Pattern to match: {pattern0}");

            ConsoleColor color = Console.ForegroundColor;

            foreach (string message in messages)
            {
                if (Regex.Match(message, pattern0).Success)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"   {message}");
                    matchedMessageCount++;
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"   {message}");
                }
            }

            Console.ForegroundColor = color;

            Console.WriteLine($"\nValid messages: {matchedMessageCount}");

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day19_InputSample2.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != "")
                {
                    _rules.Add(s);
                }

                while ((s = reader.ReadLine()) != null)
                {
                    messages.Add(s);
                }
            }
        }

        static void UpdateRules8And11()
        {
            Rule r8 = _rules.Single(s => s.RuleNumber == 8);
            Rule r11 = _rules.Single(s => s.RuleNumber == 11);


            r8.Parsed = false;
            r11.Parsed = false;

            r8.MatchPattern = "[42]|[42][8]";
            //int[] new8 = new int[r8.SubRules.Length + 1];
            //for (int i = 0; i < r8.SubRules.Length; i++)
            //    new8[i] = r8.SubRules[i];
            //new8[new8.Length - 1] = 8;

            r11.MatchPattern = "[42][31]|[42][11][31]";

            //_rules.CalculateStrings(r8);
            //_rules.CalculateStrings(r11);

            r8.MatchPattern.Replace("[8]", "");
            r11.MatchPattern.Replace("[11]", "");

            //_rules.CalculateStrings(_rules.Single(s => s.RuleNumber == 0));
        }
    }

    public class Rule : IEquatable<Rule>
    {
        #region Fields
        int ruleNumber;
        char rootChar;
        string matchPattern;
        List<List<int>> subRules = new List<List<int>>();
        string importedRuleString;
        bool parsed = false;
        #endregion


        #region Properties
        public int RuleNumber
        {
            get
            {
                return this.ruleNumber;
            }
        }

        public char RootChar
        {
            get
            {
                return this.rootChar;
            }
        }

        public string MatchPattern
        {
            get
            {
                return this.matchPattern;
            }

            set
            {
                this.matchPattern = value;
            }
        }

        public List<List<int>> SubRules
        {
            get
            {
                return this.subRules;
            }
        }

        public bool Parsed
        {
            get
            {
                return this.parsed;
            }

            set
            {
                this.parsed = value;
            }
        }
        #endregion


        #region Constructors
        public Rule(string s)
        {
            /*
            0: 4 1 5
            1: 2 3 | 3 2
            2: 4 4 | 5 5
            3: 4 5 | 5 4
            4: "a"
            5: "b"
            */

            this.importedRuleString = s;

            // Parse Rule number from rule values
            string[] parsePass1 = s.Split(": "); // parsePass1[0] - rule number, parsePass1[1] fed to parsePass2[] for values

            this.ruleNumber = Convert.ToInt32(parsePass1[0]);
            this.matchPattern = parsePass1[1];
            FixMatchPattern();

            // Parse OR rules
            string[] parsePass2 = parsePass1[1].Split(" | "); // PasePass2[] each contains an OR rule

            // Parse individual rule sets
            List<string[]> parsePass3 = new List<string[]>();
            foreach (string pass2 in parsePass2)
            {
                parsePass3.Add(pass2.Split(" "));
            }

            if (parsePass3.Count == 1 && parsePass3[0][0].Contains("\""))
            {
                this.parsed = true;
                this.rootChar = Convert.ToChar(parsePass3[0][0].Replace("\"", ""));
            }

            else
            {
                foreach (var v in parsePass2)
                {
                    this.subRules.Add(new List<int> (Array.ConvertAll(v.Split(" "), int.Parse) ));
                }
            }
        }
        #endregion


        #region Methods
        private void FixMatchPattern()
        {
            if (this.matchPattern.Contains("\""))
            {
                foreach (char c in this.matchPattern)
                    if (Char.IsLetter(c))
                        this.matchPattern = c.ToString();
            }

            else
            {
                this.matchPattern = this.matchPattern.Replace(" ", "  ");
                this.matchPattern = " " + this.matchPattern + " ";

                //var v = Regex.Matches(this.matchPattern, @" \d+ ");
                //var u = Regex.Matches(this.matchPattern, @" \d+ ").Select(m => m.Value).Distinct();

                foreach (string m in Regex.Matches(this.matchPattern, @" \d+ ").OfType<Match>().Select(m => m.Value).Distinct())
                {
                    this.matchPattern = this.matchPattern.Replace(m, 
                        " [" + m + "] ");
                }

                
                //for (int i = this.matchPattern.Length - 1; i >= 0; i--)
                //{
                //    if (Char.IsNumber(this.matchPattern[i]))
                //    {
                //        this.matchPattern = this.matchPattern.Insert(i + 1, "]");
                //        this.matchPattern = this.matchPattern.Insert(i, "[");
                //    }
                //}
                

                this.matchPattern = this.matchPattern.Replace(" ", "");
                this.matchPattern = "(" + this.matchPattern + ")";
            }
        }
        #endregion


        #region Interface Implementations
        // IEquatable<Rule>
        public bool Equals([AllowNull] Rule other)
        {
            if (this.RootChar == other.RootChar && this.SubRules == other.SubRules)
                return true;
            else
                return false;
        }
        #endregion


        #region Overrides
        public override string ToString()
        {
            return $"{this.ruleNumber}: {this.matchPattern}";
        }

        public override int GetHashCode()
        {
            return this.importedRuleString.GetHashCode();
        }
        #endregion
    }

    public class RuleCollection : ICollection<Rule>
    {
        #region Fields
        private static readonly Lazy<RuleCollection> lazyRuleCollection = new Lazy<RuleCollection>(() => new RuleCollection());
        private static List<Rule> rules = new List<Rule>();
        private static List<int> rootRules = new List<int>();
        private static Rule rule0;
        private static bool initialized = false;
        #endregion


        #region Properties
        public List<Rule> Rules
        {
            get
            {
                return rules;
            }
        }

        public bool Initialized
        {
            get
            {
                return initialized;
            }
        }

        public Rule Rule0
        {
            get
            {
                return rule0;
            }
        }
        #endregion


        #region Constructors
        private RuleCollection()
        {
        }

        public static RuleCollection Instance
        {
            get
            {
                return lazyRuleCollection.Value;
            }
        }
        #endregion


        #region Interface Implementations
        // IEnumerable<Rule>
        public IEnumerator<Rule> GetEnumerator()
        {
            return rules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return rules.GetEnumerator();
        }


        // ICollection<Rule>
        public int Count => rules.Count;

        public bool IsReadOnly => false;

        public void Add(Rule item)
        {
            rules.Add(item);
        }

        public void Add(string s)
        {
            rules.Add(new Rule(s));
        }

        public bool Remove(Rule item)
        {
            return rules.Remove(item);
        }

        public void Clear()
        {
            rules.Clear();
        }

        public bool Contains(Rule item)
        {
            return rules.Contains(item);
        }

        public void CopyTo(Rule[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }
        #endregion


        #region Methods
        public bool Initialize()
        {
            initialized = false;

            if (rules.Count == 0)
                return false;

            if (!FindRootValues())
                return false;

            if (!SetRule0())
                return false;

            initialized = true;
            return true;
        }

        private bool FindRootValues()
        {
            rootRules = rules.Where(w => w.RootChar != '\0').Select(s => s.RuleNumber).ToList();
            return true;
        }

        private bool SetRule0()
        {
            rule0 = rules.Single(s => s.RuleNumber == 0);
            return true;
        }

        public void CalculateStrings(Rule rule)
        {
            foreach (List<int> l in rule.SubRules)
            {
                foreach (int i in l)
                {
                    Rule r = rules.Single(s => s.RuleNumber == i);
                    if (!r.Parsed)
                    {
                        CalculateStrings(r);
                    }
                }
            }

            foreach (int i in rootRules)
            {
                rule.MatchPattern = rule.MatchPattern.Replace($"[{i}]", $"{rules.Single(s => s.RuleNumber == i).MatchPattern}");
            }

            foreach (Match m in Regex.Matches(rule.MatchPattern, @"[[](\d+)[]]").Distinct())
            {
                rule.MatchPattern = rule.MatchPattern.Replace(m.Groups[0].ToString(), 
                                    rules.Single(s => s.RuleNumber == Convert.ToInt32(m.Groups[1].ToString())).MatchPattern);
            }

            rule.Parsed = true;
        }
        #endregion
    }
}
