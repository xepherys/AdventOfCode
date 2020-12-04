using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day2
{
    class Program
    {
        static List<Policy> policies = new List<Policy>();

        static void Main(string[] args)
        {
            int validPart1 = 0;
            int validPart2 = 0;

            using (Stream stream = File.OpenRead(@"..\..\..\..\Day2_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    policies.Add(new Policy(line));
                }
            }

            foreach (Policy p in policies)
            {
                if (p.MeetsRequirementsPart1())
                    validPart1++;

                if (p.MeetsRequirementsPart2())
                    validPart2++;
            }

            Console.WriteLine($"Number of passwords in list: {policies.Count}");
            Console.WriteLine($"Number of valid passwords (part 1): {validPart1}");
            Console.WriteLine($"Number of valid passwords (part 2): {validPart2}");
            Console.Read();
            Environment.Exit(0);
        }
    }

    public class Policy
    {
        int countMin;
        int countMax;
        char countChar;
        string password;

        //14-15 v: vdvvvvvsvvvvvfpv
        public Policy(string s)
        {
            string[] breakdown = s.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            string[] counts = breakdown[0].Split("-", StringSplitOptions.RemoveEmptyEntries);

            countMin = Convert.ToInt32(counts[0]);
            countMax = Convert.ToInt32(counts[1]);
            countChar = breakdown[1][0];
            password = breakdown[2];
        }

        public bool MeetsRequirementsPart1()
        {
            int passwordCharCount = password.Count(c => c == countChar);
            if (passwordCharCount >= countMin && passwordCharCount <= countMax)
                return true;

            return false;
        }

        public bool MeetsRequirementsPart2()
        {
            if (password[countMin - 1] == countChar ^ password[countMax - 1] == countChar)
                return true;

            return false;
        }
    }
}