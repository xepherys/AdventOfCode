using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using AdventOfCode2018.Core;
using System.Diagnostics;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections;

namespace AdventOfCode2018
{
    public partial class Day18Form : Form
    {
        static System.Reflection.Assembly thisExe = System.Reflection.Assembly.GetExecutingAssembly();
        static string ResourceFilename = "AdventOfCode2018._data.AdventOfCode_Day18.txt";
        public float textSize = 10f;
        private static List<Tile> field = new List<Tile>();
        public int Iterations = 0;



        // Some testing WIP
        static string GameField;
        public const int OPEN = 0;
        public const int TREE = 1;
        public const int LUMBERYARD = 2;

        public const int BITS_PER_CELL = 2;
        public const int CELLS_PER_ROW = 3;
        public const int CELL_MASK = (1 << BITS_PER_CELL) - 1;
        public const int MID_OFFSET = BITS_PER_CELL;
        public const int TOP_OFFSET = BITS_PER_CELL * 2;

        public const int COL_OFFSET = BITS_PER_CELL* CELLS_PER_ROW;
        public const int RIGHT_COL_OFFSET = COL_OFFSET* 2;

        // Where the right column gets inserted
        public const int TOP_RIGHT_OFFSET = RIGHT_COL_OFFSET + TOP_OFFSET;
        public const int MID_RIGHT_OFFSET = RIGHT_COL_OFFSET + MID_OFFSET;
        public const int BOT_RIGHT_OFFSET = RIGHT_COL_OFFSET;

        public const int ME = 4;
        public static readonly int[] NOT_ME = { 0, 1, 2, 3, 5, 6, 7, 8 };

        public static int[] current;
        public static int[] next;

        public const int COL_PER_ROW_FIELD = 50;
        public const int ROWS_IN_FIELD = 50;
        public const int TOTAL_CELLS = COL_PER_ROW_FIELD * ROWS_IN_FIELD;




        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        PrivateFontCollection pfc = new PrivateFontCollection();
        Font font;

        BackgroundWorker bg = new BackgroundWorker();

        public List<Tile> Field
        {
            get
            {
                return field;
            }

            set
            {
                field = value;
            }
        }

        public Day18Form(bool isB = false)
        {
            InitializeComponent();
            Field = new List<Tile>();

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

            this.bg.WorkerReportsProgress = true;
            this.bg.WorkerSupportsCancellation = true;
            this.bg.DoWork += new DoWorkEventHandler(this.bg_DoWork);
            this.bg.ProgressChanged += new ProgressChangedEventHandler(this.bg_ProgressChanged);
            this.bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bg_RunWorkerCompleted);

            AssignFonts();

            using (Stream stream = thisExe.GetManifestResourceStream(ResourceFilename))
            using (StreamReader reader = new StreamReader(stream))
            {
                GameField = reader.ReadToEnd();
                
                //string line;
                //int Y = 0;
                //int X = 0;

                /*
                List<string> lines = new List<string>();

                while ((line = reader.ReadLine()) != null)
                {
                    string tmpLine = String.Empty;
                    //foreach (char c in line)
                    //{
                    //    Field.Add(new Tile(X, Y, c, this));
                    //    X++;
                    //}
                    //Y++;
                    //X = 0;

                    foreach (Char c in line)
                    {
                        switch (c)
                        {
                            case '.':
                                tmpLine += '0';
                                break;
                            case '|':
                                tmpLine += '1';
                                break;
                            case '#':
                                tmpLine += '2';
                                break;
                            default:
                                tmpLine += c;
                                break;
                        }
                    }

                    lines.Add(tmpLine);
                }

                current = lines.ToArray();
                */
            }

            
            current = new int[2500];
            string tmpLine = String.Empty;
            int i = 0;
            foreach (Char c in GameField)
            {
                switch (c)
                {
                    case '.':
                        current[i] = 0;
                        tmpLine += '0';
                        i++;
                        break;
                    case '|':
                        current[i] = 1;
                        tmpLine += '1';
                        i++;
                        break;
                    case '#':
                        current[i] = 2;
                        tmpLine += '2';
                        i++;
                        break;
                    default:
                        tmpLine += c;
                        break;
                }
                
            }

