using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AdventOfCode2018.Core
{
    public class OpCodes
    {
        public static Dictionary<int, List<string[]>> OpCodeValueSets = new Dictionary<int, List<string[]>>();

        #region Addition
        static public Device ADDR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value + _ret.Reg[instr.Values[1]].Value;

            return _ret;
        }

        static public Device ADDI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value + instr.Values[1];

            return _ret;
        }
        #endregion

        #region Multiplication
        static public Device MULR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value * _ret.Reg[instr.Values[1]].Value;

            return _ret;
        }

        static public Device MULI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value * instr.Values[1];

            return _ret;
        }
        #endregion

        #region Bitwise AND
        static public Device BANR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value & _ret.Reg[instr.Values[1]].Value;

            return _ret;
        }

        static public Device BANI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value & instr.Values[1];

            return _ret;
        }
        #endregion

        #region Bitwise OR
        static public Device BORR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value | _ret.Reg[instr.Values[1]].Value;

            return _ret;
        }

        static public Device BORI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value | instr.Values[1];

            return _ret;
        }
        #endregion

        #region Assignment
        static public Device SETR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = _ret.Reg[instr.Values[0]].Value;

            return _ret;
        }

        static public Device SETI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            _ret.Reg[instr.Values[2]].Value = instr.Values[0];

            return _ret;
        }
        #endregion

        #region Greater-than Testing
        static public Device GTIR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            if (instr.Values[0] > _ret.Reg[instr.Values[1]].Value)
                _ret.Reg[instr.Values[2]].Value = 1;
            else
                _ret.Reg[instr.Values[2]].Value = 0;

            return _ret;
        }

        static public Device GTRI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            if (_ret.Reg[instr.Values[0]].Value > instr.Values[1])
                _ret.Reg[instr.Values[2]].Value = 1;
            else
                _ret.Reg[instr.Values[2]].Value = 0;

            return _ret;
        }

        static public Device GTRR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            if (_ret.Reg[instr.Values[0]].Value > _ret.Reg[instr.Values[1]].Value)
                _ret.Reg[instr.Values[2]].Value = 1;
            else
                _ret.Reg[instr.Values[2]].Value = 0;

            return _ret;
        }
        #endregion

        #region Equality Testing
        static public Device EQIR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            if (instr.Values[0] == _ret.Reg[instr.Values[1]].Value)
                _ret.Reg[instr.Values[2]].Value = 1;
            else
                _ret.Reg[instr.Values[2]].Value = 0;

            return _ret;
        }

        static public Device EQRI(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            if (_ret.Reg[instr.Values[0]].Value == instr.Values[1])
                _ret.Reg[instr.Values[2]].Value = 1;
            else
                _ret.Reg[instr.Values[2]].Value = 0;

            return _ret;
        }

        static public Device EQRR(Device pre, Instruction instr, bool programRunning = false)
        {
            Device _ret;
            if (programRunning)
                _ret = pre;
            else
                _ret = pre.Clone() as Device;

            if (_ret.Reg[instr.Values[0]].Value == _ret.Reg[instr.Values[1]].Value)
                _ret.Reg[instr.Values[2]].Value = 1;
            else
                _ret.Reg[instr.Values[2]].Value = 0;

            return _ret;
        }
        #endregion


        #region Testing Classes
        public void RunProgram(List<Instruction> instructions, Device device)
        {
            foreach (Instruction i in instructions)
            {
                switch (i.OpCode)
                {
                    case 0:
                        EQIR(device, i, true);
                        break;
                    case 1:
                        BORR(device, i, true);
                        break;
                    case 2:
                        ADDR(device, i, true);
                        break;
                    case 3:
                        GTRI(device, i, true);
                        break;
                    case 4:
                        MULI(device, i, true);
                        break;
                    case 5:
                        GTIR(device, i, true);
                        break;
                    case 6:
                        MULR(device, i, true);
                        break;
                    case 7:
                        BANR(device, i, true);
                        break;
                    case 8:
                        BORI(device, i, true);
                        break;
                    case 9:
                        EQRI(device, i, true);
                        break;
                    case 10:
                        EQRR(device, i, true);
                        break;
                    case 11:
                        BANI(device, i, true);
                        break;
                    case 12:
                        SETR(device, i, true);
                        break;
                    case 13:
                        GTRR(device, i, true);
                        break;
                    case 14:
                        ADDI(device, i, true);
                        break;
                    case 15:
                        SETI(device, i, true);
                        break;
                }
            }
        }

        static public List<Device> OutputAll(Device pre, Instruction instr)
        {
            List<Device> _ret = new List<Device>();

            Device _in = pre.Clone() as Device;

            _ret.Add(ADDR(_in, instr).Clone() as Device);
            _ret.Add(ADDI(_in, instr).Clone() as Device);
            _ret.Add(MULR(_in, instr).Clone() as Device);
            _ret.Add(MULI(_in, instr).Clone() as Device);
            _ret.Add(BANR(_in, instr).Clone() as Device);
            _ret.Add(BANI(_in, instr).Clone() as Device);
            _ret.Add(BORR(_in, instr).Clone() as Device);
            _ret.Add(BORI(_in, instr).Clone() as Device);
            _ret.Add(SETR(_in, instr).Clone() as Device);
            _ret.Add(SETI(_in, instr).Clone() as Device);
            _ret.Add(GTIR(_in, instr).Clone() as Device);
            _ret.Add(GTRI(_in, instr).Clone() as Device);
            _ret.Add(GTRR(_in, instr).Clone() as Device);
            _ret.Add(EQIR(_in, instr).Clone() as Device);
            _ret.Add(EQRI(_in, instr).Clone() as Device);
            _ret.Add(EQRR(_in, instr).Clone() as Device);

            return _ret;
        }



        public int TestAll(Device pre, Instruction instr, Device post)
        {
            int _ret = 0;

            List<Device> tester = OutputAll(pre, instr);

            foreach (Device d in tester)
            {
                if (d == post)
                {
                    _ret++;
                }
            }

            return _ret;
        }


        public void TestAllForOpCodes(List<Set> sets)
        {
            foreach (Set set in sets)
                OutputAllForOpCodes(set.Pre, set.Instr, set.Post);
        }

        static public void OutputAllForOpCodes(Device pre, Instruction instr, Device post)
        {
            string[] opcodes = new string[16];

            Device _in = pre.Clone() as Device;

            if (ADDR(_in, instr) == post)
                opcodes[0] = "ADDR";
            if (ADDI(_in, instr) == post)
                opcodes[1] = "ADDI";
            if (MULR(_in, instr) == post)
                opcodes[2] = "MULR";
            if (MULI(_in, instr) == post)
                opcodes[3] = "MULI";
            if (BANR(_in, instr) == post)
                opcodes[4] = "BANR";
            if (BANI(_in, instr) == post)
                opcodes[5] = "BANI";
            if (BORR(_in, instr) == post)
                opcodes[6] = "BORR";
            if (BORI(_in, instr) == post)
                opcodes[7] = "BORI";
            if (SETR(_in, instr) == post)
                opcodes[8] = "SETR";
            if (SETI(_in, instr) == post)
                opcodes[9] = "SETI";
            if (GTIR(_in, instr) == post)
                opcodes[10] = "GTIR";
            if (GTRI(_in, instr) == post)
                opcodes[11] = "GTRI";
            if (GTRR(_in, instr) == post)
                opcodes[12] = "GTRR";
            if (EQIR(_in, instr) == post)
                opcodes[13] = "EQIR";
            if (EQRI(_in, instr) == post)
                opcodes[14] = "EQRI";
            if (EQRR(_in, instr) == post)
                opcodes[15] = "EQRR";

            if (OpCodeValueSets.ContainsKey(instr.OpCode))
            {
                OpCodeValueSets[instr.OpCode].Add(opcodes);
            }

            else
            {
                OpCodeValueSets.Add(instr.OpCode, new List<string[]>());
                OpCodeValueSets[instr.OpCode].Add(opcodes);
            }
        }

        public Dictionary<int, List<string[]>> GetValueSets()
        {
            return OpCodeValueSets;
        }
        #endregion
    }

    #region Object Classes
    public class Set : ICloneable
    {
        public Device Pre { get; set; }
        public Instruction Instr { get; set; }
        public Device Post { get; set; }

        public object Clone()
        {
            return new Set(Pre, Instr, Post);
        }

        private Set(Device pr, Instruction ins, Device po)
        {
            Pre = pr;
            Instr = ins;
            Post = po;
        }

        public Set()
        { }
    }

    public class Device : ICloneable
    {
        public Register[] Reg = new Register[6];

        public object Clone()
        {
            return new Device(Reg.ToArray());
        }

        private Device(Register[] r)
        {
            Reg[0] = new Register(r[0].Value);
            Reg[1] = new Register(r[1].Value);
            Reg[2] = new Register(r[2].Value);
            Reg[3] = new Register(r[3].Value);
            Reg[4] = new Register(r[4].Value);
            Reg[5] = new Register(r[5].Value);
        }

        public Device()
        {
            Reg[0] = new Register();
            Reg[1] = new Register();
            Reg[2] = new Register();
            Reg[3] = new Register();
            Reg[4] = new Register();
            Reg[5] = new Register();
        }

        public static bool operator ==(Device d1, Device d2)
        {
            bool _ret = false;

            if (d1.Reg[0].Value == d2.Reg[0].Value
                && d1.Reg[1].Value == d2.Reg[1].Value
                && d1.Reg[2].Value == d2.Reg[2].Value
                && d1.Reg[3].Value == d2.Reg[3].Value
                && d1.Reg[4].Value == d2.Reg[4].Value
                && d1.Reg[5].Value == d2.Reg[5].Value)
            {
                _ret = true;
            }

            return _ret;
        }

        public static bool operator !=(Device d1, Device d2)
        {
            return !(d1 == d2);
        }

        new public string ToString()
        {
            string _ret = String.Empty;

            foreach (Register r in this.Reg)
            {
                _ret += r.Value.ToString() + " ";
            }

            return _ret;
        }
    }

    public class LDevice : Device
    {
        new public Register[] Reg = new Register[6];

        new public object Clone()
        {
            return new LDevice(Reg.ToArray());
        }

        private LDevice(Register[] r)
        {
            Reg[0] = new Register(r[0].Value);
            Reg[1] = new Register(r[1].Value);
            Reg[2] = new Register(r[2].Value);
            Reg[3] = new Register(r[3].Value);
            Reg[4] = new Register(r[4].Value);
            Reg[5] = new Register(r[5].Value);
        }

        public LDevice()
        {
            Reg[0] = new Register();
            Reg[1] = new Register();
            Reg[2] = new Register();
            Reg[3] = new Register();
            Reg[4] = new Register();
            Reg[5] = new Register();
        }

        public static bool operator ==(LDevice d1, LDevice d2)
        {
            bool _ret = false;

            if (d1.Reg[0].Value == d2.Reg[0].Value
                && d1.Reg[1].Value == d2.Reg[1].Value
                && d1.Reg[2].Value == d2.Reg[2].Value
                && d1.Reg[3].Value == d2.Reg[3].Value
                && d1.Reg[4].Value == d2.Reg[4].Value
                && d1.Reg[5].Value == d2.Reg[5].Value)
            {
                _ret = true;
            }

            return _ret;
        }

        public static bool operator !=(LDevice d1, LDevice d2)
        {
            return !(d1 == d2);
        }

        new public string ToString()
        {
            string _ret = String.Empty;

            foreach (Register r in this.Reg)
            {
                _ret += r.Value.ToString() + " ";
            }

            return _ret;
        }
    }

    public class Register : ICloneable
    {
        public int Value { get; set; }

        public object Clone()
        {
            return new Register(this.Value);
        }

        public Register()
        {
            Value = 0;
        }

        public Register(int v)
        {
            Value = v;
        }
    }

    public class Instruction
    {
        int? instructionPointer = null;
        int opcode = 0;
        int[] values = new int[3];

        public int? InstructionPointer
        {
            get
            {
                return this.instructionPointer;
            }

            set
            {
                this.instructionPointer = value;
            }
        }
        public int[] Values
        {
            get
            {
                return this.values;
            }

            set
            {
                this.values = value;
            }
        }

        public int OpCode
        {
            get
            {
                return this.opcode;
            }

            set
            {
                this.opcode = value;
            }
        }

        new public string ToString()
        {
            string _ret = String.Empty;

            _ret = ((EOpCode)opcode).ToString() + " ";

            foreach (int val in this.Values)
            {
                _ret += val.ToString() + " ";
            }

            return _ret;
        }
    }
    #endregion

    public enum EOpCode
    {
        NONE = -1,
        EQIR =  0,
        BORR =  1,
        ADDR =  2,
        GTRI =  3,
        MULI =  4,
        GTIR =  5,
        MULR =  6,
        BANR =  7,
        BORI =  8,
        EQRI =  9,
        EQRR = 10,
        BANI = 11,
        SETR = 12,
        GTRR = 13,
        ADDI = 14,
        SETI = 15
    }
}

