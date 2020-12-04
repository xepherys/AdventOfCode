using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Point = AdventOfCode2018.Core.Point;

namespace AdventOfCode2018.Core
{
    class StreamFunctions
    {
        public static IEnumerable<string> EnumerateLines(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }
    }

    class CoordinateGeometry
    {
        public static char CheckManhattanDistances(int x, int y, List<Point> destinations, int maxDistance)
        {
            char _ret = '.';
            int distance = 0;

            foreach (Point p in destinations)
            {
                distance += CheckManhattanDistance(x, y, p);
            }

            if (distance < maxDistance)
                _ret = 'X';

            return _ret;
        }

        public static int CheckManhattanDistance(int x, int y, Point p)
        {
            int _ret = 0;

            _ret = Math.Abs(x - p.X) + Math.Abs(y - p.Y);

            return _ret;
        }

        public static int CheckManhattanDistance(Point p1, Point p2)
        {
            int _ret = 0;

            _ret = Math.Abs(p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);

            return _ret;
        }

        public static float GetLinearDistance(Point p1, Point p2)
        {
            return (float)Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
            
        }
    }

    class PixelLayout
    {
        static public int FontPointToPixel(int fontSize)
        {
            return (int)Math.Ceiling(fontSize * 1.3);
        }
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
                        _ret[x, y].Cost = x + y;
                        //_ret[x, y].H = CoordinateGeometry.GetLinearDistance(p, this.Finish.Location);
                        _ret[x, y].H = CoordinateGeometry.CheckManhattanDistance(p, this.Finish.Location);
                        _ret[x, y].G = 99999;
                        _ret[x, y].State = AStarNodeState.Untested;
                    }
                    
                    else if (this.Map[x, y] == 'G' || this.Map[x, y] == 'E')
                    {
                        _ret[x, y] = new AStarNode(p, false);
                        _ret[x, y].Cost = 99;
                        //_ret[x, y].H = CoordinateGeometry.GetLinearDistance(p, this.Finish.Location);
                        _ret[x, y].H = CoordinateGeometry.CheckManhattanDistance(p, this.Finish.Location);
                        _ret[x, y].G = 99999;
                        _ret[x, y].State = AStarNodeState.Untested;
                    }
                    
                    else
                    {
                        _ret[x, y] = new AStarNode(p, false);
                        _ret[x, y].Cost = x + y;
                        //_ret[x, y].H = CoordinateGeometry.GetLinearDistance(p, this.Finish.Location);
                        _ret[x, y].H = CoordinateGeometry.CheckManhattanDistance(p, this.Finish.Location);
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
            /*
            float diffX = to.Location.X - from.Location.X;
            float diffY = to.Location.Y - from.Location.Y;

            if (diffX < 0 && diffY < 0) // up and left
                _ret -= 3f;

            else if (diffX == 0 && diffY < 0) // up
                _ret -= 2f;

            else if (diffX > 0 && diffY < 0) // up and right
                _ret += 1f;

            else if (diffX < 0 && diffY == 0) // left
                _ret += 0f;

            else if (diffX > 0 && diffY == 0) // right
                _ret += 1f;

            else if (diffX < 0 && diffY > 0) // down and left
                _ret += 2f;

            else if (diffX == 0 && diffY > 0) // down
                _ret += 3f;

            else if (diffX > 0 && diffY > 0) // down and right
                _ret += 4f;
                */
            /*
            if (to.Location.Y < from.Location.Y)  // Up / North
            {
                //_ret += 1.0f;
                _ret -= 0.5f;
                //_ret *= 1.0f;                       // No change in cost if up
            }

            else if (to.Location.Y == from.Location.Y)  // Down / South
            {
                //_ret += 4.0f;
                _ret += 0f;
                //_ret *= 1.003f;                       // Even hight increase in cost if down
            }

            else if (to.Location.Y > from.Location.Y)  // Down / South
            {
                //_ret += 4.0f;
                _ret += 0.5f;
                //_ret *= 1.003f;                       // Even hight increase in cost if down
            }

            if (to.Location.X < from.Location.X)  // Left / West
            {
                //_ret += 2.0f;
                _ret -= 0.5f;
                //_ret *= 1.001f;                       // Slight increase in cost if left
            }

            else if (to.Location.X == from.Location.X)  // Down / South
            {
                //_ret += 4.0f;
                _ret += 0f;
                //_ret *= 1.003f;                       // Even hight increase in cost if down
            }

            else if (to.Location.X > from.Location.X)  // Right / East
            {
                //_ret += 3.0f;
                _ret += 0.5f;
                //_ret *= 1.002f;                       // Slightly higher increase in cost if right
            }
            */


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
                nextNode.G = currentNode.G + GetTraversal(currentNode.Location, nextNode.Location, Finish.Location);
                nextNode.F = nextNode.G + nextNode.H;

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

            _ret = list.OrderBy(o => o.H).ToList();

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
            G = 1;
            ID = Guid.NewGuid().ToString();
        }

        public AStarNode(Point p, bool walkable)
        {
            Location = p;
            IsWalkable = walkable;
            G = 1;
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

    public enum Directions
    {
        NONE    =   -99,
        N       =   0,
        E       =   1, 
        S       =   2, 
        W       =   3, 
        NE      =   4, 
        SE      =   5, 
        SW      =   6, 
        NW      =   7
    }
}