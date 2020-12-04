using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventOfCode2018
{
    public partial class Day10Form : Form
    {
        public Day10Form()
        {
            InitializeComponent();
        }

        public Day10Form(string text = "", int? value = null, int? fontSize = null)
        {
            InitializeComponent();
            if (fontSize != null)
                txtText.Font = new Font("Consolas", Convert.ToInt32(fontSize));
            txtText.Text = text;
            if (value != null)
                this.Text = value.ToString();
        }

        public Day10Form(string text = "", long? value = null, int? fontSize = null)
        {
            InitializeComponent();
            if (fontSize != null)
                txtText.Font = new Font("Consolas", Convert.ToInt32(fontSize));
            txtText.Text = text;
            if (value != null)
                this.Text = value.ToString();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            txtText.Text = Day10.Day10a(Convert.ToInt32(numUpdates.Value), Convert.ToInt32(numBy.Value), cbInvertXY.Checked);
        }

        private void cbInvertXY_CheckedChanged(object sender, EventArgs e)
        {
            txtText.Text = Day10.Day10a(Convert.ToInt32(numUpdates.Value), Convert.ToInt32(numBy.Value), cbInvertXY.Checked);
        }

        private void numBy_ValueChanged(object sender, EventArgs e)
        {
            txtText.Text = Day10.Day10a(Convert.ToInt32(numUpdates.Value), Convert.ToInt32(numBy.Value), cbInvertXY.Checked);
        }

        private void numUpdates_ValueChanged(object sender, EventArgs e)
        {
            txtText.Text = Day10.Day10a(Convert.ToInt32(numUpdates.Value), Convert.ToInt32(numBy.Value), cbInvertXY.Checked);
        }
    }
}
