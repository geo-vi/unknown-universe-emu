using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Utils.form;

namespace NettyBaseReloaded
{
    public partial class MapEditor : Form
    {
        private TabPage MapPage { get; set; }

        public MapEditor()
        {
            InitializeComponent();
            Closed += (s, e) => new Controller().Show();
        }

        private void listView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            MessageBox.Show(e.Item.ToString());
        }

        private bool SelectionMode = false;

        private PointF StartingCordinates { get; set; }
        private PointF EndingCordinates { get; set; }
        private Rectangle BuiltRectangle { get; set; }

        private Point PicBoxMouseLocation;
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (Map == null) return;
            if (SelectionMode)
            {
                if (EndingCordinates.IsEmpty) EndingCordinates = MousePosition;

                var brush = new SolidBrush(Color.FromArgb(55, 211, 211, 211));
                e.Graphics.FillRectangle(brush, BuiltRectangle);
            }
            e.Graphics.DrawString($"{Map.Name}; {Map.Entities.Count(x => x.Value is Player)} Players, {Map.Entities.Count(x => x.Value is Npc)} NPCs", Font, new SolidBrush(Color.White), 0,0);
            MapDrawing.Initiate(Map, sender, e, toggleObj.Checked, togglePOI.Checked, toggleEntities.Checked, toggleCollectables.Checked, moreCalculus.Checked, PicBoxMouseLocation);
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            pictureBox1.Select();
            
            BuiltRectangle = new Rectangle(e.X, e.Y, 0, 0);
            StartingCordinates = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            EndingCordinates = e.Location;
            SelectionMode = false;
        }

        private void MapEditor_Load(object sender, EventArgs e)
        {
            ticker.Tick += (o, args) =>
            {
                pictureBox1.Invalidate();
            };
            ticker.Start();
            MapPage = mapViewer.TabPages[1];
            mapViewer.TabPages.RemoveAt(1);
            LoadMaps();
        }

        private void LoadMaps()
        {
            foreach (var map in World.StorageManager.Spacemaps)
            {
                dataGrid.Rows.Add(map.Value.Id, map.Value.Name, map.Value.Entities.Count, map.Value.Objects.Count, map.Value.POIs.Count, map.Value.Level);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            PicBoxMouseLocation = e.Location;
            BuiltRectangle = new Rectangle(BuiltRectangle.Left, BuiltRectangle.Top, e.X - BuiltRectangle.Left, e.Y - BuiltRectangle.Top);
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && ModifierKeys.HasFlag(Keys.Control))
            {
                //TODO: Parse new properties
            }
        }

        private Spacemap Map { get; set; }

        private void dataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex < 0)
            {
                var row = dataGrid.Rows[e.RowIndex];
                var id = Convert.ToInt32(row.Cells[0].Value);
                Map = World.StorageManager.Spacemaps[id];
                if (!mapViewer.TabPages.ContainsKey("defaultMap"))
                    mapViewer.TabPages.Add(MapPage);
                UpdateDefMapPage();
            }
        }

        private void UpdateDefMapPage()
        {
            MapPage.Text = Map.Id + "; " + Map.Name;
        }

        private void closeMapPage_Click(object sender, EventArgs e)
        {
            mapViewer.TabPages.RemoveAt(1);
        }

        private void MapEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            ticker.Dispose();
            ticker = null;
        }
    }
}
