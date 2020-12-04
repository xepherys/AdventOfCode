using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using AdventOfCode2018.Core;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System;
using System.Linq;

namespace AdventOfCode2018.Day17
{
    public class Day17GameBoard : MonoBehaviour
    {
        public TextAsset input;

        int height = 0;
        int width = 0;
        int pixelHeightByFont;
        int pixelWidthByFont;
        ModArray2D<char> currentBoard;
        int msDelayCount = 50;

        bool complete = false;
        bool firsttime = true;
        int fontSize = 12;

        #region Properties
        public ModArray2D<char> CurrentBoard
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
                this.msDelayCount = value;
            }
        }
        #endregion

        #region Constructors
        public Day17GameBoard()
        {
        }
        #endregion

        private void Start()
        {
            this.Initializer();
            this.SetupBoardVisual();
        }


        public void Initializer()
        {
            Regex regex = new Regex(@"^([xy]{1})={1}([0-9]+), [xy]{1}={1}([0-9]+).{2}([0-9]+)$");
            List<Point> clayList = new List<Point>();
            int width = 0;
            int height = 0;
            int xMin = 0;
            int xMax = 999999;

            string[] lines = Regex.Split(input.text, "\n|\r|\r\n");

            foreach (string line in lines)
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
                        }
                    }

                    else if (match.Groups[1].Value == "y")
                    {
                        int y = Convert.ToInt32(match.Groups[2].Value);

                        for (int i = Convert.ToInt32(match.Groups[3].Value); i <= Convert.ToInt32(match.Groups[4].Value); i++)
                        {
                            clayList.Add(new Point(i, y));
                        }
                    }
                }
            }

            height = clayList.OrderByDescending(o => o.Y).FirstOrDefault().Y + 1;
            xMin = clayList.OrderBy(o => o.X).FirstOrDefault().X - 2;
            xMax = clayList.OrderByDescending(o => o.X).FirstOrDefault().X + 2;
            width = xMax - xMin;


            // Setup board size
            currentBoard = new ModArray2D<char>(width, height, xMin, 0);

            // Fill board with sand
            currentBoard.Populate('.');

            // Populate spring
            currentBoard.Set(500, 0, '+');

            // Setup clay
            foreach (Point p in clayList)
                currentBoard.Set(p.X, p.Y, '#');
        }




        void SetupBoardVisual()
        {
            for (int y = currentBoard.GetMinY(); y < currentBoard.GetMaxY(); y++)
            {
                for (int x = currentBoard.GetMinX(); x < currentBoard.GetMaxX(); x++)
                {
                    GameObject textGO = new GameObject();
                    textGO.transform.parent = this.transform;
                    textGO.AddComponent<Text>();
                    Text text = textGO.GetComponent<Text>();
                    text.text = currentBoard.Get(x, y).ToString();
                    text.fontSize = 12;
                    text.alignment = TextAnchor.MiddleCenter;
                    if (text.text == "#")
                        text.color = Color.grey;
                    else if (text.text == "|" || text.text == "~" || text.text == "+")
                        text.color = Color.blue;
                    else
                        text.color = Color.yellow;
                }
            }
        }
    }
}