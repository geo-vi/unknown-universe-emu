using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands.new_client;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map.mines;

namespace NettyBaseReloaded.Game.objects.world.players.settings.slotbars
{
    class MineItem : SlotbarItem
    {
        public MineItem(string itemId, int counterValue, int maxCounter, List<ClientUITooltipTextFormat> toolTipItemBar = null, short counterType = 2, bool selected = false, bool visible = true, bool buyable = false) : base(itemId, counterValue, maxCounter, toolTipItemBar, counterType, selected, visible, buyable)
        {
        }

        public override void Execute(Player player)
        {
            int id = 0;
            string hash = "";

            if (player.Cooldowns.CooldownDictionary.Any(c => c.Value is MineCooldown)) return;
            var mineCld = new MineCooldown();
            mineCld.Send(player.GetGameSession());
            player.Cooldowns.Add(mineCld);

            switch (ItemId)
            {
                case "ammunition_mine_acm-01":
                    id = player.Spacemap.GetNextObjectId();
                    hash = player.Spacemap.HashedObjects.Keys.ToList()[id];
                    player.Spacemap.AddObject(new ACM01(id, hash, player.Position, player.Spacemap));
                    break;
                case "ammunition_mine_empm-01":
                    id = player.Spacemap.GetNextObjectId();
                    hash = player.Spacemap.HashedObjects.Keys.ToList()[id];
                    player.Spacemap.AddObject(new EMPM01(id, hash, player.Position, player.Spacemap));
                    break;
                case "ammunition_mine_ddm-01":
                    id = player.Spacemap.GetNextObjectId();
                    hash = player.Spacemap.HashedObjects.Keys.ToList()[id];
                    player.Spacemap.AddObject(new DDM01(id, hash, player.Position, player.Spacemap));
                    break;
                case "ammunition_mine_sabm-01":
                    id = player.Spacemap.GetNextObjectId();
                    hash = player.Spacemap.HashedObjects.Keys.ToList()[id];
                    player.Spacemap.AddObject(new SABM01(id, hash, player.Position, player.Spacemap));
                    break;
                case "ammunition_mine_slm-01":
                    id = player.Spacemap.GetNextObjectId();
                    hash = player.Spacemap.HashedObjects.Keys.ToList()[id];
                    player.Spacemap.AddObject(new SLM01(id, hash, player.Position, player.Spacemap));
                    break;
            }
        }
    }
}
