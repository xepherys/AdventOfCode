using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    class Day16
    {
        //609 - too low
        public static void Day16a()
        {
            List<Set> sets = ParseSets();
            Dictionary<Set, int> returns = new Dictionary<Set, int>();
            OpCodes oc = new OpCodes();

            foreach (Set set in sets)
            {
                int i = oc.TestAll(set.Pre, set.Instr, set.Post);

                returns.Add(set, i);
            }

            MessageBox.Show("There were " + returns.Where(w => w.Value >= 3).Count() + " instructions that behave like three or more OpCodes.");
        }

        public static void Day16b1()
        {
            List<Set> sets = ParseSets();
            OpCodes oc = new OpCodes();

            oc.TestAllForOpCodes(sets);

            Dictionary<int, List<string[]>> valueSets = oc.GetValueSets();
        }

        public static void Day16b2()
        {
            List<Instruction> instructions = ParseInstructions();
            OpCodes oc = new OpCodes();

            Device device = new Device();
            oc.RunProgram(instructions, device);

            MessageBox.Show("Result: " + device.Reg[0].Value + " " + device.Reg[1].Value + " " + device.Reg[2].Value + " " + device.Reg[3].Value);
        }

        #region Support Methods
        static public List<Set> ParseSets()
        {
            System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string ResourceFilename = "AdventOfCode2018._data.AdventOfCode_Day16a.txt";

            List<Set> sets = new List<Set>();

            using (Stream stream = thisExe.GetManifestResourceStream(ResourceFilename))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                Set set = new Set();
                Device pre = new Device();
                Instruction instr = new Instruction();
                Device post = new Device();

                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrEmpty(line))
                        continue;

                    else if (line.StartsWith("Before:"))
                    {
                        line = line.Remove(0, 9); // Before: [
                        line = line.Remove(line.IndexOf(']'), 1); // ]
                        string[] values = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        pre.Reg[0].Value = Convert.ToInt32(values[0].RemoveWhitespace());
                        pre.Reg[1].Value = Convert.ToInt32(values[1].RemoveWhitespace());
                        pre.Reg[2].Value = Convert.ToInt32(values[2].RemoveWhitespace());
                        pre.Reg[3].Value = Convert.ToInt32(values[3].RemoveWhitespace());
                    }

                    else if (line.StartsWith("After:"))
                    {
                        line = line.Remove(0, 9); // Before: [
                        line = line.Remove(line.IndexOf(']'), 1); // ]
                        string[] values = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        post.Reg[0].Value = Convert.ToInt32(values[0].RemoveWhitespace());
                        post.Reg[1].Value = Convert.ToInt32(values[1].RemoveWhitespace());
                        post.Reg[2].Value = Convert.ToInt32(values[2].RemoveWhitespace());
                        post.Reg[3].Value = Convert.ToInt32(values[3].RemoveWhitespace());

                        set.Pre = pre;
                        set.Instr = instr;
                        set.Post = post;
                        sets.Add(set.Clone() as Set);

                        set = new Set();
                        pre = new Device();
                        instr = new Instruction();
                        post = new Device();
                    }

                    else
                    {
                        string[] values = line.Split(new char['\0']);
                        instr.OpCode = Convert.ToInt32(values[0]);
                        instr.Values[0] = Convert.ToInt32(values[1]);
                        instr.Values[1] = Convert.ToInt32(values[2]);
                        instr.Values[2] = Convert.ToInt32(values[3]);
                    }
                }
            }

            return sets;
        }

        static public List<Instruction> ParseInstructions()
        {
            System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            string ResourceFilename = "AdventOfCode2018._data.AdventOfCode_Day16b.txt";

            List<Instruction> instrList = new List<Instruction>();

            using (Stream stream = thisExe.GetManifestResourceStream(ResourceFilename))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                Instruction instr;

                while ((line = reader.ReadLine()) != null)
                {
                    instr = new Instruction();

                    string[] values = line.Split(new char['\0']);
                    instr.OpCode = Convert.ToInt32(values[0]);
                    instr.Values[0] = Convert.ToInt32(values[1]);
                    instr.Values[1] = Convert.ToInt32(values[2]);
                    instr.Values[2] = Convert.ToInt32(values[3]);
                    instrList.Add(instr);
                }
            }

            return instrList;
        }
        #endregion
    }

    
}