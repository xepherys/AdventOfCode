using System;
using System.Collections.Generic;

namespace AoC
{
    public class GameConsole
    {
        #region Fields
        public OperationsCollection ops = new OperationsCollection();
        #endregion

        #region Properties
        
        #endregion

        #region Internal Classes and Structs - Collections
        public class OperationsCollection
        {
            #region Fields
            long accumulator = 0;
            IOp[] ops;
            long index = 0;
            List<long> indexes = new List<long>();
            #endregion

            #region Properties
            public long Accumulator
            {
                get
                {
                    return this.accumulator;
                }

                protected set
                {
                    this.accumulator = value;
                }
            }

            public IOp[] Ops
            {
                get
                {
                    return this.ops;
                }
            }

            public void Index(int i)
            {
                this.index += i;
            }
            #endregion

            #region Methods
            public void Accumulate(int a)
            {
                this.accumulator += a;
            }

            public void Init(IOp[] ops)
            {
                this.ops = ops;
            }

            public void Init(List<string> list)
            {
                this.ops = new IOp[list.Count];

                for (int i = 0; i < this.ops.Length; i++)
                {
                    string[] s = list[i].Split(' ');

                    switch (s[0])
                    {
                        case "acc":
                            ACC a = new ACC(Int32.Parse(s[1]), this);
                            this.ops[i] = a;
                            break;
                        case "jmp":
                            JMP j = new JMP(Int32.Parse(s[1]), this);
                            this.ops[i] = j;
                            break;
                        case "nop":
                            NOP n = new NOP(Int32.Parse(s[1]), this);
                            this.ops[i] = n;
                            break;
                    }
                }
            }

            public void Run()
            {
                while (index >= 0 && index < ops.Length)
                {
                    if (!indexes.Contains(index))
                    {
                        indexes.Add(this.index);
                        ops[index].Action();
                    }

                    else
                        break;
                }
            }

            public void FixAndRun(out int changedIndex)
            {
                changedIndex = -1;

                while (index != ops.Length)
                {
                    this.accumulator = 0;
                    this.index = 0;
                    changedIndex = FindFix(changedIndex + 1);

                    while (index >= 0 && index < ops.Length)
                    {
                        if (!indexes.Contains(index))
                        {
                            indexes.Add(this.index);
                            ops[index].Action();
                        }

                        else
                            break;
                    }

                    indexes.Clear();

                    if (changedIndex < ops.Length)
                        UndoFix(changedIndex);
                }
            }

            private int FindFix(int idx)
            {
                for (int i = idx; i < ops.Length; i++)
                {
                    if (ops[i].GetType() == typeof(JMP))
                    {
                        NOP n = new NOP(ops[i].Value, this);
                        ops[i] = n;
                        return i;
                    }

                    else if (ops[i].GetType() == typeof(NOP))
                    {
                        JMP j = new JMP(ops[i].Value, this);
                        ops[i] = j;
                        return i;
                    }
                }

                return idx;
            }

            private void UndoFix(int idx)
            {
                if (ops[idx].GetType() == typeof(JMP))
                {
                    NOP n = new NOP(ops[idx].Value, this);
                    ops[idx] = n;
                }

                else if (ops[idx].GetType() == typeof(NOP))
                {
                    JMP j = new JMP(ops[idx].Value, this);
                    ops[idx] = j;
                }
            }
            #endregion
        }
        #endregion

        #region Internal Classes and Structs - Operations
        public interface IOp
        {
            public int Value
            {
                get;
            }

            public int Move()
            {
                return 0;
            }

            virtual void Action()
            {
            }
        }

        public struct ACC : IOp
        {
            int val;
            OperationsCollection o;

            public int Value
            {
                get
                {
                    return this.val;
                }
            }

            public ACC(int val, OperationsCollection o)
            {
                this.val = val;
                this.o = o;
            }

            public int Move()
            {
                return 1;
            }

            public void Action()
            {
                o.Accumulate(this.val);
                o.Index(this.Move());
            }

            public override string ToString()
            {
                return String.Format("ACC {0}{1}", this.val > 0 ? "+" : "", this.val);
            }
        }

        public struct JMP : IOp
        {
            int val;
            OperationsCollection o;

            public int Value
            {
                get
                {
                    return this.val;
                }
            }

            public JMP(int val, OperationsCollection o)
            {
                this.val = val;
                this.o = o;
            }

            public int Move()
            {
                return this.val;
            }

            public void Action()
            {
                o.Index(this.Move());
            }

            public override string ToString()
            {
                return String.Format("JMP {0}{1}", this.val > 0 ? "+" : "", this.val);
            }
        }

        public struct NOP : IOp
        {
            int val;
            OperationsCollection o;

            public int Value
            {
                get
                {
                    return this.val;
                }
            }

            public NOP(int val, OperationsCollection o)
            {
                this.val = val;
                this.o = o;
            }

            public int Move()
            {
                return 1;
            }

            public void Action()
            {
                o.Index(this.Move());
            }

            public override string ToString()
            {
                return String.Format("NOP {0}{1}", this.val > 0 ? "+" : "", this.val);
            }
        }
        #endregion
    }
}