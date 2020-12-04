namespace AdventOfCode2018
{
    partial class GridDisplayDay3a
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
            this.txtDay3aGrid = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtDay3aGrid
            // 
            this.txtDay3aGrid.Font = new System.Drawing.Font("Hack", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDay3aGrid.Location = new System.Drawing.Point(13, 13);
            this.txtDay3aGrid.MaximumSize = new System.Drawing.Size(946, 625);
            this.txtDay3aGrid.MaxLength = 2147483647;
            this.txtDay3aGrid.MinimumSize = new System.Drawing.Size(946, 625);
            this.txtDay3aGrid.Multiline = true;
            this.txtDay3aGrid.Name = "txtDay3aGrid";
            this.txtDay3aGrid.ReadOnly = true;
            this.txtDay3aGrid.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDay3aGrid.Size = new System.Drawing.Size(946, 625);
            this.txtDay3aGrid.TabIndex = 0;
            // 
            // GridDisplayDay3a
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 650);
            this.Controls.Add(this.txtDay3aGrid);
            this.Name = "GridDisplayDay3a";
            this.Text = "GridDisplayDay3a";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDay3aGrid;
    }
}