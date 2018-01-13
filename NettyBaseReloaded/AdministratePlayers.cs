using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Utils.form;
using Newtonsoft.Json;

namespace NettyBaseReloaded
{
    partial class AdministratePlayers : Form
    {
        private TabPage DefaultPlayerTab;

        public AdministratePlayers()
        {
            InitializeComponent();
            Shown += (s, args) =>
            {
                DefaultPlayerTab = tabController.TabPages[1];
                tabController.TabPages.RemoveAt(1);
                playerTicker.Start();
            };
            Closed += (s, e) => new Controller().Show();
        }

        public void AddPlayer(Player player)
        {
            players.Rows.Add(player.Id, player.Name, player.RankId.ToString(), player.Hangar.Ship.ToStringLoot(), Default, Default, Default, JsonConvert.SerializeObject(player.Hangar.Configurations), player.Information.Experience.Get(), player.Information.Honor.Get(), player.Information.Credits.Get(), player.Information.Uridium.Get());
        }

        private Bitmap Default => new Bitmap(100,20);

        private Bitmap GetProgressBmp(Color color, float perc)
        {
            var bmp = new Bitmap(100,20);
            var gfx = Graphics.FromImage(bmp);
            //clear graphics
            gfx.Clear(Color.Black);

            //draw progressbar
            gfx.FillRectangle(new SolidBrush(color), new Rectangle(0, 0, (int)(perc), 20));

            //draw % complete
            gfx.DrawString(perc + "%", new Font("Arial", 8), Brushes.White, new PointF(40, 2));

            //load bitmap in picturebox picboxPB
            return bmp;
        }

        public void UpdatePlayers()
        {
            foreach (DataGridViewRow playerRow in players.Rows)
            {
                var player = World.StorageManager.GetGameSession((int) playerRow.Cells[0].Value).Player;
                playerRow.Cells[4].Value = GetProgressBmp(Color.Green, Convert.ToSingle((double)player.CurrentHealth / player.MaxHealth) * 100);
                playerRow.Cells[5].Value = GetProgressBmp(Color.Yellow, Convert.ToSingle((double)player.CurrentNanoHull / player.MaxNanoHull) * 100);
                playerRow.Cells[6].Value = GetProgressBmp(Color.LightSkyBlue, Convert.ToSingle((double)player.CurrentShield / player.MaxShield) * 100);
                playerRow.Cells[8].Value = player.Information.Experience.Get();
                playerRow.Cells[9].Value = player.Information.Honor.Get();
                playerRow.Cells[10].Value = player.Information.Credits.Get();
                playerRow.Cells[11].Value = player.Information.Uridium.Get();
            }
        }

        private void players_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
            {
                var selectedRow = players.Rows[e.RowIndex];
                var gameSession = World.StorageManager.GetGameSession((int) selectedRow.Cells[0].Value);
                if (gameSession != null)
                    UpdateSelectedPlayer(gameSession.Player);
            }
        }

        private void UpdateSelectedPlayer(Player player)
        {
            selectedPlayer = player;
            if (!tabController.TabPages.ContainsKey("playerSelected")) tabController.TabPages.Add(DefaultPlayerTab);
        }

        private Player selectedPlayer { get; set; }
        public void SelectedPlayerTicker()
        {
            if (selectedPlayer == null) return;
            DefaultPlayerTab.Text = selectedPlayer.Name;
            expLabel.Text = $"Exp: {selectedPlayer.Information.Experience.Get()}";
            honLabel.Text = $"Hon: {selectedPlayer.Information.Honor.Get()}";
            creLabel.Text = $"Credits: {selectedPlayer.Information.Credits.Get()}";
            uriLabel.Text = $"Uri: {selectedPlayer.Information.Uridium.Get()}";
            lastCombat.Text = $"Last Combat time: {selectedPlayer.LastCombatTime.ToLongTimeString()}";
            attackers.Text = "Attackers: " + JsonConvert.SerializeObject(selectedPlayer.Controller.Attack.Attackers);
            groupLabel.Text = "Group: " + JsonConvert.SerializeObject(selectedPlayer.Group);
            clan.Text = "Clan: " + JsonConvert.SerializeObject(selectedPlayer.Clan);
            playerState.Text = selectedPlayer.EntityState.ToString();
            mapName.Text = selectedPlayer.Spacemap.Name;
            pos.Text = selectedPlayer.Position.ToString();
            hpProgress.Maximum = selectedPlayer.MaxHealth;
            hpProgress.Value = selectedPlayer.CurrentHealth;
            nanoProgress.Maximum = selectedPlayer.MaxNanoHull;
            nanoProgress.Value = selectedPlayer.CurrentNanoHull;
            shdProgress.Maximum = selectedPlayer.MaxShield;
            shdProgress.Value = selectedPlayer.CurrentShield;
        }

        private void closeTab_Click(object sender, EventArgs e)
        {
            var targetTab = tabController.SelectedTab;
            tabController.SelectTab(targetTab.TabIndex - 1);
            tabController.TabPages.Remove(targetTab);
            selectedPlayer = null;
        }

        private void playerTicker_Tick(object sender, EventArgs e)
        {
            if (players.Rows.Count != World.StorageManager.GameSessions.Count)
            {
                UpdateCollection();
            }
            UpdatePlayers();
            SelectedPlayerTicker();
        }

        private void UpdateCollection()
        {
            players.Rows.Clear();
            foreach (var gameSession in World.StorageManager.GameSessions.Values)
            {
                AddPlayer(gameSession.Player);
            }
        }

        private void AdministratePlayers_FormClosed(object sender, FormClosedEventArgs e)
        {
            playerTicker.Dispose();
            playerTicker = null;
        }
    }
}
