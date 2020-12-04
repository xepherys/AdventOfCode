using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AdventOfCode2018.Resources
{
    public partial class CustomTextBox : TextBox
    {
        public CustomTextBox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            BackColor = Color.Transparent;
        }
        public Control Buddy { get; set; }

        private static bool scrolling;   // In case buddy tries to scroll us

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // Trap WM_VSCROLL (0x115) or WM_HSCROLL (0x114) message and pass to buddy
            if ((m.Msg == 0x115 || m.Msg == 0x114) && !scrolling && Buddy != null && Buddy.IsHandleCreated)
            {
                scrolling = true;
                SendMessage(Buddy.Handle, m.Msg, m.WParam, m.LParam);
                scrolling = false;
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);
    }
}
