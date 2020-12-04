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
//using Point = System.Drawing.Point;
using Point = AdventOfCode2018.Core.Point;

using AdventOfCode2018.Core;

namespace AdventOfCode2018
{
    public partial class Day15Form : Form
    {
        //Day15a
        //473616 - too high
        //470304 - too high
        //208579 - too high
        //202797 - nope
        int maxBoardHeight = 0;
        int maxBoardWidth = 0;
        public float textSize = 8.25f;

        PrivateFontCollection pfc = new PrivateFontCollection();
        Font fdot;
        Font fhash;
        Font felf;
        Font fgoblin;

        BackgroundWorker bg = new BackgroundWorker();

        public Day15GameBoard board;

        public Day15Form(bool isB = false)
        {
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

            this.bg.WorkerReportsProgress = true;
            this.bg.WorkerSupportsCancellation = true;
            this.bg.DoWork += new DoWorkEventHandler(this.bg_DoWork);
            this.bg.ProgressChanged += new ProgressChangedEventHandler(this.bg_ProgressChanged);
            this.bg.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.bg_RunWorkerCompleted);

            AssignFonts();
            maxBoardHeight = txtGameBoard.Size.Height;
            maxBoardWidth = txtGameBoard.Size.Width;

            board = new Day15GameBoard(this);
        }

        private void AssignFonts(float size = 8.25f)
        {
            foreach (FontFamily ff in pfc.Families)
            {
                if (ff.Name == "Hack")
                {
                    fdot = new Font(ff, size, FontStyle.Regular);
                    fhash = new Font(ff, size, FontStyle.Bold);
                    felf = new Font(ff, size, FontStyle.Regular);
                    fgoblin = new Font(ff, size, FontStyle.Regular);
                }
            }
        }

        public void CalculateBoardWindowSize(int _ht, int _wd)
        {
            Point currPos = txtGameBoard.Location;
            int ptX = currPos.X;
            int ptY = currPos.Y;

            if (_ht < maxBoardHeight)
            {
                ptY = (maxBoardHeight - _ht) / 2 + currPos.Y;
            }

            if (_wd < maxBoardWidth)
            {
                ptX = (maxBoardWidth - _wd) / 2 + currPos.X;
            }

            txtGameBoard.Location = new Point(ptX, ptY);

            if (_ht < txtGameBoard.Height)
                txtGameBoard.Height = _ht;
            if (_wd < txtGameBoard.Width)
                txtGameBoard.Width = _wd;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGo_Click(object sender, EventArgs e)
        {
            if (!this.bg.IsBusy)
            {
                this.bg.RunWorkerAsync(null);
            }
        }

        private void btnStep_Click(object sender, EventArgs e)
        {
            if (!this.bg.IsBusy)
            {
                this.bg.RunWorkerAsync(1);
            }
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (this.bg.IsBusy)
            {
                this.bg.CancelAsync();
            }
        }

        void bg_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //complete = true;
        }

