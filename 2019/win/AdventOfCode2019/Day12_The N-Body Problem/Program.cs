using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

/*
Total Energy in system after 100 steps is: 2094
Completed in 25ms (251559 ticks).

It took 292653556339368 steps to return to initial state.
Completed in 144ms (1440465 ticks).
*/

namespace Day12_The_N_Body_Problem
{
    class Program
    {
        static Bodies bodies;
        static string[] names = new string[] { "Io", "Europa", "Ganymede", "Callisto" };
        static int nameCount = 0;
        static List<Coord> xC = new List<Coord>();
        static List<Coord> yC = new List<Coord>();
        static List<Coord> zC = new List<Coord>();

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();

            //Part 1:
            bodies = new Bodies();
            int stepsToTake = 100;
            sw.Start();
            FetchInput();

            for (int i = 0; i < stepsToTake; i++)
            {
                RunWork();
            }
            sw.Stop();

            Console.WriteLine("Total Energy in system after {0} steps is: {1}", stepsToTake, bodies.TotalEnergy());
            Console.WriteLine("Completed in {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine();
            sw.Reset();

            //Part 2:
            sw.Start();
            FetchInput2();

            long offset = 0;
            long period = 1;
            var result = FindCycle(xC);
            offset = (result.Item1 > offset) ? result.Item1 : offset;
            period = LCM(result.Item2, period);

            result = FindCycle(yC);
            offset = (result.Item1 > offset) ? result.Item1 : offset;
            period = LCM(result.Item2, period);

            result = FindCycle(zC);
            offset = (result.Item1 > offset) ? result.Item1 : offset;
            period = LCM(result.Item2, period);

            sw.Stop();

            Console.WriteLine("It took {0} steps to return to initial state.", period + offset);
            Console.WriteLine("Completed in {0}ms ({1} ticks).", sw.ElapsedMilliseconds, sw.ElapsedTicks);
            Console.WriteLine();
            Console.ReadLine();
            Environment.Exit(0);
        }

        public static Tuple<long, long> FindCycle(List<Coord> coords)
        {
            Dictionary<ulong, Int64> record = new Dictionary<ulong, Int64>();
            long step = 0;
            while (true)
            {
                ulong state = ToState(coords);
                if (record.ContainsKey(state))
                {
                    Int64 prev = record[state];
                    return new Tuple<Int64, Int64>(prev, step - prev);
                }
                record[state] = step;
                Step(coords);
                step++;
            }
        }

        public static ulong ToState(List<Coord> coords)
        {
            return ((ulong)(coords[0].pos & 0xff)) |
                    ((ulong)(coords[0].v & 0xff) << 8) |
                    ((ulong)(coords[1].pos & 0xff) << 16) |
                    ((ulong)(coords[1].v & 0xff) << 24) |
                    ((ulong)(coords[2].pos & 0xff) << 32) |
                    ((ulong)(coords[2].v & 0xff) << 40) |
                    ((ulong)(coords[3].pos & 0xff) << 48) |
                    ((ulong)(coords[3].v & 0xff) << 56);
        }

        public static int sign(int x)
        {
            return (x > 0) ? 1 : (x < 0) ? -1 : 0;
        }

        public static int abs(int x)
        {
            return Math.Abs(x);
        }

        public static void Step(List<Coord> coords)
        {
            for (int i = 0; i < coords.Count; i++)
            {
                Coord a = coords[i];
                for (int j = 0; j < coords.Count; j++)
                    if (i != j)
                    {
                        Coord b = coords[j];
                        a.v += sign(b.pos - a.pos);
                    }
            }
            foreach (Coord c in coords)
            {
                c.pos += c.v;
            }
        }

        public static long GCD(long a, long b)
        {
            while (a != b)
            {
                if (a % b == 0) return b;
                if (b % a == 0) return a;
                if (a > b)
                    a -= b;
                if (b > a)
                    b -= a;
            }
            return a;
        }

        public static long LCM(long a, long b)
        {
            return a * b / GCD(a, b);
        }

        static void RunWork()
        {
            bodies.GetNewVelocities();
            bodies.EqualibriumCheck();
        }

        public static void FetchInput()
        {
            string line;

            using (Stream stream = File.OpenRead(@"..\..\Day12_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    //<x=-4, y=3, z=15>
                    line = line.Replace("<x=", "").Replace(" y=", "").Replace(" z=", "").Replace(">", "");
                    int[] vals = line.Split(',').Select(x => Int32.Parse(x)).ToArray();
                    bodies.BodyList.Add(new Body(vals[0], vals[1], vals[2]) { Name = names[nameCount++] });
                }
            }

            nameCount = 0;
        }

