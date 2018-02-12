﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map.gg;

namespace NettyBaseReloaded.Game.objects.world.map.objects.jumpgates
{
    class LowPortal : Jumpgate
    {
        public LowPortal(int id, Vector pos, Spacemap map, int vw) : base(id, Faction.NONE, pos, map, new Vector(1000, 11800), 200, true, 0, 0, 34)
        {
            DestinationVirtualWorldId = vw;
        }

        public override void click(Character character)
        {
            var player =  character as Player;
            if (player == null) return;
            if (player.Group == null)
            {
                //TODO Send message player not in group
                return;
            }

            // If any other group member has the gate
            var groupMemberWithGateInitiated =
                player.Group.Members.FirstOrDefault(x => x.Value.OwnedGates.Any(y => y is LowGate));

            if (groupMemberWithGateInitiated.Value != null)
            {
                var low = groupMemberWithGateInitiated.Value.OwnedGates.FirstOrDefault(x => x is LowGate);
                if (low.VWID != 0 && low.VirtualMap != null)
                {
                    player.Controller.Miscs.Jump(low.VirtualMap.Id, Destination, Id, low.VWID);
                    low.PendingPlayers.Add(player);
                }
                else Console.WriteLine("Escape");
            }
            else
            {
                var low = new LowGate(0, World.StorageManager.Spacemaps[200]);
                low.DefineOwner(player);
                low.InitiateVirtualWorld();
                Console.WriteLine(low.VWID.ToString());
                player.Controller.Miscs.Jump(low.VirtualMap.Id, Destination, Id, low.VWID);
                low.PendingPlayers.Add(player);
            }
        }
    }
}