using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day19
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Instruction> instrList = new List<Instruction>();
            List<int> factorValues = new List<int>();
            int instructionPointerRegister = -1;

            Device device = new Device();
            //device.Reg[0].Value = 1;
            int _opPtr = -1;
            int _watchPtr = 1;
            long iterations = 0;
            int baseFactor = 1;
            int countWatchSame = 0;

            Stopwatch sw = new Stopwatch();
            
            #region Read in input
            try
            {
                using (Stream stream = File.OpenRead(@"day19.in"))
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
            }

            catch (FileNotFoundException fnf)
            {
                Console.Write(@"File 'day19.in' not found.");
                Console.Write(Environment.NewLine);
                Console.Write("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.Write(Environment.NewLine);
                Console.Write("Press any key to exit.");
                Console.ReadKey();
                Environment.Exit(0);
            }
            #endregion


            #region Run work
            try
            {
                sw.Start();

                while (_opPtr < instrList.Count && countWatchSame < 20)
                {
                    iterations++;
                    _opPtr = device.Reg[instructionPointerRegister].Value;
                    
                    if (_watchPtr > -1)
                    {
                        if (device.Reg[_watchPtr].Value == baseFactor)
                            countWatchSame++;
                        else
                        {
                            baseFactor = device.Reg[_watchPtr].Value;
                            countWatchSame = 0;
                        }
                    }
                    

                    
                    if (_watchPtr == -1 && instrList[_opPtr].OpCode == (int)EOpCode.MULR &&
                       (instrList[_opPtr].Values[0] == instrList[_opPtr].Values[1]) &&
                       (instrList[_opPtr].Values[1] == instrList[_opPtr].Values[2]))
                    {
                        _watchPtr = instrList[_opPtr].Values[0];
                    }
                    

                    switch (instrList[_opPtr].OpCode)
                    {
                        case 0:
                            OpCodes.EQIR(device, instrList[_opPtr]);
                            break;
                        case 1:
                            OpCodes.BORR(device, instrList[_opPtr]);
                            break;
                        case 2:
                            OpCodes.ADDR(device, instrList[_opPtr]);
                            break;
                        case 3:
                            OpCodes.GTRI(device, instrList[_opPtr]);
                            break;
                        case 4:
                            OpCodes.MULI(device, instrList[_opPtr]);
                            break;
                        case 5:
                            OpCodes.GTIR(device, instrList[_opPtr]);
                            break;
                        case 6:
                            OpCodes.MULR(device, instrList[_opPtr]);
                            break;
                        case 7:
                            OpCodes.BANR(device, instrList[_opPtr]);
                            break;
                        case 8:
                            OpCodes.BORI(device, instrList[_opPtr]);
                            break;
                        case 9:
                            OpCodes.EQRI(device, instrList[_opPtr]);
                            break;
                        case 10:
                            OpCodes.EQRR(device, instrList[_opPtr]);
                            break;
                        case 11:
                            OpCodes.BANI(device, instrList[_opPtr]);
                            break;
                        case 12:
                            OpCodes.SETR(device, instrList[_opPtr]);
                            break;
                        case 13:
                            OpCodes.GTRR(device, instrList[_opPtr]);
                            break;
                        case 14:
                            OpCodes.ADDI(device, instrList[_opPtr]);
                            break;
                        case 15:
                            OpCodes.SETI(device, instrList[_opPtr]);
                            break;
                    }

                    if (device.Reg[instructionPointerRegister].Value + 1 < instrList.Count)
                        device.Reg[instructionPointerRegister].Value++;
                    else
                        break;

                }
            }

            catch (ArgumentException ae)
            {
                Console.Write(ae.Message);
            }

            catch (TypeInitializationException typeex)
            {
                Console.Write(typeex.Message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            #region Factor and solve
            for (int f = 1; f <= baseFactor ; f++)
            {
                if (baseFactor % f == 0)
                {
                    factorValues.Add(f);
                }
            }

            int sumOfFactors = 0;

            foreach (int val in factorValues) sumOfFactors += val;

            sw.Stop();
            Console.Write("Program complete in " + sw.ElapsedMilliseconds + "ms (" + sw.ElapsedTicks + " ticks) running " + iterations + " instructions." + Environment.NewLine +
                                "Solution:" + sumOfFactors + Environment.NewLine + "Final device: " + device.ToString());
            Console.Read();
            Environment.Exit(0);
            #endregion
        }
    }
}
