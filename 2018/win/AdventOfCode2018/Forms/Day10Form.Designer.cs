namespace AdventOfCode2018

{
    partial class Day10Form
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
            this.txtText = new System.Windows.Forms.TextBox();
            this.numUpdates = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numBy = new System.Windows.Forms.NumericUpDown();
            this.cbInvertXY = new System.Windows.Forms.CheckBox();
            this.btnGo = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numUpdates)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBy)).BeginInit();
            this.SuspendLayout();
            // 
            // txtText
            // 
            this.txtText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtText.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtText.Font = new System.Drawing.Font("Consolas", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtText.ForeColor = System.Drawing.Color.Gold;
            this.txtText.Location = new System.Drawing.Point(13, 38);
            this.txtText.MaxLength = 500000;
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.ReadOnly = true;
            this.txtText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtText.Size = new System.Drawing.Size(1431, 603);
            this.txtText.TabIndex = 0;
            this.txtText.WordWrap = false;
            // 
            // numUpdates
            // 
            this.numUpdates.Location = new System.Drawing.Point(117, 15);
            this.numUpdates.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.numUpdates.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpdates.Name = "numUpdates";
            this.numUpdates.Size = new System.Drawing.Size(278, 20);
            this.numUpdates.TabIndex = 1;
            this.numUpdates.Value = new decimal(new int[] {
            10500,
            0,
            0,
            0});
            this.numUpdates.ValueChanged += new System.EventHandler(this.numUpdates_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Number of Updates:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(419, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "By:";
            // 
            // numBy
            // 
            this.numBy.Location = new System.Drawing.Point(443, 15);
            this.numBy.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBy.Name = "numBy";
            this.numBy.Size = new System.Drawing.Size(96, 20);
            this.numBy.TabIndex = 3;
            this.numBy.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBy.ValueChanged += new System.EventHandler(this.numBy_ValueChanged);
            // 
            // cbInvertXY
            // 
            this.cbInvertXY.AutoSize = true;
            this.cbInvertXY.Location = new System.Drawing.Point(579, 18);
            this.cbInvertXY.Name = "cbInvertXY";
            this.cbInvertXY.Size = new System.Drawing.Size(67, 17);
            this.cbInvertXY.TabIndex = 5;
            this.cbInvertXY.Text = "InvertXY";
            this.cbInvertXY.UseVisualStyleBackColor = true;
            this.cbInvertXY.CheckedChanged += new System.EventHandler(this.cbInvertXY_CheckedChanged);
            // 
            // btnGo
            // 
            this.btnGo.Location = new System.Drawing.Point(711, 11);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(92, 23);
            this.btnGo.TabIndex = 6;
            this.btnGo.Text = "Show Me Stars";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // Day10Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1456, 653);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cbInvertXY);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.numBy);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numUpdates);
            this.Controls.Add(this.txtText);
            this.Name = "Day10Form";
            this.Text = "Day10Form";
            ((System.ComponentModel.ISupportInitialize)(this.numUpdates)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBy)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtText;
        private System.Windows.Forms.NumericUpDown numUpdates;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numBy;
        private System.Windows.Forms.CheckBox cbInvertXY;
        private System.Windows.Forms.Button btnGo;
    }
}