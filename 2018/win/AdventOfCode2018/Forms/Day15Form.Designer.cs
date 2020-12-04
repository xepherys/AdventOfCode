namespace AdventOfCode2018

{
    partial class Day15Form
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
            this.components = new System.ComponentModel.Container();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStep = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtGameBoard = new System.Windows.Forms.RichTextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.txtElvesRemaining = new System.Windows.Forms.TextBox();
            this.txtElvesHP = new System.Windows.Forms.TextBox();
            this.txtGoblinsRemaining = new System.Windows.Forms.TextBox();
            this.txtGoblinsHP = new System.Windows.Forms.TextBox();
            this.txtTurnNumber = new System.Windows.Forms.TextBox();
            this.textBox8 = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.Black;
            this.groupBox1.Controls.Add(this.btnPause);
            this.groupBox1.Controls.Add(this.btnStep);
            this.groupBox1.Controls.Add(this.btnTest);
            this.groupBox1.Controls.Add(this.btnGo);
            this.groupBox1.Controls.Add(this.btnClose);
            this.groupBox1.ForeColor = System.Drawing.Color.DarkRed;
            this.groupBox1.Location = new System.Drawing.Point(0, -6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1348, 43);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
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
            this.toolTip1.SetToolTip(this.btnPause, "Pause");
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
            this.toolTip1.SetToolTip(this.btnStep, "Step");
            this.btnStep.UseVisualStyleBackColor = false;
            this.btnStep.Click += new System.EventHandler(this.btnStep_Click);
            // 
            // btnTest
            // 
            this.btnTest.BackColor = System.Drawing.Color.Aqua;
            this.btnTest.Enabled = false;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.Black;
            this.btnTest.Location = new System.Drawing.Point(655, 8);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(38, 35);
            this.btnTest.TabIndex = 2;
            this.btnTest.Text = "T";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Visible = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
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
            this.toolTip1.SetToolTip(this.btnGo, "Go");
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
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
            // txtGameBoard
            // 
            this.txtGameBoard.BackColor = System.Drawing.Color.Black;
            this.txtGameBoard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGameBoard.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGameBoard.ForeColor = System.Drawing.Color.LightGray;
            this.txtGameBoard.Location = new System.Drawing.Point(12, 43);
            this.txtGameBoard.Name = "txtGameBoard";
            this.txtGameBoard.ReadOnly = true;
            this.txtGameBoard.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.txtGameBoard.Size = new System.Drawing.Size(987, 795);
            this.txtGameBoard.TabIndex = 1;
            this.txtGameBoard.Text = "";
            this.txtGameBoard.TextChanged += new System.EventHandler(this.txtGameBoard_TextChanged);
            this.txtGameBoard.VisibleChanged += new System.EventHandler(this.FixInitial);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.DimGray;
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(1006, 44);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(332, 794);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.DimGray;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.ForeColor = System.Drawing.Color.Chartreuse;
            this.textBox1.Location = new System.Drawing.Point(1006, 149);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(332, 19);
            this.textBox1.TabIndex = 3;
            this.textBox1.Text = "Elves";
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.DimGray;
            this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox2.ForeColor = System.Drawing.Color.Maroon;
            this.textBox2.Location = new System.Drawing.Point(1006, 506);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(332, 19);
            this.textBox2.TabIndex = 4;
            this.textBox2.Text = "Goblins";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            this.textBox4.BackColor = System.Drawing.Color.DimGray;
            this.textBox4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox4.ForeColor = System.Drawing.Color.Chartreuse;
            this.textBox4.Location = new System.Drawing.Point(1006, 274);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(174, 15);
            this.textBox4.TabIndex = 6;
            this.textBox4.Text = "Total HP Remaining:";
            this.textBox4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox3
            // 
            this.textBox3.BackColor = System.Drawing.Color.DimGray;
            this.textBox3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox3.ForeColor = System.Drawing.Color.Chartreuse;
            this.textBox3.Location = new System.Drawing.Point(1006, 216);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(174, 15);
            this.textBox3.TabIndex = 7;
            this.textBox3.Text = "Elves Remaining:";
            this.textBox3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox5
            // 
            this.textBox5.BackColor = System.Drawing.Color.DimGray;
            this.textBox5.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox5.ForeColor = System.Drawing.Color.Maroon;
            this.textBox5.Location = new System.Drawing.Point(1006, 591);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(174, 15);
            this.textBox5.TabIndex = 8;
            this.textBox5.Text = "Goblins Remaining:";
            this.textBox5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // textBox6
            // 
            this.textBox6.BackColor = System.Drawing.Color.DimGray;
            this.textBox6.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox6.ForeColor = System.Drawing.Color.Maroon;
            this.textBox6.Location = new System.Drawing.Point(1006, 655);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(174, 15);
            this.textBox6.TabIndex = 9;
            this.textBox6.Text = "Total HP Remaining:";
            this.textBox6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtElvesRemaining
            // 
            this.txtElvesRemaining.BackColor = System.Drawing.Color.DimGray;
            this.txtElvesRemaining.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtElvesRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtElvesRemaining.ForeColor = System.Drawing.Color.Chartreuse;
            this.txtElvesRemaining.Location = new System.Drawing.Point(1203, 216);
            this.txtElvesRemaining.Name = "txtElvesRemaining";
            this.txtElvesRemaining.ReadOnly = true;
            this.txtElvesRemaining.Size = new System.Drawing.Size(82, 15);
            this.txtElvesRemaining.TabIndex = 10;
            this.txtElvesRemaining.Text = "0";
            // 
            // txtElvesHP
            // 
            this.txtElvesHP.BackColor = System.Drawing.Color.DimGray;
            this.txtElvesHP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtElvesHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtElvesHP.ForeColor = System.Drawing.Color.Chartreuse;
            this.txtElvesHP.Location = new System.Drawing.Point(1203, 274);
            this.txtElvesHP.Name = "txtElvesHP";
            this.txtElvesHP.ReadOnly = true;
            this.txtElvesHP.Size = new System.Drawing.Size(82, 15);
            this.txtElvesHP.TabIndex = 11;
            this.txtElvesHP.Text = "0";
            // 
            // txtGoblinsRemaining
            // 
            this.txtGoblinsRemaining.BackColor = System.Drawing.Color.DimGray;
            this.txtGoblinsRemaining.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGoblinsRemaining.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGoblinsRemaining.ForeColor = System.Drawing.Color.Maroon;
            this.txtGoblinsRemaining.Location = new System.Drawing.Point(1203, 591);
            this.txtGoblinsRemaining.Name = "txtGoblinsRemaining";
            this.txtGoblinsRemaining.ReadOnly = true;
            this.txtGoblinsRemaining.Size = new System.Drawing.Size(82, 15);
            this.txtGoblinsRemaining.TabIndex = 12;
            this.txtGoblinsRemaining.Text = "0";
            // 
            // txtGoblinsHP
            // 
            this.txtGoblinsHP.BackColor = System.Drawing.Color.DimGray;
            this.txtGoblinsHP.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtGoblinsHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGoblinsHP.ForeColor = System.Drawing.Color.Maroon;
            this.txtGoblinsHP.Location = new System.Drawing.Point(1203, 655);
            this.txtGoblinsHP.Name = "txtGoblinsHP";
            this.txtGoblinsHP.ReadOnly = true;
            this.txtGoblinsHP.Size = new System.Drawing.Size(82, 15);
            this.txtGoblinsHP.TabIndex = 13;
            this.txtGoblinsHP.Text = "0";
            // 
            // txtTurnNumber
            // 
            this.txtTurnNumber.BackColor = System.Drawing.Color.DimGray;
            this.txtTurnNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtTurnNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTurnNumber.ForeColor = System.Drawing.Color.LavenderBlush;
            this.txtTurnNumber.Location = new System.Drawing.Point(1203, 61);
            this.txtTurnNumber.Name = "txtTurnNumber";
            this.txtTurnNumber.ReadOnly = true;
            this.txtTurnNumber.Size = new System.Drawing.Size(82, 17);
            this.txtTurnNumber.TabIndex = 15;
            this.txtTurnNumber.Text = "0";
            // 
            // textBox8
            // 
            this.textBox8.BackColor = System.Drawing.Color.DimGray;
            this.textBox8.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox8.ForeColor = System.Drawing.Color.LavenderBlush;
            this.textBox8.Location = new System.Drawing.Point(1006, 61);
            this.textBox8.Name = "textBox8";
            this.textBox8.ReadOnly = true;
            this.textBox8.Size = new System.Drawing.Size(174, 17);
            this.textBox8.TabIndex = 14;
            this.textBox8.Text = "Turn Number:";
            this.textBox8.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Day15Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1350, 850);
            this.Controls.Add(this.txtTurnNumber);
            this.Controls.Add(this.textBox8);
            this.Controls.Add(this.txtGoblinsHP);
            this.Controls.Add(this.txtGoblinsRemaining);
            this.Controls.Add(this.txtElvesHP);
            this.Controls.Add(this.txtElvesRemaining);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.txtGameBoard);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.Color.Silver;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Day15Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form2";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnGo;
        public  System.Windows.Forms.RichTextBox txtGameBoard;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.TextBox txtElvesRemaining;
        private System.Windows.Forms.TextBox txtElvesHP;
        private System.Windows.Forms.TextBox txtGoblinsRemaining;
        private System.Windows.Forms.TextBox txtGoblinsHP;
        private System.Windows.Forms.TextBox txtTurnNumber;
        private System.Windows.Forms.TextBox textBox8;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button btnStep;
    }
}