        public static void FetchInput2()
        {
            xC.Clear();
            yC.Clear();
            zC.Clear();
            string line;
            using (Stream stream = File.OpenRead(@"..\..\Day12_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    //<x=-4, y=3, z=15>
                    line = line.Replace("<x=", "").Replace(" y=", "").Replace(" z=", "").Replace(">", "");
                    int[] vals = line.Split(',').Select(x => Int32.Parse(x)).ToArray();
                    xC.Add(new Coord(vals[0]));
                    yC.Add(new Coord(vals[1]));
                    zC.Add(new Coord(vals[2]));
                }
            }
        }
    }

    public class Vector3
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3() {}

        public Vector3(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.X + v2.X, v1.Y + v2.Y, v1.Z + v2.Z);
        }

        public static bool operator ==(Vector3 v1, Vector3 v2)
        {
            return (v1.X == v2.X && v1.Y == v2.Y && v1.Z == v2.Z);
        }

        public static bool operator !=(Vector3 v1, Vector3 v2)
        {
            return !(v1 == v2);
        }

        public override bool Equals(object o)
        {
            if (o.GetType() == typeof(Vector3))
                return this == (Vector3)o;

            else
                throw new NotImplementedException(String.Format("Vector3.Equals({0}) is not implemented.", o.GetType()));
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class Body
    {
        public string Name;
        public Vector3 Location;
        public Vector3 Velocity;
        public Vector3 InitialLocation;
        public Vector3 InitialVelocity;
        long initialStateAt = 0;

        public long InitialStateAt
        {
            get
            {
                return this.initialStateAt;
            }

            set
            {
                this.initialStateAt = value;
                Console.WriteLine("InitialStateAT value set for {0} with step count: {1}.", this.Name, value);
            }
        }

        public Body(int x, int y, int z)
        {
            this.Location = new Vector3(x, y, z);
            this.Velocity = new Vector3();
            this.InitialLocation = new Vector3(x, y, z);
            this.InitialVelocity = new Vector3();
        }

        public bool AtInitialState()
        {
            return (this.Location == this.InitialLocation && this.Velocity == this.InitialVelocity);
        }

        public void UpdateLocation()
        {
            Location = Location + Velocity;
        }

        public int PotentialEnergy()
        {
            return Math.Abs(this.Location.X) + Math.Abs(this.Location.Y) + Math.Abs(this.Location.Z);
        }

        public int KineticEnergy()
        {
            return Math.Abs(this.Velocity.X) + Math.Abs(this.Velocity.Y) + Math.Abs(this.Velocity.Z);
        }

        public int TotalEnergy()
        {
            return this.PotentialEnergy() * this.KineticEnergy();
        }

        public override string ToString()
        {
            return String.Format("Position: {{{0}, {1}, {2}}}  Velocity: {{{3}, {4}, {5}}}.", this.Location.X, this.Location.Y, this.Location.Z, this.Velocity.X, this.Velocity.Y, this.Velocity.Z);
        }
    }

    public class Bodies
    {
        public List<Body> BodyList;
        public long Step;
        public bool AllBodiesFoundEqualibrium = false;

        public Bodies()
        {
            BodyList = new List<Body>();
        }

        public void EqualibriumCheck()
        {
            bool[] check = new bool[this.BodyList.Count()];

            for (int i = 0; i < this.BodyList.Count(); i++)
            {
                if (BodyList[i].AtInitialState())
                {
                    check[i] = true;

                    if (BodyList[i].InitialStateAt == 0)
                        BodyList[i].InitialStateAt = this.Step;
                }
            }

            if (this.BodyList.Where(w => w.InitialStateAt != 0).Count() == BodyList.Count())
                this.AllBodiesFoundEqualibrium = true;
        }

        public void GetNewVelocities()
        {
            foreach (Body current in BodyList)
            {
                foreach (Body target in BodyList.Where(w => w != current))
                {
                    if (target.Location.X > current.Location.X)
                        current.Velocity.X++;
                    else if (target.Location.X < current.Location.X)
                        current.Velocity.X--;

                    if (target.Location.Y > current.Location.Y)
                        current.Velocity.Y++;
                    else if (target.Location.Y < current.Location.Y)
                        current.Velocity.Y--;

                    if (target.Location.Z > current.Location.Z)
                        current.Velocity.Z++;
                    else if (target.Location.Z < current.Location.Z)
                        current.Velocity.Z--;
                }
            }

            UpdateLocations();
        }

        public void UpdateLocations()
        {
            foreach (Body current in BodyList)
            {
                current.UpdateLocation();
            }
        }

        public int TotalEnergy()
        {
            int _ret = 0;

            foreach (Body b in BodyList)
            {
                _ret += b.TotalEnergy();
            }

            return _ret;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Body b in BodyList)
            {
                sb.Append(b.ToString());
                sb.Append(Environment.NewLine);
            }

            return sb.ToString();
        }
    }

    public class Coord
    {
        public int pos;
        public int v;

        public Coord(int pos)
        {
            this.pos = pos;
            v = 0;
        }

        public Coord(Coord c)
        {
            pos = c.pos;
            v = c.v;
        }
    }
}