/*
Addition:

addr (add register) stores into register C the result of adding register A and register B.
addi (add immediate) stores into register C the result of adding register A and value B.

Multiplication:

mulr (multiply register) stores into register C the result of multiplying register A and register B.
muli (multiply immediate) stores into register C the result of multiplying register A and value B.

Bitwise AND:

banr (bitwise AND register) stores into register C the result of the bitwise AND of register A and register B.
bani (bitwise AND immediate) stores into register C the result of the bitwise AND of register A and value B.

Bitwise OR:

borr (bitwise OR register) stores into register C the result of the bitwise OR of register A and register B.
bori (bitwise OR immediate) stores into register C the result of the bitwise OR of register A and value B.

Assignment:

setr (set register) copies the contents of register A into register C. (Input B is ignored.)
seti (set immediate) stores value A into register C. (Input B is ignored.)

Greater-than testing:

gtir (greater-than immediate/register) sets register C to 1 if value A is greater than register B. Otherwise, register C is set to 0.
gtri (greater-than register/immediate) sets register C to 1 if register A is greater than value B. Otherwise, register C is set to 0.
gtrr (greater-than register/register) sets register C to 1 if register A is greater than register B. Otherwise, register C is set to 0.

Equality testing:

eqir (equal immediate/register) sets register C to 1 if value A is equal to register B. Otherwise, register C is set to 0.
eqri (equal register/immediate) sets register C to 1 if register A is equal to value B. Otherwise, register C is set to 0.
eqrr (equal register/register) sets register C to 1 if register A is equal to register B. Otherwise, register C is set to 0.
*/

/*
0  EQIR
1  BORR
2  ADDR
3  GTRI
4  MULI
5  GTIR
6  MULR
7  BANR
8  BORI
9  EQRI
10 EQRR
11 BANI
12 SETR
13 GTRR
14 ADDI
15 SETI
*/