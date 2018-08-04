namespace NettyBaseReloaded
{
    partial class ServerEditor
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
            this.performanceRate = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.rewardMultiplyer = new System.Windows.Forms.TrackBar();
            this.debugCommands = new System.Windows.Forms.CheckBox();
            this.debugLegacyCommands = new System.Windows.Forms.CheckBox();
            this.debugCheckbox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.performanceRate)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rewardMultiplyer)).BeginInit();
            this.SuspendLayout();
            // 
            // performanceRate
            // 
            this.performanceRate.Enabled = false;
            this.performanceRate.LargeChange = 3;
            this.performanceRate.Location = new System.Drawing.Point(13, 25);
            this.performanceRate.Maximum = 5;
            this.performanceRate.Name = "performanceRate";
            this.performanceRate.Size = new System.Drawing.Size(208, 45);
            this.performanceRate.TabIndex = 0;
            this.performanceRate.TickFrequency = 2;
            this.performanceRate.Value = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Server performance";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.performanceRate);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(238, 93);
            this.panel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(186, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 21);
            this.button1.TabIndex = 2;
            this.button1.Text = "Switch to Console mode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.rewardMultiplyer);
            this.panel2.Location = new System.Drawing.Point(256, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(238, 93);
            this.panel2.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Reward multiplyer";
            // 
            // rewardMultiplyer
            // 
            this.rewardMultiplyer.LargeChange = 3;
            this.rewardMultiplyer.Location = new System.Drawing.Point(14, 25);
            this.rewardMultiplyer.Maximum = 5;
            this.rewardMultiplyer.Minimum = 1;
            this.rewardMultiplyer.Name = "rewardMultiplyer";
            this.rewardMultiplyer.Size = new System.Drawing.Size(208, 45);
            this.rewardMultiplyer.TabIndex = 0;
            this.rewardMultiplyer.TickFrequency = 2;
            this.rewardMultiplyer.Value = 5;
            this.rewardMultiplyer.Scroll += new System.EventHandler(this.rewardMultiplyer_Scroll);
            // 
            // debugCommands
            // 
            this.debugCommands.AutoSize = true;
            this.debugCommands.Location = new System.Drawing.Point(295, 119);
            this.debugCommands.Name = "debugCommands";
            this.debugCommands.Size = new System.Drawing.Size(107, 17);
            this.debugCommands.TabIndex = 5;
            this.debugCommands.Text = "Show commands";
            this.debugCommands.UseVisualStyleBackColor = true;
            this.debugCommands.CheckedChanged += new System.EventHandler(this.debugCommands_CheckedChanged);
            // 
            // debugLegacyCommands
            // 
            this.debugLegacyCommands.AutoSize = true;
            this.debugLegacyCommands.Location = new System.Drawing.Point(148, 120);
            this.debugLegacyCommands.Name = "debugLegacyCommands";
            this.debugLegacyCommands.Size = new System.Drawing.Size(141, 17);
            this.debugLegacyCommands.TabIndex = 6;
            this.debugLegacyCommands.Text = "Show legacy commands";
            this.debugLegacyCommands.UseVisualStyleBackColor = true;
            this.debugLegacyCommands.CheckedChanged += new System.EventHandler(this.debugPackets_CheckedChanged);
            // 
            // debugCheckbox
            // 
            this.debugCheckbox.AutoSize = true;
            this.debugCheckbox.Location = new System.Drawing.Point(408, 120);
            this.debugCheckbox.Name = "debugCheckbox";
            this.debugCheckbox.Size = new System.Drawing.Size(70, 17);
            this.debugCheckbox.TabIndex = 7;
            this.debugCheckbox.Text = "!DEBUG!";
            this.debugCheckbox.UseVisualStyleBackColor = true;
            this.debugCheckbox.CheckedChanged += new System.EventHandler(this.debugCheckbox_CheckedChanged);
            // 
            // ServerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(505, 203);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.debugCheckbox);
            this.Controls.Add(this.debugLegacyCommands);
            this.Controls.Add(this.debugCommands);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ServerEditor";
            this.Text = "Server Editor";
            ((System.ComponentModel.ISupportInitialize)(this.performanceRate)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rewardMultiplyer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar performanceRate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar rewardMultiplyer;
        private System.Windows.Forms.CheckBox debugCommands;
        private System.Windows.Forms.CheckBox debugLegacyCommands;
        private System.Windows.Forms.CheckBox debugCheckbox;
        private System.Windows.Forms.Button button1;
    }
}