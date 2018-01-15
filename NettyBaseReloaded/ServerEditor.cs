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
using NettyBaseReloaded.Game.objects.world.events;

namespace NettyBaseReloaded
{
    public partial class ServerEditor : Form
    {
        public ServerEditor()
        {
            InitializeComponent();
            Shown += (s, e) =>
            {
                debugCommands.Checked = Properties.Game.PRINTING_COMMANDS;
                debugLegacyCommands.Checked = Properties.Game.PRINTING_LEGACY_COMMANDS;
                rewardMultiplyer.Value = Properties.Game.REWARD_MULTIPLYER;
            };
            Closed += (s, e) => new Controller().Show();
        }

        private void debugPackets_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Game.PRINTING_LEGACY_COMMANDS = debugLegacyCommands.Checked;
        }

        private void debugCommands_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Game.PRINTING_COMMANDS = debugCommands.Checked;
        }

        private void debugCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Server.DEBUG = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!Properties.Server.CONSOLE_MODE)
                Properties.Server.CONSOLE_MODE = true;
            else Properties.Server.CONSOLE_MODE = false;
        }

        private void rewardMultiplyer_Scroll(object sender, EventArgs e)
        {
            Properties.Game.REWARD_MULTIPLYER = rewardMultiplyer.Value;
        }

        private void startScoreMageddon_Click(object sender, EventArgs e)
        {
            World.StorageManager.Events.Add(0, new GameEvent(0, "Scoremageddon", EventTypes.SCOREMAGEDDON));
            World.StorageManager.Events[0].Start();
        }
    }
}
