namespace AdventOfCode2018

{
    partial class Day13Form
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
            this.txtTrack = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStepNumber = new System.Windows.Forms.TextBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStepFwd = new System.Windows.Forms.Button();
            this.btnStepBack = new System.Windows.Forms.Button();
            this.sliderSpeed = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRails = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.sliderSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTrack
            // 
            this.txtTrack.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTrack.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.txtTrack.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTrack.ForeColor = System.Drawing.Color.Wheat;
            this.txtTrack.Location = new System.Drawing.Point(13, 57);
            this.txtTrack.MaxLength = 500000;
            this.txtTrack.Multiline = true;
            this.txtTrack.Name = "txtTrack";
            this.txtTrack.ReadOnly = true;
            this.txtTrack.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTrack.Size = new System.Drawing.Size(1431, 584);
            this.txtTrack.TabIndex = 0;
            this.txtTrack.WordWrap = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Step Number:";
            // 
            // txtStepNumber
            // 
            this.txtStepNumber.Location = new System.Drawing.Point(87, 10);
            this.txtStepNumber.Name = "txtStepNumber";
            this.txtStepNumber.ReadOnly = true;
            this.txtStepNumber.Size = new System.Drawing.Size(100, 20);
            this.txtStepNumber.TabIndex = 2;
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlay.Location = new System.Drawing.Point(266, 1);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(42, 37);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = ">";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnPause
            // 
            this.btnPause.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPause.Location = new System.Drawing.Point(314, 1);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(42, 37);
            this.btnPause.TabIndex = 4;
            this.btnPause.Text = "||";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnStepFwd
            // 
            this.btnStepFwd.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStepFwd.Location = new System.Drawing.Point(404, 1);
            this.btnStepFwd.Name = "btnStepFwd";
            this.btnStepFwd.Size = new System.Drawing.Size(42, 37);
            this.btnStepFwd.TabIndex = 5;
            this.btnStepFwd.Text = ">|";
            this.btnStepFwd.UseVisualStyleBackColor = true;
            this.btnStepFwd.Click += new System.EventHandler(this.btnStepFwd_Click);
            // 
            // btnStepBack
            // 
            this.btnStepBack.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStepBack.Location = new System.Drawing.Point(452, 1);
            this.btnStepBack.Name = "btnStepBack";
            this.btnStepBack.Size = new System.Drawing.Size(42, 37);
            this.btnStepBack.TabIndex = 6;
            this.btnStepBack.Text = "|<";
            this.btnStepBack.UseVisualStyleBackColor = true;
            this.btnStepBack.Click += new System.EventHandler(this.btnStepBack_Click);
            // 
            // sliderSpeed
            // 
            this.sliderSpeed.Location = new System.Drawing.Point(611, 6);
            this.sliderSpeed.Minimum = 1;
            this.sliderSpeed.Name = "sliderSpeed";
            this.sliderSpeed.Size = new System.Drawing.Size(229, 45);
            this.sliderSpeed.TabIndex = 7;
            this.sliderSpeed.Value = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(709, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Speed";
            // 
            // txtRails
            // 
            this.txtRails.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRails.BackColor = System.Drawing.Color.Black;
            this.txtRails.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRails.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.txtRails.Location = new System.Drawing.Point(13, 57);
            this.txtRails.Name = "txtRails";
            this.txtRails.ReadOnly = true;
            this.txtRails.Size = new System.Drawing.Size(1431, 584);
            this.txtRails.TabIndex = 9;
            this.txtRails.Text = "";
            // 
            // Day13Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1456, 653);
            this.Controls.Add(this.txtRails);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sliderSpeed);
            this.Controls.Add(this.btnStepBack);
            this.Controls.Add(this.btnStepFwd);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.txtStepNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtTrack);
            this.Name = "Day13Form";
            this.Text = "Day13Form";
            ((System.ComponentModel.ISupportInitialize)(this.sliderSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTrack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtStepNumber;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStepFwd;
        private System.Windows.Forms.Button btnStepBack;
        private System.Windows.Forms.TrackBar sliderSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RichTextBox txtRails;
    }
}