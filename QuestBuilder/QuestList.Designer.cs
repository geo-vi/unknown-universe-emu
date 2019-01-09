namespace QuestBuilder
{
    partial class QuestList
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
            this.questView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.questView)).BeginInit();
            this.SuspendLayout();
            // 
            // questView
            // 
            this.questView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.questView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.questView.Location = new System.Drawing.Point(0, 0);
            this.questView.Name = "questView";
            this.questView.RowTemplate.Height = 24;
            this.questView.Size = new System.Drawing.Size(800, 450);
            this.questView.TabIndex = 0;
            // 
            // QuestList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.questView);
            this.Name = "QuestList";
            this.Text = "QuestList";
            this.Load += new System.EventHandler(this.QuestList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.questView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView questView;
    }
}