using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode2018.Core;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;

namespace AdventOfCode2018
{
    public class Day19
    {
        public static int instructionPointerRegister;
        public static string ResourceFilename = "AdventOfCode2018._data.AdventOfCode_Day19.txt";
        public static List<Instruction> instrList = ParseInstructions(out instructionPointerRegister);
        public static Stopwatch sw = new Stopwatch();

        static public void Day19a(bool b = false)
        {
            RunWork(b);
        }

        static List<Instruction> ParseInstructions(out int instructionPointerRegister)
        {
            System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            
            List<Instruction> instrList = new List<Instruction>();
            instructionPointerRegister = -1;

            using (Stream stream = thisExe.GetManifestResourceStream(ResourceFilename))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                Instruction instr;

                while ((line = reader.ReadLine()) != null)
                {
                    instr = new Instruction();

                    if (instructionPointerRegister == -1)
                    {
                        instructionPointerRegister = Convert.ToInt32(line.Last().ToString());
                    }

                    else
                    {
                        string[] values = line.Split(new char['\0']);
                        instr.OpCode = (int)Enum.Parse(typeof(EOpCode), values[0].ToUpper());
                        instr.Values[0] = Convert.ToInt32(values[1]);
                        instr.Values[1] = Convert.ToInt32(values[2]);
                        instr.Values[2] = Convert.ToInt32(values[3]);
                        instrList.Add(instr);
                    }
                }
            }

            return instrList;
        }

        static void RunWork(bool dayB = false)
        {
            Device device = new Device();
            if (dayB) device.Reg[0].Value = 1;
            int _ptr = -1;
            long iterations = 0;
            /*
            Register a = device.Reg[0];
            Register b = device.Reg[1];
            Register c = device.Reg[2];
            Register d = device.Reg[3];
            Register e = device.Reg[4];
            Register f = device.Reg[5];
            Register ip = device.Reg[instructionPointerRegister];

            a.Value = 0;
            b.Value = 867;
            c.Value = 3;
            d.Value = 1;
            e.Value = 0;
            f.Value = 10;
            */
            try
            {
                sw.Start();
                while (_ptr < instrList.Count)
                {
                    iterations++;
                    _ptr = device.Reg[instructionPointerRegister].Value;

                    /*
                    if (instrList[_ptr].OpCode == (int)EOpCode.ADDI)
                    {
                        device = OpCodes.ADDI(device, instrList[_ptr], true);
                    }

                    else if (instrList[_ptr].OpCode == (int)EOpCode.ADDR)
                    {
                        device = OpCodes.ADDR(device, instrList[_ptr], true);
                    }

                    else if (instrList[_ptr].OpCode == (int)EOpCode.SETI)
                    {
                        device = OpCodes.SETI(device, instrList[_ptr], true);
                    }

                    else if (instrList[_ptr].OpCode == (int)EOpCode.SETR)
                    {
                        device = OpCodes.SETR(device, instrList[_ptr], true);
                    }

                    else
                    {
                        //throw new ArgumentException((EOpCode)instrList[_ptr].OpCode + " is not a valid OpCode for this run.");
                    }
                    */


                    
                    switch (instrList[_ptr].OpCode)
                    {
                        case 0:
                            OpCodes.EQIR(device, instrList[_ptr], true);
                            break;
                        case 1:
                            OpCodes.BORR(device, instrList[_ptr], true);
                            break;
                        case 2:
                            OpCodes.ADDR(device, instrList[_ptr], true);
                            break;
                        case 3:
                            OpCodes.GTRI(device, instrList[_ptr], true);
                            break;
                        case 4:
                            OpCodes.MULI(device, instrList[_ptr], true);
                            break;
                        case 5:
                            OpCodes.GTIR(device, instrList[_ptr], true);
                            break;
                        case 6:
                            OpCodes.MULR(device, instrList[_ptr], true);
                            break;
                        case 7:
                            OpCodes.BANR(device, instrList[_ptr], true);
                            break;
                        case 8:
                            OpCodes.BORI(device, instrList[_ptr], true);
                            break;
                        case 9:
                            OpCodes.EQRI(device, instrList[_ptr], true);
                            break;
                        case 10:
                            OpCodes.EQRR(device, instrList[_ptr], true);
                            break;
                        case 11:
                            OpCodes.BANI(device, instrList[_ptr], true);
                            break;
                        case 12:
                            OpCodes.SETR(device, instrList[_ptr], true);
                            break;
                        case 13:
                            OpCodes.GTRR(device, instrList[_ptr], true);
                            break;
                        case 14:
                            OpCodes.ADDI(device, instrList[_ptr], true);
                            break;
                        case 15:
                            OpCodes.SETI(device, instrList[_ptr], true);
                            break;
                    }
                    


                    //File.AppendAllText("dump.log", device.ToString() + Environment.NewLine);
                    //values += device.ToString() + Environment.NewLine;

                    //File.AppendAllText("dump.log", device.ToString() + "     " + instrList[_ptr].ToString() + Environment.NewLine);
                    //else


                    if (device.Reg[instructionPointerRegister].Value + 1 < instrList.Count)
                        device.Reg[instructionPointerRegister].Value++;
                    else
                        break;
                    
                }
                sw.Stop();
                //File.AppendAllText("dump.log", values);
                MessageBox.Show("Program complete in " + sw.ElapsedMilliseconds + "ms (" + sw.ElapsedTicks + " ticks) running " + iterations + " instructions." + Environment.NewLine +
                                device.ToString());
            }

            catch (ArgumentException ae)
            {
                MessageBox.Show(ae.Message);
            }

            catch (TypeInitializationException typeex)
            {
                MessageBox.Show(typeex.Message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
