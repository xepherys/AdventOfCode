using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Point = AdventOfCode2018.Core.Point;

namespace AdventOfCode2018
{
    public partial class Day20 : Form
    {
        static List<Room> rooms = new List<Room>();
        static Point[] corners = new Point[4];
        static Paths paths = new Paths();
        static string regexDirections = String.Empty;

        static int roomBufferPx = 2;
        public Day20()
        {
            InitializeComponent();

            using (Stream stream = File.OpenRead(@"day20.in"))
            using (StreamReader reader = new StreamReader(stream))
            {
                regexDirections = reader.ReadLine();
            }

            paths.NewPath();

            foreach (Char c in regexDirections)
            {
                foreach (Path p in paths.PathList)
                {

                        switch (c)
                        {
                            case 'N':
                            case 'S':
                            case 'E':
                            case 'W':
                                if (p.IsOpen()) p.DirectionList.Enqueue(c);
                                break;
                            case '(':
                                if (p.IsOpen())
                                {
                                    paths.PathList.Add(new Path(p));
                                }
                                break;
                        }

                }
            }
        }

        /*
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
        */

        #region Form Elements
        private void btnCloseApp_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        
    }

    #region Support Classes/Structs
    
    
    

    public struct Room
    {
        public Point Location { get; set; }
        public bool N { get; set; }
        public bool S { get; set; }
        public bool W { get; set; }
        public bool E { get; set; }
    }

    public struct Paths
    {
        public List<Path> PathList;

        public void ClosePath(Path p)
        {
            p.DirectionList.Enqueue('!');
        }

        public void NewPath()
        {
            this.PathList.Add(new Path());
        }

        public void NewPath(Path p)
        {
            this.PathList.Add(new Path(p));
        }
    }

    public struct Path
    {
        public Queue<Char> DirectionList;

        public Path(Path p)
        {
            this.DirectionList = new Queue<Char>(p.DirectionList);
        }

        public int PathLength()
        {
            return this.DirectionList.Where(w => w != '#' && w != '!').Count();
        }

        public bool IsOpen()
        {
            return !(this.DirectionList.Last() == '!');
        }

        public bool IsWaiting()
        {
            return this.DirectionList.Last() == '#';
        }

        public void Add(Char c)
        {
            this.DirectionList.Enqueue(c);
        }

        public void Close()
        {
            this.DirectionList.Enqueue('!');
        }
    }
    #endregion
}
