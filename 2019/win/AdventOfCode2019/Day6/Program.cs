using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day6
{
    class Program
    {
        public static SolarBodies sb;
        static void Main(string[] args)
        {
            sb = new SolarBodies();
            OrbitPair[] input = FetchInput();

            foreach (var v in input)
            {
                sb.Insert(v);
            }

            int answer = sb.CountYouToSanta();

            Console.WriteLine("Total jump distance: {0}", answer);
            Console.ReadLine();
            Environment.Exit(0);
        }

        public static OrbitPair[] FetchInput()
        {
            List<OrbitPair> orbitList = new List<OrbitPair>();

            using (Stream stream = File.OpenRead(@"..\..\..\Day06_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    orbitList.Add(new OrbitPair(s));
                }

                return orbitList.ToArray();
            }
        }
    }

    public class OrbitPair
    {
        public string Parent;
        public string Child;

        public OrbitPair(string line)
        {
            string[] s = line.Split(')');
            Parent = s[0];
            Child = s[1];
        }
    }

    public class SolarBody
    {
        public string Name;
        public string Parent;
        public List<SolarBody> Children;

        public SolarBody(string name, string parent = null)
        {
            this.Name = name;
            this.Parent = parent;
            Children = new List<SolarBody>();
        }
    }

    public class SolarBodies
    {
        public List<SolarBody> Bodies = new List<SolarBody>();

        public void Insert(SolarBody body)
        {
            Bodies.Add(body);

            foreach (SolarBody b in Bodies.Where(w => w.Name == body.Parent))
            {
                b.Children.Add(body);
            }
        }

        public void Insert(OrbitPair op)
        {
            if (Bodies.Where(c => c.Name == op.Child).Count() == 1 && Bodies.Where(c => c.Name == op.Parent).Count() == 1)
            {
                SolarBody child = Bodies.Single(s => s.Name == op.Child);
                SolarBody parent = Bodies.Single(s => s.Name == op.Parent);
                child.Parent = op.Parent;
                parent.Children.Add(child);
            }

            else if (Bodies.Where(c => c.Name == op.Child).Count() == 1)
            {
                SolarBody child = Bodies.Single(s => s.Name == op.Child);
                SolarBody parent = new SolarBody(op.Parent);
                child.Parent = op.Parent;
                parent.Children.Add(child);
                Bodies.Add(parent);
            }

            else if (Bodies.Where(c => c.Name == op.Parent).Count() == 1)
            {
                SolarBody parent = Bodies.Single(s => s.Name == op.Parent);
                SolarBody child = new SolarBody(op.Child, op.Parent);
                parent.Children.Add(child);
                Bodies.Add(child);
            }

            else
            {
                SolarBody parent = new SolarBody(op.Parent);
                SolarBody child = new SolarBody(op.Child, op.Parent);
                parent.Children.Add(child);
                Bodies.Add(child);
                Bodies.Add(parent);
            }
        }

        public int CountYouToSanta()
        {
            SolarBody you = Bodies.Single(s => s.Name == "YOU");
            SolarBody santa = Bodies.Single(s => s.Name == "SAN");

            List<string> youToRoot = new List<string>();
            List<string> santaToRoot = new List<string>();

            SolarBody parent = Bodies.Single(s => s.Name == you.Parent);

            while (parent.Parent != null)
            {
                youToRoot.Add(parent.Name);
                parent = Bodies.Single(s => s.Name == parent.Parent);
            }

            parent = Bodies.Single(s => s.Name == santa.Parent);

            while (parent.Parent != null)
            {
                santaToRoot.Add(parent.Name);
                parent = Bodies.Single(s => s.Name == parent.Parent);
            }

            // Find first intersection
            string cross = youToRoot.Intersect(santaToRoot).FirstOrDefault();

            Console.WriteLine("Crossing body: {0}", cross);

            List<string> youToCross = new List<string>();
            List<string> santaToCross = new List<string>();

            parent = Bodies.Single(s => s.Name == you.Name);

            while (parent.Parent != cross)
            {
                youToCross.Add(parent.Name);
                parent = Bodies.Single(s => s.Name == parent.Parent);

            }

            parent = Bodies.Single(s => s.Name == santa.Name);

            while (parent.Parent != cross)
            {
                santaToCross.Add(parent.Name);
                parent = Bodies.Single(s => s.Name == parent.Parent);
            }

            return youToCross.Count() + santaToCross.Count();
        }
    }

    public class OrbitTree
    {
        private Dictionary<string, List<string>> tree;

        public Dictionary<string, List<string>> Tree
        {
            get
            {
                return this.tree;
            }
        }

        public OrbitTree()
        {
            tree = new Dictionary<string, List<string>>();
        }

        public void Insert(OrbitPair orb)
        {
            // 1. If the tree is empty, return a new, single node 
            if (tree == null)
            {
                tree.Add(orb.Parent, new List<string> { orb.Child });
            }

            // 2. Otherwise, recur down the tree 
            InsertRec(orb.Parent, orb.Child);
        }
        private void InsertRec(string left, string right)
        {
            if (tree.ContainsKey(left))
            {
                tree[left].Add(right);
            }

            else
                tree.Add(left, new List<string> { right });
        }

        public int CountOrbits()
        {
            int count = 0;

            foreach (var v in this.Tree.Values)
            {
                count += RecurseCount(v);
            }

            return count;
        }

        private int RecurseCount(List<string> input)
        {
            int count = 0;
            foreach (var v in input)
            {
                string[] s = v.RemoveWhitespace().Split(',');
                foreach (var k in s)
                {
                    count++;

                    if (this.Tree.ContainsKey(k))
                    {
                        count += RecurseCount(this.Tree[k]);
                    }
                }
            }

            return count;
        }
    }

    public static class StringExtensions
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
    }

}
