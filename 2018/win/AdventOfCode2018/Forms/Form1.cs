using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AdventOfCode2018
{
    public partial class Form1 : Form
    {
        #region Setup
        public Form1()
        {
            InitializeComponent();
        }
        #endregion

        /// <summary>
        /// The 'Form Stuff' region mostly just holds the function calls from buttons on the main form that call each primary logic routine.
        /// </summary>
        #region Form Stuff
        private void btnDay1b_Click(object sender, EventArgs e)
        {
            Day1.Day1a();
        }

        private void btnDay2a_Click(object sender, EventArgs e)
        {
            Day2.Day2a();
        }

        private void btnDay2b_Click(object sender, EventArgs e)
        {
            Day2.Day2b();
        }

        private void btnDay2bThreaded_Click(object sender, EventArgs e)
        {
            Day2.Day2bThreaded();
        }

        private void btnDay3a_Click(object sender, EventArgs e)
        {
            Day3.Day3a();
        }

        private void btnDay3b_Click(object sender, EventArgs e)
        {
            Day3.Day3b();
        }

        private void btnDay4a_Click(object sender, EventArgs e)
        {
            Day4.Day4a();
        }

        private void btnDay5a_Click(object sender, EventArgs e)
        {
            Day5.Day5a();
        }

        private void btnDay5b_Click(object sender, EventArgs e)
        {
            Day5.Day5b();
        }

        private void btnDay6a_Click(object sender, EventArgs e)
        {
            Day6.Day6a();
        }

        private void btnDay6b_Click(object sender, EventArgs e)
        {
            Day6.Day6b();
        }

        private void btnDay7a_Click(object sender, EventArgs e)
        {
            Day7.Day7a();
        }

        private void btnDay7b_Click(object sender, EventArgs e)
        {
            Day7.Day7b();
        }

        private void btnDay8a_Click(object sender, EventArgs e)
        {
            Day8.Day8a(sender);
        }

        private void btnDay8b_Click(object sender, EventArgs e)
        {
            Day8.Day8b();
        }

        private void btnDay9a_Click(object sender, EventArgs e)
        {
            Day9.Day9a();
        }

        private void btnDay9b_Click(object sender, EventArgs e)
        {
            Day9.Day9b();
        }

        private void btnDay10a_Click(object sender, EventArgs e)
        {
            //Day10.Day10a();
            Day10Form _form = new Day10Form();
            _form.Show();
        }

        private void btnDay11a_Click(object sender, EventArgs e)
        {
            Day11.Day11a();
        }

        private void btnDay11b_Click(object sender, EventArgs e)
        {
            Day11.Day11a(true);
        }

        private void btnDay12a_Click(object sender, EventArgs e)
        {
            Day12.Day12a(20);
        }

        private void btnDay12b_Click(object sender, EventArgs e)
        {
            Day12.Day12a(50000000000);
        }

        private void btnDay13a_Click(object sender, EventArgs e)
        {
            Day13.Day13a();
        }

        private void btnDay14a_Click(object sender, EventArgs e)
        {
            Day14Form _frm = new Day14Form();
            _frm.Show();
        }

        private void btnDay15a_Click(object sender, EventArgs e)
        {
            Day15Form _frm = new Day15Form();
            _frm.Show();
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnDay16a_Click(object sender, EventArgs e)
        {
            Day16.Day16a();
        }

        private void btnDay16Form_Click(object sender, EventArgs e)
        {
            Day16Form _form = new Day16Form();
            _form.Show();
        }

        private void btnDay16b1_Click(object sender, EventArgs e)
        {
            Day16.Day16b1();
        }

        private void btnDay16b2_Click(object sender, EventArgs e)
        {
            Day16.Day16b2();
        }

        private void btnDay17_Click(object sender, EventArgs e)
        {
            Day17Form form = new Day17Form();
            form.Show();
        }

        private void btnDay9aDll_Click(object sender, EventArgs e)
        {
            Day9DoublyLinkedList.RunWork(false);
        }

        private void btnDay9bDll_Click(object sender, EventArgs e)
        {
            Day9DoublyLinkedList.RunWork(true);
        }
        #endregion

        private void btnDay18_Click(object sender, EventArgs e)
        {
            Day18Form form = new Day18Form();
            form.Show();
        }

        private void btnDay19_Click(object sender, EventArgs e)
        {
            try
            {
                Day19.Day19a();
            }

            catch (TypeInitializationException typeex)
            {
                string opcode = typeex.InnerException.Message.Substring(typeex.InnerException.Message.IndexOf('\'') + 1, 4);
                MessageBox.Show("\'" + opcode + "\' is not a valid OpCode");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDay19b_Click(object sender, EventArgs e)
        {
            try
            {
                Day19.Day19a(true);
            }

            catch (TypeInitializationException typeex)
            {
                string opcode = typeex.InnerException.Message.Substring(typeex.InnerException.Message.IndexOf('\'') + 1, 4);
                MessageBox.Show("\'" + opcode + "\' is not a valid OpCode");
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDay20_Click(object sender, EventArgs e)
        {
            Day20 form = new Day20();
            form.Show();
        }
    }
}
