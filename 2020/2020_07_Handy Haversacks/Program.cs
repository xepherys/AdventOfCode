using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020_07_Handy_Haversacks
{
    class Program
    {
        public static List<Bag> bags = new List<Bag>();
        public static List<Rule> rules = new List<Rule>();
        public static List<Rule> part1 = new List<Rule>();
        public static List<Rule> applicableRules = new List<Rule>();

        static void Main(string[] args)
        {
            ImportData();

            Bag b = new Bag("shiny", "gold");

            Part1(b);

            int answer = part1.Count();

            Console.WriteLine("Part 1: " + answer + " bags can eventually contain at least one " + b.Adjective + " " + b.Color + " bag.");
            Console.Read();
            Console.Clear();

            answer = Part2(rules.Single(s => s.Bag == b));
            //foreach (var r in ruleList.nodes)
            //    Console.WriteLine((Rule)r.nodeObject);

            
            Console.WriteLine("Part 2: " + answer);
            
            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day7_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null)
                {
                    string[] outerArr = s.Split(" bags contain ", StringSplitOptions.RemoveEmptyEntries);
                    string[] innerArr = outerArr[1].Split(", ", StringSplitOptions.RemoveEmptyEntries);

                    string[] outerBag = outerArr[0].Split(' ');
                    Bag oBag = new Bag(outerBag[0], outerBag[1]);
                    if (!bags.Contains(oBag))
                        bags.Add(oBag);

                    Rule r = new Rule();
                    r.Bag = oBag;
                    r.HeldBag = new Dictionary<Bag, int>();

                    foreach (string inners in innerArr)
                    {
                        if (inners == "no other bags.")
                            break;

                        else
                        {
                            string[] ruleParse = inners.Split(' ');
                            Bag iBag = new Bag(ruleParse[1], ruleParse[2]);
                            if (!bags.Contains(iBag))
                                bags.Add(iBag);

                            r.HeldBag.Add(iBag, Int32.Parse(ruleParse[0]));
                        }
                    }

                    rules.Add(r);
                }
            }
        }

        static void Part1(Bag b)
        {
            foreach (Rule r in rules)
            {
                if (r.HeldBag.ContainsKey(b))
                {
                    if (!part1.Contains(r))
                        part1.Add(r);
                    
                    Part1(r.Bag);
                }
            }
        }

        static int Part2(Rule r)
        {
            int _ret = 0;

            foreach (KeyValuePair<Bag, int> kvp in r.HeldBag)
            {
                _ret += kvp.Value * CountDown(kvp.Key);
            }

            return _ret;
        }

        static int CountDown(Bag b)
        {
            int _ret = 1;

            Rule r = rules.Single(s => s.Bag == b);

            if (r.HeldBag.Count > 0)
            {
                foreach (KeyValuePair<Bag, int> kvp in r.HeldBag)
                {
                    _ret += kvp.Value * CountDown(kvp.Key);
                }
            }

            Console.WriteLine("Adding: " + _ret + " from rule: " + r);
            return _ret;
        }
    }

    public class Rule : IEquatable<Rule>
    {
        public Bag Bag { get; set; }
        public Dictionary<Bag, int> HeldBag { get; set; }

        public override string ToString()
        {
            string _ret = $"{Bag.Adjective} {Bag.Color} bags contain ";

            if (HeldBag.Count() == 0)
                _ret += "no other bags.";

            else
            {
                foreach (KeyValuePair<Bag, int> kvp in HeldBag)
                {
                    _ret += kvp.Value + " " + kvp.Key.ToString() + ((kvp.Value > 1) ? " bags, " : " bag, ");
                }

                _ret = _ret.Remove(_ret.Length - 2, 2);
                _ret += ".";
            }

            return _ret;
        }

        public bool Equals(Rule other)
        {
            return (this.Bag == other.Bag);
        }

        public override int GetHashCode()
        {
            return Bag.GetHashCode();
        }
    }

    public class Bag : IEquatable<Bag>
    {
        public string Adjective { get; set; }
        public string Color { get; set; }

        public Bag(string a, string c)
        {
            this.Adjective = a;
            this.Color = c;
        }

        public override string ToString()
        {
            return $"{Adjective} {Color}";
        }

        public static bool operator ==(Bag a, Bag b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Bag a, Bag b)
        {
            return !a.Equals(b);
        }

        public bool Equals(Bag other)
        {
            return (this.Adjective == other.Adjective && this.Color == other.Color);
        }

        public override bool Equals(Object other)
        {
            return (this.Adjective == (other as Bag).Adjective && this.Color == (other as Bag).Color);
        }

        public override int GetHashCode()
        {
            return (Adjective + Color).GetHashCode();
        }
    }

    public class NonBinaryTree<T>
    {
        public List<Node<T>> nodes = new List<Node<T>>();

        public void Add(T item)
        {
            Node<T> n = new Node<T>();
            n.nodeObject = item;
            n.nodeId = nodes.Count();
            nodes.Add(n);
        }

        public void AddChild(T item, T parent)
        {
            Node<T> n = new Node<T>();
            n.nodeObject = item;
            n.nodeId = nodes.Count();
            nodes.Add(n);

            Node<T> p = nodes.Single(s => s.nodeObject.Equals(parent));
            p.nodeChildrenIds.Add(n.nodeId);

            n.nodeParentId = p.nodeId;
        }

        public class Node<U>
        {
            public U nodeObject;
            public int nodeId;
            public int nodeParentId;
            public List<int> nodeChildrenIds = new List<int>();
        }
    }
}