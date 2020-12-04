using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using AdventOfCode2018.Core;
using Point =  AdventOfCode2018.Core.Point;

namespace AdventOfCode2018
{
    public partial class Day17Form : Form
    {
        #region Class Stuff
        public float textSize = 9f;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public PrivateFontCollection pfc = new PrivateFontCollection();
        public Font hackFont;
        public Font miniHackFont;

        BackgroundWorker bg = new BackgroundWorker();

        private int X;
        private int Y;

        public Day17GameBoard board;
        #endregion

        public Day17Form()
        {
            #region Setup Form
            InitializeComponent();

            Stream fontStream = this.GetType().Assembly.GetManifestResourceStream("AdventOfCode2018.Fonts.Hack-Regular.ttf");
            Stream fontStreamB = this.GetType().Assembly.GetManifestResourceStream("AdventOfCode2018.Fonts.Hack-Bold.ttf");
            byte[] fontdata = new byte[fontStream.Length];
            byte[] fontdataB = new byte[fontStreamB.Length];
            fontStream.Read(fontdata, 0, (int)fontStream.Length);
            fontStream.Close();
            fontStreamB.Read(fontdataB, 0, (int)fontStreamB.Length);
            fontStreamB.Close();

            unsafe
            {
                fixed (byte* pFontData = fontdata)
                {
                    pfc.AddMemoryFont((System.IntPtr)pFontData, fontdata.Length);
                }
                fixed (byte* pFontData = fontdataB)
                {
                    pfc.AddMemoryFont((System.IntPtr)pFontData, fontdataB.Length);
                }
            }

            AssignFonts();
            #endregion

            #region Background Worker
            this.bg.WorkerReportsProgress = true;
            this.bg.WorkerSupportsCancellation = true;
            this.bg.DoWork += new DoWorkEventHandler(this.bg_DoWork);
            this.bg.ProgressChanged += new ProgressChangedEventHandler(this.bg_ProgressChanged);
            this.bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bg_RunWorkerCompleted);
            #endregion

            #region Setup Game Board
            board = new Day17GameBoard(this);
            board.SetupBoardVisual();
            #endregion
        }

        private void AssignFonts(float size = 8.25f)
        {
            foreach (FontFamily ff in pfc.Families)
            {
                if (ff.Name == "Hack")
                {
                    hackFont = new Font(ff, size, FontStyle.Regular);
                    miniHackFont = new Font(ff, 6f, FontStyle.Regular);
                }
            }
        }

