using System;
using System.Collections.Generic;
using System.Linq;

namespace Day19
{
    public class OpCodes
    {
        #region Addition
        static public void ADDR(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value + pre.Reg[instr.Values[1]].Value;
        }

        static public void ADDI(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value + instr.Values[1];
        }
        #endregion

        #region Multiplication
        static public void MULR(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value * pre.Reg[instr.Values[1]].Value;
        }

        static public void MULI(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value * instr.Values[1];
        }
        #endregion

        #region Bitwise AND
        static public void BANR(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value & pre.Reg[instr.Values[1]].Value;
        }

        static public void BANI(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value & instr.Values[1];
        }
        #endregion

        #region Bitwise OR
        static public void BORR(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value | pre.Reg[instr.Values[1]].Value;
        }

        static public void BORI(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value | instr.Values[1];
        }
        #endregion

        #region Assignment
        static public void SETR(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = pre.Reg[instr.Values[0]].Value;
        }

        static public void SETI(Device pre, Instruction instr)
        {
            pre.Reg[instr.Values[2]].Value = instr.Values[0];
        }
        #endregion

        #region Greater-than Testing
        static public void GTIR(Device pre, Instruction instr)
        {
            if (instr.Values[0] > pre.Reg[instr.Values[1]].Value)
                pre.Reg[instr.Values[2]].Value = 1;
            else
                pre.Reg[instr.Values[2]].Value = 0;
        }

        static public void GTRI(Device pre, Instruction instr)
        {
            if (pre.Reg[instr.Values[0]].Value > instr.Values[1])
                pre.Reg[instr.Values[2]].Value = 1;
            else
                pre.Reg[instr.Values[2]].Value = 0;
        }

        static public void GTRR(Device pre, Instruction instr)
        {
            if (pre.Reg[instr.Values[0]].Value > pre.Reg[instr.Values[1]].Value)
                pre.Reg[instr.Values[2]].Value = 1;
            else
                pre.Reg[instr.Values[2]].Value = 0;
        }
        #endregion

        #region Equality Testing
        static public void EQIR(Device pre, Instruction instr)
        {
            if (instr.Values[0] == pre.Reg[instr.Values[1]].Value)
                pre.Reg[instr.Values[2]].Value = 1;
            else
                pre.Reg[instr.Values[2]].Value = 0;
        }

        static public void EQRI(Device pre, Instruction instr)
        {
            if (pre.Reg[instr.Values[0]].Value == instr.Values[1])
                pre.Reg[instr.Values[2]].Value = 1;
            else
                pre.Reg[instr.Values[2]].Value = 0;
        }

        static public void EQRR(Device pre, Instruction instr)
        {
            if (pre.Reg[instr.Values[0]].Value == pre.Reg[instr.Values[1]].Value)
                pre.Reg[instr.Values[2]].Value = 1;
            else
                pre.Reg[instr.Values[2]].Value = 0;
        }
        #endregion
    }

    #region Object Classes
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
        EQIR = 0,
        BORR = 1,
        ADDR = 2,
        GTRI = 3,
        MULI = 4,
        GTIR = 5,
        MULR = 6,
        BANR = 7,
        BORI = 8,
        EQRI = 9,
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
