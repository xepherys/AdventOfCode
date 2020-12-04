using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventOfCode2018.Core
{
    public class Point : IEquatable<Point>
    {
        #region Fields
        int x;
        int y;
        #endregion

        #region Properties
        public int X
        { 
            get
            {
                return this.x;
            }

            set
            {
                this.x = value;
            }
        }

        public int Y
        {
            get
            {
                return this.y;
            }

            set
            {
                this.y = value;
            }
        }
        #endregion

        #region Constructors
        public Point(int _x, int _y)
        {
            this.X = _x;
            this.Y = _y;
        }
        #endregion

        #region Operators
        public static bool operator ==(Point p1, Point p2)
        {
            return ((p1.X == p2.x) && (p1.Y == p2.Y));           
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return !(p1 == p2);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Point);
        }

        public bool Equals(Point other)
        {
            return other != null &&
                   this.X == other.X &&
                   this.Y == other.Y;
        }

        public override int GetHashCode()
        {
            var hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
        #endregion
    }

    public class Tuple<T1, T2>
    {
        public T1 Item1 { get; private set; }
        public T2 Item2 { get; private set; }
        internal Tuple(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
    }

    public static class Tuple
    {
        public static Tuple<T1, T2> New<T1, T2>(T1 item1, T2 item2)
        {
            var tuple = new Tuple<T1, T2>(item1, item2);
            return tuple;
        }
    }

    public class ModArray<T>
    {
        T[] _internalarray;
        int size = 0;
        int shift = 0;

        public ModArray(int _sz, int _sh)
        {
            this.size = _sz;
            this.shift = _sh;
            _internalarray = new T[this.size];
        }

        public T[] Array
        {
            get
            {
                return _internalarray;
            }
        }

        public void Set(int i, T value)
        {
            _internalarray[i - this.shift] = value;
        }

        public T Get(int i)
        {
            return _internalarray[i - this.shift];
        }

        public void Populate(T val)
        {
            _internalarray.Populate(val);
        }

        public int GetMin()
        {
            return this.shift;
        }

        public int GetMax()
        {
            return this.shift + this.size;
        }

        public string GetString()
        {
            return new string(_internalarray as char[]);
        }
    }

    public class ModArray2D<T>
    {
        T[,] _internalarray;
        int sizeX = 0;
        int sizeY = 0;
        int shiftX = 0;
        int shiftY = 0;

        public ModArray2D(int _szX, int _szY, int _shX, int _shY)
        {
            this.sizeX = _szX;
            this.sizeY = _szY;
            this.shiftX = _shX;
            this.shiftY = _shY;
            _internalarray = new T[this.sizeX, this.sizeY];
        }

        public T[,] Array
        {
            get
            {
                return _internalarray;
            }
        }

        public void Set(int x, int y, T value)
        {
            _internalarray[x - this.shiftX, y - this.shiftY] = value;
        }

        public T Get(int x, int y)
        {
            return _internalarray[x - this.shiftX, y - this.shiftY];
        }

        public void Populate(T val)
        {
            _internalarray.Populate(val);
        }

        public int GetMinX()
        {
            return this.shiftX;
        }

        public int GetMinY()
        {
            return this.shiftY;
        }

        public int GetMaxX()
        {
            return this.shiftX + this.sizeX;
        }

        public int GetMaxY()
        {
            return this.shiftY + this.sizeY;
        }

        public string GetString()
        {
            //StringBuilder sb = new StringBuilder();

            //foreach (var v in (_internalarray as char[]))

            //return sb.ToString();

            return new string(_internalarray as char[]);
        }
    }
}