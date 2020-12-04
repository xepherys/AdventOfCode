using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventOfCode2018.Core
{
    static class Extensions
    {
        public static bool IsWet(this Char source)
        {
            if (source == '|' || source == '~')
                return true;
            else
                return false;
        }
        public static Point Up(this Point source)
        {
            return new Point(source.X, source.Y - 1);
        }

        public static Point Down(this Point source)
        {
            return new Point(source.X, source.Y + 1);
        }

        public static Point Left(this Point source)
        {
            return new Point(source.X - 1, source.Y);
        }

        public static Point Right(this Point source)
        {
            return new Point(source.X + 1, source.Y);
        }

        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        public static string RemoveWhitespace(this string source)
        {
            return new string(source.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string MoveToFront(this string source, char c)
        {
            int _idx = source.IndexOf(c);

            source = source.Remove(_idx, 1);

            source = source.Insert(0, c.ToString());

            return source;
        }

        public static string MoveInFrontOf(this string source, char move, char inFrontOf)
        {
            int _idx = source.IndexOf(move);
            int _idx2 = source.IndexOf(inFrontOf);

            if (_idx > _idx2)
            {
                int sub = 0;
                source = source.Remove(_idx, 1);
                bool opComplete = false;
                int val;

                while (!opComplete)
                {
                    val = Math.Max(_idx2 - sub - 1, 0);

                    if (val == 0)
                    {
                        source = source.Insert(0, move.ToString());
                        opComplete = true;
                    }

                    else if (move > source[val])
                    {
                        source = source.Insert(Math.Max(_idx2 - sub, 0), move.ToString());
                        opComplete = true;
                    }

                    else
                        sub = Math.Min(sub + 1, 25);
                }
            }

            return source;
        }

        public static string ListToString<T>(this List<T> source, string delimiter = ", ")
        {
            string _ret = String.Empty;
            string s = String.Empty;

            foreach (var v in source)
            {
                try
                {
                    s = v.ToString();
                    _ret += s + delimiter;
                }

                catch
                {
                    throw;
                }
            }

            return _ret;
        }

        public static string PadCenter(this string source, int newWidth)
        {
            const char filler = ' ';
            int length = source.Length;
            int charactersToPad = newWidth - length;
            if (charactersToPad < 0) throw new ArgumentException("New width must be greater than string length.", "newWidth");
            int padLeft = charactersToPad / 2 + charactersToPad % 2;
            //add a space to the left if the string is an odd number
            int padRight = charactersToPad / 2;

            StringBuilder resultBuilder = new StringBuilder(newWidth);
            for (int i = 0; i < padLeft; i++) resultBuilder.Insert(i, filler);
            for (int i = 0; i < length; i++) resultBuilder.Insert(i + padLeft, source[i]);
            for (int i = newWidth - padRight; i < newWidth; i++) resultBuilder.Insert(i, filler);
            return resultBuilder.ToString();
        }

        public static T AssignCircular<T>(this Queue<T> source)
        {
            T _ret;

            _ret = source.Dequeue();

            source.Enqueue(_ret);

            return _ret;
        }
    }

    internal static class FormExtensions
    {
        private static void ApplicationRunProc(object state)
        {
            Application.Run(state as Form);
        }

        public static void RunInNewThread(this Form form, bool isBackground)
        {
            if (form == null)
                throw new ArgumentNullException("form");
            if (form.IsHandleCreated)
                throw new InvalidOperationException("Form is already running.");
            Thread thread = new Thread(ApplicationRunProc);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = isBackground;
            thread.Start(form);
        }
    }

    static class ArrayExtensions
    {
        public static void Populate<T>(this T[] source, T value)
        {
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = value;
            }
        }

        public static void Populate<T>(this T[,] source, T value)
        {
            for (int y = 0; y < source.GetLength(1); y++)
            {
                for (int x = 0; x < source.GetLength(0); x++)
                {
                    source[x, y] = value;
                }
            }
        }
    }
}