            GameField = tmpLine;
            txtGameBoard.Text = GameField;
            ColorizeGameboard(txtGameBoard);
            CalculateResources();
        }

        void CalculateResources()
        {
            int trees = 0;
            int open = 0;
            int lyards = 0;

            for (int i = 0; i < txtGameBoard.Text.Length; i++)
            {
                if (txtGameBoard.Text[i] == '0')
                {
                    open++;
                }

                else if (txtGameBoard.Text[i] == '1')
                {
                    trees++;
                }

                else if (txtGameBoard.Text[i] == '2')
                {
                    lyards++;
                }
            }

            txtTrees.Text = trees.ToString();
            txtOpen.Text = open.ToString();
            txtLumberyards.Text = lyards.ToString();
            txtResources.Text = (trees * lyards).ToString();
        }

        private void AssignFonts(float size = 10f)
        {
            foreach (FontFamily ff in pfc.Families)
            {
                if (ff.Name == "Hack")
                {
                    font = new Font(ff, size, FontStyle.Regular);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            int i = 0;
            while (i < 10)
            {
                RunWork();
                i++;
            }
            sw.Stop();

            txtGameBoard.Text = GetFieldString(Field);
            ColorizeGameboard(txtGameBoard);
            CalculateResources();
            txtTurnNumber.Text = Iterations.ToString();
            
            MessageBox.Show(sw.ElapsedMilliseconds + "ms elapsed");
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            RunWork();
            txtGameBoard.Text = GetFieldString(Field);
            ColorizeGameboard(txtGameBoard);
            CalculateResources();
            txtTurnNumber.Text = Iterations.ToString();
            Thread.Sleep(5);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (this.bg.IsBusy)
            {
                this.bg.CancelAsync();
            }
        }

        private void Day18Form_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //complete = true;
        }

        void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        void bg_ProgressChanged(object sender, string s)
        {

        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        static void RunWork()
        {
            int iterator = 0;
            while (iterator < TOTAL_CELLS)
            {
                for (int bitwise = 1; bitwise <= 18; bitwise++)
                {
                    int trees = 0;
                    int lumber = 0;
                    int val;
                    foreach (int j in NOT_ME)
                    {
                        int n = (bitwise >> (j * BITS_PER_CELL)) & CELL_MASK;
                        if (n == TREE)
                            trees += 1;
                        else if (n == LUMBERYARD)
                            lumber += 1;
                    }
                    
                    switch ((bitwise >> (ME * BITS_PER_CELL)) & CELL_MASK)
                    {
                        case OPEN:
                            val = (trees >= 3) ? TREE : OPEN;
                            break;
                        case TREE:
                            val = (lumber >= 3) ? LUMBERYARD : TREE;
                            break;
                        case LUMBERYARD:
                            val = (lumber > 0 && trees > 0) ? LUMBERYARD : OPEN;
                            break;
                        default:
                            break;
                        // Note that 3 is unfortunately a waste of space.
                    }
                }
                iterator++;
            }



            //ConcurrentBag<Tile> newField = new ConcurrentBag<Tile>();

            // Below is the current best option.
            /*
            Parallel.ForEach(Field, (originalTile) =>
            {
                originalTile.Ttype[Iterations + 1] = originalTile.CheckSurrounding(Iterations);
                //newField.Add(new Tile(originalTile, newTileType));
            });
            */
            /*
            Parallel.ForEach(Field, (originalTile) =>
            {
                originalTile.Ttype = originalTile.NewTileType;
                //newField.Add(new Tile(originalTile, newTileType));
            });
            */

            //Field = new List<Tile>(newField);

            //Iterations++;
        }

        private string GetFieldString(List<Tile> f)
        {
            string _ret = String.Empty;

            int minLine = f.OrderBy(o => o.Location.Y).First().Location.Y;
            int maxLine = f.OrderByDescending(o => o.Location.Y).First().Location.Y;

            for (int i = minLine; i <= maxLine; i++)
            {
                foreach (Tile t in f.Where(w => w.Location.Y == i).OrderBy(o => o.Location.X))
                {
                    /*
                    switch (t.Ttype)
                    {
                        case TileType.OPEN:
                            _ret += ".";
                            break;
                        case TileType.LUMBERYARD:
                            _ret += "#";
                            break;
                        case TileType.TREES:
                            _ret += "|";
                            break;
                    }
                    */

                    _ret += t.Ttype[Iterations].ToString();
                }
                _ret += Environment.NewLine;
            }

            return _ret;
        }

        private void ColorizeGameboard(RichTextBox field)
        {
            for (int i = 0; i < field.Text.Length; i++)
            {
                if (field.Text[i] == '0')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.Silver;
                    field.SelectionFont = new Font("Hack", textSize);
                }

                else if (field.Text[i] == '2')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.SteelBlue;
                    field.SelectionFont = new Font("Hack", textSize, FontStyle.Bold);
                }

                else if (field.Text[i] == '1')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.Green;
                    field.SelectionFont = new Font("Hack", textSize);
                }
            }
        }
    }

    public class Tile
    {
        //public TileType Ttype;
        public char[] Ttype = new char[1001];
        public Core.Point Location;
        public Core.Point[] Surrounding;
        public Day18Form form;
        public char NewTileType;

        public Tile(int _x, int _y, char c, Day18Form f)
        {
            Location = new Core.Point(_x, _y);
            form = f;
            Ttype[0] = c;
            /*
            switch (c)
            {
                case '.':
                    Ttype = TileType.OPEN;
                    break;
                case '#':
                    Ttype = TileType.LUMBERYARD;
                    break;
                case '|':
                    Ttype = TileType.TREES;
                    break;
                default:
                    Ttype = TileType.NONE;
                    break;
            }
            */
            Surrounding = new Core.Point[8] { new Core.Point(_x, _y - 1), new Core.Point(_x + 1, _y - 1),
                                              new Core.Point(_x + 1, _y), new Core.Point(_x + 1, _y + 1),
                                              new Core.Point(_x, _y + 1), new Core.Point(_x - 1, _y + 1),
                                              new Core.Point(_x - 1, _y), new Core.Point(_x - 1, _y - 1)};
        }

        public Tile(Tile t, char tt)
        {
            Location = t.Location;
            form = t.form;
            Surrounding = t.Surrounding;
            Ttype[0] = tt;
        }

        public char CheckSurrounding(int val)
        {
            int trees = 0;
            int open = 0;
            int lyard = 0;
            foreach (Core.Point p in Surrounding)
            {
                if (form.Field.Exists(e => e.Location == p))
                {
                    Tile _tempTile = form.Field.Single(s => s.Location == p);
                    switch (_tempTile.Ttype[val])
                    {
                        case '.':
                            open++;
                            break;
                        case '#':
                            lyard++;
                            break;
                        case '|':
                            trees++;
                            break;
                    }
                }
            }

            // Rules:

            // An open acre will become filled with trees if three or more adjacent acres contained trees. Otherwise, nothing happens.
            if (this.Ttype[val] == '.' && trees > 2)
                return '|';

            // An acre filled with trees will become a lumberyard if three or more adjacent acres were lumberyards. Otherwise, nothing happens.
            else if (this.Ttype[val] == '|' && lyard > 2)
                return '#';

            // An acre containing a lumberyard will remain a lumberyard if it was adjacent to at least one other lumberyard and at least one acre containing trees. Otherwise, it becomes open.
            else if (this.Ttype[val] == '#' && (lyard < 1 || trees < 1))
                return '.';

            else
                return this.Ttype[val];
        }
    }

    public enum TileType
    {
        NONE,
        OPEN,
        TREES,
        LUMBERYARD
    }
}
