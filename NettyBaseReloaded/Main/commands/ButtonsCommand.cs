using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game.objects.world.players.settings;

namespace NettyBaseReloaded.Main.commands
{
    class ButtonsCommand : Command
    {
        public ButtonsCommand() : base("button", "Buttons command", false, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            //none
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            try
            {
                if (!session.Player.RCON) return;

                var gameSession = session.GetEquivilentGameSession();
                if (gameSession == null) return;

                switch (args[1])
                {
                    case "hide":
                        gameSession.Player.Settings.Slotbar.HideButton(Buttons.SELECTION_LASER_CBO100);
                        break;
                    case "show":
                        gameSession.Player.Settings.Slotbar.ShowButton(Buttons.SELECTION_LASER_CBO100);
                        break;
                    case "showflash":
                        gameSession.Player.Settings.Slotbar.ShowFlash(Buttons.SELECTION_LASER_CBO100, false, 5);
                        break;
                    case "hideflash":
                        gameSession.Player.Settings.Slotbar.HideFlash(Buttons.SELECTION_LASER_CBO100);
                        break;
                }
            }
            catch
            {

            }
        }
    }
}
