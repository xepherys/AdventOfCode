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
    public partial class Day13Form : Form
    {
        Day13Manager d3m;

        public Day13Form(ref Day13Manager _mgr)
        {
            InitializeComponent();
            d3m = _mgr;
            _mgr.RunWork("AdventOfCode2018._data.AdventOfCode_Day13.txt");
            DrawTrackMap(d3m.Map);
            txtStepNumber.Text = d3m.StepNum.ToString();
        }

        void DrawTrackMap(char[,] trackMap)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < trackMap.GetLength(1); y++)
            {
                for (int x = 0; x < trackMap.GetLength(0); x++)
                {
                    /*
                    string s = trackMap[x, y].ToString();

                    if (s == "<" || s == ">" || s == "v" || s == "^")
                    {
                        txtRails.SelectionFont = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold);
                        txtRails.SelectionColor = Color.Gold;
                    }
                    else if (s == "X")
                    {
                        txtRails.SelectionFont = new Font(FontFamily.GenericMonospace, 10, FontStyle.Bold);
                        txtRails.SelectionColor = Color.Red;
                    }
                    else
                    {
                        txtRails.SelectionFont = new Font(FontFamily.GenericMonospace, 10);
                        txtRails.SelectionColor = Color.Gray;
                    }

                    txtRails.AppendText(s);
                    */
                    sb.Append(trackMap[x, y]);
                }
                sb.Append(Environment.NewLine);
            }

            txtRails.Text = sb.ToString();
        }

        private void btnStepFwd_Click(object sender, EventArgs e)
        {
            string s = d3m.Update();
            DrawTrackMap(d3m.Map);

            if (!String.IsNullOrEmpty(s))
                this.Text = s;

            txtStepNumber.Text = d3m.StepNum.ToString();
        }

        private void btnStepBack_Click(object sender, EventArgs e)
        {
            string s = d3m.Update(-1);
            DrawTrackMap(d3m.Map);

            if (!String.IsNullOrEmpty(s))
                this.Text = s;

            txtStepNumber.Text = d3m.StepNum.ToString();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            string s = String.Empty;

            while (String.IsNullOrEmpty(s))// && d3m.StepNum < 242)
            {
                s = d3m.Update();
            }

            /*
            if (d3m.StepNum >= 242)
            {
                s = d3m.Update();
            }
            */

            DrawTrackMap(d3m.Map);

            this.Text = s;
            
            txtStepNumber.Text = d3m.StepNum.ToString();
        }
    }
}
