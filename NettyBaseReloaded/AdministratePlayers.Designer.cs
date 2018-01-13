namespace NettyBaseReloaded
{
    partial class AdministratePlayers
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
            this.tabController = new System.Windows.Forms.TabControl();
            this.playersView = new System.Windows.Forms.TabPage();
            this.players = new System.Windows.Forms.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.rankColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.shipColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hpColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.nhColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.shieldColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.configsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.expColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.honColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.creditColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.uriColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.playerSelected = new System.Windows.Forms.TabPage();
            this.closeTab = new System.Windows.Forms.Button();
            this.playerState = new System.Windows.Forms.Label();
            this.clan = new System.Windows.Forms.Label();
            this.groupLabel = new System.Windows.Forms.Label();
            this.attackers = new System.Windows.Forms.Label();
            this.lastCombat = new System.Windows.Forms.Label();
            this.uriLabel = new System.Windows.Forms.Label();
            this.creLabel = new System.Windows.Forms.Label();
            this.honLabel = new System.Windows.Forms.Label();
            this.expLabel = new System.Windows.Forms.Label();
            this.pos = new System.Windows.Forms.Label();
            this.mapName = new System.Windows.Forms.Label();
            this.shdProgress = new System.Windows.Forms.ProgressBar();
            this.nanoProgress = new System.Windows.Forms.ProgressBar();
            this.hpProgress = new System.Windows.Forms.ProgressBar();
            this.playerTicker = new System.Windows.Forms.Timer(this.components);
            this.tabController.SuspendLayout();
            this.playersView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.players)).BeginInit();
            this.panel1.SuspendLayout();
            this.playerSelected.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabController
            // 
            this.tabController.Controls.Add(this.playersView);
            this.tabController.Controls.Add(this.playerSelected);
            this.tabController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabController.Location = new System.Drawing.Point(0, 0);
            this.tabController.Name = "tabController";
            this.tabController.SelectedIndex = 0;
            this.tabController.Size = new System.Drawing.Size(617, 379);
            this.tabController.TabIndex = 0;
            // 
            // playersView
            // 
            this.playersView.Controls.Add(this.players);
            this.playersView.Controls.Add(this.panel1);
            this.playersView.Location = new System.Drawing.Point(4, 22);
            this.playersView.Name = "playersView";
            this.playersView.Padding = new System.Windows.Forms.Padding(3);
            this.playersView.Size = new System.Drawing.Size(609, 353);
            this.playersView.TabIndex = 0;
            this.playersView.Text = "Players";
            this.playersView.UseVisualStyleBackColor = true;
            // 
            // players
            // 
            this.players.AllowUserToAddRows = false;
            this.players.AllowUserToDeleteRows = false;
            this.players.BackgroundColor = System.Drawing.Color.White;
            this.players.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.players.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.nameColumn,
            this.rankColumn,
            this.shipColumn,
            this.hpColumn,
            this.nhColumn,
            this.shieldColumn,
            this.configsColumn,
            this.expColumn,
            this.honColumn,
            this.creditColumn,
            this.uriColumn});
            this.players.Dock = System.Windows.Forms.DockStyle.Fill;
            this.players.GridColor = System.Drawing.Color.White;
            this.players.Location = new System.Drawing.Point(3, 32);
            this.players.Name = "players";
            this.players.Size = new System.Drawing.Size(603, 318);
            this.players.TabIndex = 0;
            this.players.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.players_CellClick);
            // 
            // idColumn
            // 
            this.idColumn.Frozen = true;
            this.idColumn.HeaderText = "Id";
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.Frozen = true;
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            // 
            // rankColumn
            // 
            this.rankColumn.HeaderText = "Rank";
            this.rankColumn.Name = "rankColumn";
            this.rankColumn.ReadOnly = true;
            // 
            // shipColumn
            // 
            this.shipColumn.HeaderText = "Ship";
            this.shipColumn.Name = "shipColumn";
            this.shipColumn.ReadOnly = true;
            // 
            // hpColumn
            // 
            this.hpColumn.HeaderText = "Health";
            this.hpColumn.Name = "hpColumn";
            this.hpColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // nhColumn
            // 
            this.nhColumn.HeaderText = "Nanohull";
            this.nhColumn.Name = "nhColumn";
            this.nhColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // shieldColumn
            // 
            this.shieldColumn.HeaderText = "Shield";
            this.shieldColumn.Name = "shieldColumn";
            this.shieldColumn.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // configsColumn
            // 
            this.configsColumn.HeaderText = "Configs";
            this.configsColumn.Name = "configsColumn";
            // 
            // expColumn
            // 
            this.expColumn.HeaderText = "Experience";
            this.expColumn.Name = "expColumn";
            this.expColumn.ReadOnly = true;
            // 
            // honColumn
            // 
            this.honColumn.HeaderText = "Honor";
            this.honColumn.Name = "honColumn";
            this.honColumn.ReadOnly = true;
            // 
            // creditColumn
            // 
            this.creditColumn.HeaderText = "Credits";
            this.creditColumn.Name = "creditColumn";
            this.creditColumn.ReadOnly = true;
            // 
            // uriColumn
            // 
            this.uriColumn.HeaderText = "Uridium";
            this.uriColumn.Name = "uriColumn";
            this.uriColumn.ReadOnly = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(603, 29);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(555, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "{x} Hits";
            this.label1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(185, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Search";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(173, 20);
            this.textBox1.TabIndex = 0;
            // 
            // playerSelected
            // 
            this.playerSelected.Controls.Add(this.closeTab);
            this.playerSelected.Controls.Add(this.playerState);
            this.playerSelected.Controls.Add(this.clan);
            this.playerSelected.Controls.Add(this.groupLabel);
            this.playerSelected.Controls.Add(this.attackers);
            this.playerSelected.Controls.Add(this.lastCombat);
            this.playerSelected.Controls.Add(this.uriLabel);
            this.playerSelected.Controls.Add(this.creLabel);
            this.playerSelected.Controls.Add(this.honLabel);
            this.playerSelected.Controls.Add(this.expLabel);
            this.playerSelected.Controls.Add(this.pos);
            this.playerSelected.Controls.Add(this.mapName);
            this.playerSelected.Controls.Add(this.shdProgress);
            this.playerSelected.Controls.Add(this.nanoProgress);
            this.playerSelected.Controls.Add(this.hpProgress);
            this.playerSelected.Location = new System.Drawing.Point(4, 22);
            this.playerSelected.Name = "playerSelected";
            this.playerSelected.Padding = new System.Windows.Forms.Padding(3);
            this.playerSelected.Size = new System.Drawing.Size(609, 353);
            this.playerSelected.TabIndex = 1;
            this.playerSelected.Text = "DEFAULT";
            this.playerSelected.UseVisualStyleBackColor = true;
            // 
            // closeTab
            // 
            this.closeTab.Location = new System.Drawing.Point(275, 282);
            this.closeTab.Name = "closeTab";
            this.closeTab.Size = new System.Drawing.Size(75, 23);
            this.closeTab.TabIndex = 15;
            this.closeTab.Text = "Close";
            this.closeTab.UseVisualStyleBackColor = true;
            this.closeTab.Click += new System.EventHandler(this.closeTab_Click);
            // 
            // playerState
            // 
            this.playerState.AutoSize = true;
            this.playerState.Location = new System.Drawing.Point(440, 13);
            this.playerState.Name = "playerState";
            this.playerState.Size = new System.Drawing.Size(37, 13);
            this.playerState.TabIndex = 14;
            this.playerState.Text = "ALIVE";
            // 
            // clan
            // 
            this.clan.AutoSize = true;
            this.clan.Location = new System.Drawing.Point(17, 107);
            this.clan.Name = "clan";
            this.clan.Size = new System.Drawing.Size(42, 13);
            this.clan.TabIndex = 13;
            this.clan.Text = "Clan: {}";
            // 
            // groupLabel
            // 
            this.groupLabel.AutoSize = true;
            this.groupLabel.Location = new System.Drawing.Point(17, 93);
            this.groupLabel.Name = "groupLabel";
            this.groupLabel.Size = new System.Drawing.Size(50, 13);
            this.groupLabel.TabIndex = 12;
            this.groupLabel.Text = "Group: {}";
            // 
            // attackers
            // 
            this.attackers.AutoSize = true;
            this.attackers.Location = new System.Drawing.Point(17, 80);
            this.attackers.Name = "attackers";
            this.attackers.Size = new System.Drawing.Size(66, 13);
            this.attackers.TabIndex = 11;
            this.attackers.Text = "Attackers: {}";
            // 
            // lastCombat
            // 
            this.lastCombat.AutoSize = true;
            this.lastCombat.Location = new System.Drawing.Point(17, 67);
            this.lastCombat.Name = "lastCombat";
            this.lastCombat.Size = new System.Drawing.Size(138, 13);
            this.lastCombat.TabIndex = 10;
            this.lastCombat.Text = "Last Combat Time: 2:39 AM";
            // 
            // uriLabel
            // 
            this.uriLabel.AutoSize = true;
            this.uriLabel.Location = new System.Drawing.Point(139, 29);
            this.uriLabel.Name = "uriLabel";
            this.uriLabel.Size = new System.Drawing.Size(54, 13);
            this.uriLabel.TabIndex = 9;
            this.uriLabel.Text = "Uridium: 0";
            // 
            // creLabel
            // 
            this.creLabel.AutoSize = true;
            this.creLabel.Location = new System.Drawing.Point(139, 13);
            this.creLabel.Name = "creLabel";
            this.creLabel.Size = new System.Drawing.Size(51, 13);
            this.creLabel.TabIndex = 8;
            this.creLabel.Text = "Credits: 0";
            // 
            // honLabel
            // 
            this.honLabel.AutoSize = true;
            this.honLabel.Location = new System.Drawing.Point(17, 29);
            this.honLabel.Name = "honLabel";
            this.honLabel.Size = new System.Drawing.Size(72, 13);
            this.honLabel.TabIndex = 7;
            this.honLabel.Text = "Honor: 10000";
            // 
            // expLabel
            // 
            this.expLabel.AutoSize = true;
            this.expLabel.Location = new System.Drawing.Point(17, 13);
            this.expLabel.Name = "expLabel";
            this.expLabel.Size = new System.Drawing.Size(61, 13);
            this.expLabel.TabIndex = 6;
            this.expLabel.Text = "Exp: 10000";
            // 
            // pos
            // 
            this.pos.AutoSize = true;
            this.pos.Location = new System.Drawing.Point(489, 93);
            this.pos.Name = "pos";
            this.pos.Size = new System.Drawing.Size(30, 13);
            this.pos.TabIndex = 5;
            this.pos.Text = "{0,0}";
            // 
            // mapName
            // 
            this.mapName.AutoSize = true;
            this.mapName.Location = new System.Drawing.Point(489, 67);
            this.mapName.Name = "mapName";
            this.mapName.Size = new System.Drawing.Size(22, 13);
            this.mapName.TabIndex = 4;
            this.mapName.Text = "1-1";
            // 
            // shdProgress
            // 
            this.shdProgress.Location = new System.Drawing.Point(492, 45);
            this.shdProgress.Name = "shdProgress";
            this.shdProgress.Size = new System.Drawing.Size(100, 10);
            this.shdProgress.TabIndex = 3;
            this.shdProgress.Value = 25;
            // 
            // nanoProgress
            // 
            this.nanoProgress.Location = new System.Drawing.Point(492, 29);
            this.nanoProgress.Name = "nanoProgress";
            this.nanoProgress.Size = new System.Drawing.Size(100, 10);
            this.nanoProgress.TabIndex = 2;
            // 
            // hpProgress
            // 
            this.hpProgress.Location = new System.Drawing.Point(492, 13);
            this.hpProgress.Name = "hpProgress";
            this.hpProgress.Size = new System.Drawing.Size(100, 10);
            this.hpProgress.TabIndex = 0;
            this.hpProgress.Value = 100;
            // 
            // playerTicker
            // 
            this.playerTicker.Tick += new System.EventHandler(this.playerTicker_Tick);
            // 
            // AdministratePlayers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 379);
            this.Controls.Add(this.tabController);
            this.Name = "AdministratePlayers";
            this.Text = "Administrate Players";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AdministratePlayers_FormClosed);
            this.tabController.ResumeLayout(false);
            this.playersView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.players)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.playerSelected.ResumeLayout(false);
            this.playerSelected.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabController;
        private System.Windows.Forms.TabPage playersView;
        private System.Windows.Forms.TabPage playerSelected;
        private System.Windows.Forms.DataGridView players;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn rankColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn shipColumn;
        private System.Windows.Forms.DataGridViewImageColumn hpColumn;
        private System.Windows.Forms.DataGridViewImageColumn nhColumn;
        private System.Windows.Forms.DataGridViewImageColumn shieldColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn configsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn expColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn honColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn creditColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn uriColumn;
        private System.Windows.Forms.Label playerState;
        private System.Windows.Forms.Label clan;
        private System.Windows.Forms.Label groupLabel;
        private System.Windows.Forms.Label attackers;
        private System.Windows.Forms.Label lastCombat;
        private System.Windows.Forms.Label uriLabel;
        private System.Windows.Forms.Label creLabel;
        private System.Windows.Forms.Label honLabel;
        private System.Windows.Forms.Label expLabel;
        private System.Windows.Forms.Label pos;
        private System.Windows.Forms.Label mapName;
        private System.Windows.Forms.ProgressBar shdProgress;
        private System.Windows.Forms.ProgressBar nanoProgress;
        private System.Windows.Forms.ProgressBar hpProgress;
        private System.Windows.Forms.Button closeTab;
        private System.Windows.Forms.Timer playerTicker;
    }
}