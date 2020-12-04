using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode2018.Core;
using System.Diagnostics;

namespace Day22
{
    class Program
    {
        public static long depth = 3339;
        static Point strandedDude = new Point(10, 715);
        static Point extendedBound = new Point(strandedDude.X + 15, strandedDude.Y + 15);
        static Point entryPoint = new Point(0, 0);
        static List<Region> regions = new List<Region>();

        static void Main(string[] args)
        {
            long risk = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Console.WriteLine("Working on part A...");
            for (int y = entryPoint.Y; y <= strandedDude.Y; y++)
            {
                for (int x = entryPoint.X; x <= strandedDude.X; x++)
                {
                    regions.Add(new Region(x, y));
                }
            }
            
            Region e = regions.Single(s => s.P.X == entryPoint.X && s.P.Y == entryPoint.Y);
            e.C = 'E';
            Region t = regions.Single(s => s.P.X == strandedDude.X && s.P.Y == strandedDude.Y);
            t.C = 'T';

            // Set region types
            foreach (Region r in regions)
            {
                r.CalcType(regions);
            }

            foreach (Region r in regions)
            {
                if (r.T == RType.WET)
                    risk += 1;

                else if (r.T == RType.NARROW)
                    risk += 2;
            }

            /*
            for (int y = entryPoint.Y; y <= strandedDude.Y; y++)
            {
                for (int x = entryPoint.X; x <= strandedDude.X; x++)
                {
                    Console.Write(regions.Single(s => s.P.X == x && s.P.Y == y).C.ToString());
                }
                Console.Write(Environment.NewLine);
            }
            */

            sw.Stop();
            Console.WriteLine("Total risk is: " + risk.ToString() + ", calculated in " + sw.ElapsedMilliseconds + "ms.");
            Console.WriteLine("Working on part B...");
            sw.Reset();
            sw.Start();
            for (int y = strandedDude.Y + 1; y <= extendedBound.Y; y++)
            {
                for (int x = strandedDude.X + 1; x <= extendedBound.X; x++)
                {
                    regions.Add(new Region(x, y));
                }
            }

            foreach (Region r in regions.Where(w => w.T == RType.NONE))
            {
                r.CalcType(regions);
            }

            //AStarNav aNav = new AStarNav();

            Console.ReadKey();
            Environment.Exit(0);
        }
    }

    public class Region
    {
        public Point P { get; set; }
        public RType T { get; set; }
        public Char C { get; set; }
        public long GeologicalIndex { get; set; }
        public long ErosionLevel { get; set; }

        public Region(int x, int y)
        {
            P = new Point(x, y);
            T = RType.NONE;
            C = '-';
            GeologicalIndex = -1;
            ErosionLevel = -1;
        }

        public void CalcType(List<Region> regions)
        {
            if (GeologicalIndex == -1)
            {
                if ((this.C == 'E') || (this.C == 'T'))
                {
                    this.GeologicalIndex = 0;
                }

                else if (this.P.Y == 0)
                {
                    this.GeologicalIndex = this.P.X * 16807;
                }

                else if (this.P.X == 0)
                {
                    this.GeologicalIndex = this.P.Y * 48271;
                }

                else
                {
                    int thisX = P.X;
                    int thisY = P.Y;
                    Region a = regions.Single(s => s.P.X == thisX - 1 && s.P.Y == thisY);
                    Region b = regions.Single(s => s.P.X == thisX && s.P.Y == thisY - 1);

                    if (a.GeologicalIndex == -1)
                        a.CalcType(regions);
                    if (b.GeologicalIndex == -1)
                        b.CalcType(regions);

                    this.GeologicalIndex = a.ErosionLevel * b.ErosionLevel;
                }
            }

            if (ErosionLevel == -1)
            {
                long e = (Program.depth + this.GeologicalIndex) % 20183;

                this.ErosionLevel = e;

                e = e % 3;

                switch (e)
                {
                    case 0:
                        this.T = RType.ROCKY;
                        //if (this.C == '-')
                        //    this.C = '.';
                        break;
                    case 1:
                        this.T = RType.WET;
                        //if (this.C == '-')
                        //    this.C = '=';
                        break;
                    case 2:
                        this.T = RType.NARROW;
                        //if (this.C == '-')
                        //    this.C = '|';
                        break;
                    default:
                        throw new IndexOutOfRangeException("Value " + e.ToString() + " isn't valid in ErosionLevel modulus operation.");
                }
            }
        }
    }

