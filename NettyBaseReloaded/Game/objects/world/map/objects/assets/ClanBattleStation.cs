using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.players.equipment;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.objects.world.map.objects.assets
{
    class ClanBattleStation : AttackableAsset, IClickable
    {
        public int BattleStationId { get; set; }
        public bool DeflectorShieldActive => true;
        public int DeflectorShieldRate => 1;
        public int RepairPrice => 5000;
        public int DeflectorShieldMin => 30;
        public int DeflectorShieldMax => 600;
        public int DeflectorShieldIncrement => 1;

        public Dictionary<int, BattleStationModule> EquippedModules;

        public ClanBattleStation(int id, int battleStationId, string name, Faction faction, Vector position, Spacemap map, Player creator, Dictionary<int,BattleStationModule> modules) : base(id, name, AssetTypes.BATTLESTATION, faction, creator.Clan, 65538, 0, position, map, false, false, false, 1000, 1000, 1000, 1000, 0,0,0,0)
        {
            BattleStationId = battleStationId;
            EquippedModules = modules;
        }

        public override void execute(Character character)
        {
            var player = character as Player;
            if (player != null && !player.UsingNewClient)
            {
                click(character);
            }
        }

        public void click(Character character)
        {
            var player = character as Player;
            if (player != null && player.Clan == Clan)
            {
                Packet.Builder.BattleStationManagementUiInitializationCommand(player.GetGameSession(), this);
                //player.GetGameSession().Client.Send(netty.commands.old_client.AssetInfoCommand.write(Id, new AssetTypeModule((short)Type), 2 | 2, 2, 3, 4, false, 6, 8).Bytes);
            }
        }

        public void SwapModule(BattleStationModule module)
        {

        }

        public void RepairModule(BattleStationModule module)
        {

        }

        public int GetDeflectorShieldSeconds()
        {
            return 0;
        }

        public int DeflectorShieldSecondsMax()
        {
            return 600;
        }

        public int GetAttackRating()
        {
            return 0;
        }

        public int GetDefenceRating()
        {
            return 0;
        }

        public int GetRepairRating()
        {
            return 0;
        }

        public int GetHonorBoostRating()
        {
            return 0;
        }

        public int GetExperienceBoostRating()
        {
            return 0;
        }

        public int GetDamageBoostRating()
        {
            return 0;
        }

        public bool DeflectorDeactivationPossible()
        {
            return false;
        }
    }
}
