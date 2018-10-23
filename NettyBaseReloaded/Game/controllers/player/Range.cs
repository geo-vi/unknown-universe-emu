using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.managers;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map.objects;
using NettyBaseReloaded.Game.objects.world.map.zones;

namespace NettyBaseReloaded.Game.controllers.player
{
    class Range : IChecker
    {
        private PlayerController baseController;

        public Range(PlayerController controller)
        {
            baseController = controller;
        }

        public void Check()
        {
            LookupRangeZones();
            LookupRangeObjects();
        }

        private DateTime LastTimeCheckedZones = new DateTime();
        public void LookupRangeZones()
        {
            if (LastTimeCheckedZones.AddMilliseconds(250) > DateTime.Now) return;

            try
            {
                if (baseController.Player.Range.Zones.Values.Count(x => x is DemiZone && (x.ZoneFaction == baseController.Player.FactionId || x.ZoneFaction == Faction.NONE)) > 0)
                {
                    if (!baseController.Player.State.InDemiZone && !baseController.Attack.Attacking)
                    {
                        baseController.Player.State.InDemiZone = true;
                    }
                }
                else
                {
                    if (baseController.Player.State.InDemiZone)
                    {
                        baseController.Player.State.InDemiZone = false;
                    }
                }
            }
            catch (Exception e)
            {
            }
            LastTimeCheckedZones = DateTime.Now;
        }

        private DateTime LastTimeCheckedObjects = new DateTime();
        public void LookupRangeObjects()
        {
            if (LastTimeCheckedObjects.AddMilliseconds(3000) > DateTime.Now) return;

            //TODO
            try
            {
                if (baseController.Player.Range.Objects.Values.Any(x => x is Station))
                {
                    var session = baseController.Player.GetGameSession();
                    if (session != null)
                    {
                        baseController.Player.State.InTradeArea = true;
                        baseController.Player.State.InEquipmentArea = true;
                        //Packet.Builder.EquipReadyCommand(session, true);
                        //World.DatabaseManager.Refresh(baseController.Player);
                        //baseController.Player.Equipment.Hangars =
                        //    World.DatabaseManager.LoadHangars(baseController.Player);
                        //baseController.Player.Hangar.Drones = World.DatabaseManager.LoadDrones(baseController.Player);
                        //baseController.Player.Hangar.Configurations =
                        //    World.DatabaseManager.LoadConfig(baseController.Player);
                    }
                }
                else
                {
                    //var session = baseController.Player.GetGameSession();
                    //if (session != null)
                    //    Packet.Builder.EquipReadyCommand(session, false);
                    baseController.Player.State.InEquipmentArea = false;
                    baseController.Player.State.InTradeArea = false;
                }
            }
            catch (Exception e)
            {
            }
            LastTimeCheckedObjects = DateTime.Now;
        }
    }
}
