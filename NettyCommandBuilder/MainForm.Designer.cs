namespace NettyCommandBuilder
{
    partial class MainForm
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
            this.OutputBox = new System.Windows.Forms.RichTextBox();
            this.ByteOutputBox = new System.Windows.Forms.RichTextBox();
            this.varValue = new System.Windows.Forms.TextBox();
            this.addInfoButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.varName = new System.Windows.Forms.TextBox();
            this.typeOfVariable = new System.Windows.Forms.ComboBox();
            this.resetButton = new System.Windows.Forms.Button();
            this.currentOutput = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // OutputBox
            // 
            this.OutputBox.Enabled = false;
            this.OutputBox.Location = new System.Drawing.Point(13, 234);
            this.OutputBox.Name = "OutputBox";
            this.OutputBox.Size = new System.Drawing.Size(450, 96);
            this.OutputBox.TabIndex = 0;
            this.OutputBox.Text = "";
            // 
            // ByteOutputBox
            // 
            this.ByteOutputBox.Enabled = false;
            this.ByteOutputBox.Location = new System.Drawing.Point(13, 209);
            this.ByteOutputBox.Name = "ByteOutputBox";
            this.ByteOutputBox.Size = new System.Drawing.Size(375, 19);
            this.ByteOutputBox.TabIndex = 1;
            this.ByteOutputBox.Text = "";
            // 
            // varValue
            // 
            this.varValue.Location = new System.Drawing.Point(277, 13);
            this.varValue.Name = "varValue";
            this.varValue.Size = new System.Drawing.Size(100, 20);
            this.varValue.TabIndex = 2;
            // 
            // addInfoButton
            // 
            this.addInfoButton.Location = new System.Drawing.Point(383, 13);
            this.addInfoButton.Name = "addInfoButton";
            this.addInfoButton.Size = new System.Drawing.Size(75, 21);
            this.addInfoButton.TabIndex = 3;
            this.addInfoButton.Text = "Add";
            this.addInfoButton.UseVisualStyleBackColor = true;
            this.addInfoButton.Click += new System.EventHandler(this.addInfoButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(15, 167);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(223, 36);
            this.generateButton.TabIndex = 4;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            // 
            // varName
            // 
            this.varName.Location = new System.Drawing.Point(171, 13);
            this.varName.Name = "varName";
            this.varName.Size = new System.Drawing.Size(100, 20);
            this.varName.TabIndex = 6;
            // 
            // typeOfVariable
            // 
            this.typeOfVariable.FormattingEnabled = true;
            this.typeOfVariable.Items.AddRange(new object[] {
            "CMD ID",
            "int",
            "short",
            "float",
            "double",
            "utf",
            "bytes",
            "Output type"});
            this.typeOfVariable.Location = new System.Drawing.Point(15, 13);
            this.typeOfVariable.Name = "typeOfVariable";
            this.typeOfVariable.Size = new System.Drawing.Size(150, 21);
            this.typeOfVariable.TabIndex = 7;
            // 
            // resetButton
            // 
            this.resetButton.Location = new System.Drawing.Point(244, 167);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(219, 36);
            this.resetButton.TabIndex = 8;
            this.resetButton.Text = "Reset";
            this.resetButton.UseVisualStyleBackColor = true;
            // 
            // currentOutput
            // 
            this.currentOutput.Location = new System.Drawing.Point(15, 51);
            this.currentOutput.Name = "currentOutput";
            this.currentOutput.Size = new System.Drawing.Size(448, 110);
            this.currentOutput.TabIndex = 9;
            this.currentOutput.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(395, 209);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(68, 19);
            this.button1.TabIndex = 10;
            this.button1.Text = "Copy";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 342);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.currentOutput);
            this.Controls.Add(this.resetButton);
            this.Controls.Add(this.typeOfVariable);
            this.Controls.Add(this.varName);
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.addInfoButton);
            this.Controls.Add(this.varValue);
            this.Controls.Add(this.ByteOutputBox);
            this.Controls.Add(this.OutputBox);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox OutputBox;
        private System.Windows.Forms.RichTextBox ByteOutputBox;
        private System.Windows.Forms.TextBox varValue;
        private System.Windows.Forms.Button addInfoButton;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.TextBox varName;
        private System.Windows.Forms.ComboBox typeOfVariable;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.RichTextBox currentOutput;
        private System.Windows.Forms.Button button1;
    }
}

