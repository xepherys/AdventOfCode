using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdventOfCode2017
{
    public partial class Form1 : Form
    {
        #region Setup
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();

        public Form1()
        {
            InitializeComponent();
        }
        #endregion


        /// <summary>
        /// The 'Days' region contains all of the main puzzle logic.
        /// </summary>
        #region Days

        // Day1a includes Day1b via boolean
        static public void Day1a(bool doDay1b = false)
        {
            string numbers = String.Empty;
            int totalValue = 0;


            var test = thisExe.GetManifestResourceNames();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2017._data.2017AdventOfCode_Day1.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                numbers = reader.ReadToEnd();
            }

            if (!doDay1b)
            {
                //Test values for Day1a
                //numbers = "1122";  //result should be 3
                //numbers = "1111";  //result should be 4
                //numbers = "1234";  //result should be 0
                //numbers = "9121212129"; //result should be 9

                for (int i = 0; i < numbers.Length; i++)
                {
                    if (i < numbers.Length - 1)
                    {
                        if (Convert.ToInt32(numbers[i].ToString()) == Convert.ToInt32(numbers[i + 1].ToString()))
                            totalValue += Convert.ToInt32(numbers[i].ToString());
                    }

                    if (i == 0)
                    {
                        if (Convert.ToInt32(numbers[i].ToString()) == Convert.ToInt32(numbers[numbers.Length - 1].ToString()))
                            totalValue += Convert.ToInt32(numbers[i].ToString());
                    }
                }

                MessageBox.Show("Day1a value: " + totalValue);
            }

            else
            {
                //Test values for Day1b
                //numbers = "1212";  //result should be 6
                //numbers = "1221";  //result should be 0
                //numbers = "123425";  //result should be 4
                //numbers = "123123";  //result should be 12
                //numbers = "12131415"; //result should be 4

                for (int i = 0; i < numbers.Length; i++)
                {
                    int j = FindCircularOpposite(i, numbers.Length);
                    if (Convert.ToInt32(numbers[i].ToString()) == Convert.ToInt32(numbers[j].ToString()))
                        totalValue += Convert.ToInt32(numbers[i].ToString());
                }

                MessageBox.Show("Day1b value: " + totalValue);
            }
        }

        static public void Day2a()
        {
            List<int[]> listOfRows = new List<int[]>();
            using (Stream stream = thisExe.GetManifestResourceStream("AdventOfCode2017._data.2017AdventOfCode_Day1.txt"))
            using (StreamReader reader = new StreamReader(stream))
            {
                //string s = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries)
            }
        }

        static public void Day2b()
        {
           
        }

        static public void Day2bThreaded()
        {
          
        }

        static public void Day3a()
        {
           
        }

        static public void Day3b()
        {
           
        }

        static public void Day4a()
        {
          
        }
        #endregion


        /// <summary>
        /// The 'Support Functions' region holds various functions that support main puzzle functions.
        /// </summary>
        #region Support Functions/Methods
        static IEnumerable<string> EnumerateLines(TextReader reader)
        {
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                yield return line;
            }
        }

        static int FindCircularOpposite(int i, int length)
        {
            int _ret = -1;
            i++;  //Set i value to be 1-max rather than 0-(max-1)

            if (length % 2 != 0)
                return _ret;

            _ret = i + (length / 2);

            if (_ret > length)
                _ret -= length;

            _ret--;

            return _ret;
        }
        #endregion


        /// <summary>
        /// The 'Form Stuff' region mostly just holds the function calls from buttons on the main form that call each primary logic routine.
        /// </summary>
        #region Form Stuff
        private void btnDay1a_Click(object sender, EventArgs e)
        {
            Day1a();
        }

        private void btnDay2b_Click(object sender, EventArgs e)
        {
            Day2b();
        }

        private void btnDay2bThreaded_Click(object sender, EventArgs e)
        {
            Day2bThreaded();
        }

        private void btnDay3a_Click(object sender, EventArgs e)
        {
            Day3a();
        }

        private void btnDay3b_Click(object sender, EventArgs e)
        {
            Day3b();
        }
        private void btnDay4a_Click(object sender, EventArgs e)
        {
            Day4a();
        }
        private void btnDay1b_Click(object sender, EventArgs e)
        {
            Day1a(true);
        }

        #endregion
    }
}
