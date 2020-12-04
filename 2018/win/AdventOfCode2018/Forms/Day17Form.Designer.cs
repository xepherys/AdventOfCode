namespace AdventOfCode2018
{
    partial class Day17Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnToggleChars = new System.Windows.Forms.Button();
            this.btnToggleGridlines = new System.Windows.Forms.Button();
            this.btnDown = new System.Windows.Forms.Button();
            this.btnUp = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnVisualization = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.pbGameBoard = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Maroon;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(1, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(38, 35);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Black;
            this.groupBox1.Controls.Add(this.btnToggleChars);
            this.groupBox1.Controls.Add(this.btnToggleGridlines);
            this.groupBox1.Controls.Add(this.btnDown);
            this.groupBox1.Controls.Add(this.btnUp);
            this.groupBox1.Controls.Add(this.btnPause);
            this.groupBox1.Controls.Add(this.btnStep);
            this.groupBox1.Controls.Add(this.btnVisualization);
            this.groupBox1.Controls.Add(this.btnGo);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.ForeColor = System.Drawing.Color.DarkRed;
            this.groupBox1.Location = new System.Drawing.Point(0, -6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1348, 43);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // btnToggleChars
            // 
            this.btnToggleChars.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnToggleChars.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleChars.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleChars.ForeColor = System.Drawing.Color.Black;
            this.btnToggleChars.Location = new System.Drawing.Point(1009, 7);
            this.btnToggleChars.Name = "btnToggleChars";
            this.btnToggleChars.Size = new System.Drawing.Size(38, 35);
            this.btnToggleChars.TabIndex = 8;
            this.btnToggleChars.Text = "ABC";
            this.btnToggleChars.UseVisualStyleBackColor = false;
            this.btnToggleChars.Click += new System.EventHandler(this.btnToggleChars_Click);
            // 
            // btnToggleGridlines
            // 
            this.btnToggleGridlines.BackColor = System.Drawing.Color.RoyalBlue;
            this.btnToggleGridlines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleGridlines.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToggleGridlines.ForeColor = System.Drawing.Color.Black;
            this.btnToggleGridlines.Location = new System.Drawing.Point(967, 7);
            this.btnToggleGridlines.Name = "btnToggleGridlines";
            this.btnToggleGridlines.Size = new System.Drawing.Size(38, 35);
            this.btnToggleGridlines.TabIndex = 7;
            this.btnToggleGridlines.Text = "#";
            this.btnToggleGridlines.UseVisualStyleBackColor = false;
            this.btnToggleGridlines.Click += new System.EventHandler(this.btnToggleGridlines_Click);
            // 
            // btnDown
            // 
            this.btnDown.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDown.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDown.ForeColor = System.Drawing.Color.Black;
            this.btnDown.Location = new System.Drawing.Point(273, 7);
            this.btnDown.Name = "btnDown";
            this.btnDown.Size = new System.Drawing.Size(38, 35);
            this.btnDown.TabIndex = 6;
            this.btnDown.Text = "V";
            this.btnDown.UseVisualStyleBackColor = false;
            this.btnDown.Click += new System.EventHandler(this.btnDown_Click);
            // 
            // btnUp
            // 
            this.btnUp.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUp.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUp.ForeColor = System.Drawing.Color.Black;
            this.btnUp.Location = new System.Drawing.Point(229, 7);
            this.btnUp.Name = "btnUp";
            this.btnUp.Size = new System.Drawing.Size(38, 35);
            this.btnUp.TabIndex = 5;
            this.btnUp.Text = "^";
            this.btnUp.UseVisualStyleBackColor = false;
            this.btnUp.Click += new System.EventHandler(this.btnUp_Click);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Gold;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.ForeColor = System.Drawing.Color.Black;
            this.btnPause.Location = new System.Drawing.Point(1221, 7);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(38, 35);
            this.btnPause.TabIndex = 4;
            this.btnPause.Text = "||";
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStep
            // 
            this.btnStep.BackColor = System.Drawing.Color.GreenYellow;
            this.btnStep.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStep.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStep.ForeColor = System.Drawing.Color.Black;
            this.btnStep.Location = new System.Drawing.Point(1265, 7);
            this.btnStep.Name = "btnStep";
            this.btnStep.Size = new System.Drawing.Size(38, 35);
            this.btnStep.TabIndex = 3;
            this.btnStep.Text = ">";
            this.btnStep.UseVisualStyleBackColor = false;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnVisualization
            // 
            this.btnVisualization.BackColor = System.Drawing.Color.Aqua;
            this.btnVisualization.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVisualization.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnVisualization.ForeColor = System.Drawing.Color.Black;
            this.btnVisualization.Location = new System.Drawing.Point(659, 7);
            this.btnVisualization.Name = "btnVisualization";
            this.btnVisualization.Size = new System.Drawing.Size(38, 35);
            this.btnVisualization.TabIndex = 2;
            this.btnVisualization.Text = "VIS";
            this.btnVisualization.UseVisualStyleBackColor = false;
            this.btnVisualization.Click += new System.EventHandler(this.btnVisualization_Click);
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.Lime;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGo.ForeColor = System.Drawing.Color.Black;
            this.btnGo.Location = new System.Drawing.Point(1309, 7);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(38, 35);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = ">>";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // pbGameBoard
            // 
            this.pbGameBoard.Location = new System.Drawing.Point(12, 43);
            this.pbGameBoard.Name = "pbGameBoard";
            this.pbGameBoard.Size = new System.Drawing.Size(1324, 602);
            this.pbGameBoard.TabIndex = 3;
            this.pbGameBoard.TabStop = false;
            this.pbGameBoard.Click += new System.EventHandler(this.gameField_Click);
            this.pbGameBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.gameField_MouseDown);
            // 
            // Day17Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1348, 657);
            this.Controls.Add(this.pbGameBoard);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Day17Form";
            this.Text = "Day17Form";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Day17Form_MouseDown);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStep;
        private System.Windows.Forms.Button btnVisualization;
        private System.Windows.Forms.Button btnGo;
        public System.Windows.Forms.PictureBox pbGameBoard;
        private System.Windows.Forms.Button btnDown;
        private System.Windows.Forms.Button btnUp;
        private System.Windows.Forms.Button btnToggleGridlines;
        private System.Windows.Forms.Button btnToggleChars;
    }
}