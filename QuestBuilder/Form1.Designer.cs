namespace QuestBuilder
{
    partial class Form1
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
            this.questIconBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.uriRewardBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.creRewardBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.honRewardBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.expRewardBox = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.manCount = new System.Windows.Forms.TextBox();
            this.orderCase = new System.Windows.Forms.CheckBox();
            this.mandCase = new System.Windows.Forms.CheckBox();
            this.activeCase = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.targetVal = new System.Windows.Forms.TextBox();
            this.mandCondition = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.matchesBox = new System.Windows.Forms.RichTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.conditionTypesBox = new System.Windows.Forms.ComboBox();
            this.questTree = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveBttn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.questsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.matchesHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generalHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.idLabel = new System.Windows.Forms.Label();
            this.questTypesBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // questIconBox
            // 
            this.questIconBox.FormattingEnabled = true;
            this.questIconBox.Location = new System.Drawing.Point(57, 98);
            this.questIconBox.Name = "questIconBox";
            this.questIconBox.Size = new System.Drawing.Size(217, 24);
            this.questIconBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(282, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Quest Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(134, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 17);
            this.label3.TabIndex = 5;
            this.label3.Text = "Quest Icon";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.panel1);
            this.flowLayoutPanel1.Controls.Add(this.panel2);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 138);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(776, 288);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.uriRewardBox);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.creRewardBox);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.honRewardBox);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.expRewardBox);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 100);
            this.panel1.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(360, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 17);
            this.label4.TabIndex = 1;
            this.label4.Text = "Rewards";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(578, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(56, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Uridium";
            // 
            // uriRewardBox
            // 
            this.uriRewardBox.Location = new System.Drawing.Point(536, 58);
            this.uriRewardBox.Name = "uriRewardBox";
            this.uriRewardBox.Size = new System.Drawing.Size(137, 22);
            this.uriRewardBox.TabIndex = 6;
            this.uriRewardBox.TextChanged += new System.EventHandler(this.uriRewardBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(439, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(52, 17);
            this.label7.TabIndex = 5;
            this.label7.Text = "Credits";
            // 
            // creRewardBox
            // 
            this.creRewardBox.Location = new System.Drawing.Point(393, 58);
            this.creRewardBox.Name = "creRewardBox";
            this.creRewardBox.Size = new System.Drawing.Size(137, 22);
            this.creRewardBox.TabIndex = 4;
            this.creRewardBox.TextChanged += new System.EventHandler(this.creRewardBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 38);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 17);
            this.label6.TabIndex = 3;
            this.label6.Text = "Honor";
            // 
            // honRewardBox
            // 
            this.honRewardBox.Location = new System.Drawing.Point(250, 58);
            this.honRewardBox.Name = "honRewardBox";
            this.honRewardBox.Size = new System.Drawing.Size(137, 22);
            this.honRewardBox.TabIndex = 2;
            this.honRewardBox.TextChanged += new System.EventHandler(this.honRewardBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(133, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 17);
            this.label5.TabIndex = 1;
            this.label5.Text = "Experience";
            // 
            // expRewardBox
            // 
            this.expRewardBox.Location = new System.Drawing.Point(107, 58);
            this.expRewardBox.Name = "expRewardBox";
            this.expRewardBox.Size = new System.Drawing.Size(137, 22);
            this.expRewardBox.TabIndex = 0;
            this.expRewardBox.TextChanged += new System.EventHandler(this.expRewardBox_TextChanged);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Controls.Add(this.questTree);
            this.panel2.Location = new System.Drawing.Point(3, 109);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(773, 175);
            this.panel2.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(539, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(234, 175);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.manCount);
            this.tabPage1.Controls.Add(this.orderCase);
            this.tabPage1.Controls.Add(this.mandCase);
            this.tabPage1.Controls.Add(this.activeCase);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(226, 146);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Case";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 88);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(116, 17);
            this.label10.TabIndex = 6;
            this.label10.Text = "Mandatory Count";
            // 
            // manCount
            // 
            this.manCount.Location = new System.Drawing.Point(136, 85);
            this.manCount.Name = "manCount";
            this.manCount.Size = new System.Drawing.Size(75, 22);
            this.manCount.TabIndex = 5;
            this.manCount.TextChanged += new System.EventHandler(this.manCount_TextChanged);
            // 
            // orderCase
            // 
            this.orderCase.AutoSize = true;
            this.orderCase.Location = new System.Drawing.Point(16, 62);
            this.orderCase.Name = "orderCase";
            this.orderCase.Size = new System.Drawing.Size(83, 21);
            this.orderCase.TabIndex = 4;
            this.orderCase.Text = "Ordered";
            this.orderCase.UseVisualStyleBackColor = true;
            this.orderCase.CheckedChanged += new System.EventHandler(this.orderCase_CheckedChanged);
            // 
            // mandCase
            // 
            this.mandCase.AutoSize = true;
            this.mandCase.Location = new System.Drawing.Point(16, 34);
            this.mandCase.Name = "mandCase";
            this.mandCase.Size = new System.Drawing.Size(97, 21);
            this.mandCase.TabIndex = 3;
            this.mandCase.Text = "Mandatory";
            this.mandCase.UseVisualStyleBackColor = true;
            this.mandCase.CheckedChanged += new System.EventHandler(this.mandCase_CheckedChanged);
            // 
            // activeCase
            // 
            this.activeCase.AutoSize = true;
            this.activeCase.Location = new System.Drawing.Point(16, 6);
            this.activeCase.Name = "activeCase";
            this.activeCase.Size = new System.Drawing.Size(68, 21);
            this.activeCase.TabIndex = 2;
            this.activeCase.Text = "Active";
            this.activeCase.UseVisualStyleBackColor = true;
            this.activeCase.CheckedChanged += new System.EventHandler(this.activeCase_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 24);
            this.button1.TabIndex = 1;
            this.button1.Text = "Create Quest Case";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.label13);
            this.tabPage2.Controls.Add(this.targetVal);
            this.tabPage2.Controls.Add(this.mandCondition);
            this.tabPage2.Controls.Add(this.label12);
            this.tabPage2.Controls.Add(this.matchesBox);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.conditionTypesBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(226, 146);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Condition";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(10, 111);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(97, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "Create Sub";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 82);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(86, 17);
            this.label13.TabIndex = 6;
            this.label13.Text = "TargetValue";
            // 
            // targetVal
            // 
            this.targetVal.Location = new System.Drawing.Point(110, 79);
            this.targetVal.Name = "targetVal";
            this.targetVal.Size = new System.Drawing.Size(100, 22);
            this.targetVal.TabIndex = 5;
            this.targetVal.TextChanged += new System.EventHandler(this.targetVal_TextChanged);
            // 
            // mandCondition
            // 
            this.mandCondition.AutoSize = true;
            this.mandCondition.Location = new System.Drawing.Point(113, 111);
            this.mandCondition.Name = "mandCondition";
            this.mandCondition.Size = new System.Drawing.Size(97, 21);
            this.mandCondition.TabIndex = 4;
            this.mandCondition.Text = "Mandatory";
            this.mandCondition.UseVisualStyleBackColor = true;
            this.mandCondition.CheckedChanged += new System.EventHandler(this.mandCondition_CheckedChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(2, 39);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 17);
            this.label12.TabIndex = 3;
            this.label12.Text = "Matches";
            // 
            // matchesBox
            // 
            this.matchesBox.Location = new System.Drawing.Point(66, 36);
            this.matchesBox.Name = "matchesBox";
            this.matchesBox.Size = new System.Drawing.Size(144, 37);
            this.matchesBox.TabIndex = 2;
            this.matchesBox.Text = "";
            this.matchesBox.TextChanged += new System.EventHandler(this.matchesBox_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 17);
            this.label11.TabIndex = 1;
            this.label11.Text = "Type";
            // 
            // conditionTypesBox
            // 
            this.conditionTypesBox.FormattingEnabled = true;
            this.conditionTypesBox.Location = new System.Drawing.Point(66, 6);
            this.conditionTypesBox.Name = "conditionTypesBox";
            this.conditionTypesBox.Size = new System.Drawing.Size(144, 24);
            this.conditionTypesBox.TabIndex = 0;
            this.conditionTypesBox.SelectedValueChanged += new System.EventHandler(this.conditionTypesBox_SelectedValueChanged);
            // 
            // questTree
            // 
            this.questTree.ContextMenuStrip = this.contextMenuStrip1;
            this.questTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.questTree.Location = new System.Drawing.Point(0, 0);
            this.questTree.Name = "questTree";
            this.questTree.Size = new System.Drawing.Size(539, 175);
            this.questTree.TabIndex = 0;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2,
            this.editToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(199, 76);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(198, 24);
            this.toolStripMenuItem1.Text = "Add New Element";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(198, 24);
            this.toolStripMenuItem2.Text = "Remove";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(198, 24);
            this.editToolStripMenuItem.Text = "Edit";
            this.editToolStripMenuItem.Click += new System.EventHandler(this.editToolStripMenuItem_Click);
            // 
            // saveBttn
            // 
            this.saveBttn.Location = new System.Drawing.Point(327, 467);
            this.saveBttn.Name = "saveBttn";
            this.saveBttn.Size = new System.Drawing.Size(149, 36);
            this.saveBttn.TabIndex = 7;
            this.saveBttn.Text = "Save";
            this.saveBttn.UseVisualStyleBackColor = true;
            this.saveBttn.Click += new System.EventHandler(this.saveBttn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(362, 51);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Quest ID:";
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.questsToolStripMenuItem,
            this.matchesHelpToolStripMenuItem,
            this.generalHelpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 28);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // questsToolStripMenuItem
            // 
            this.questsToolStripMenuItem.Name = "questsToolStripMenuItem";
            this.questsToolStripMenuItem.Size = new System.Drawing.Size(65, 24);
            this.questsToolStripMenuItem.Text = "Quests";
            this.questsToolStripMenuItem.Click += new System.EventHandler(this.questsToolStripMenuItem_Click);
            // 
            // matchesHelpToolStripMenuItem
            // 
            this.matchesHelpToolStripMenuItem.Name = "matchesHelpToolStripMenuItem";
            this.matchesHelpToolStripMenuItem.Size = new System.Drawing.Size(112, 24);
            this.matchesHelpToolStripMenuItem.Text = "Matches Help";
            // 
            // generalHelpToolStripMenuItem
            // 
            this.generalHelpToolStripMenuItem.Name = "generalHelpToolStripMenuItem";
            this.generalHelpToolStripMenuItem.Size = new System.Drawing.Size(108, 24);
            this.generalHelpToolStripMenuItem.Text = "General Help";
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(435, 51);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(16, 17);
            this.idLabel.TabIndex = 13;
            this.idLabel.Text = "1";
            // 
            // questTypesBox
            // 
            this.questTypesBox.FormattingEnabled = true;
            this.questTypesBox.Location = new System.Drawing.Point(292, 98);
            this.questTypesBox.Name = "questTypesBox";
            this.questTypesBox.Size = new System.Drawing.Size(214, 24);
            this.questTypesBox.TabIndex = 14;
            this.questTypesBox.SelectedIndexChanged += new System.EventHandler(this.questTypesBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(362, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 17);
            this.label2.TabIndex = 15;
            this.label2.Text = "Quest Types";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 534);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.questTypesBox);
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.saveBttn);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.questIconBox);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox questIconBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox uriRewardBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox creRewardBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox honRewardBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox expRewardBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox manCount;
        private System.Windows.Forms.CheckBox orderCase;
        private System.Windows.Forms.CheckBox mandCase;
        private System.Windows.Forms.CheckBox activeCase;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox targetVal;
        private System.Windows.Forms.CheckBox mandCondition;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RichTextBox matchesBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox conditionTypesBox;
        private System.Windows.Forms.TreeView questTree;
        private System.Windows.Forms.Button saveBttn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem questsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem matchesHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generalHelpToolStripMenuItem;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ComboBox questTypesBox;
        private System.Windows.Forms.Label label2;
    }
}

