using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day3
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();

            Bitmap bmp = new Bitmap(pbForm.Width, pbForm.Height);
            Pen redPen = new Pen(Color.Red, 5);
            Point point1 = new Point(0, 0);
            Point point2 = new Point(3, 7);
            Point point3 = new Point(10, 10);
            Point[] curvePoints = { point1, point2, point3 };

            //bmp.Graphics.DrawLines(redPen, curvePoints);
        }

        protected override void OnClosed(EventArgs e)
        {
            Thread t = Thread.CurrentThread;
            base.OnClosed(e);
        }
    }
}