    public enum RType
    {
        NONE = 0,
        ROCKY = 1,
        WET = 2,
        NARROW = 3,
        MOUTH = 8,
        TARGET = 9
    }

    public class AStarNav
    {
        public AStarNode Start;
        public AStarNode Finish;
        private int width;
        private int height;
        public char[,] Map;
        private char[] walkable;
        private AStarNode[,] nodes;
        private List<AStarNode> path = new List<AStarNode>();

        public AStarNav(char[,] grid, char[] walkable, Point start, Point finish)
        {
            this.Start = new AStarNode(start);
            this.Finish = new AStarNode(finish);
            this.Map = grid;
            this.height = this.Map.GetLength(1);
            this.width = this.Map.GetLength(0);
            this.walkable = walkable;

            InitNodes();
        }

        public AStarNode[,] InitNodes()
        {
            AStarNode[,] _ret = new AStarNode[this.width, this.height];

            for (int y = 0; y < this.Map.GetLength(1); y++)
            {
                for (int x = 0; x < this.Map.GetLength(0); x++)
                {
                    Point p = new Point(x, y);
                    if (this.Map[x, y] == '.')
                    {
                        _ret[x, y] = new AStarNode(p, true);
                        _ret[x, y].Cost = 1;
                        _ret[x, y].H = CheckManhattanDistance(p, this.Finish.Location);
                        _ret[x, y].G = 99999;
                        _ret[x, y].State = AStarNodeState.Untested;
                    }

                    else if (this.Map[x, y] == 'G' || this.Map[x, y] == 'E')
                    {
                        _ret[x, y] = new AStarNode(p, false);
                        _ret[x, y].Cost = 99;
                        _ret[x, y].H = CheckManhattanDistance(p, this.Finish.Location);
                        _ret[x, y].G = 99999;
                        _ret[x, y].State = AStarNodeState.Untested;
                    }

                    else
                    {
                        _ret[x, y] = new AStarNode(p, false);
                        _ret[x, y].Cost = 1;
                        _ret[x, y].H = CheckManhattanDistance(p, this.Finish.Location);
                        _ret[x, y].G = 99999;
                    }
                }
            }

            this.nodes = _ret;
            this.Start = nodes[Start.Location.X, Start.Location.Y];
            this.Finish = nodes[Finish.Location.X, Finish.Location.Y];
            this.Start.G = 0;
            return _ret;
        }

        public Point AStar(char[,] grid, char[] walkable, Point start, Point finish)
        {
            Point _ret = new Point(0, 0);

            AStarNode _start = new AStarNode(start);
            AStarNode _finish = new AStarNode(finish);

            return _ret;
        }

        IEnumerable<Point> IsWalkableFrom(AStarNode fromNode)
        {
            List<Point> _ret = new List<Point>();
            Point p = fromNode.Location;

            if (p.Y > 0)// && this.nodes[p.X, p.Y - 1].IsWalkable)
                _ret.Add(new Point(p.X, p.Y - 1));
            if (p.X < this.nodes.GetLength(0))// && this.nodes[p.X + 1, p.Y].IsWalkable)
                _ret.Add(new Point(p.X + 1, p.Y));
            if (p.Y < this.nodes.GetLength(1))// && this.nodes[p.X, p.Y + 1].IsWalkable)
                _ret.Add(new Point(p.X, p.Y + 1));
            if (p.X > 0)// && this.nodes[p.X - 1, p.Y].IsWalkable)
                _ret.Add(new Point(p.X - 1, p.Y));

            return _ret;
        }

