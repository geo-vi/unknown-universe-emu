namespace NettyBaseReloaded
{
    partial class Controller
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Controller));
            this.label1 = new System.Windows.Forms.Label();
            this.administratePlayers = new System.Windows.Forms.Button();
            this.serverEditor = new System.Windows.Forms.Button();
            this.mapEditor = new System.Windows.Forms.Button();
            this.errors = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.timeRunning = new System.Windows.Forms.Label();
            this.ticker = new System.Windows.Forms.Timer(this.components);
            this.onlinePlayers = new System.Windows.Forms.Label();
            this.errorCounter = new System.Windows.Forms.Label();
            this.consoleBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.consoleBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server status";
            // 
            // administratePlayers
            // 
            this.administratePlayers.Location = new System.Drawing.Point(12, 256);
            this.administratePlayers.Name = "administratePlayers";
            this.administratePlayers.Size = new System.Drawing.Size(112, 33);
            this.administratePlayers.TabIndex = 1;
            this.administratePlayers.Text = "Administrate Players";
            this.administratePlayers.UseVisualStyleBackColor = true;
            this.administratePlayers.Click += new System.EventHandler(this.button1_Click);
            // 
            // serverEditor
            // 
            this.serverEditor.Location = new System.Drawing.Point(130, 256);
            this.serverEditor.Name = "serverEditor";
            this.serverEditor.Size = new System.Drawing.Size(112, 33);
            this.serverEditor.TabIndex = 2;
            this.serverEditor.Text = "Server Editor";
            this.serverEditor.UseVisualStyleBackColor = true;
            this.serverEditor.Click += new System.EventHandler(this.serverEditor_Click);
            // 
            // mapEditor
            // 
            this.mapEditor.Location = new System.Drawing.Point(248, 256);
            this.mapEditor.Name = "mapEditor";
            this.mapEditor.Size = new System.Drawing.Size(112, 33);
            this.mapEditor.TabIndex = 3;
            this.mapEditor.Text = "Map Editor";
            this.mapEditor.UseVisualStyleBackColor = true;
            this.mapEditor.Click += new System.EventHandler(this.button3_Click);
            // 
            // errors
            // 
            this.errors.Location = new System.Drawing.Point(366, 256);
            this.errors.Name = "errors";
            this.errors.Size = new System.Drawing.Size(112, 33);
            this.errors.TabIndex = 4;
            this.errors.Text = "Errors";
            this.errors.UseVisualStyleBackColor = true;
            this.errors.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 168);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Online Players";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(87, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Errors";
            // 
            // timeRunning
            // 
            this.timeRunning.AutoSize = true;
            this.timeRunning.Font = new System.Drawing.Font("Bebas Neue Bold", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeRunning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.timeRunning.Location = new System.Drawing.Point(174, 136);
            this.timeRunning.Name = "timeRunning";
            this.timeRunning.Size = new System.Drawing.Size(244, 77);
            this.timeRunning.TabIndex = 8;
            this.timeRunning.Text = "0.00:00:00";
            // 
            // ticker
            // 
            this.ticker.Tick += new System.EventHandler(this.ticker_Tick);
            // 
            // onlinePlayers
            // 
            this.onlinePlayers.AutoSize = true;
            this.onlinePlayers.BackColor = System.Drawing.Color.Transparent;
            this.onlinePlayers.Location = new System.Drawing.Point(52, 168);
            this.onlinePlayers.Name = "onlinePlayers";
            this.onlinePlayers.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.onlinePlayers.Size = new System.Drawing.Size(13, 13);
            this.onlinePlayers.TabIndex = 10;
            this.onlinePlayers.Text = "0";
            // 
            // errorCounter
            // 
            this.errorCounter.AutoSize = true;
            this.errorCounter.Location = new System.Drawing.Point(52, 192);
            this.errorCounter.Name = "errorCounter";
            this.errorCounter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.errorCounter.Size = new System.Drawing.Size(13, 13);
            this.errorCounter.TabIndex = 11;
            this.errorCounter.Text = "0";
            this.errorCounter.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // consoleBox
            // 
            this.consoleBox.Image = global::NettyBaseReloaded.Properties.Resources.univ3rse_logo;
            this.consoleBox.Location = new System.Drawing.Point(52, 12);
            this.consoleBox.Name = "consoleBox";
            this.consoleBox.Size = new System.Drawing.Size(366, 121);
            this.consoleBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.consoleBox.TabIndex = 9;
            this.consoleBox.TabStop = false;
            this.consoleBox.WaitOnLoad = true;
            this.consoleBox.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintConsole);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(53, 145);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(10, 10);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // Controller
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(488, 301);
            this.Controls.Add(this.errorCounter);
            this.Controls.Add(this.onlinePlayers);
            this.Controls.Add(this.consoleBox);
            this.Controls.Add(this.timeRunning);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.errors);
            this.Controls.Add(this.mapEditor);
            this.Controls.Add(this.serverEditor);
            this.Controls.Add(this.administratePlayers);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Controller";
            this.Text = "Controller";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Controller_FormClosed);
            this.Load += new System.EventHandler(this.Controller_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Controller_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Controller_KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.consoleBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button administratePlayers;
        private System.Windows.Forms.Button serverEditor;
        private System.Windows.Forms.Button mapEditor;
        private System.Windows.Forms.Button errors;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label timeRunning;
        private System.Windows.Forms.PictureBox consoleBox;
        private System.Windows.Forms.Timer ticker;
        private System.Windows.Forms.Label onlinePlayers;
        private System.Windows.Forms.Label errorCounter;
    }
}