        void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.SetText(e.UserState.ToString());
        }

        void bg_ProgressChanged(object sender, string s)
        {
            this.SetText(sender, s);
        }

        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            int? arg = (int?)e.Argument;
            board.RunWork(ref bg, arg);
        }

        delegate void StringArgReturningVoidDelegate(object sender, string text);

        public void SetText(object sender, string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtGameBoard.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }

            else
            {
                this.txtGameBoard.Text = text;
            }
        }

        public void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtGameBoard.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }

            else
            {
                this.txtGameBoard.Text = text;
            }
        }

        private void txtGameBoard_TextChanged(object sender, EventArgs e)
        {
            ColorizeGameboard(txtGameBoard);
            UpdateScores();
        }

        private void ColorizeGameboard(RichTextBox field)
        {
            for (int i = 0; i < field.Text.Length; i++)
            {
                if (field.Text[i] == '.')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.Silver;
                    field.SelectionFont = new Font("Hack", textSize);
                }

                else if (field.Text[i] == '#')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.White;
                    field.SelectionFont = new Font("Hack", textSize, FontStyle.Bold);
                }

                else if (field.Text[i] == 'E')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.Green;
                    field.SelectionFont = new Font("Hack", textSize);
                }

                else if (field.Text[i] == 'G')
                {
                    field.SelectionStart = i;
                    field.SelectionLength = 1;
                    field.SelectionColor = Color.Red;
                    field.SelectionFont = new Font("Hack", textSize);
                }
            }
        }

        private void UpdateScores()
        {
            int numElves = 0;
            int hpElves = 0;
            int numGoblins = 0;
            int hpGoblins = 0;

            var players = this.board.Players;
            foreach (Player p in players)
            {
                if (p.Side == 'E')
                {
                    numElves++;
                    hpElves += p.HP;
                }

                else if (p.Side == 'G')
                {
                    numGoblins++;
                    hpGoblins += p.HP;
                }
            }

            txtElvesRemaining.Text = numElves.ToString();
            txtElvesHP.Text = hpElves.ToString();
            txtGoblinsRemaining.Text = numGoblins.ToString();
            txtGoblinsHP.Text = hpGoblins.ToString();

            txtTurnNumber.Text = this.board.Turn.ToString();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            
        }

        private void FixInitial(object sender, EventArgs e)
        {
            string s = txtGameBoard.Text;
            txtGameBoard.Text = s;
        }
    }

    public class Day15GameBoard
    {
        static System.Reflection.Assembly thisExe;

        int height = 0;
        int width = 0;
        int defaultHP = 200;
        int pixelHeightByFont;
        int pixelWidthByFont;
        char[,] currentBoard;
        Day15Form form;
        string resource = "AdventOfCode2018._data.AdventOfCode_Day15_sample2.txt";
        List<Player> players = new List<Player>();
        AStarNav nav;
        int turnNumber = 1; // Starts at turn 1
        int msDelayCount = 50;
        
        
        bool complete = false;
        bool firsttime = true;

        #region Properties
        public char[,] CurrentBoard
        {
            get
            {
                return this.currentBoard;
            }
        }

        public int Delay
        {
            get
            {
                return this.msDelayCount;
            }

            private set
            {
                // base 50ms + 10ms per active player
                this.msDelayCount = 50 + (value * 10);
            }
        }

        public int Turn
        {
            get
            {
                return this.turnNumber;
            }

            private set
            {
                this.turnNumber = value;
            }
        }

        public List<Player> Players
        {
            get
            {
                return this.players;
            }

            private set
            {
                this.players = value;
            }
        }
        #endregion

        #region Constructors
        public Day15GameBoard(Day15Form _form)
        {
            form = _form;
            
            

            Initializer();
            SetupBoardVisual();
        }
        #endregion

        public void RunWork(ref BackgroundWorker bg, int? arg = null)
        {
            if (arg == 1)
            {
                DoWork(ref bg);
                Thread.Sleep(this.Delay);
            }

            else if (arg == null)
            {
                while (!complete && !bg.CancellationPending)
                {
                    DoWork(ref bg);
                    Thread.Sleep(this.Delay);
                }
            }
        }

        
        void DoWork(ref BackgroundWorker bg)
        {
            //Thread.Sleep(500);
            Queue<Player> turns = new Queue<Player>(this.players.OrderBy(o => o.Location.Y).ThenBy(o => o.Location.X));
            Player currPlayer;
            Player currEnemy;

            if (turns.Count == 1)
            {
                complete = true;
            }

            while (turns.Count > 0 && !complete)
            {
                currPlayer = turns.Dequeue();

                if (players.Where(w => w.ID == currPlayer.ID).Count() < 1)
                    continue;

                if ((currEnemy = currPlayer.CanAttack(this.players)) != null)
                {
                    // Attack if opponent within range at beginning of turn.
                    currEnemy.HP -= currPlayer.AttackPower;
                    if (currEnemy.HP <= 0)
                    {
                        currentBoard[currEnemy.Location.X, currEnemy.Location.Y] = '.';
                        players.Remove(currEnemy);
                    }
                }

                else
                {
                    // Move if no opponent within range at beginning of turn.
                    NavPackage targetEnemy = currPlayer.GetClosestEnemy2(this.players);

                    if (targetEnemy != null)
                    {
                        //this.nav.Map = currentBoard;
                        //this.nav.Start = new AStarNode(currPlayer.Location);
                        //this.nav.Finish = new AStarNode(targetEnemy.Location);
                        //this.nav.InitNodes();
                        //Point p = this.nav.Step();
                        Point p = targetEnemy.NextMove;
                        if (IsFree(p))
                        {
                            this.CurrentBoard[currPlayer.Location.X, currPlayer.Location.Y] = '.';
                            currPlayer.Location = p;
                            this.CurrentBoard[currPlayer.Location.X, currPlayer.Location.Y] = currPlayer.Side;
                            SetupBoardVisual(ref bg);
                            //Thread.Sleep(this.Delay);
                        }



                        if ((currEnemy = currPlayer.CanAttack(this.players)) != null)
                        {
                            // Attack after move if an opponent is now available.
                            currEnemy.HP -= currPlayer.AttackPower;
                            if (currEnemy.HP <= 0)
                            {
                                currentBoard[currEnemy.Location.X, currEnemy.Location.Y] = '.';
                                players.Remove(currEnemy);
                            }
                        }
                    }
                }

                this.Delay = players.Count();
                SetupBoardVisual(ref bg);
                Thread.Sleep(this.Delay);
            }

            complete = CheckComplete();
            if (!complete)
                this.Turn++;
        }

        bool CheckComplete()
        {
            if (players.Where(w => w.Side == 'E').Count() == 0 || players.Where(w => w.Side == 'G').Count() == 0)
                return true;
            else
                return false;
        }

        bool IsFree(Point p)
        {
            return (this.CurrentBoard[p.X, p.Y] == '.');
        }

        public void Initializer()
        {
            thisExe = System.Reflection.Assembly.GetExecutingAssembly();

            using (Stream stream = thisExe.GetManifestResourceStream(resource))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    width = line.Length;
                    height++;
                }

                currentBoard = new char[width, height];
            }

            using (Stream stream = thisExe.GetManifestResourceStream(resource))
            using (StreamReader reader = new StreamReader(stream))
            {
                string line;
                int currHeight = 0;
                int currWidth = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    foreach (char c in line)
                    {
                        currentBoard[currWidth, currHeight] = c;
                        
                        if (c == 'E' || c == 'G')
                            players.Add(new Player(c, defaultHP, new Point(currWidth, currHeight), this));

                        currWidth++;
                    }
                    currWidth = 0;
                    currHeight++;
                }
            }

            nav = new AStarNav(currentBoard, new char[] { '.' }, new Point(0, 0), new Point(0, 0));

            foreach (Player p in players)
            {
                p.Nav = nav;
            }

            this.Delay = players.Count();
        }




        void SetupBoardVisual()
        {
            if (firsttime)
            {
                if (height <= 32 && width <= 32)
                    form.textSize = 14f;

                else if (height <= 48 && width <= 48)
                    form.textSize = 10f;

                else if (height <= 64 && width <= 64)
                    form.textSize = 6f;

                else
                    form.textSize = 4f;

                int formHeight = (int)(PixelLayout.FontPointToPixel((int)form.textSize) * 1.25 * height);
                int formWidth = (int)(PixelLayout.FontPointToPixel((int)form.textSize) * 0.75 * width);

                form.CalculateBoardWindowSize(formHeight, formWidth);
                firsttime = false;
            }

            string s = "";

            for (int y = 0; y < currentBoard.GetLength(1); y++)
            {
                for (int x = 0; x < currentBoard.GetLength(0); x++)
                {
                    s += currentBoard[x, y].ToString();
                }

                //if (y < currentBoard.GetLength(1) - 1)
                s += Environment.NewLine;
            }

            form.SetText(s);
        }

        void SetupBoardVisual(ref BackgroundWorker bg)
        {
            string s = "";

            for (int y = 0; y < currentBoard.GetLength(1); y++)
            {
                for (int x = 0; x < currentBoard.GetLength(0); x++)
                {
                    s += currentBoard[x, y].ToString();
                }

                //if (y < currentBoard.GetLength(1) - 1)
                    s += Environment.NewLine;
            }

            bg.ReportProgress(0, s);// = s;
        }
    }

    public class Player
    {
        #region Fields
        string id = "";
        char side;
        int hp;
        Point location;
        object parent;
        char enemy;
        int attackPower;
        AStarNav nav;
        #endregion

        #region Properties
        public AStarNav Nav
        {
            get
            {
                return this.nav;
            }

            set
            {
                this.nav = value;
            }
        }
        public string ID
        {
            get
            {
                return this.id;
            }

            private set
            {
                if (this.id == "")
                    this.id = value;
            }
        }
        public char Side
        {
            get
            {
                return this.side;
            }

            set
            {
                if (value == 'E' || value == 'G')
                    this.side = value;

                else
                    throw new ArgumentOutOfRangeException("Only 'E'lf of 'G'oblin are valid player types. [Player.Side]");
            }
        }
        public int HP
        {
            get
            {
                return this.hp;
            }

            set
            {
                this.hp = value;
            }
        }
        public Point Location
        {
            get
            {
                return this.location;
            }
            
            set
            {
                this.location = value;
            }
        }
        public int AttackPower
        {
            get
            {
                return this.attackPower;
            }
        }
        #endregion

        #region Constructors
        public Player(char _s, int _h, Point _l, object o)
        {
            this.ID = Guid.NewGuid().ToString();
            this.Side = _s;
            this.HP = _h;
            this.Location = _l;
            this.parent = o;
            this.enemy = (this.side == 'E') ? 'G' : 'E';
            this.attackPower = 3;
        }
        #endregion

        #region Methods
        public Player CanAttack(List<Player> players)
        {
            Player _ret = null;
            int hpRemaining = 999999;
            Point p = this.Location;

            if (p.Y > 0 && players.Exists(e => e.Location == new Point(p.X, p.Y - 1) && e.Side == this.enemy) && players.Single(s => s.Location == new Point(p.X, p.Y - 1)).HP < hpRemaining)
            {
                _ret = players.Single(s => s.Location == new Point(p.X, p.Y - 1));
                hpRemaining = _ret.HP;
            }
            if (p.X > 0 && players.Exists(e => e.Location == new Point(p.X - 1, p.Y) && e.Side == this.enemy) && players.Single(s => s.Location == new Point(p.X - 1, p.Y)).HP < hpRemaining)
            {
                _ret = players.Single(s => s.Location == new Point(p.X - 1, p.Y));
                hpRemaining = _ret.HP;
            }
            if (p.X < (parent as Day15GameBoard).CurrentBoard.GetLength(0) && players.Exists(e => e.Location == new Point(p.X + 1, p.Y) && e.Side == this.enemy) && players.Single(s => s.Location == new Point(p.X + 1, p.Y)).HP < hpRemaining)
            {
                _ret = players.Single(s => s.Location == new Point(p.X + 1, p.Y));
                hpRemaining = _ret.HP;
            }
            if (p.Y < (parent as Day15GameBoard).CurrentBoard.GetLength(1) && players.Exists(e => e.Location == new Point(p.X, p.Y + 1) && e.Side == this.enemy) && players.Single(s => s.Location == new Point(p.X, p.Y + 1)).HP < hpRemaining)
            {
                _ret = players.Single(s => s.Location == new Point(p.X, p.Y + 1));
                hpRemaining = _ret.HP;
            }

            return _ret;
        }

        public Player GetClosestEnemy(List<Player> players)
        {
            List<Player> _ret = new List<Player>();
            int distance = 999999999;
            int _dst;

            foreach (Player e in players.Where(w => w.side == this.enemy))
            {
                if ((_dst = CoordinateGeometry.CheckManhattanDistance(this.Location, e.Location)) == distance)
                {
                    distance = _dst;
                    _ret.Add(e);
                }

                else if ((_dst = CoordinateGeometry.CheckManhattanDistance(this.Location, e.Location)) < distance)
                {
                    distance = _dst;
                    _ret.Clear();
                    _ret.Add(e);
                }
            }

            return _ret.OrderBy(o => o.Location.Y).ThenBy(o => o.Location.X).FirstOrDefault();
        }

        public NavPackage GetClosestEnemy2(List<Player> players)
        {
            List<Player> _ret = new List<Player>();
            IDictionary<string, int> enemyDistances = new Dictionary<string, int>();
            List<NavPackage> navPackList = new List<NavPackage>();

            this.nav.Map = (this.parent as Day15GameBoard).CurrentBoard;

            foreach (Player e in players.Where(w => w.side == this.enemy))
            {
                this.nav.Start = new AStarNode(this.Location);
                this.nav.Finish = new AStarNode(e.Location);
                this.nav.InitNodes();
                //int _distance = this.nav.GetDistance();
                //if (_distance > 0)
                //    enemyDistances.Add(new KeyValuePair<string, int> (e.ID, _distance));
                List<Point> path = this.nav.GetDistance();
                if (path.Count > 0)
                {
                    NavPackage np = new NavPackage(path.Count(), path.DefaultIfEmpty(this.Location).FirstOrDefault(), e);
                    navPackList.Add(np);
                }
            }

            //string id = enemyDistances.OrderBy(kvp => kvp.Value).FirstOrDefault().Key;

            //return players.Where(w => w.ID == id).FirstOrDefault();
            if (navPackList.Count > 0)
                return PickFromList(navPackList);
            else
                return null;
        }

        private NavPackage PickFromList(List<NavPackage> list)
        {
            if (list.Count == 1)
                return list.First();//.Target;

            else
            {
                int minimumDistance = list.OrderBy(o => o.Distance).First().Distance;
                list = list.Where(w => w.Distance == minimumDistance).ToList();

                return list.OrderBy(o => o.Target.Location.Y).ThenBy(o => o.Target.Location.X).First();//.Target;
            }
        }
        #endregion

        public enum Directions
        {
            NONE = -99,
            N = 0,
            E = 1,
            S = 2,
            W = 3,
            NE = 4,
            SE = 5,
            SW = 6,
            NW = 7
        }
    }

    public class NavPackage
    {
        public int Distance { get; set; }
        public Point NextMove { get; set; }
        public Player Target { get; set; }

        public NavPackage(int d, Point p, Player t)
        {
            this.Distance = d;
            this.NextMove = p;
            this.Target = t;
        }

    }
}
