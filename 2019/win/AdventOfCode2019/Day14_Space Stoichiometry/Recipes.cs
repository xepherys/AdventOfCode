using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Day14_Space_Stoichiometry
{
    public class Recipes
    {
        #region Fields
        private static readonly Lazy<Recipes> lazy = new Lazy<Recipes>(() => new Recipes());
        public List<Reaction> RecipesList = new List<Reaction>();
        public Dictionary<string, int> Needs = new Dictionary<string, int>();
        public int OreCount = 0;
        public string outcomeItem = "FUEL";
        public string atomicItem = "ORE";
        #endregion

        #region Properties
        #endregion

        #region Constructors
        private Recipes()
        {
        }

        public static Recipes Instance
        {
            get
            {
                return lazy.Value;
            }
        }
        #endregion

        #region Methods
        public void Rundown(Reaction recipe, int qty = 1)
        {
            foreach (Reactant ing in recipe.Ingredients)
            {
                if (ing.Name != atomicItem)
                {
                    Reaction r = RecipesList.Single(s => s.Result.Name == ing.Name);
                    if (!Needs.ContainsKey(ing.Name))
                        Needs.Add(ing.Name, 0);

                    int i = 1;
                    while (i * recipe.Result.Quantity < qty)
                        i++;

                    Console.WriteLine(String.Format("Adding {0} {1} to the queue.", ing.Quantity * i, ing.Name));
                    Needs[ing.Name] += ing.Quantity * i;

                    Rundown(r, ing.Quantity * i);
                }
            }
        }

        public int CountAtomic()
        {
            int _ret = 0;

            foreach (Reaction r in RecipesList.Where(w => w.ConvertsAtomic))
            {
                int left = r.Ingredients[0].Quantity;
                int right = r.Result.Quantity;
                int need = Needs[r.Result.Name];
                int i = 1;

                while (i * right < need)
                    i++;

                int result = i * left;
                Console.WriteLine(String.Format("Consuming {0} {1} to make {2} {3}.", result, atomicItem, i * right, r.Result.Name));
                _ret += result;
            }

            return _ret;
        }
        #endregion
    }

    public struct Reaction
    {
        public Reactant[] Ingredients;
        public Reactant Result;
        private Recipes recipes;
        public bool ConvertsAtomic;

        public Reaction(string s)
        {
            this.ConvertsAtomic = false;

            string[] vals = s.Split(new string[] { " => " }, StringSplitOptions.RemoveEmptyEntries);
            this.Result = new Reactant(vals[1]);

            string[] ing = vals[0].Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
            this.Ingredients = new Reactant[ing.Length];

            for (int i = 0; i < ing.Length; i++)
            {
                this.Ingredients[i] = new Reactant(ing[i]);
            }

            this.recipes = Recipes.Instance;

            if (ing.Length == 1 && ing[0].Contains(recipes.atomicItem))
            {
                this.ConvertsAtomic = true;
            }
        }

        public void MakeRecipe()
        {
        }

        public bool IsOre()
        {
            if (Ingredients.Length > 1 || Ingredients[0].Name != "ORE")
                return false;

            else
                return true;
        }

        public string IngredientsToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.Ingredients.Length; i++)
            {
                sb.Append(this.Ingredients[i]);

                if (i < this.Ingredients.Length - 1)
                    sb.Append(", ");
            }

            return sb.ToString();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < this.Ingredients.Length; i++)
            {
                sb.Append(this.Ingredients[i]);

                if (i < this.Ingredients.Length - 1)
                    sb.Append(", ");
            }

            sb.Append(" => ");

            sb.Append(this.Result);

            return sb.ToString();
        }
    }

    class Reaction2
    {
        public string Name
        {
            get;
        }
        public long Output
        {
            get; set;
        }
        private Dictionary<string, long> input = new Dictionary<string, long>();       // Chemicals that go into Name
        private Dictionary<string, long> product = new Dictionary<string, long>();    // Chemicals that require Name

        public Reaction2(string name, long output = 1)
        {
            this.Name = name;
            this.Output = output;
        }

        public void AddSource(string name, long quantity)
        {
            input.Add(name, quantity);
        }

        public void AddProduct(string name, long quantity)
        {
            product.Add(name, quantity);
        }

        public IEnumerable<KeyValuePair<string, long>> GetDependencies() => input;

        public IEnumerable<KeyValuePair<string, long>> GetProducts() => product;
    }

    class Ore : Reaction2
    {
        public Ore() : base("Ore") { }
    }

    public struct Reactant
    {
        public int Quantity;
        public string Name;

        public Reactant(string s)
        {
            string[] vals = s.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            Int32.TryParse(vals[0], out this.Quantity);
            this.Name = vals[1];
        }

        public override string ToString()
        {
            return this.Quantity + " " + this.Name;
        }
    }

    public class Item
    {
        public int QuantityNeeded;
        public int QuantityCreated;
        public Reaction recipe;
        public string Name;

        public Item(string name, Reaction recipe)
        {
            this.Name = name;
            this.QuantityCreated = 0;
            this.QuantityNeeded = 0;
            this.recipe = recipe;
        }

        public void AddNeeded(int amount)
        {
            this.QuantityNeeded += amount;
            //MakeIfNeeded();
        }

        private void MakeIfNeeded()
        {
            if (this.QuantityNeeded > this.QuantityCreated)
            {
                recipe.MakeRecipe();
            }
        }

        public override string ToString()
        {
            return String.Format("{0}: \nNeed: {1}\nMade: {2}", this.Name, this.QuantityNeeded, this.QuantityCreated);
        }
    }

    class Topological
    {
        private List<string> depthFirstOrder;
        private HashSet<string> marked;

        public Topological(Dictionary<string, Reaction2> reaction)
        {
            depthFirstOrder = new List<string>(reaction.Count);
            marked = new HashSet<string>(reaction.Count);

            foreach (string item in reaction.Keys)
                if (!marked.Contains(item))
                    DepthFirstSearch(reaction, item);

            // depthFirstOrder.Reverse();
        }

        private void DepthFirstSearch(Dictionary<string, Reaction2> reaction, string start)
        {
            marked.Add(start);

            foreach (var item in reaction[start].GetProducts())
                if (!marked.Contains(item.Key))
                    DepthFirstSearch(reaction, item.Key);

            depthFirstOrder.Add(start);
        }

        public IEnumerable<string> GetOrdered() => depthFirstOrder;
    }
}
