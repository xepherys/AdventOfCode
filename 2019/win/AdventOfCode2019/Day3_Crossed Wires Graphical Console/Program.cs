using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using XephLibs.Mathematics;
using Point = XephLibs.Mathematics.Point;


/*
    Lowest Manhattan distance intersection between Wire 1 and Wire 2 is: 266
    Lowest latency intersection between Wire 1 and Wire 2 is: 19242
*/

namespace Day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] wires = new string[2];
            int count = 0;

            SetColor(ConsoleColor.DarkGray, 45, 45, 45);

            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();

            using (Stream stream = File.OpenRead(@"..\..\Day03_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                while ((s = reader.ReadLine()) != null)
                {
                    wires[count] = s;
                    count++;
                }
            }

            Motion[] wire1Path = GetPath(wires[0]);
            Motion[] wire2Path = GetPath(wires[1]);

            List<Point> wire1AllPoints = ExplodePath(wire1Path);
            List<Point> wire2AllPoints = ExplodePath(wire2Path);

            List<Point> intersections = FindIntersections(wire1AllPoints, wire2AllPoints);
            List<int> manhattanDistances = FindManhattanDistances(intersections);
            List<int> latencySteps = FindLatencySteps(intersections, wire1AllPoints, wire2AllPoints);

            Console.WriteLine("Lowest Manhattan distance intersection between Wire 1 and Wire 2 is: {0}", manhattanDistances.Where(w => w > 0).Min());
            Console.WriteLine("Lowest latency intersection between Wire 1 and Wire 2 is: {0}", latencySteps.Where(w => w > 0).Min());
            Console.ReadLine();

            VectorPair[] wire1Vectors = GetAllWireVectorPairs(wire1Path);
            VectorPair[] wire2Vectors = GetAllWireVectorPairs(wire2Path);

            Console.WriteLine("Press any key to terminate...");
            Console.Read();
            Environment.Exit(0);
        }

        public static VectorPair[] GetAllWireVectorPairs(Motion[] path)
        {
            VectorPair[] _ret = new VectorPair[(int)Math.Floor((double)path.Length / 2)];

            for (int i = 0; i < _ret.Length; i++)
            {
                VectorPair vp = new VectorPair(path[i], path[i + 1]);
                _ret[i] = vp;
            }

            return _ret;
        }

        public static Motion[] GetPath(string wireInput)
        {
            string[] input = wireInput.Split(',').ToArray();
            Motion[] _ret = new Motion[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                _ret[i] = new Motion(input[i]);
            }

            return _ret;
        }

        public static List<int> FindLatencySteps(List<Point> points, List<Point> a, List<Point> b)
        {
            List<int> _ret = new List<int>();

            foreach (Point p in points)
            {
                int aL = a.IndexOf(p);
                int bL = b.IndexOf(p);
                _ret.Add(aL + bL);
            }

            return _ret;
        }

        public static List<int> FindManhattanDistances(List<Point> points)
        {
            List<int> _ret = new List<int>();

            foreach (var v in points)
            {
                _ret.Add(Math.Abs(v.X) + Math.Abs(v.Y));
            }

            return _ret;
        }

        public static List<Point> FindIntersections(List<Point> a, List<Point> b)
        {
            return a.Intersect(b, new PointComparer()).ToList<Point>();
        }

        public static List<Point> FindIntersections(Point[][] pArr)
        {
            List<Point> _ret = new List<Point>();

            foreach (var v in pArr[0].Intersect(pArr[1]))
                _ret.Add(v);

            return _ret;
        }


        public static List<Point> ExplodePath(Motion[] motions)
        {
            List<Point> _ret = new List<Point>();
            _ret.Add(new Point(0, 0));

            for (int i = 0; i < motions.Length; i++)
            {
                for (int spaces = 1; spaces <= motions[i].Spaces; spaces++)
                {
                    if (motions[i].Dir == Directions.UP)
                    {
                        _ret.Add(new Point(_ret.Last().X, _ret.Last().Y + 1));
                    }

                    else if (motions[i].Dir == Directions.RIGHT)
                    {
                        _ret.Add(new Point(_ret.Last().X + 1, _ret.Last().Y));
                    }

                    else if (motions[i].Dir == Directions.DOWN)
                    {
                        _ret.Add(new Point(_ret.Last().X, _ret.Last().Y - 1));
                    }

                    else if (motions[i].Dir == Directions.LEFT)
                    {
                        _ret.Add(new Point(_ret.Last().X - 1, _ret.Last().Y));
                    }
                }
            }

            return _ret;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct COORD
        {
            internal short X;
            internal short Y;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct SMALL_RECT
        {
            internal short Left;
            internal short Top;
            internal short Right;
            internal short Bottom;
        }
        [StructLayout(LayoutKind.Sequential)]
        internal struct COLORREF
        {
            internal uint ColorDWORD;

            internal COLORREF(Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }

            internal COLORREF(uint r, uint g, uint b)
            {
                ColorDWORD = r + (g << 8) + (b << 16);
            }

            internal Color GetColor()
            {
                return Color.FromArgb((int)(0x000000FFU & ColorDWORD),
                   (int)(0x0000FF00U & ColorDWORD) >> 8, (int)(0x00FF0000U & ColorDWORD) >> 16);
            }

            internal void SetColor(Color color)
            {
                ColorDWORD = (uint)color.R + (((uint)color.G) << 8) + (((uint)color.B) << 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct CONSOLE_SCREEN_BUFFER_INFO_EX
        {
            internal int cbSize;
            internal COORD dwSize;
            internal COORD dwCursorPosition;
            internal ushort wAttributes;
            internal SMALL_RECT srWindow;
            internal COORD dwMaximumWindowSize;
            internal ushort wPopupAttributes;
            internal bool bFullscreenSupported;
            internal COLORREF black;
            internal COLORREF darkBlue;
            internal COLORREF darkGreen;
            internal COLORREF darkCyan;
            internal COLORREF darkRed;
            internal COLORREF darkMagenta;
            internal COLORREF darkYellow;
            internal COLORREF gray;
            internal COLORREF darkGray;
            internal COLORREF blue;
            internal COLORREF green;
            internal COLORREF cyan;
            internal COLORREF red;
            internal COLORREF magenta;
            internal COLORREF yellow;
            internal COLORREF white;
        }

        const int STD_OUTPUT_HANDLE = -11;                                        // per WinBase.h
        internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);    // per WinBase.h

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleScreenBufferInfoEx(IntPtr hConsoleOutput, ref CONSOLE_SCREEN_BUFFER_INFO_EX csbe);

        public static int SetColor(ConsoleColor consoleColor, Color targetColor)
        {
            return SetColor(consoleColor, targetColor.R, targetColor.G, targetColor.B);
        }

        public static int SetColor(ConsoleColor color, uint r, uint g, uint b)
        {
            CONSOLE_SCREEN_BUFFER_INFO_EX csbe = new CONSOLE_SCREEN_BUFFER_INFO_EX();
            csbe.cbSize = (int)Marshal.SizeOf(csbe);                    // 96 = 0x60
            IntPtr hConsoleOutput = GetStdHandle(STD_OUTPUT_HANDLE);    // 7
            if (hConsoleOutput == INVALID_HANDLE_VALUE)
            {
                return Marshal.GetLastWin32Error();
            }
            bool brc = GetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }

            switch (color)
            {
                case ConsoleColor.Black:
                    csbe.black = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkBlue:
                    csbe.darkBlue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGreen:
                    csbe.darkGreen = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkCyan:
                    csbe.darkCyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkRed:
                    csbe.darkRed = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkMagenta:
                    csbe.darkMagenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkYellow:
                    csbe.darkYellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Gray:
                    csbe.gray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.DarkGray:
                    csbe.darkGray = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Blue:
                    csbe.blue = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Green:
                    csbe.green = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Cyan:
                    csbe.cyan = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Red:
                    csbe.red = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Magenta:
                    csbe.magenta = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.Yellow:
                    csbe.yellow = new COLORREF(r, g, b);
                    break;
                case ConsoleColor.White:
                    csbe.white = new COLORREF(r, g, b);
                    break;
            }
            ++csbe.srWindow.Bottom;
            ++csbe.srWindow.Right;
            brc = SetConsoleScreenBufferInfoEx(hConsoleOutput, ref csbe);
            if (!brc)
            {
                return Marshal.GetLastWin32Error();
            }
            return 0;
        }

        public static int SetScreenColors(Color foregroundColor, Color backgroundColor)
        {
            int irc;
            irc = SetColor(ConsoleColor.Gray, foregroundColor);
            if (irc != 0) return irc;
            irc = SetColor(ConsoleColor.Black, backgroundColor);
            if (irc != 0) return irc;

            return 0;
        }
    }

    public class Motion
    {
        public Directions Dir;
        public int Spaces;

        public Motion(string input)
        {
            char dir = input[0];
            this.Spaces = Convert.ToInt32(input.Substring(1));

            if (dir == 'U')
                this.Dir = Directions.UP;
            else if (dir == 'R')
                this.Dir = Directions.RIGHT;
            else if (dir == 'D')
                this.Dir = Directions.DOWN;
            else if (dir == 'L')
                this.Dir = Directions.LEFT;
        }
    }

    public class VectorPair
    {
        public Directions[] Dir = new Directions[2];
        public int[] Spaces = new int[2];
        public double LinearMotion = 0;
        public int[] Weight = new int[2];

        public VectorPair(Motion m1, Motion m2)
        {
            this.Dir[0] = m1.Dir;
            this.Dir[1] = m2.Dir;
            this.Spaces[0] = m1.Spaces;
            this.Spaces[1] = m2.Spaces;
            this.LinearMotion = Math.Sqrt(Math.Pow(this.Spaces[0], 2) + Math.Pow(this.Spaces[1], 2));
            this.Weight[0] = (int)((decimal)this.Spaces[0] / (decimal)(this.Spaces[0] + this.Spaces[1]) * 100);
            this.Weight[1] = 100 - this.Weight[0];
        }

        public void DrawBezier()
        {
            Point p0 = new Point(0, 0);
            Point p1 = new Point(0, 0).AddPoint(this.Dir[0], this.Spaces[0]).AddPoint(this.Dir[1], this.Spaces[1]);
        }
    }

    public static class PointExtensions
    {
        public static Point AddPoint(this Point source, Directions dir, int spaces)
        {
            if (dir == Directions.UP)
                return new Point(0, spaces);

            if (dir == Directions.RIGHT)
                return new Point(spaces, 0);

            if (dir == Directions.DOWN)
                return new Point(0, -spaces);

            if (dir == Directions.LEFT)
                return new Point(-spaces, 0);

            return new Point(0, 0);
        }
    }

    public enum Directions
    {
        UP = 1,
        NORTH = 1,
        RIGHT = 2,
        EAST = 2,
        DOWN = 3,
        SOUTH = 3,
        LEFT = 4,
        WEST = 4
    }
}