        private List<AStarNode> GetAdjacentWalkableNodes(AStarNode fromNode)
        {
            List<AStarNode> walkableNodes = new List<AStarNode>();
            IEnumerable<Point> nextLocations = IsWalkableFrom(fromNode);

            foreach (var location in nextLocations)
            {
                int x = location.X;
                int y = location.Y;

                // Stay within the grid's boundaries
                if (x < 0 || x >= this.width || y < 0 || y >= this.height)
                    continue;

                AStarNode node = this.nodes[x, y];

                //float traversalCost = GetTraversal(fromNode.Location, location, Finish.Location);   //1;
                float traversalCost = GetTraversal(fromNode, node, Finish);

                //node.ParentNode = fromNode;


                if (node.Location == this.Finish.Location)
                {
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        node.G = fromNode.G + traversalCost;
                        node.F = node.G + node.H;
                        walkableNodes.Add(node);
                        continue;
                    }
                }

                // Ignore non-walkable nodes
                if (!node.IsWalkable)
                    continue;

                // Ignore already-closed nodes
                if (node.State == AStarNodeState.Closed)
                    continue;
                /*
                {
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        walkableNodes.Add(node);
                    }
                }
                */

                // Already-open nodes are only added to the list if their G-value is lower going via this route.
                if (node.State == AStarNodeState.Open)// || node.State == AStarNodeState.Closed)
                {
                    float gTemp = fromNode.G + traversalCost;
                    if (gTemp < node.G)
                    {
                        node.ParentNode = fromNode;
                        node.G = fromNode.G + traversalCost;
                        node.F = node.G + node.H;
                        walkableNodes.Add(node);
                    }
                }
                else
                {
                    // If it's untested, set the parent and flag it as 'Open' for consideration
                    node.ParentNode = fromNode;
                    node.G = fromNode.G + traversalCost;
                    node.F = node.G + node.H;
                    node.State = AStarNodeState.Open;
                    walkableNodes.Add(node);
                }
            }

            return walkableNodes;
        }

        float GetTraversal(Point from, Point to, Point destination)
        {
            // Prefer reading order, meaning U, L, R, D (N, W, E, S)
            // This should also work for eight-direction or diagonals, though current intent is four cardinal directions.
            float _ret = 1.00f;

            if (to.Y < from.Y)  // Up / North
                _ret += 0f;
            //_ret *= 1.0f;                       // No change in cost if up
            if (to.X < from.X)  // Left / West
                _ret += 0.001f;
            //_ret *= 1.1f;                       // Slight increase in cost if left
            if (to.X > from.X)  // Right / East
                _ret += 0.002f;
            //_ret *= 1.2f;                       // Slightly higher increase in cost if right
            if (to.Y > from.Y)  // Down / South
                _ret += 0.003f;
            // _ret *= 1.3f;                       // Even hight increase in cost if down

            return _ret;
        }

        float GetTraversal(AStarNode from, AStarNode to, AStarNode destination)
        {
            // Prefer reading order, meaning U, L, R, D (N, W, E, S)
            // This should also work for eight-direction or diagonals, though current intent is four cardinal directions.
            float _ret = to.Cost;

            if (to.Location.Y < from.Location.Y)  // Up / North
            {
                //_ret += 1.0f;
                _ret += 0f;
                //_ret *= 1.0f;                       // No change in cost if up
            }
            else if (to.Location.X < from.Location.X)  // Left / West
            {
                //_ret += 2.0f;
                _ret += 0.1f;
                //_ret *= 1.001f;                       // Slight increase in cost if left
            }
            else if (to.Location.X > from.Location.X)  // Right / East
            {
                //_ret += 3.0f;
                _ret += 0.2f;
                //_ret *= 1.002f;                       // Slightly higher increase in cost if right
            }
            else if (to.Location.Y > from.Location.Y)  // Down / South
            {
                //_ret += 4.0f;
                _ret += 0.3f;
                //_ret *= 1.003f;                       // Even hight increase in cost if down
            }

            return _ret;
        }

