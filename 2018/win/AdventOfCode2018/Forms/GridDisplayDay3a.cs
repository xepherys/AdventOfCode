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
    public partial class GridDisplayDay3a : Form
    {
        public GridDisplayDay3a(string text, int countOver1)
        {
            InitializeComponent();
            txtDay3aGrid.Text = text;
            this.Text = "Count Over 1: " + countOver1;
        }
    }
}
