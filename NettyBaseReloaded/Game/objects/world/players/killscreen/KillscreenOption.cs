using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.killscreen
{
    class KillscreenOption
    {
        public object Object { get; set; }

        public static short NEAREST_BASE = 1;
        public static short PAYMENT_LINK = 9;
        public static short HADES_FREE_REPAIR = 7;
        public static short varN3e = 6;
        public static short SPOT = 3;
        public static short JUMP_GATE = 2;
        public static short varj4R = 5;
        public static short varVD = 0;
        public static short FREE_REPAIR_SECS = 4;
        public static short TDM_FREE_REPAIR = 8;

        public KillscreenOption(bool newClient, short buttonType, short currency = 0, short amount = 0)
        {
            //TODO remove desc_killscreen_free_repair_cause_test_phase
            if (newClient)
            {
                if (amount > 0)
                {
                    var currencyTooltip = (currency == 0) ? "C." : "U.";

                    Object = new netty.commands.new_client.KillScreenOptionModule(0,
                        new netty.commands.new_client.KillScreenOptionTypeModule(buttonType),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()), //ttip_killscreen_basic_repair
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("btn_killscreen_repair_for_money",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>
                            {
                                new netty.commands.new_client.commandF5("%COUNT%", "" + amount,
                                    new netty.commands.new_client.commandWw(0)),
                                new netty.commands.new_client.commandF5("%CURRENCY%", currencyTooltip,
                                    new netty.commands.new_client.commandWw(0))
                            }),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()),
                        new netty.commands.new_client.PriceModule(currency, amount),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand(
                            "desc_killscreen_free_repair_cause_test_phase", new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()), //explication
                        true
                    );
                }
                else
                {
                    Object = new netty.commands.new_client.KillScreenOptionModule(0,
                        new netty.commands.new_client.KillScreenOptionTypeModule(buttonType),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()), //ttip_killscreen_basic_repair
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()),
                        new netty.commands.new_client.PriceModule(currency, amount),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand(
                            "desc_killscreen_free_repair_cause_test_phase", new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()), //explication
                        true
                    );
                }
            }
            else
            {
                if (amount > 0)
                {
                    var currencyTooltip = (currency == 0) ? "C." : "U.";

//                    Object = new netty.commands.old_client.KillScreenOptionModule(new netty.commands.old_client.KillScreenOptionTypeModule(buttonType),
//new netty.commands.old_client.PriceModule(currency, amount), true, 0,                    );
                }
                else
                {
                    Object = new netty.commands.new_client.KillScreenOptionModule(0,
                        new netty.commands.new_client.KillScreenOptionTypeModule(buttonType),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()), //ttip_killscreen_basic_repair
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand("",
                            new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()),
                        new netty.commands.new_client.PriceModule(currency, amount),
                        new netty.commands.new_client.MessageLocalizedWildcardCommand(
                            "desc_killscreen_free_repair_cause_test_phase", new netty.commands.new_client.commandWw(0),
                            new List<netty.commands.new_client.commandF5>()), //explication
                        true
                    );
                }
            }
        }
    }

}
