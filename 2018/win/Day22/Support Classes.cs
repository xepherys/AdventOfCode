using System;

namespace AdventOfCode2018.Core
{
    public class Point : IEquatable<Point>
    {
        public int X { get; set; }
        public int Y { get; set; }

        #region Constructors
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public Point(System.Drawing.Point p)
        {
            X = p.X;
            Y = p.Y;
        }
        #endregion

        #region Conversions
        public static implicit operator System.Drawing.Point(Point p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        public static implicit operator Point(System.Drawing.Point p)
        {
            return new Point(p.X, p.Y);
        }
        #endregion

        #region Equalities
        public override bool Equals(Object o)
        {
            if (o == null || GetType() != o.GetType())
                return false;

            else
                return (this == (Point)o);
        }

        public bool Equals(Point other)
        {
            return (this.X == other.X && this.Y == other.Y);
        }

        public bool Equals(System.Drawing.Point other)
        {
            return (this.X == other.X && this.Y == other.Y);
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return (p1.X == p2.X && p1.Y == p2.Y);
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }
        #endregion

        #region Hashcode
        public override int GetHashCode()
        {
            unchecked
            {
                int result = 1;
                result = (result * 397) ^ this.X;
                result = (result * 397) ^ this.Y;
                result = (result * 397) ^ (this.X * this.Y);
                return result;
            }
        }
        #endregion
    }
}