using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    public partial class Day16Form : Form
    {
        public Day16Form()
        {
            InitializeComponent();
        }

        private void Work(string func)
        {
            Device pre = new Device();
            Instruction instr = new Instruction();
            Device post = new Device();

            pre.Reg[0].Value = Convert.ToInt32(txtInitREG0.Text);
            pre.Reg[1].Value = Convert.ToInt32(txtInitREG1.Text);
            pre.Reg[2].Value = Convert.ToInt32(txtInitREG2.Text);
            pre.Reg[3].Value = Convert.ToInt32(txtInitREG3.Text);

            instr.OpCode = Convert.ToInt32(txtInstrOp.Text);
            instr.Values[0] = Convert.ToInt32(txtInstrVal1.Text);
            instr.Values[1] = Convert.ToInt32(txtInstrVal2.Text);
            instr.Values[2] = Convert.ToInt32(txtInstrVal3.Text);

            switch (func)
            {
                case "ADDR":
                    post = OpCodes.ADDR(pre, instr);
                    break;
                case "ADDI":
                    post = OpCodes.ADDI(pre, instr);
                    break;
                case "MULR":
                    post = OpCodes.MULR(pre, instr);
                    break;
                case "MULI":
                    post = OpCodes.MULI(pre, instr);
                    break;
                case "BANR":
                    post = OpCodes.BANR(pre, instr);
                    break;
                case "BANI":
                    post = OpCodes.BANI(pre, instr);
                    break;
                case "BORR":
                    post = OpCodes.BORR(pre, instr);
                    break;
                case "BORI":
                    post = OpCodes.BORI(pre, instr);
                    break;
                case "SETR":
                    post = OpCodes.SETR(pre, instr);
                    break;
                case "SETI":
                    post = OpCodes.SETI(pre, instr);
                    break;
                case "GTIR":
                    post = OpCodes.GTIR(pre, instr);
                    break;
                case "GTRI":
                    post = OpCodes.GTRI(pre, instr);
                    break;
                case "GTRR":
                    post = OpCodes.GTRR(pre, instr);
                    break;
                case "EQIR":
                    post = OpCodes.EQIR(pre, instr);
                    break;
                case "EQRI":
                    post = OpCodes.EQRI(pre, instr);
                    break;
                case "EQRR":
                    post = OpCodes.EQRR(pre, instr);
                    break;
            }

            txtFinalREG0.Text = post.Reg[0].Value.ToString();
            txtFinalREG1.Text = post.Reg[1].Value.ToString();
            txtFinalREG2.Text = post.Reg[2].Value.ToString();
            txtFinalREG3.Text = post.Reg[3].Value.ToString();
        }

        #region Function Buttons
        private void btnADDR_Click(object sender, EventArgs e)
        {
            Work("ADDR");
        }

        private void btnADDI_Click(object sender, EventArgs e)
        {
            Work("ADDI");
        }

        private void btnMULR_Click(object sender, EventArgs e)
        {
            Work("MULR");
        }

        private void btnMULI_Click(object sender, EventArgs e)
        {
            Work("MULI");
        }

        private void btnBANR_Click(object sender, EventArgs e)
        {
            Work("BANR");
        }

        private void btnBANI_Click(object sender, EventArgs e)
        {
            Work("BANI");
        }

        private void btnBORR_Click(object sender, EventArgs e)
        {
            Work("BORR");
        }

        private void btnBORI_Click(object sender, EventArgs e)
        {
            Work("BORI");
        }

        private void btnSETR_Click(object sender, EventArgs e)
        {
            Work("SETR");
        }

        private void btnSETI_Click(object sender, EventArgs e)
        {
            Work("SETI");
        }

        private void btnGTIR_Click(object sender, EventArgs e)
        {
            Work("GTIR");
        }

        private void btnGTRI_Click(object sender, EventArgs e)
        {
            Work("GTRI");
        }

        private void btnGTRR_Click(object sender, EventArgs e)
        {
            Work("GTRR");
        }

        private void btnEQIR_Click(object sender, EventArgs e)
        {
            Work("EQIR");
        }

        private void btnEQRI_Click(object sender, EventArgs e)
        {
            Work("EQRI");
        }

        private void btnEQRR_Click(object sender, EventArgs e)
        {
            Work("EQRR");
        }
        #endregion
    }
}
