using System.Collections.Generic;
using NettyBaseReloaded.Game.netty.commands.new_client;

namespace NettyBaseReloaded.Game.objects.world.players.killscreen
{
    class KillscreenOption
    {
        public KillScreenOptionModule Object { get; set; }

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

        public KillscreenOption(short buttonType, short currency = 0, short amount = 0)
        {
            //TODO remove desc_killscreen_free_repair_cause_test_phase
            if (amount > 0)
            {
                var currencyTooltip = (currency == 0) ? "C." : "U.";

                Object = new KillScreenOptionModule(0,
                    new KillScreenOptionTypeModule(buttonType),
                    new MessageLocalizedWildcardCommand("", new commandWw(0), new List<commandF5>()), //ttip_killscreen_basic_repair
                    new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_money", new commandWw(0),
                        new List<commandF5>
                        {
                            new commandF5("%COUNT%", "" + amount, new commandWw(0)),
                            new commandF5("%CURRENCY%", currencyTooltip, new commandWw(0))
                        }),
                    new MessageLocalizedWildcardCommand("", new commandWw(0), new List<commandF5>()),
                    new PriceModule(currency, amount),
                    new MessageLocalizedWildcardCommand("desc_killscreen_free_repair_cause_test_phase", new commandWw(0), new List<commandF5>()), //explication
                    true
                );
            }
            else
            {
                Object = new KillScreenOptionModule(0,
                    new KillScreenOptionTypeModule(buttonType),
                    new MessageLocalizedWildcardCommand("", new commandWw(0), new List<commandF5>()), //ttip_killscreen_basic_repair
                    new MessageLocalizedWildcardCommand("btn_killscreen_repair_for_free", new commandWw(0), new List<commandF5>()),
                    new MessageLocalizedWildcardCommand("", new commandWw(0), new List<commandF5>()),
                    new PriceModule(currency, amount),
                    new MessageLocalizedWildcardCommand("desc_killscreen_free_repair_cause_test_phase", new commandWw(0), new List<commandF5>()), //explication
                    true
                );
            }
        }
    }
}