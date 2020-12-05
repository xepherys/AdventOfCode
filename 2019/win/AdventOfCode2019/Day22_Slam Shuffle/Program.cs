using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day22_Slam_Shuffle
{
    class Program
    {
        public static List<Instruction> instructions = new List<Instruction>();
        public static List<long> cards;
        //public static List<int> cards = Enumerable.Range(0, 10).ToList();  // for test problem

        static void Main(string[] args)
        {
            cards = FetchCards(10007);
            FetchInput();

            ProcessInstructions();

            Console.WriteLine("Part A:  Card 2019 is in position {0}.", cards.IndexOf(2019));
            Console.WriteLine();

            cards = FetchCards(119315717514047);
            

            Console.ReadLine();
            Environment.Exit(0);
        }

        public static List<long> FetchCards(long val)
        {
            List<long> _ret = new List<long>();

            for (long i = 0; i < val; i++)
                _ret.Add(i);

            return _ret;
        }

        public static void ProcessInstructions()
        {
            foreach (var v in instructions)
            {
                switch (v.Type)
                {
                    case InstructionType.DealIntoNewStack:
                        DealIntoNewStack();
                        break;
                    case InstructionType.DealWithIncrement:
                        DealWithIncrement(v.Value);
                        break;
                    case InstructionType.Cut:
                        Cut(v.Value);
                        break;
                }

                //Console.WriteLine(cards.ListToString<int>(true));
            }
        }

        public static void DealIntoNewStack()
        {
            cards.Reverse();
        }

        public static void Cut(int count)
        {
            List<long> temp = new List<long>();

            if (count > 0)
            {
                temp = cards.Take(count).ToList();
                cards.RemoveRange(0, count);
                cards.AddRange(temp);
            }

            else
            {
                count = Math.Abs(count);
                cards.Reverse();
                temp = cards.Take(count).ToList();
                cards.RemoveRange(0, count);
                //temp.Reverse();
                cards.AddRange(temp);
                cards.Reverse();
            }
        }

        public static void DealWithIncrement(int count)
        {
            List<long> temp = FetchCards(cards.Count);

            for (int i = 0; i < cards.Count; i++)
            {
                int place = i * count;
                while (place >= cards.Count)
                    place -= cards.Count;
                temp[place] = cards[i];
            }

            cards = temp;
        }

        public static void FetchInput()
        {
            string line;

            using (Stream stream = File.OpenRead(@"..\..\Day22_Input.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    if (line == "deal into new stack")
                        instructions.Add(new Instruction(InstructionType.DealIntoNewStack, 0));

                    else if (line.Contains("deal with"))
                    {
                        string[] s = line.Split(' ');
                        instructions.Add(new Instruction(InstructionType.DealWithIncrement, Convert.ToInt32(s[3])));
                    }

                    else if (line.Contains("cut"))
                    {
                        string[] s = line.Split(' ');
                        instructions.Add(new Instruction(InstructionType.Cut, Convert.ToInt32(s[1])));
                    }

                    else
                        throw new NotImplementedException();
                }
            }
        }
    }

    public enum InstructionType
    {
        DealIntoNewStack,
        DealWithIncrement,
        Cut
    }

    public class Instruction
    {
        public InstructionType Type;
        public int Value;

        public Instruction(InstructionType type, int value)
        {
            this.Type = type;
            this.Value = value;
        }
    }
}
