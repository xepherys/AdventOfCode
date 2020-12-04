using System;
using System.IO;

namespace Day1
{
    class Program
    {
        static void Main(string[] args)
        {
            int totalFuel = 0;

            using (Stream stream = File.OpenRead(@"..\..\..\Day01_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                int addFuel = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    addFuel = CalculateFuel(Convert.ToInt32(line));
                    totalFuel += addFuel;

                    while (addFuel > 0)
                    {
                        addFuel = CalculateFuel(Convert.ToInt32(addFuel));
                        totalFuel += addFuel;
                    }
                }
            }

            Console.WriteLine("Fuel needed is: {0}", totalFuel);

            Console.Read();
            Environment.Exit(0);
        }

        public static int CalculateFuel(int mass)
        {
            int _ret = Convert.ToInt32(Math.Floor((decimal)(Convert.ToDecimal(mass) / 3m)) - 2);
            return (_ret > 0) ? _ret : 0;
        }
    }
}
