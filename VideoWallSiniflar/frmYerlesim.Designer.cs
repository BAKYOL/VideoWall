namespace VideoWallSiniflar
{
    partial class frmYerlesim
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
            this.pnlIslemler = new System.Windows.Forms.Panel();
            this.pnlYerlesim = new System.Windows.Forms.Panel();
            this.pnlfrmTb = new System.Windows.Forms.Panel();
            this.pnlYerlesim.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlIslemler
            // 
            this.pnlIslemler.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlIslemler.Location = new System.Drawing.Point(0, 482);
            this.pnlIslemler.Name = "pnlIslemler";
            this.pnlIslemler.Size = new System.Drawing.Size(1013, 38);
            this.pnlIslemler.TabIndex = 0;
            // 
            // pnlYerlesim
            // 
            this.pnlYerlesim.Controls.Add(this.pnlfrmTb);
            this.pnlYerlesim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlYerlesim.Location = new System.Drawing.Point(0, 0);
            this.pnlYerlesim.Name = "pnlYerlesim";
            this.pnlYerlesim.Size = new System.Drawing.Size(1013, 482);
            this.pnlYerlesim.TabIndex = 1;
            // 
            // pnlfrmTb
            // 
            this.pnlfrmTb.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.pnlfrmTb.Location = new System.Drawing.Point(182, 254);
            this.pnlfrmTb.Name = "pnlfrmTb";
            this.pnlfrmTb.Size = new System.Drawing.Size(200, 100);
            this.pnlfrmTb.TabIndex = 0;
            this.pnlfrmTb.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlfrmTb_Paint);
            // 
            // frmYerlesim
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1013, 520);
            this.Controls.Add(this.pnlYerlesim);
            this.Controls.Add(this.pnlIslemler);
            this.Name = "frmYerlesim";
            this.Text = "frmYerlesim";
            this.Load += new System.EventHandler(this.frmYerlesim_Load);
            this.pnlYerlesim.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlIslemler;
        private System.Windows.Forms.Panel pnlYerlesim;
        private System.Windows.Forms.Panel pnlfrmTb;
    }
}