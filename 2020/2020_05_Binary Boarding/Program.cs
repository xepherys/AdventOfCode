using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace _2020_05_Binary_Boarding
{
    class Program
    {
        static List<string> boardingPasses = new List<string>();
        static List<Seat> seats = new List<Seat>();

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ImportData();
            ParseData();

            Console.WriteLine($"Highest seat ID: {seats.Max(m => m.SeatId)}");
            Console.WriteLine($"My seat ID: {GetMySeat()}");
            sw.Stop();
            Console.WriteLine();
            Console.WriteLine($"Parsing both parts took: {sw.ElapsedMilliseconds}ms ({sw.ElapsedTicks} ticks).");

            Console.Read();
            Environment.Exit(0);
        }

        static void ImportData()
        {
            using (Stream stream = File.OpenRead(@"..\..\..\Day5_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string s;
                StringBuilder sb = new StringBuilder();
                while ((s = reader.ReadLine()) != null)
                {
                    boardingPasses.Add(s);
                }
            }
        }

        static void ParseData()
        {
            foreach (string s in boardingPasses)
                seats.Add(new Seat(s));
        }

        static int GetMySeat()
        {
            List<int> seatNums = new List<int>();
            int currSeat = 0;

            foreach (Seat s in seats)
                seatNums.Add(s.SeatId);

            seatNums = seatNums.OrderBy(o => o).ToList();
            currSeat = seatNums[0];

            for (int i = 1; i < seatNums.Count; i++)
            {
                if (seatNums[i] - 1 != currSeat)
                {
                    return seatNums[i] - 1;
                }

                currSeat = seatNums[i];
            }

            return -1;
        }

        class Seat
        {
            public string Identifier { get; set; }
            public int Row { get;set; }
            public int Column { get; set; }
            public int SeatId { get; set; }

            public Seat(string s)
            {
                this.Identifier = s.Replace('F', '0').Replace('B', '1').Replace('L', '0').Replace('R', '1');
                ParseSeatingInformation();
            }

            private void ParseSeatingInformation()
            {
                string sTmp = String.Empty;

                if (this.Identifier.Length != 10)
                    throw new Exception();

                /*
                // Get row in first 7 values
                for (int i = 0; i < 7; i++)
                {
                    if (this.Identifier[i] == 'F')
                        sTmp += '0';
                    else if (this.Identifier[i] == 'B')
                        sTmp += '1';
                    else
                        throw new Exception();
                }
                */

                sTmp = this.Identifier.Substring(0, 7);

                this.Row = Convert.ToInt32(sTmp, 2);
                sTmp = String.Empty;

                /*
                // Get column in last 3 values
                for (int i = 7; i < 10; i++)
                {
                    if (this.Identifier[i] == 'L')
                        sTmp += '0';
                    else if (this.Identifier[i] == 'R')
                        sTmp += '1';
                    else
                        throw new Exception();
                }
                */

                sTmp = this.Identifier.Substring(7, 3);

                this.Column = Convert.ToInt32(sTmp, 2);

                // Get Unique Seat ID
                this.SeatId = (this.Row * 8) + this.Column;
            }
        }
    }
}
