using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020_11_Seating_System
{
    class Program
    {
        public static char[,] seating;
        public static int xMax = 0;
        public static int yMax = 0;
        public static int rounds = 0;

        static void Main(string[] args)
        {
            ImportData();


            // Part 1
            //ParseSeats1();

            // Part 2
            ParseSeats2();

            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    Console.Write(seating[x, y]);
                }

                Console.Write(Environment.NewLine);
            }

            int seatsTaken = 0;
            foreach (char c in seating)
                if (c == '#')
                    seatsTaken++;

            Console.WriteLine($"Total taken seats: {seatsTaken}");
            Console.WriteLine($"Found in {rounds} rounds.");

            

            Console.Read();
            Environment.Exit(0);
        }

        public static int RaycastNeighborCount(int x, int y)
        {
            bool done = false;
            int val = 1;
            bool ul = false, up = false, ur = false, lf = false, rt = false, dl = false, dn = false, dr = false;
            int neighbors = 0;

            while (!done)
            {
                int xPos = x + val;
                int xNeg = x - val;
                int yPos = y + val;
                int yNeg = y - val;

                if ((xNeg < 0 && yNeg < 0 && xPos > xMax - 1 && yPos > yMax - 1) ||
                    (ul && up && ur && lf && rt && dl && dn && dr))
                {
                    done = true;
                }

                else
                {
                    if (xNeg >= 0)
                    {
                        if (!lf)
                        {
                            if (seating[xNeg, y] == '#')
                            {
                                neighbors++;
                                lf = true;
                            }

                            else if (seating[xNeg, y] == 'L')
                            {
                                lf = true;
                            }
                        }

                        if (!ul)
                        {
                            if (yNeg >= 0)
                            {
                                if (seating[xNeg, yNeg] == '#')
                                {
                                    neighbors++;
                                    ul = true;
                                }

                                if (seating[xNeg, yNeg] == 'L')
                                {
                                    ul = true;
                                }
                            }

                        }

                        if (!dl)
                        {
                            if (yPos < yMax)
                            {
                                if (seating[xNeg, yPos] == '#')
                                {
                                    neighbors++;
                                    dl = true;
                                }

                                if (seating[xNeg, yPos] == 'L')
                                {
                                    dl = true;
                                }
                            }
                        }
                    }

                    if (xPos < xMax)
                    {
                        if (!rt)
                        {
                            if (seating[xPos, y] == '#')
                            {
                                neighbors++;
                                rt = true;
                            }

                            else if (seating[xPos, y] == 'L')
                            {
                                rt = true;
                            }
                        }

                        if (!ur)
                        {
                            if (yNeg >= 0)
                            {
                                if (seating[xPos, yNeg] == '#')
                                {
                                    neighbors++;
                                    ur = true;
                                }

                                else if (seating[xPos, yNeg] == 'L')
                                {
                                    ur = true;
                                }
                            }
                        }

                        if (!dr)
                        {
                            if (yPos < yMax)
                            {
                                if (seating[xPos, yPos] == '#')
                                {
                                    neighbors++;
                                    dr = true;
                                }

                                else if (seating[xPos, yPos] == 'L')
                                {
                                    dr = true;
                                }
                            }
                        }
                    }

                    if (yNeg >= 0 && !up)
                    {
                        if (seating[x, yNeg] == '#')
                        {
                            neighbors++;
                            up = true;
                        }

                        else if (seating[x, yNeg] == 'L')
                        {
                            up = true;
                        }
                    }

                    if (yPos < yMax && !dn)
                    {
                        if (seating[x, yPos] == '#')
                        {
                            neighbors++;
                            dn = true;
                        }

                        else if (seating[x, yPos] == 'L')
                        {
                            dn = true;
                        }
                    }
                }

                val++;
            }

            return neighbors;
        }

        public static void RaycastProto(int x, int y)
        {
            bool done = false;
            int val = 1;
            bool ul = false, up = false, ur = false, lf = false, rt = false, dl = false, dn = false, dr = false;

            while (!done)
            {
                int xPos = x + val;
                int xNeg = x - val;
                int yPos = y + val;
                int yNeg = y - val;

                if ((xNeg < 0 && yNeg < 0 && xPos > xMax - 1 && yPos > yMax - 1) ||
                    (ul && up && ur && lf && rt && dl && dn && dr))
                {
                    done = true;
                }

                else
                {
                    if (xNeg >= 0)
                    {
                        if (!lf)
                        {
                            if (seating[xNeg, y] == '#')
                            {
                                seating[xNeg, y] = '*';
                                lf = true;
                            }

                            else
                                seating[xNeg, y] = '-';
                        }

                        if (!ul)
                        {
                            if (yNeg >= 0)
                            {
                                if (seating[xNeg, yNeg] == '#')
                                {
                                    seating[xNeg, yNeg] = '*';
                                    ul = true;
                                }

                                else
                                    seating[xNeg, yNeg] = '\\';
                            }
                        }

                        if (!dl)
                        {
                            if (yPos < yMax)
                            {
                                if (seating[xNeg, yPos] == '#')
                                {
                                    seating[xNeg, yPos] = '*';
                                    dl = true;
                                }

                                else
                                    seating[xNeg, yPos] = '/';
                            }
                        }
                    }

                    if (xPos < xMax)
                    {
                        if (!rt)
                        {
                            if (seating[xPos, y] == '#')
                            {
                                seating[xPos, y] = '*';
                                rt = true;
                            }

                            else
                                seating[xPos, y] = '-';
                        }

                        if (!ur)
                        {
                            if (yNeg >= 0)
                            {
                                if (seating[xPos, yNeg] == '#')
                                {
                                    seating[xPos, yNeg] = '*';
                                    ur = true;
                                }

                                else
                                    seating[xPos, yNeg] = '/';
                            }
                        }

                        if (!dr)
                        {
                            if (yPos < yMax)
                            {
                                if (seating[xPos, yPos] == '#')
                                {
                                    seating[xPos, yPos] = '*';
                                    dr = true;
                                }

                                else
                                    seating[xPos, yPos] = '\\';
                            }
                        }
                    }

                    if (yNeg >= 0 && !up)
                    {
                        if (seating[x, yNeg] == '#')
                        {
                            seating[x, yNeg] = '*';
                            up = true;
                        }

                        else
                            seating[x, yNeg] = '|';
                    }

                    if (yPos < yMax && !dn)
                    {
                        if (seating[x, yPos] == '#')
                        {
                            seating[x, yPos] = '*';
                            dn = true;
                        }

                        else
                            seating[x, yPos] = '|';
                    }
                }

                val++;
            }

            for (int yy = 0; yy < yMax; yy++)
            {
                for (int xx = 0; xx < xMax; xx++)
                {
                    Console.Write(seating[xx, yy]);
                }

                Console.Write(Environment.NewLine);
            }
        }

        static void ImportData()
        {
            List<string> input = new List<string>();
            using (Stream stream = File.OpenRead(@"..\..\..\Day11_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;

                while ((s = reader.ReadLine()) != null)
                {
                    input.Add(s);
                }
            }

            xMax = input[0].Length;
            yMax = input.Count;
            seating = new char[xMax, yMax];

            for (int y = 0; y < yMax; y++)
                for (int x = 0; x < xMax; x++)
                    seating[x, y] = input[y][x];
        }

        static void ParseSeats1()
        {
            while (true)
            {
                char[,] newSeats = new char[xMax, yMax];

                for (int y = 0; y < yMax; y++)
                    for (int x = 0; x < xMax; x++)
                    {
                        if (seating[x, y] == '.')
                            newSeats[x, y] = '.';

                        else
                            newSeats[x, y] = CheckSeat(x, y);
                    }

                if (ArrEqual(seating, newSeats))
                    return;

                else
                {
                    rounds++;
                    seating = newSeats;
                }
            }
        }

        static void ParseSeats2()
        {
            while (true)
            {
                char[,] newSeats = new char[xMax, yMax];

                for (int y = 0; y < yMax; y++)
                    for (int x = 0; x < xMax; x++)
                    {
                        if (seating[x, y] == '.')
                            newSeats[x, y] = '.';

                        else
                        {
                            int neighbors = RaycastNeighborCount(x, y);

                            if (seating[x, y] == 'L' && neighbors == 0)
                                newSeats[x, y] = '#';

                            else if (seating[x, y] == '#' && neighbors >= 5)
                                newSeats[x, y] = 'L';

                            else
                                newSeats[x, y] = seating[x, y];
                        }
                    }

                if (ArrEqual(seating, newSeats))
                    return;

                else
                {
                    rounds++;
                    seating = newSeats;
                }
            }
        }

        static char CheckSeat(int x, int y)
        {
            int neighbors = 0;
            int xMinCheck = 0;
            int xMaxCheck = xMax - 1;
            int yMinCheck = 0;
            int yMaxCheck = yMax - 1;

            if (x > 0)
                xMinCheck = x - 1;

            if (x < xMaxCheck)
                xMaxCheck = x + 1;

            if (y > 0)
                yMinCheck = y - 1;

            if (y < yMaxCheck)
                yMaxCheck = y + 1;

            for (int yy = yMinCheck; yy <= yMaxCheck; yy++)
                for (int xx = xMinCheck; xx <= xMaxCheck; xx++)
                {
                    if (!(xx == x && yy == y))
                    {
                        if (seating[xx, yy] == '#')
                            neighbors++;
                    }
                }

            if (seating[x, y] == 'L' && neighbors == 0)
                return '#';

            else if (seating[x, y] == '#' && neighbors >= 4)
                return 'L';

            else
                return seating[x, y];
        }

        public static bool ArrEqual(char[,] c1, char[,] c2)
        {
            if (c1.Rank != c2.Rank)
                return false;

            int ranks = c1.Rank;
            List<int> c1Lengths = new List<int>();
            List<int> c2Lengths = new List<int>();

            for (int r = 0; r < c1.Rank; r++)
            {
                c1Lengths.Add(c1.GetLength(r));
                c2Lengths.Add(c2.GetLength(r));
            }

            for (int r = 0; r < c1.Rank; r++)
            {
                if (c1Lengths[r] != c2Lengths[r])
                    return false;
            }

            for (int y = 0; y < yMax; y++)
                for (int x = 0; x < xMax; x++)
                {
                    if (c1[x, y] != c2[x, y])
                        return false;
                }
            
            return true;
        }
    }
}
