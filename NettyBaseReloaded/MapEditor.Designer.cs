namespace NettyBaseReloaded
{
    partial class MapEditor
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "POI"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.White, null);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("Asset");
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem("MMO");
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem("EIC");
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem("VRU");
            System.Windows.Forms.ListViewItem listViewItem6 = new System.Windows.Forms.ListViewItem("Pirate");
            System.Windows.Forms.ListViewItem listViewItem7 = new System.Windows.Forms.ListViewItem("Portal");
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem("Basic NPC");
            System.Windows.Forms.ListViewItem listViewItem9 = new System.Windows.Forms.ListViewItem("Aggressive NPC");
            System.Windows.Forms.ListViewItem listViewItem10 = new System.Windows.Forms.ListViewItem("Mothership NPC");
            System.Windows.Forms.ListViewItem listViewItem11 = new System.Windows.Forms.ListViewItem("Relay");
            this.mapViewer = new System.Windows.Forms.TabControl();
            this.mapFinder = new System.Windows.Forms.TabPage();
            this.dataGrid = new System.Windows.Forms.DataGridView();
            this.idColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.nameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.entityCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.objColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.poiCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.levelColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.defaultMap = new System.Windows.Forms.TabPage();
            this.moreCalculus = new System.Windows.Forms.CheckBox();
            this.toggleCollectables = new System.Windows.Forms.CheckBox();
            this.toggleObj = new System.Windows.Forms.CheckBox();
            this.togglePOI = new System.Windows.Forms.CheckBox();
            this.toggleEntities = new System.Windows.Forms.CheckBox();
            this.closeMapPage = new System.Windows.Forms.Button();
            this.vwSelection = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ticker = new System.Windows.Forms.Timer(this.components);
            this.mapViewer.SuspendLayout();
            this.mapFinder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            this.defaultMap.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // mapViewer
            // 
            this.mapViewer.Controls.Add(this.mapFinder);
            this.mapViewer.Controls.Add(this.defaultMap);
            this.mapViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapViewer.Location = new System.Drawing.Point(0, 0);
            this.mapViewer.Name = "mapViewer";
            this.mapViewer.SelectedIndex = 0;
            this.mapViewer.Size = new System.Drawing.Size(729, 345);
            this.mapViewer.TabIndex = 0;
            // 
            // mapFinder
            // 
            this.mapFinder.Controls.Add(this.dataGrid);
            this.mapFinder.Location = new System.Drawing.Point(4, 22);
            this.mapFinder.Name = "mapFinder";
            this.mapFinder.Padding = new System.Windows.Forms.Padding(3);
            this.mapFinder.Size = new System.Drawing.Size(721, 319);
            this.mapFinder.TabIndex = 0;
            this.mapFinder.Text = "Maps";
            this.mapFinder.UseVisualStyleBackColor = true;
            // 
            // dataGrid
            // 
            this.dataGrid.AllowUserToAddRows = false;
            this.dataGrid.AllowUserToDeleteRows = false;
            this.dataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.idColumn,
            this.nameColumn,
            this.entityCount,
            this.objColumn,
            this.poiCount,
            this.levelColumn});
            this.dataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGrid.Location = new System.Drawing.Point(3, 3);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ReadOnly = true;
            this.dataGrid.Size = new System.Drawing.Size(715, 313);
            this.dataGrid.TabIndex = 0;
            this.dataGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGrid_CellClick);
            // 
            // idColumn
            // 
            this.idColumn.HeaderText = "Id";
            this.idColumn.Name = "idColumn";
            this.idColumn.ReadOnly = true;
            // 
            // nameColumn
            // 
            this.nameColumn.HeaderText = "Name";
            this.nameColumn.Name = "nameColumn";
            this.nameColumn.ReadOnly = true;
            // 
            // entityCount
            // 
            this.entityCount.HeaderText = "Entities";
            this.entityCount.Name = "entityCount";
            this.entityCount.ReadOnly = true;
            // 
            // objColumn
            // 
            this.objColumn.HeaderText = "Objects";
            this.objColumn.Name = "objColumn";
            this.objColumn.ReadOnly = true;
            // 
            // poiCount
            // 
            this.poiCount.HeaderText = "POIs";
            this.poiCount.Name = "poiCount";
            this.poiCount.ReadOnly = true;
            // 
            // levelColumn
            // 
            this.levelColumn.HeaderText = "Level";
            this.levelColumn.Name = "levelColumn";
            this.levelColumn.ReadOnly = true;
            // 
            // defaultMap
            // 
            this.defaultMap.Controls.Add(this.moreCalculus);
            this.defaultMap.Controls.Add(this.toggleCollectables);
            this.defaultMap.Controls.Add(this.toggleObj);
            this.defaultMap.Controls.Add(this.togglePOI);
            this.defaultMap.Controls.Add(this.toggleEntities);
            this.defaultMap.Controls.Add(this.closeMapPage);
            this.defaultMap.Controls.Add(this.vwSelection);
            this.defaultMap.Controls.Add(this.tabControl1);
            this.defaultMap.Controls.Add(this.pictureBox1);
            this.defaultMap.Location = new System.Drawing.Point(4, 22);
            this.defaultMap.Name = "defaultMap";
            this.defaultMap.Padding = new System.Windows.Forms.Padding(3);
            this.defaultMap.Size = new System.Drawing.Size(721, 319);
            this.defaultMap.TabIndex = 1;
            this.defaultMap.Text = "DEF";
            this.defaultMap.UseVisualStyleBackColor = true;
            // 
            // moreCalculus
            // 
            this.moreCalculus.AutoSize = true;
            this.moreCalculus.Location = new System.Drawing.Point(22, 276);
            this.moreCalculus.Name = "moreCalculus";
            this.moreCalculus.Size = new System.Drawing.Size(144, 17);
            this.moreCalculus.TabIndex = 9;
            this.moreCalculus.Text = "Special angle calculation";
            this.moreCalculus.UseVisualStyleBackColor = true;
            // 
            // toggleCollectables
            // 
            this.toggleCollectables.AutoSize = true;
            this.toggleCollectables.Location = new System.Drawing.Point(315, 253);
            this.toggleCollectables.Name = "toggleCollectables";
            this.toggleCollectables.Size = new System.Drawing.Size(119, 17);
            this.toggleCollectables.TabIndex = 8;
            this.toggleCollectables.Text = "Toggle Collectables";
            this.toggleCollectables.UseVisualStyleBackColor = true;
            // 
            // toggleObj
            // 
            this.toggleObj.AutoSize = true;
            this.toggleObj.Location = new System.Drawing.Point(124, 253);
            this.toggleObj.Name = "toggleObj";
            this.toggleObj.Size = new System.Drawing.Size(98, 17);
            this.toggleObj.TabIndex = 7;
            this.toggleObj.Text = "Toggle Objects";
            this.toggleObj.UseVisualStyleBackColor = true;
            // 
            // togglePOI
            // 
            this.togglePOI.AutoSize = true;
            this.togglePOI.Location = new System.Drawing.Point(228, 253);
            this.togglePOI.Name = "togglePOI";
            this.togglePOI.Size = new System.Drawing.Size(80, 17);
            this.togglePOI.TabIndex = 6;
            this.togglePOI.Text = "Toggle POI";
            this.togglePOI.UseVisualStyleBackColor = true;
            // 
            // toggleEntities
            // 
            this.toggleEntities.AutoSize = true;
            this.toggleEntities.Location = new System.Drawing.Point(22, 253);
            this.toggleEntities.Name = "toggleEntities";
            this.toggleEntities.Size = new System.Drawing.Size(96, 17);
            this.toggleEntities.TabIndex = 5;
            this.toggleEntities.Text = "Toggle Entities";
            this.toggleEntities.UseVisualStyleBackColor = true;
            // 
            // closeMapPage
            // 
            this.closeMapPage.Location = new System.Drawing.Point(638, 288);
            this.closeMapPage.Name = "closeMapPage";
            this.closeMapPage.Size = new System.Drawing.Size(75, 23);
            this.closeMapPage.TabIndex = 4;
            this.closeMapPage.Text = "Close";
            this.closeMapPage.UseVisualStyleBackColor = true;
            this.closeMapPage.Click += new System.EventHandler(this.closeMapPage_Click);
            // 
            // vwSelection
            // 
            this.vwSelection.DisplayMember = "0";
            this.vwSelection.FormattingEnabled = true;
            this.vwSelection.Items.AddRange(new object[] {
            "VW 0"});
            this.vwSelection.Location = new System.Drawing.Point(22, 7);
            this.vwSelection.Name = "vwSelection";
            this.vwSelection.Size = new System.Drawing.Size(121, 21);
            this.vwSelection.TabIndex = 3;
            this.vwSelection.Text = "VW 0";
            this.vwSelection.ValueMember = "0";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(403, 29);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(318, 217);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.listView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(310, 191);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Toolbox";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5,
            listViewItem6,
            listViewItem7,
            listViewItem8,
            listViewItem9,
            listViewItem10,
            listViewItem11});
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(304, 185);
            this.listView1.TabIndex = 1;
            this.listView1.TileSize = new System.Drawing.Size(200, 50);
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Tile;
            this.listView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView1_ItemDrag);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.richTextBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(310, 191);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Properties";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(304, 185);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "{Id: 0, Name: \"\", Position:{0,0}}";
            this.richTextBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.richTextBox1_KeyDown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Location = new System.Drawing.Point(22, 29);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(375, 217);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // MapEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 345);
            this.Controls.Add(this.mapViewer);
            this.Name = "MapEditor";
            this.Text = "MapEditor";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MapEditor_FormClosed);
            this.Load += new System.EventHandler(this.MapEditor_Load);
            this.mapViewer.ResumeLayout(false);
            this.mapFinder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            this.defaultMap.ResumeLayout(false);
            this.defaultMap.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mapViewer;
        private System.Windows.Forms.TabPage mapFinder;
        private System.Windows.Forms.TabPage defaultMap;
        private System.Windows.Forms.DataGridView dataGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn idColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn entityCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn objColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn poiCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn levelColumn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer ticker;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.ComboBox vwSelection;
        private System.Windows.Forms.Button closeMapPage;
        private System.Windows.Forms.CheckBox toggleCollectables;
        private System.Windows.Forms.CheckBox toggleObj;
        private System.Windows.Forms.CheckBox togglePOI;
        private System.Windows.Forms.CheckBox toggleEntities;
        private System.Windows.Forms.CheckBox moreCalculus;
    }
}