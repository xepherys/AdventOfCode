namespace AdventOfCode2018
{
    partial class Day14Form
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGo = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStartScores = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.numInputValue = new System.Windows.Forms.NumericUpDown();
            this.txtScores = new System.Windows.Forms.TextBox();
            this.cbB = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInputValue)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbB);
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtStartScores);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.numInputValue);
            this.panel1.Location = new System.Drawing.Point(12, 588);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1038, 56);
            this.panel1.TabIndex = 0;
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(947, 23);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(75, 23);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(199, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Starting Scores:";
            // 
            // txtStartScores
            // 
            this.txtStartScores.Location = new System.Drawing.Point(287, 25);
            this.txtStartScores.Name = "txtStartScores";
            this.txtStartScores.Size = new System.Drawing.Size(100, 20);
            this.txtStartScores.TabIndex = 2;
            this.txtStartScores.Text = "3, 7";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Input Value:";
            // 
            // numInputValue
            // 
            this.numInputValue.Location = new System.Drawing.Point(73, 25);
            this.numInputValue.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.numInputValue.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInputValue.Name = "numInputValue";
            this.numInputValue.Size = new System.Drawing.Size(120, 20);
            this.numInputValue.TabIndex = 0;
            this.numInputValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numInputValue.Value = new decimal(new int[] {
            580741,
            0,
            0,
            0});
            // 
            // txtScores
            // 
            this.txtScores.BackColor = System.Drawing.Color.Black;
            this.txtScores.ForeColor = System.Drawing.Color.LightGray;
            this.txtScores.Location = new System.Drawing.Point(12, 13);
            this.txtScores.MaxLength = 99999999;
            this.txtScores.Multiline = true;
            this.txtScores.Name = "txtScores";
            this.txtScores.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtScores.Size = new System.Drawing.Size(1038, 569);
            this.txtScores.TabIndex = 1;
            this.txtScores.WordWrap = false;
            // 
            // cbB
            // 
            this.cbB.AutoSize = true;
            this.cbB.Location = new System.Drawing.Point(595, 23);
            this.cbB.Name = "cbB";
            this.cbB.Size = new System.Drawing.Size(69, 17);
            this.cbB.TabIndex = 5;
            this.cbB.Text = "Day14b?";
            this.cbB.UseVisualStyleBackColor = true;
            // 
            // Day14Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1062, 656);
            this.Controls.Add(this.txtScores);
            this.Controls.Add(this.panel1);
            this.Name = "Day14Form";
            this.Text = "Day14Form";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numInputValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStartScores;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numInputValue;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.TextBox txtScores;
        private System.Windows.Forms.CheckBox cbB;
    }
}