        public Point Step()
        {
            List<Point> path = FindPath();
            Point _ret = path.DefaultIfEmpty(Start.Location).FirstOrDefault();
            //path.Clear();
            return _ret;
        }

        public List<Point> GetDistance()
        {
            return FindPath();
        }

        public List<Point> StepList()
        {
            List<Point> _ret = FindPath();
            return _ret;
        }

        public List<Point> FindPath()
        {
            List<Point> path = new List<Point>();
            bool success = Search(this.Start);
            if (success)
            {
                AStarNode node = this.Finish;
                while (node.ParentNode != null)
                {
                    path.Add(node.Location);
                    node = node.ParentNode;
                }
                path.Reverse();
            }
            return path;
        }

        private bool Search(AStarNode currentNode)
        {
            currentNode.State = AStarNodeState.Closed;
            List<AStarNode> nextNodes = GetAdjacentWalkableNodes(currentNode);
            //nextNodes.Sort((node1, node2) => node1.F.CompareTo(node2.F));
            nextNodes = OrderList(nextNodes, currentNode);
            bool? startNodeTrue = null;
            foreach (var nextNode in nextNodes)
            {
                nextNode.ParentNode = currentNode;
                //nextNode.G = currentNode.G + GetTraversal(currentNode.Location, nextNode.Location, Finish.Location);
                //nextNode.F = nextNode.G + nextNode.H;

                if (nextNode.Location == this.Finish.Location)
                {
                    return true;
                }
                else
                {
                    if (Search(nextNode)) // Note: Recurses back into Search(Node)
                    {
                        // This allows additional recursive searches from the root node to ensure the best path.
                        if (currentNode.ID == Start.ID)
                            startNodeTrue = true;
                        else
                            return true;
                    }
                }
            }
            if (Convert.ToBoolean(startNodeTrue))
                return true;
            else
                return false;
        }

        private List<AStarNode> OrderList(List<AStarNode> list, AStarNode current)
        {
            List<AStarNode> _ret = new List<AStarNode>();

            _ret = list.OrderBy(o => o.F).ThenBy(o => o.Location.Y).ThenBy(o => o.Location.X).ToList();

            return _ret;
        }

        public static int CheckManhattanDistance(Point p1, Point p2)
        {
            int _ret = 0;

            _ret = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

            return _ret;
        }
    }

    public class AStarNode
    {
        #region Fields
        private string id = "";
        public Point Location { get; set; }
        public bool IsWalkable { get; set; }
        float g;
        float h;
        float f;
        public AStarNodeState State { get; set; }
        public AStarNode ParentNode { get; set; }
        public int Cost { get; set; }
        #endregion

        #region Properties
        public string ID
        {
            get
            {
                return this.id;
            }

            set
            {
                if (this.id == "")
                    this.id = value;
            }
        }
        public float G
        {
            get
            {
                return this.g;
            }

            set
            {
                this.g = value;
            }
        }
        public float H
        {
            get
            {
                return this.h;
            }

            set
            {
                this.h = value;
            }
        }
        public float F
        {
            get
            {
                return this.f;
            }

            set
            {
                this.f = value;
            }
        }
        #endregion

        #region Constructors
        public AStarNode(Point p)
        {
            Location = p;
            G = 0;
            ID = Guid.NewGuid().ToString();
        }

        public AStarNode(Point p, bool walkable)
        {
            Location = p;
            IsWalkable = walkable;
            G = 0;
            ID = Guid.NewGuid().ToString();
        }
        #endregion
    }

    public enum AStarNodeState
    {
        Untested,
        Open,
        Closed
    }
}