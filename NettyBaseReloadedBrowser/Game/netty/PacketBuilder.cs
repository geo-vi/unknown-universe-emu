using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloadedBrowser.Game.netty.commands;
using NettyBaseReloadedBrowser.Game.netty.commands.settingsModules;

namespace NettyBaseReloadedBrowser.Game.netty
{
    class PacketBuilder
    {
        public static byte[] ShipInitializationCommand()
        {
            return new ShipInitializationCommand(
                1,
                "AAAAAAAAAAAA",
                "ship_leonov",
                420,
                0,
                0,
                1000,
                1069,
                0, //freeCargo
                4000, //maxCargo
                69,
                0,
                1000,
                1000,
                1,
                1,
                1,//idk
                3, //idk
                true,
                0,
                0,
                1,
                0,
                0,
                0,
                21,
                "PX", //clanTag
                100,
                true,
                false, //cloaked
                true,
                new List<VisualModifierCommand>()
                ).getBytes();
        }

        public static byte[] UserSettingsCommand()
        {
            var qs = new QualitySettingsModule(false, 3, 3, 3, false, 3, 3, 3, 3, 3, 3);
            var asm = new AudioSettingsModule(false, 50, 0, 50, false);
            var ws = new WindowSettingsModule(8, "23,1|24,1|25,1|27,1|26,1|100,1|34,1|36,1|33,1|35,1|39,1|38,1|37,1|32,1|", false);
            var gm = new GameplaySettingsModule(false, false, false, false, false, true, false, false, false, false);
            var z9 = new QuestSettingsModule(false, true, true, false, false, false);
            var ds = new DisplaySettingsModule(true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, true, 3, 4, 4, 3, 3, 4, 3, 3, true, true, true, true);

            return new UserSettingsCommand(qs, asm, ws, gm, z9, ds).getBytes();
        }

        public static byte[] LegacyModule(string message)
        {
            return new LegacyModule(message).getBytes();
        }
    }
}
