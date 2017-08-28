namespace NettyBaseReloadedBrowser
{
    partial class Esc
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
            this.exitBox = new System.Windows.Forms.Panel();
            this.minimizeBox = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.exitBox.SuspendLayout();
            this.minimizeBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // exitBox
            // 
            this.exitBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.exitBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.exitBox.Controls.Add(this.label1);
            this.exitBox.Location = new System.Drawing.Point(12, 58);
            this.exitBox.Name = "exitBox";
            this.exitBox.Size = new System.Drawing.Size(151, 40);
            this.exitBox.TabIndex = 0;
            this.exitBox.Click += new System.EventHandler(this.exit_Click);
            // 
            // minimizeBox
            // 
            this.minimizeBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.minimizeBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.minimizeBox.Controls.Add(this.label2);
            this.minimizeBox.Location = new System.Drawing.Point(12, 12);
            this.minimizeBox.Name = "minimizeBox";
            this.minimizeBox.Size = new System.Drawing.Size(151, 40);
            this.minimizeBox.TabIndex = 1;
            this.minimizeBox.Click += new System.EventHandler(this.minimize_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Bebas Neue", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(57, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Exit";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.exit_Click);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Bebas Neue", 12.3F);
            this.label2.ForeColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(46, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "Minimize";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.minimize_Click);
            // 
            // Esc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1123, 571);
            this.Controls.Add(this.minimizeBox);
            this.Controls.Add(this.exitBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Esc";
            this.Text = "Esc";
            this.Click += new System.EventHandler(this.Esc_Click);
            this.exitBox.ResumeLayout(false);
            this.exitBox.PerformLayout();
            this.minimizeBox.ResumeLayout(false);
            this.minimizeBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel exitBox;
        private System.Windows.Forms.Panel minimizeBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}