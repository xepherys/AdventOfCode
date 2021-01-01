using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace _2020_20_Jurassic_Jigsaw
{
    class Program
    {
        static public ChunkBag chunks = new ChunkBag();
        static public string importPath = @"..\..\..\Day20_Input.txt";

        static void Main(string[] args)
        {
            ImportData();

            chunks.InitMap();
            chunks.ParseMap();
            chunks.BuildMap();

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(importPath))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                string[] chunkData = new string[11];
                int counter = 0;

                while ((s = reader.ReadLine()) != null)
                {
                    if (s == "")
                    {
                        chunks.Add(new Chunk(chunkData));
                        chunkData = new string[11];
                        counter = 0;
                    }

                    else
                    {
                        chunkData[counter++] = s;
                    }
                }

                chunks.Add(new Chunk(chunkData));
            }
        }
    }

    public class Chunk
    {
        #region Fields
        int tileID;
        bool[,] map;

        int rotation;
        bool mirrored;

        int[] edgeBits;
        int[] edgeBitsInverse;
        #endregion

        #region Properties
        public int ID
        {
            get
            {
                return this.tileID;
            }
        }

        public bool[,] Map
        {
            get
            {
                return this.map;
            }
        }

        public int Rotation
        {
            get
            {
                return this.rotation;
            }

            set
            {
                if (this.mirrored)
                {
                    while (value > 0)
                        value -= 4;

                    this.rotation = (-value) % 4;
                }

                else
                {
                    while (value < 0)
                        value += 4;

                    this.rotation = value % 4;
                }
            }
        }

        public int[] EdgeBits
        {
            get
            {
                return this.edgeBits;
            }
        }

        public int[] EdgeBitsInverse
        {
            get
            {
                return this.edgeBitsInverse;
            }
        }
        #endregion

        #region Constructors
        public Chunk(string[] s)
        {
            edgeBits = new int[4];
            edgeBitsInverse = new int[4];
            this.rotation = 0;

            if (s.Length != 11)
                throw new Exception("Invalid chunk data.");

            this.tileID = Convert.ToInt32(s[0].Replace(":", "").Replace("Tile ", ""));
            this.map = new bool[10, 10];

            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    this.map[col, row] = (s[row + 1][col] == '#');
                }
            }


            for (int row = 0; row < 10; row++)
            {
                if (map[0, row])
                {
                    edgeBits[3] += (int)Math.Pow(2, 9 - row);
                    edgeBitsInverse[3] += (int)Math.Pow(2, row);
                }

                if (map[9, row])
                {
                    edgeBits[1] += (int)Math.Pow(2, 9 - row);
                    edgeBitsInverse[1] += (int)Math.Pow(2, row);
                }
            }

            for (int col = 0; col < 10; col++)
            {
                if (map[col, 0])
                {
                    edgeBits[0] += (int)Math.Pow(2, 9 - col);
                    edgeBitsInverse[0] += (int)Math.Pow(2, col);
                }

                if (map[col, 9])
                {
                    edgeBits[2] += (int)Math.Pow(2, 9 - col);
                    edgeBitsInverse[2] += (int)Math.Pow(2, col);
                }
            }
        }
        #endregion

        #region Methods
        public void Rotate() //Clockwise rotation
        {
            /*
               rc   c   c

            r  00  01  02  03  04  05  06  07  08  09                       90  80  70  60  50  40  30  20  10  00      [9,0] -> [0,0]   [8,0] -> [0,1]  ...  [0,0] -> [0,9]
            r  10  11  12  13  14  15  16  17  18  19                       91  81  71  61  51  41  31  21  11  01
               20  21  22  23  24  25  26  27  28  29                       92  82  72  62  52  42  32  22  12  02
               30  31  32  33  34  35  36  37  38  39                       93  83  73  63  53  43  33  23  13  03
               40  41  42  43  44  45  46  47  48  49          → ↓          94  84  74  64  54  44  34  24  14  04
               50  51  52  53  54  55  56  57  58  59                       95  85  75  65  55  45  35  25  15  05
               60  61  62  63  64  65  66  67  68  69                       96  86  76  66  56  46  36  26  16  06
               70  71  72  73  74  75  76  77  78  79                       97  87  77  67  57  47  37  27  17  07
               80  81  82  83  84  85  86  87  88  89                       98  88  78  68  58  48  38  28  18  08
               90  91  92  93  94  95  96  97  98  99                       99  89  79  69  59  49  39  29  19  09      [9,9] -> [9,0]   [8,9] -> [9,1]  ...  [0,9] -> [9,9]
            */
            bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
            int newR = 0;
            int newC = 0;

            for (int col = 9; col >= 0; col--, newR++)
            {
                newC = 0;

                for (int row = 0; row < 10; row++, newC++)
                {
                    newMap[newC, newR] = map[col, row];
                }
            }

            map = newMap;

            this.Rotation += 1;
        }

        public void RotateCounterClockwise()
        {
            bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
            int newR = 0;
            int newC = 0;

            for (int col = 0; col < 10; col++, newR++)
            {
                newC = 0;

                for (int row = 9; row >= 0; row--, newC++)
                {
                    newMap[newC, newR] = map[col, row];
                }
            }

            map = newMap;

            this.Rotation -= 1;
        }

        public void MirrorV()
        {
            bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
            int newR = 0;
            int newC = 0;

            for (int col = 0; col < 10; col++, newR++)
            {
                newC = 0;

                for (int row = 9; row >= 0; row--, newC++)
                {
                    newMap[newR, newC] = map[col, row];
                }
            }

            map = newMap;
        }

        public void MirrorH()
        {
            bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
            int newR = 0;
            int newC = 0;

            for (int col = 0; col < 10; col++, newR++)
            {
                newC = 0;

                for (int row = 9; row >= 0; row--, newC++)
                {
                    newMap[newC, newR] = map[row, col];
                }
            }

            map = newMap;
        }

        static string centeredString(string s, int width)
        {
            if (s.Length >= width)
            {
                return s;
            }

            int leftPadding = (width - s.Length) / 2;
            int rightPadding = width - s.Length - leftPadding;

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            string _ret = $"   Tile {this.tileID}:   \n";

            _ret += centeredString(this.edgeBits[0].ToString(), 16);
            _ret += Environment.NewLine;
            _ret += centeredString(this.edgeBitsInverse[0].ToString(), 16);
            string left = centeredString(this.edgeBits[3].ToString(), 10);
            string right = centeredString(this.edgeBits[1].ToString(), 10);

            string leftI = centeredString(this.edgeBitsInverse[3].ToString(), 10);
            string rightI = centeredString(this.edgeBitsInverse[1].ToString(), 10);

            for (int row = 0; row < 10; row++)
            {
                _ret += Environment.NewLine;

                _ret += left[row].ToString() + leftI[row].ToString() + " ";

                for (int col = 0; col < 10; col++)
                {
                    if (map[col, row] == true)
                        _ret += "#";
                    else
                        _ret += ".";
                }

                _ret += " " + rightI[row].ToString() + right[row].ToString();
            }

            _ret += Environment.NewLine;
            _ret += centeredString(this.edgeBitsInverse[2].ToString(), 16);
            _ret += Environment.NewLine;
            _ret += centeredString(this.edgeBits[2].ToString(), 16);
            
            _ret += Environment.NewLine;

            return _ret;
        }
        #endregion
    }

    public class ChunkBag : IEnumerable<Chunk>
    {
        #region Fields
        List<Chunk> chunks = new List<Chunk>();
        List<ChunkConnection> connections = new List<ChunkConnection>();
        Chunk[,] chunkMap;
        #endregion

        #region Properties
        public List<Chunk> Chunks
        {
            get
            {
                return this.chunks;
            }
        }

        public List<ChunkConnection> Connections
        {
            get
            {
                return this.connections;
            }
        }

        public int Count
        {
            get
            {
                return this.chunks.Count;
            }
        }
        #endregion

        #region Methods
        public void Add(Chunk c)
        {
            this.chunks.Add(c);
        }

        public void InitMap()
        {
            int fieldWidth = (int)Math.Sqrt(this.chunks.Count);

            if (fieldWidth * fieldWidth != this.chunks.Count)
                throw new Exception("Uneven map in ChunkBag.");

            else
            {
                chunkMap = new Chunk[fieldWidth, fieldWidth];
            }
        }

        public void ParseMap()
        {
            for (int i = 0; i < chunks.Count; i++)
            {
                for (int side = 0; side < 4; side++)
                {
                    int bit = chunks[i].EdgeBits[side];
                    //int ibit = chunks[i].EdgeBitsInverse[side];

                    foreach (Chunk c in chunks.Where(w => w.ID != chunks[i].ID))
                    {
                        for (int innerSide = 0; innerSide < 4; innerSide++)
                        {
                            if (c.EdgeBits[innerSide] == bit)
                            {
                                ChunkConnection cc = new ChunkConnection();
                                cc.c1 = chunks[i];
                                cc.c2 = c;
                                cc.cs1 = new ChunkSide(side, false);
                                cc.cs2 = new ChunkSide(innerSide, false);
                                if (!connections.Contains(cc))
                                    connections.Add(cc);
                            }

                            if (c.EdgeBitsInverse[innerSide] == bit)
                            {
                                ChunkConnection cc = new ChunkConnection();
                                cc.c1 = chunks[i];
                                cc.c2 = c;
                                cc.cs1 = new ChunkSide(side, false);
                                cc.cs2 = new ChunkSide(innerSide, true);
                                if (!connections.Contains(cc))
                                    connections.Add(cc);
                            }

                            /*
                            if (c.EdgeBits[innerSide] == ibit)
                            {
                                ChunkConnection cc = new ChunkConnection();
                                cc.c1 = chunks[i];
                                cc.c2 = c;
                                cc.cs1 = new ChunkSide(side, true);
                                cc.cs2 = new ChunkSide(innerSide, false);
                                if (!connections.Contains(cc))
                                    connections.Add(cc);
                            }

                            if (c.EdgeBitsInverse[innerSide] == ibit)
                            {
                                ChunkConnection cc = new ChunkConnection();
                                cc.c1 = chunks[i];
                                cc.c2 = c;
                                cc.cs1 = new ChunkSide(side, true);
                                cc.cs2 = new ChunkSide(innerSide, true);
                                if (!connections.Contains(cc))
                                    connections.Add(cc);
                            }
                            */
                        }
                    }
                }
            }
        }

        public void BuildMap()
        {
            Dictionary<int, int> connectionOptions = new Dictionary<int, int>();
            List<int>[,] mapList = new List<int>[chunkMap.GetLength(0), chunkMap.GetLength(1)];

            for (int i = 0; i < mapList.GetLength(1); i++)
                for (int j = 0; j < mapList.GetLength(0); j++)
                    mapList[j, i] = new List<int>();

            foreach (Chunk c in chunks)
            {
                connectionOptions[c.ID] = 0;
                connectionOptions[c.ID] += connections.Where(w => w.c1 == c).Count();
                connectionOptions[c.ID] += connections.Where(w => w.c2 == c).Count();
            }

            // Set corner options - anything with 2 values
            foreach (KeyValuePair<int, int> kvp in connectionOptions)
            {
                if (kvp.Value == 2)
                {
                    mapList[0, 0].Add(kvp.Key);
                    mapList[0, mapList.GetLength(1) - 1].Add(kvp.Key);
                    mapList[mapList.GetLength(0) - 1, 0].Add(kvp.Key);
                    mapList[mapList.GetLength(0) - 1, mapList.GetLength(1) - 1].Add(kvp.Key);
                }

                if (kvp.Value == 3)
                {
                    for (int i = 0; i < mapList.GetLength(1); i++)
                    {
                        mapList[0, i].Add(kvp.Key);
                        mapList[mapList.GetLength(0) - 1, i].Add(kvp.Key);
                    }

                    for (int i = 0; i < mapList.GetLength(0); i++)
                    {
                        mapList[i, 0].Add(kvp.Key);
                        mapList[i, mapList.GetLength(1) - 1].Add(kvp.Key);
                    }
                }

                else
                {
                    for (int i = 0; i < mapList.GetLength(1); i++)
                        for (int j = 0; j < mapList.GetLength(0); j++)
                            mapList[j, i].Add(kvp.Key);
                }
            }

            Console.Read();
        }
        #endregion

        #region Interface Methods
        public IEnumerator<Chunk> GetEnumerator()
        {
            return this.chunks.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.chunks.GetEnumerator();
        }
        #endregion
    }

    public class ChunkConnection : IEquatable<ChunkConnection>
    {
        public Chunk c1;
        public Chunk c2;
        public ChunkSide cs1;
        public ChunkSide cs2;

        public bool Equals(ChunkConnection other)
        {
            if (this.c1 != other.c1 && this.c1 != other.c2)
                return false;

            if (this.c2 != other.c2 && this.c2 != other.c1)
                return false;

            if (this.cs1 != other.cs1 && this.cs1 != other.cs2)
                return false;

            if (this.cs2 != other.cs2 && this.cs2 != other.cs1)
                return false;

            return true;
        }

        public static bool operator ==(ChunkConnection a, ChunkConnection b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ChunkConnection a, ChunkConnection b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            return $"{c1.ID} {cs1} <-> {cs2} {c2.ID}";
        }
    }

    public class ChunkSide : IEquatable<ChunkSide>
    {
        public int Side;
        public bool Inverse;

        public ChunkSide(int side, bool inv)
        {
            this.Side = side;
            this.Inverse = inv;
        }

        public bool Equals(ChunkSide other)
        {
            return (this.Side == other.Side && this.Inverse == other.Inverse);
        }

        public static bool operator ==(ChunkSide a, ChunkSide b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ChunkSide a, ChunkSide b)
        {
            return !a.Equals(b);
        }

        public override string ToString()
        {
            string indicator = (Inverse) ? "+" : "-";
            return $"{Side}{indicator}";
        }
    }
}
