using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.settings
{
    class Window
    {
        public const string DEFAULT_OLD = "";
        public const string DEFAULT_NEW = "";

        public class New_Client
        {
            public static short STANDARD = ClientUITooltipTextFormat.STANDARD;
            public static short RED = ClientUITooltipTextFormat.RED;

            public WindowButtonModule Window { get; set; }
            public List<ClientUITooltipTextFormat> TextFormat { get; set; }

            public New_Client(string windowId, string tooltipId, bool visible = true) : this(windowId, tooltipId, visible, ClientUITooltipTextFormat.STANDARD) { }

            //If you want to change the text color :D
            public New_Client(string windowId, string tooltipId, bool visible, short textColor)
            {
                TextFormat = new List<ClientUITooltipTextFormat>();
                TextFormat.Add(new ClientUITooltipTextFormat(textColor, tooltipId, new commandWw(commandWw.LOCALIZED), new List<commandF5>()));

                var tooltip = new ClientUITooltip(TextFormat);
                Window = new WindowButtonModule(windowId, visible, tooltip);
            }

        }
    }
}
