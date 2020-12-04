using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2018.Core
{
    class Sorting
    {
        /// <summary>
        /// Topological Sorting (Kahn's algorithm) 
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <remarks>https://gist.github.com/Sup3rc4l1fr4g1l1571c3xp14l1d0c10u5/3341dba6a53d7171fe3397d13d00ee3f</remarks>
        /// <remarks>https://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <returns>Sorted node in topological order.</returns>
        public static List<T> TopologicalSort<T>(HashSet<T> nodes, HashSet<Tuple<T, T>> edges, bool orderedEdges = false) where T : IEquatable<T>
        {
            // Empty list that will contain the sorted elements
            List<T> L = new List<T>();

            // Set of all nodes with no incoming edges
            HashSet<T> S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            // while S is non-empty do
            while (S.Any())
            {
                T n;
                //  remove a node n from S
                if (orderedEdges)
                    n = S.OrderBy(s => s).First();
                else
                    n = S.First();

                S.Remove(n);

                // add n to tail of L
                L.Add(n);

                // for each node m with an edge e from n to m do
                foreach (var e in edges.Where(e => e.Item1.Equals(n)).ToList())
                {
                    var m = e.Item2;

                    // remove edge e from the graph
                    edges.Remove(e);

                    // if m has no other incoming edges then
                    if (edges.All(me => me.Item2.Equals(m) == false))
                    {
                        // insert m into S
                        S.Add(m);
                    }
                }
            }

            // if graph has edges then
            if (edges.Any())
            {
                // return error (graph has at least one cycle)
                return null;
            }
            else
            {
                // return L (a topologically sorted order)
                return L;
            }
        }

        /// <summary>
        /// Topological Sorting (Modified Kahn's algorithm to return ordered list of groups) 
        /// </summary>
        /// <remarks>https://en.wikipedia.org/wiki/Topological_sorting</remarks>
        /// <remarks>https://gist.github.com/Sup3rc4l1fr4g1l1571c3xp14l1d0c10u5/3341dba6a53d7171fe3397d13d00ee3f</remarks>
        /// <remarks>https://www.codeproject.com/Articles/869059/Topological-sorting-in-Csharp</remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="nodes">All nodes of directed acyclic graph.</param>
        /// <param name="edges">All edges of directed acyclic graph.</param>
        /// <returns>Sorted node in topological order.</returns>
        public static List<List<T>> TopologicalSortGroups<T>(HashSet<T> nodes, HashSet<Tuple<T, T>> edges, bool orderedEdges = false) where T : IEquatable<T>
        {
            // Empty list that will contain the sorted elements
            List<List<T>> L = new List<List <T>>();

            // Set of all nodes with no incoming edges
            HashSet<T> S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));

            // while S is non-empty do
            while (S.Any())
            {
                List<T> nl = new List<T>();
                
                // Grab all first-order (non-edge) nodes and give L a List<T> of them
                if (orderedEdges)
                    nl = S.OrderBy(s => s).ToList();
                else
                    nl = S.ToList();

                //S.Clear();

                // add n to L
                L.Add(nl);

                foreach (var ni in nl)
                {
                    foreach (var e in edges.Where(e => e.Item1.Equals(ni)).ToList())
                    {
                        edges.Remove(e);
                    }

                    nodes.Remove(ni);
                }

                S = new HashSet<T>(nodes.Where(n => edges.All(e => e.Item2.Equals(n) == false)));
            }

            // if graph has edges then
            if (edges.Any())
            {
                // return error (graph has at least one cycle)
                return null;
            }
            else
            {
                // return L (a topologically sorted order)
                return L;
            }
        }
    }


    /// <summary>
    /// https://stackoverflow.com/questions/828398/how-to-create-a-binary-tree as starting point.
    /// </summary>
    public class BinaryTree
    {
        #region Fields
        Queue<int> dataArray = new Queue<int>();
        List<Day8Node> resultingNodes = new List<Day8Node>();
        Day8Node currentNode;
        int nodeIdCounter = 0;
        #endregion

        #region Properties
        public Queue<int> DataArray
        {
            get
            {
                return this.dataArray;
            }

            set
            {
                this.dataArray = value;
            }
        }
        public Day8Node CurrentNode
        {
            get
            {
                return this.currentNode;
            }
            set
            {
                this.currentNode = value;
            }
        }
        public Day8Node ParentNode
        {
            get
            {
                return this.resultingNodes.FirstOrDefault(s => s.ID == currentNode.ParentID);
            }

            set
            {
                
            }
        }
        public int NodeIdCounter
        {
            get
            {
                return this.nodeIdCounter;
            }

            set
            {
                this.nodeIdCounter = value;
            }
        }
        #endregion

        #region Constructors
        public BinaryTree()
        {
            
        }

        public BinaryTree(int[] _array)
        {
            this.DataArray = new Queue<int>(_array);
        }
        #endregion

        #region Methods
        protected int AssignId()
        {
            int i = this.NodeIdCounter;
            this.NodeIdCounter++;
            return i;
        }
        public void RunTree()
        {
            if (this.DataArray.Count == 0)
            {
                return;
                // Exception?
            }

            // Initial node
            Day8Node node = new Day8Node();
            node.ID = AssignId();
            node.NumChildNodes = this.DataArray.Dequeue();
            node.NumMetadataEntries = this.DataArray.Dequeue();
            resultingNodes.Add(node);
            this.CurrentNode = node;

            while (this.CurrentNode != null)
            {
                BuildNextNodes();
            }
        }

        public void BuildNextNodes()
        {
            while (this.CurrentNode.NumChildNodes > this.CurrentNode.ChildNodeIds.Count)
            {
                Day8Node node = new Day8Node();
                node.ID = AssignId();
                node.ParentID = this.CurrentNode.ID;
                node.NumChildNodes = this.DataArray.Dequeue();
                node.NumMetadataEntries = this.DataArray.Dequeue();
                resultingNodes.Add(node);
                this.CurrentNode = node;
                this.ParentNode.ChildNodeIds.Add(node.ID);

                if (this.CurrentNode.NumChildNodes > this.CurrentNode.ChildNodeIds.Count)
                {
                    
                    BuildNextNodes();
                }
            }

            for (int i = 0; i < this.CurrentNode.NumMetadataEntries; i++)
            {
                this.CurrentNode.MetadataEntries.Add(this.DataArray.Dequeue());
            }

            if (this.CurrentNode.ParentID != -99)
                this.ParentNode.ChildNodesCompleted.Add(this.CurrentNode.ID);

            if (this.CurrentNode.NumChildNodes == this.CurrentNode.ChildNodesCompleted.Count && this.CurrentNode != null)
            {
                CheckParent();

                return;
            }
        }

        public void CheckParent()
        {
            this.CurrentNode = this.resultingNodes.FirstOrDefault(s => s.ID == this.CurrentNode.ParentID);
        }

        public int CalculateMetadataSum()
        {
            int _ret = 0;

            foreach (Day8Node node in this.resultingNodes.Where(w => w.NumMetadataEntries > 0))
            {
                foreach (int datapoint in node.MetadataEntries)
                {
                    _ret += datapoint;
                }
            }

            return _ret;
        }

        public int CalculateNodeValue(Day8Node node)
        {
            if (node.NodeValue != 0)
            {
                return node.NodeValue;
            }

            int _ret = 0;
            List<int> childIds = new List<int>();

            if (node.NumChildNodes == 0)
            {
                node.NodeValue = node.Sum();
                return node.NodeValue;
            }

            foreach (int i in node.MetadataEntries)
            {
                if (node.ChildNodeIds.Count >= i && i > 0)
                {
                    _ret += CalculateNodeValue(resultingNodes.FirstOrDefault(w => w.ID == node.ChildNodeIds[i - 1]));
                }
            }

            node.NodeValue = _ret;
            return _ret;
        }

        /*
        private List<Day8Node> SetBottomValues()
        {
            List<Day8Node> _ret = new List<Day8Node>();
            foreach (Day8Node node in resultingNodes)
            {
                if (node.NumChildNodes == 0)
                {
                    node.NodeValue = node.Sum();
                    _ret.Add(node);
                }
            }

            return _ret;
        }

        private List<Day8Node> SetNextLevelValues(List<Day8Node> currentLevel)
        {
            List<Day8Node> _ret = new List<Day8Node>();

            foreach (Day8Node node in currentLevel)
            {
                _ret.Add(node);

            }

            return _ret;
        }
        */
        public Day8Node GetRootNode()
        {
            return resultingNodes.Single(w => w.ID == 0);
        }
        #endregion
    }

    public class Day8Node
    {
        #region Fields
        private int id = -99;
        private int parentId = -99;
        private int numChildNodes = -99;
        private List<int> childNodeIds = new List<int>();
        private List<int> childNodeCompleted = new List<int>();
        private int numMetadataEntries = -99;
        private List<int> metadataEntries = new List<int>();
        private int nodeValue = 0;
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id == -99)
                    this.id = value;
                else
                    return;  // Exception?
            }
        }

        public int ParentID
        {
            get
            {
                if (this.parentId == -99)
                {
                    return -99;
                    // Exception?
                }
                else
                    return this.parentId;
            }

            set
            {
                if (this.parentId == -99)
                    this.parentId = value;
                else
                    return;  // Exception?
            }
        }

        public int NumChildNodes
        {
            get
            {
                if (this.numChildNodes == -99)
                {
                    return -99;
                    // Exception?
                }
                else
                    return this.numChildNodes;
            }

            set
            {
                if (this.numChildNodes == -99)
                    this.numChildNodes = value;
                else
                    return;
                    // Exception?
            }
        }

        public List<int> ChildNodeIds
        {
            get
            {
                return this.childNodeIds;
            }

            set
            {
                this.childNodeIds = value;
            }
        }

        public List<int> ChildNodesCompleted
        {
            get
            {
                return this.childNodeCompleted;
            }

            set
            {
                this.childNodeCompleted = value;
            }
        }

        public int NumMetadataEntries
        {
            get
            {
                if (this.numMetadataEntries == -99)
                {
                    return -99;
                    // Exception?
                }
                else
                    return this.numMetadataEntries;
            }

            set
            {
                if (this.numMetadataEntries == -99)
                    this.numMetadataEntries = value;
                else
                    return;
                // Exception?
            }
        }

        public List<int> MetadataEntries
        {
            get
            {
                return this.metadataEntries;
            }

            set
            {
                this.metadataEntries = value;
            }
        }

        public int NodeValue
        {
            get
            {
                return this.nodeValue;
            }

            set
            {
                this.nodeValue = value;
            }
        }
        #endregion

        #region Methods
        public int Sum()
        {
            int _ret = 0;

            foreach (int i in this.MetadataEntries)
            {
                _ret += i;
            }

            return _ret;
        }
        #endregion
    }
}
