using System;
using System.IO;
using System.Linq;

namespace Day8_SpaceImageFormat
{
    class Program
    {
        static int width;
        static int height;
        static string pixels;
        static string[] rows;
        static string[,] layers;
        static int[,] values;

        const char _0 = '.';
        const char _1 = '#';
        const char _2 = ' ';
        
        static void Main(string[] args)
        {
            width = 25;
            height = 6;
            pixels = FetchInput();

            rows = ParseRows();
            layers = ParseLayers();

            values = CountEachLayersValues();

            int layerWithFewestZeroes = GetLayerWIthFewestZeroes();
            int answer = values[layerWithFewestZeroes, 1] * values[layerWithFewestZeroes, 2];

            Console.WriteLine("Layer with fewest zeroes results in ones and twos multiplied to equal: {0}", answer);

            string[] linesToWrite = GetLinesOfASCII();

            foreach (string s in linesToWrite)
                Console.WriteLine(s);

            Console.ReadLine();
            Environment.Exit(0);
        }

        static string[] GetLinesOfASCII()
        {
            string[] _ret = new string[height];

            char[,] temp = InitializeCharArray(height, width, _2);

            for (int l = 0; l < layers.GetLength(0); l++)
                for (int h = 0; h < height; h++)
                    for (int w = 0; w < width; w++)
                    {
                        if (temp[h, w] == _2)
                        {
                            temp[h, w] = GetChar(layers[l, h][w]);
                        }
                    }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    _ret[i] += temp[i, j];
                }
            }

            return _ret;
        }

        static char GetChar(char c)
        {
            if (c == (char)(48))
                return _0;

            else if (c == (char)(49))
                return _1;

            else
                return _2;
        }

        static char[,] InitializeCharArray(int h, int w, char c)
        {
            char[,] _ret = new char[h, w];

            for (int i = 0; i < h; i++)
                for (int j = 0; j < w; j++)
                    _ret[i, j] = c;

            return _ret;
        }

        public static int GetLayerWIthFewestZeroes()
        {
            int _ret = -99;
            int numZeroes = 999;

            for (int i = 0; i < values.GetLength(0); i++)
            {
                if (values[i, 0] < numZeroes)
                {
                    numZeroes = values[i, 0];
                    _ret = i;
                }
            }

            return _ret;
        }

        public static int[,] CountEachLayersValues()
        {
            int[,] _ret = new int[layers.GetLength(0), 3];

            for (int i = 0; i < layers.GetLength(0); i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        _ret[i, k] += layers[i, j].ToCharArray().Count(c => c == 48 + k);
                    }
                }
            }

            return _ret;
        }

        public static string[,] ParseLayers()
        {
            string[,] _ret = new string[rows.Length / height, 6];

            for (int i = 0; i < _ret.GetLength(0); i++)
            {
                for (int j = 0; j < height; j++)
                {
                    _ret[i, j] = (string)rows[(height * i) + j].Clone();
                }
            }

            return _ret;
        }

        public static string[] ParseRows()
        {
            string[] _ret = new string[pixels.Length / width];

            for (int i = 0; i < _ret.Length; i++)
            {
                _ret[i] = pixels.Substring(i * width, width);
            }

            return _ret;
        }

        public static string FetchInput()
        {
            string str;

            using (Stream stream = File.OpenRead(@"..\..\..\Day08_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                str = reader.ReadLine();
            }

            return str;
        }
    }
}
