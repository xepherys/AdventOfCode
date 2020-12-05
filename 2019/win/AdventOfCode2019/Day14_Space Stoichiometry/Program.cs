using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Day14_Space_Stoichiometry
{
    class Program
    {
        public static Recipes recipes = Recipes.Instance;
        public static Dictionary<string, Reaction2> reaction = new Dictionary<string, Reaction2>();
        public static string[] sep1 = new string[] { " => " };
        public static string[] sep2 = new string[] { ", " };

        static void Main(string[] args)
        {
            //FetchInput();
            reaction.Add("ORE", new Reaction2("ORE"));
            FetchInput2();
            /*
            foreach (Reaction recipe in recipes.RecipesList)
            {
                Console.WriteLine(recipe);
            }
            Console.ReadLine();

            //recipes.Rundown(recipes.RecipesList.Single(s => s.Result.Name == recipes.outcomeItem));

            foreach (KeyValuePair<string, int> kvp in recipes.Needs)
            {
                Console.WriteLine(String.Format("{0}: {1}", kvp.Key, kvp.Value));
            }
            Console.ReadLine();

            Console.WriteLine(String.Format("Making 1 {0} requires {1} {2}.", recipes.outcomeItem, recipes.CountAtomic(), recipes.atomicItem));
            */

            long requiredOre = GetRequiredOre(reaction);
            System.Console.WriteLine($"1. {requiredOre}");

            Console.ReadLine();

            long target = 1_000_000_000_000;
            long lower = (target / requiredOre) - 1_000;
            long higher = (target / requiredOre) + 1_000_000_000;
            while (lower < higher)
            {
                long mid = (lower + higher) / 2;
                long guess = GetRequiredOre(reaction, mid);
                if (guess > target)
                {
                    // System.Console.WriteLine($"MORE: {guess}");
                    higher = mid;
                }
                else if (guess < target)
                {
                    // System.Console.WriteLine($"LESS: {guess}");
                    if (mid == lower)
                        break;
                    lower = mid;
                }
                else
                {
                    lower = mid;
                    break;
                }
            }
            System.Console.WriteLine($"2. {lower}");

            Console.ReadLine();

            Environment.Exit(0);
        }

        public static void FetchInput()
        {
            string line;
            

            using (Stream stream = File.OpenRead(@"..\..\Day14_Input_Sample5.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    //19 NHKZB, 3 MBQVK, 10 HTSXK, 2 GXVQG, 8 VKHZC, 1 XHWH, 1 RCZF => 5 ZCLVG

                    recipes.RecipesList.Add(new Reaction(line));

                }
            }
        }

        public static void FetchInput2()
        {
            string line;

            using (Stream stream = File.OpenRead(@"..\..\Day14_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    string[] formula = line.Split(new string[] { " => " }, StringSplitOptions.RemoveEmptyEntries);

                    // result
                    string[] item = formula[1].Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    if (!reaction.ContainsKey(item[1]))
                        reaction.Add(item[1], new Reaction2(item[1], Int64.Parse(item[0])));
                    Reaction2 result = reaction[item[1]];
                    result.Output = Int64.Parse(item[0]);

                    // sources
                    foreach (string s in formula[0].Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        item = s.Trim().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        result.AddSource(item[1], Int64.Parse(item[0]));
                        if (!reaction.ContainsKey(item[1]))
                            reaction.Add(item[1], new Reaction2(item[1], 0));    // Skeleton reaction, to add product to
                        reaction[item[1]].AddProduct(result.Name, result.Output);
                    }
                }
            }
        }

        private static long GetRequiredOre(Dictionary<string, Reaction2> reaction, long fuelTarget = 1)
        {
            IEnumerable<string> ordered = new Topological(reaction).GetOrdered();
            Dictionary<string, long> quantity = new Dictionary<string, long>(ordered.Count());
            quantity["FUEL"] = fuelTarget;

            foreach (string item in ordered)
            {
                long output = reaction[item].Output;
                long needed = quantity[item];
                long toMake = (long)Math.Ceiling((decimal)needed / output);
                foreach (var dependency in reaction[item].GetDependencies())
                {
                    if (quantity.ContainsKey(dependency.Key))
                        quantity[dependency.Key] += dependency.Value * toMake;
                    else
                        quantity.Add(dependency.Key, dependency.Value * toMake);
                }
            }
            return quantity["ORE"];
        }
    }
}