        private void Day17Form_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            
        }

        void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            int? arg = (int?)e.Argument;
            board.RunWork(ref bg, arg);
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (!this.bg.IsBusy)
            {
                this.bg.RunWorkerAsync(1);
            }
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!this.bg.IsBusy)
            {
                this.bg.RunWorkerAsync(null);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (this.bg.IsBusy)
            {
                this.bg.CancelAsync();
            }
        }
        
        private void btnUp_MouseDown(object sender, EventArgs e)
        {

        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            this.board.CurrentLine -= 20;
            this.board.SetupBoardVisual();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            this.board.CurrentLine += 20;
            this.board.SetupBoardVisual();
        }

        private void btnToggleGridlines_Click(object sender, EventArgs e)
        {
            this.board.ToggleGridLines();
        }

        private void btnToggleChars_Click(object sender, EventArgs e)
        {
            this.board.ToggleCharacters();
        }

        private void gameField_Click(object sender, EventArgs e)
        {
            int pixelHeight = this.board.pixelHeight;
            int pixelWidth = this.board.pixelWidth;
            MessageBox.Show(string.Format("X: {0} Y: {1}", X, Y));
        }

        private void gameField_MouseDown(object sender, MouseEventArgs e)
        {
            X = e.X;
            Y = e.Y;
        }

        private void btnVisualization_Click(object sender, EventArgs e)
        {
            board.Visualization = !board.Visualization;
            pbGameBoard.Visible = !pbGameBoard.Visible;
        }
    }

    public class Day17GameBoard
    {
        static System.Reflection.Assembly thisExe;

        int height = 0;
        int width = 0;
        public int pixelHeight = 0;
        public int pixelWidth = 0;
        ModArray2D<char> currentBoard;
        Day17Form form;
        string resource = "AdventOfCode2018._data.AdventOfCode_Day17.txt";
        int msDelayCount = 10;
        int step = 0;
        public int currentLine = 0;
        public int RowsToDisplay = 40;
        public List<Point> Cursors = new List<Point>();
        int iterator = 0;
        bool gridLines = false;
        bool characterDisplay = false;
        public bool Visualization = true;
        int minx = 0;

        List<Point> flowPoints = new List<Point>();

        bool complete = false;

        #region Properties
        public int CurrentLine
        {
            get
            {
                return this.currentLine;
            }

            set
            {
                if (value < 0)
                    this.currentLine = 0;
                else if (value > this.height)
                    this.currentLine = height;
                else
                    this.currentLine = value;
            }
        }

        public ModArray2D<char> CurrentBoard
        {
            get
            {
                return this.currentBoard;
            }
        }
        #endregion

        #region Constructors
        public Day17GameBoard(Day17Form _form)
        {
            form = _form;

            Initializer();
            
        }
        #endregion

        public void RunWork(ref BackgroundWorker bg, int? arg = null)
        {
            Stopwatch sw = new Stopwatch();
            if (arg == 1)
            {
                DoWork(ref bg);
            }

            else if (arg == null)
            {
                sw.Start();
                while (!complete && !bg.CancellationPending)
                {
                    DoWork(ref bg);
                }
                sw.Stop();

                if (complete)
                {
                    int wetTiles = currentBoard.WetTileCount();
                    int restingWetTiles = currentBoard.RestingWetTileCount();
                    MessageBox.Show("There are " + wetTiles + " wet tiles on this map." + Environment.NewLine +
                                    "There are " + restingWetTiles + " resting wet tiles on this map." + Environment.NewLine +
                                    "Map run in " + sw.ElapsedMilliseconds + "ms.");
                }
            }
        }

        void DoWork(ref BackgroundWorker bg)
        {
            // If there are no more cursors, we should be done.
            if (Cursors.Count < 1)
            {
                this.complete = true;
                return;
            }

            List<Point> newCursors = new List<Point>();

            foreach (Point p in Cursors)
            {
                Point down = p.Down();
                Point up = p.Up();
                Point left = p.Left();
                Point right = p.Right();

                bool enclosed = CheckForEnclosed(p);
                bool skip = false;

                // If the cursor has fallen below MaxY, drop that cursor path
                if (down.Y > this.CurrentBoard.GetMaxY() - 1)
                {
                    this.CurrentBoard.Set(p, '|');
                    skip = true;
                }

                // If there's already water here
                else if (this.CurrentBoard.Get(down) == '|')
                {
                    this.CurrentBoard.Set(p, '|');
                    skip = true;
                }

                // If it's falling through sand unobstructed
                else if (this.CurrentBoard.Get(down) == '.')
                {
                    this.CurrentBoard.Set(p, '|');
                    if (!newCursors.Contains(down) && ! Cursors.Contains(down))
                        newCursors.Add(down);
                }

                // It hits clay
                else if ((this.CurrentBoard.Get(down) == '#' || this.CurrentBoard.Get(down).IsWet()))
                {
                    this.CurrentBoard.Set(p, '~');

                    while ((this.CurrentBoard.Get(left.Down()) == '#' || this.CurrentBoard.Get(left.Down()).IsWet()) && this.CurrentBoard.Get(left) != '#')
                    {
                        this.CurrentBoard.Set(left, '~');
                        if (this.CurrentBoard.Get(left.Left()) == '.' || this.CurrentBoard.Get(left.Left()) == '|')
                            left = left.Left();
                        else
                            break;
                    }

                    while ((this.CurrentBoard.Get(right.Down()) == '#' || this.CurrentBoard.Get(right.Down()).IsWet()) && this.CurrentBoard.Get(right) != '#')
                    {
                        this.CurrentBoard.Set(right, '~');
                        if (this.CurrentBoard.Get(right.Right()) == '.' || this.CurrentBoard.Get(right.Right()) == '|')
                            right = right.Right();
                        else
                            break;
                    }

                   
                    if (this.CurrentBoard.Get(right.Down()) == '.')
                    {
                        Point tmpLeft = right.Left();
                        this.CurrentBoard.Set(p, '|');
                        if (!newCursors.Contains(right))
                            newCursors.Add(right);
                        while (this.CurrentBoard.Get(tmpLeft).IsWet())
                        {
                            this.CurrentBoard.Set(tmpLeft, '|');
                            tmpLeft = tmpLeft.Left();
                        }
                    }

                    if (this.CurrentBoard.Get(left.Down()) == '.')
                    {
                        Point tmpRight = left.Right();
                        this.CurrentBoard.Set(p, '|');
                        if (!newCursors.Contains(left))
                            newCursors.Add(left);
                        while (this.CurrentBoard.Get(tmpRight).IsWet())
                        {
                            this.CurrentBoard.Set(tmpRight, '|');
                            tmpRight = tmpRight.Right();
                        }
                    }

                    if (enclosed && !skip)
                    {
                        if (!newCursors.Contains(up))
                            newCursors.Add(up);
                    }
                }
            }

            Cursors.Clear();
            Cursors = new List<Point>(newCursors);
            if (Cursors.Count() > 0)
                CurrentLine = Cursors.OrderByDescending(o => o.Y).First().Y;

            if (Visualization)
            {
                SetupBoardVisual();
                //Thread.Sleep(1);
            }
        }

        bool CheckForEnclosed(Point p)
        {
            bool _ret = false;
            Point left = p;
            Point right = p;
            bool rightWall = false;
            bool leftWall = false;
            bool falling = false;

            //if (this.CurrentBoard.Get(p) == '~')
            //return true;

            while (!leftWall && left.X > this.CurrentBoard.GetMinX() && !falling)
            {
                leftWall = (this.CurrentBoard.Get(left) == '#');
                if (!(p.Down().Y > this.CurrentBoard.GetMaxY() - 1))
                    falling = (this.CurrentBoard.Get(left.Down()) == '.');
                left = left.Left();

            }
            while (!rightWall && right.X < this.CurrentBoard.GetMaxX() && !falling)
            {
                rightWall = (this.CurrentBoard.Get(right) == '#');
                if (!(p.Down().Y > this.CurrentBoard.GetMaxY() - 1))
                    falling = (this.CurrentBoard.Get(right.Down()) == '.');
                right = right.Right();
            }

            if (rightWall && leftWall)
                _ret = true;

            return _ret;
        }

        public void Initializer()
        {
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();
            Regex regex = new Regex(@"^([xy]{1})={1}([0-9]+), [xy]{1}={1}([0-9]+).{2}([0-9]+)$");
            List<Point> clayList = new List<Point>();
            int xMin = 0;
            int xMax = 999999;
            int yMin = 999;

            using (Stream stream = thisExe.GetManifestResourceStream(resource))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    Match match = regex.Match(line);
                    if (match.Success)
                    {
                        if (match.Groups[1].Value == "x")
                        {
                            int x = Convert.ToInt32(match.Groups[2].Value);

                            for (int i = Convert.ToInt32(match.Groups[3].Value); i <= Convert.ToInt32(match.Groups[4].Value); i++)
                            {
                                clayList.Add(new Point(x, i));
                                if (i < yMin) yMin = i;
                            }
                        }

                        else if (match.Groups[1].Value == "y")
                        {
                            int y = Convert.ToInt32(match.Groups[2].Value);

                            for (int i = Convert.ToInt32(match.Groups[3].Value); i <= Convert.ToInt32(match.Groups[4].Value); i++)
                            {
                                clayList.Add(new Point(i, y));
                                if (y < yMin) yMin = y;
                            }
                        }
                    }
                }
            }
            this.height = clayList.OrderByDescending(o => o.Y).FirstOrDefault().Y + 1;
            xMin = clayList.OrderBy(o => o.X).FirstOrDefault().X - 2;
            xMax = clayList.OrderByDescending(o => o.X).FirstOrDefault().X + 2;
            this.width = xMax - xMin;

            // Setup board size
            currentBoard = new ModArray2D<char>(width, height, xMin, 0);
            currentBoard.yMin = yMin;

            // Fill board with sand
            currentBoard.Populate('.');

            // Populate spring
            Point springPoint = new Point(500, 0);
            currentBoard.Set(500, 0, '+');
            this.Cursors.Add(springPoint.Down());

            // Setup clay
            foreach (Point p in clayList)
                currentBoard.Set(p.X, p.Y, '#');

            this.pixelHeight = form.pbGameBoard.Size.Height / this.RowsToDisplay;
            this.pixelWidth = form.pbGameBoard.Size.Width / width;
        }

        public void ToggleGridLines()
        {
            this.gridLines = !this.gridLines;
            SetupBoardVisual();
        }

        public void ToggleCharacters()
        {
            this.characterDisplay = !this.characterDisplay;
            SetupBoardVisual();
        }

        public void SetupBoardVisual()
        {
            Bitmap bmp = new Bitmap(form.pbGameBoard.Size.Width, form.pbGameBoard.Size.Height);

            int pixelX = 0;
            int pixelY = 0;

            if (this.CurrentLine < 0) this.CurrentLine = 0;
            if (this.CurrentLine > this.height - 1) this.CurrentLine = this.height - 1;

            int minRow = Math.Max(this.CurrentLine - (RowsToDisplay / 2), 0);
            int maxRow = Math.Min(minRow + RowsToDisplay, this.height);
            if (maxRow == this.height)
                minRow = maxRow - RowsToDisplay;

            Graphics image = Graphics.FromImage(bmp);

            int padding = (gridLines) ? 1 : 0;

            for (int y = minRow; y < maxRow; y++)
            {
                for (int x = this.CurrentBoard.GetMinX(); x < this.CurrentBoard.GetMaxX(); x++)
                {
                    Brush brushcolor = Brushes.Black;
                    int count = Cursors.Where(w => w == new Point(x, y)).Count();
                    if (count == 1)
                        brushcolor = Brushes.White;
                    if (count > 1)
                        brushcolor = Brushes.Red;

                    image.FillRectangle(brushcolor, pixelX * pixelWidth, pixelY * pixelHeight, pixelWidth, pixelHeight);
                    switch (this.CurrentBoard.Get(x, y))
                    {
                        case '.':
                            image.FillRectangle(Brushes.DarkGoldenrod, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding, pixelWidth - (padding * 2), pixelHeight - (padding * 2));
                            if (characterDisplay) image.DrawString(".", form.miniHackFont, Brushes.Black, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding);
                            break;
                        case '#':
                            image.FillRectangle(Brushes.DarkGray, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding, pixelWidth - (padding * 2), pixelHeight - (padding * 2));
                            if (characterDisplay) image.DrawString("#", form.miniHackFont, Brushes.SteelBlue, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding);
                            break;
                        case '+':
                            image.FillRectangle(Brushes.DarkBlue, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding, pixelWidth - (padding * 2), pixelHeight - (padding * 2));
                            if (characterDisplay) image.DrawString("+", form.miniHackFont, Brushes.White, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding);
                            break;
                        case '~':
                            image.FillRectangle(Brushes.DarkBlue, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding, pixelWidth - (padding * 2), pixelHeight - (padding * 2));
                            if (characterDisplay) image.DrawString("~", form.miniHackFont, Brushes.White, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding);
                            break;
                        case '|':
                            image.FillRectangle(Brushes.DarkBlue, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding, pixelWidth - (padding * 2), pixelHeight - (padding * 2));
                            if (characterDisplay) image.DrawString("|", form.miniHackFont, Brushes.White, (pixelX * pixelWidth) - padding, (pixelY * pixelHeight) - padding);
                            break;
                    }

                    pixelX++;
                }

                pixelY++;
                pixelX = 0;
            }

            form.pbGameBoard.Image = bmp;
        }
    }

    public class ModArray<T>
    {
        T[] _internalarray;
        int size = 0;
        int shift = 0;

        public ModArray(int _sz, int _sh)
        {
            this.size = _sz;
            this.shift = _sh;
            _internalarray = new T[this.size];
        }

        public T[] Array
        {
            get
            {
                return _internalarray;
            }
        }

        public void Set(int i, T value)
        {
            _internalarray[i - this.shift] = value;
        }

        public T Get(int i)
        {
            return _internalarray[i - this.shift];
        }

        public void Populate(T val)
        {
            _internalarray.Populate(val);
        }

        public int GetMin()
        {
            return this.shift;
        }

        public int GetMax()
        {
            return this.shift + this.size;
        }

        public string GetString()
        {
            return new string(_internalarray as char[]);
        }
    }

    public class ModArray2D<T>
    {
        T[,] _internalarray;
        int sizeX = 0;
        int sizeY = 0;
        int shiftX = 0;
        int shiftY = 0;
        public int yMin = 0;

        public ModArray2D(int _szX, int _szY, int _shX, int _shY)
        {
            this.sizeX = _szX;
            this.sizeY = _szY;
            this.shiftX = _shX;
            this.shiftY = _shY;
            _internalarray = new T[this.sizeX, this.sizeY];
        }

        public void Set(int x, int y, T value)
        {
            _internalarray[x - this.shiftX, y - this.shiftY] = value;
        }

        public void Set(Point p, T value)
        {
            _internalarray[p.X - this.shiftX, p.Y - this.shiftY] = value;
        }

        public T Get(int x, int y)
        {
            return _internalarray[x - this.shiftX, y - this.shiftY];
        }

        public T Get(Point p, bool converted = false)
        {
            if (converted)
                return _internalarray[p.X, p.Y];
            else
                return _internalarray[p.X - this.shiftX, p.Y - this.shiftY];
        }

        public void Populate(T val)
        {
            _internalarray.Populate(val);
        }

        public int GetMinX()
        {
            return this.shiftX;
        }

        public int GetMaxX()
        {
            return this.shiftX + this.sizeX;
        }

        public int GetMaxY()
        {
            return this.shiftY + this.sizeY;
        }

        public int WetTileCount()
        {
            int _wetreturn = 0;

            for (int y = yMin; y < _internalarray.GetLength(1); y++)
            {
                for (int x = 0; x < _internalarray.GetLength(0); x++)
                {
                    if (Convert.ToChar(_internalarray[x, y].ToString()).IsWet())
                    {
                        _wetreturn++;
                    }

                }
            }

            return _wetreturn;
        }

        public int RestingWetTileCount()
        {
            int _wetreturn = 0;

            for (int y = yMin; y < _internalarray.GetLength(1); y++)
            {
                for (int x = 0; x < _internalarray.GetLength(0); x++)
                {
                    if (Convert.ToChar(_internalarray[x, y].ToString()) == '~')
                    {
                        _wetreturn++;
                    }

                }
            }

            return _wetreturn;
        }
    }
}
