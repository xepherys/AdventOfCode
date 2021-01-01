using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020_21_Allergen_Assessment
{
    class Program
    {
        public static string importPath = @"..\..\..\Day21_Input.txt";
        public static RecipeBook rb = new RecipeBook();

        static void Main(string[] args)
        {
            ImportData();

            rb.InitializeAllergenPossibilities();

            Console.WriteLine("Part 1:\n");

            int count = 0;
            foreach (Recipe r in rb.Recipes)
            {
                foreach (string ingredient in r.Ingredients.Where(w => !(rb.PossibleAllergens.Contains(w))))
                {
                    count++;
                }
            }

            Console.WriteLine($"Total ingredients definitely not an allergen: {count}");

            Console.WriteLine("\nPart 2:\n");

            string line = String.Empty;

            foreach (KeyValuePair<string, List<string>> kvp in rb.AllergenIngredients.OrderBy(o => o.Key))
            {
                    line += $"{kvp.Value[0]},";
            }

            line = line.Remove(line.Length - 1, 1);

            Console.WriteLine(line);

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(importPath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null)
                {
                    s = s.Replace("(", "").Replace(")", "");

                    string[] parsed = s.Split("contains ", StringSplitOptions.RemoveEmptyEntries);

                    Recipe r = new Recipe();
                    r.Ingredients = new List<string>(parsed[0].Split(" ", StringSplitOptions.RemoveEmptyEntries));

                    if (parsed.Length > 1 && !String.IsNullOrEmpty(parsed[1]))
                        r.Allergens = new List<string>(parsed[1].Split(", ", StringSplitOptions.RemoveEmptyEntries));

                    rb.Add(r);
                }
            }
        }
    }

    public class Recipe : IEquatable<Recipe>
    {
        List<string> ingredients = new List<string>();
        List<string> allergens = new List<string>();

        public List<string> Ingredients
        {
            get
            {
                return this.ingredients;
            }

            set
            {
                this.ingredients = value;
            }
        }

        public List<string> Allergens
        {
            get
            {
                return this.allergens;
            }

            set
            {
                this.allergens = value;
            }
        }

        public bool Equals(Recipe other)
        {
            return this.ToString() == other.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Recipe))
                return false;

            return this.Equals((Recipe)obj);
        }

        public static bool operator ==(Recipe a, Recipe b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Recipe a, Recipe b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        public override string ToString()
        {
            string _ret = String.Empty;

            foreach (string ingredient in this.ingredients)
            {
                _ret += ingredient + " ";
            }

            _ret = _ret.Remove(_ret.Length - 1, 1);

            _ret += "(contains ";

            foreach (string allergen in this.allergens)
            {
                _ret += allergen + ", ";
            }

            _ret = _ret.Remove(_ret.Length - 2, 2);

            _ret += ")";

            return _ret;
        }
    }

    public class RecipeBook : IEnumerable<Recipe>
    {
        List<Recipe> recipes = new List<Recipe>();
        List<string> knownAllergenIngredients = new List<string>();
        Dictionary<string, List<string>> allergenIngredients = new Dictionary<string, List<string>>();

        public List<Recipe> Recipes
        {
            get
            {
                return this.recipes;
            }
        }

        public Dictionary<string, List<string>> AllergenIngredients
        {
            get
            {
                return this.allergenIngredients;
            }
        }

        public List<string> PossibleAllergens
        {
            get
            {
                List<string> _ret = new List<string>();

                foreach (KeyValuePair<string, List<string>> kvp in this.allergenIngredients)
                {
                    foreach (string s in kvp.Value)
                    {
                        if (!_ret.Contains(s))
                            _ret.Add(s);
                    }
                }

                return _ret;
            }
        }

        public int Count
        {
            get
            {
                return this.recipes.Count;
            }
        }

        public void Add(Recipe r)
        {
            this.recipes.Add(r);
        }

        public void InitializeAllergenPossibilities()
        {
            foreach (Recipe r in this.recipes)
            {
                foreach (string allergen in r.Allergens)
                {
                    bool initialAdd = false;

                    // Adding an allergen for the first time
                    if (!allergenIngredients.ContainsKey(allergen))
                    {
                        allergenIngredients[allergen] = new List<string>();
                        initialAdd = true;
                    }

                    // If we just added a new allergen, add all ingredients as possible options
                    if (initialAdd)
                    {
                        foreach (string ingredient in r.Ingredients)
                        {
                            allergenIngredients[allergen].Add(ingredient);
                        }
                    }

                    // If we're seeing an allergen for the 2+ time, remove any ingredients that already exist and aren't in the new list
                    else
                    {
                        List<string> removeIngredient = new List<string>();

                        foreach (string ingredient in allergenIngredients[allergen])
                        {
                            if (!r.Ingredients.Contains(ingredient))
                            {
                                removeIngredient.Add(ingredient);
                            }
                        }

                        allergenIngredients[allergen].Remove(removeIngredient);
                    }
                }
            }

            while (allergenIngredients.Any(a => a.Value.Count > 1))
                foreach (List<string> ingredient in allergenIngredients.Values.Where(w => w.Count == 1).ToList())
                    foreach (List<string> list in allergenIngredients.Values.Where(w => w.Contains(ingredient[0]) && w.Count > 1))
                        list.Remove(ingredient);
        }

        // For debug use
        public void PrintAllergens()
        {
            foreach (KeyValuePair<string, List<string>> kvp in allergenIngredients)
            {
                string line = $"{kvp.Key}: ";
                foreach (string s in kvp.Value)
                {
                    line += $"{s}, ";
                }

                line = line.Remove(line.Length - 2, 2);

                Console.WriteLine(line);
            }
        }

        public IEnumerable<string> FindNonAllergenicIngredients()
        {
            foreach (Recipe r in this.recipes)
            {
                foreach (string i in r.Ingredients)
                {
                    bool allergen = false;

                    foreach (Recipe r2 in this.recipes.Where(w => w != r))
                    {
                        if (r2.Ingredients.Contains(i) && r.Allergens.Any(a => r2.Allergens.Contains(a)))
                        {
                            allergen = true;
                            break;
                        }
                    }

                    if (allergen && !knownAllergenIngredients.Contains(i))
                        knownAllergenIngredients.Add(i);
                }
            }

            foreach (Recipe r in this.recipes)
                foreach (string i in r.Ingredients)
                    if (!knownAllergenIngredients.Contains(i))
                        yield return i;
        }

        public IEnumerator<Recipe> GetEnumerator()
        {
            return this.recipes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.recipes.GetEnumerator();
        }
    }

    public static class ListExtensions
    {
        public static void Remove<T>(this List<T> source, List<T> other)
        {
            foreach (T item in other)
            {
                source.Remove(item);
            }
        }
    }
}
