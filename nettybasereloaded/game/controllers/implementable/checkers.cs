using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.zones;
using NettyBaseReloaded.Main;
using NettyBaseReloaded.Main.interfaces;
using NettyBaseReloaded.Main.objects;

namespace NettyBaseReloaded.Game.controllers.implementable
{
    class Checkers : IAbstractCharacter
    {
        public int VisibilityRange { get; set; }

        //public int PacketSendRange => 1000;

        public bool InVisibleZone => !Character.Range.Zones.Any(x => x.Value is PalladiumZone);

        public Checkers(AbstractCharacterController controller) : base(controller)
        {
            VisibilityRange = 2000;//900
        }

        public void Start()
        {
        }

        public override void Tick()
        {
            EntityChecker();
            ZoneChecker();
            ObjectChecker();
        }

        public override void Stop()
        {
            ResetEntityRange();
            Character.Range.Objects.Clear();
            Character.Range.Resources.Clear();
            Character.Range.Collectables.Clear();
            Character.Range.Zones.Clear();
        }

        #region Character related

        private ConcurrentDictionary<int, Character> DisplayedRangeCharacters => Controller.Character.Range.Entities;

        private ConcurrentDictionary<int, Character> SpacemapEntities => Controller.Character.Spacemap.Entities;

        private void EntityChecker()
        {
            foreach (var entity in SpacemapEntities)
            {
                var eValue = entity.Value;
                if (Character.InRange(eValue) && !DisplayedRangeCharacters.ContainsKey(entity.Key))
                {
                    AddCharacterToDisplay(eValue);
                }
            }

            foreach (var entity in DisplayedRangeCharacters)
            {
                var eValue = entity.Value;
                if (!Character.InRange(eValue))
                {
                    RemoveCharacterFromDisplay(eValue);
                }
            }
        }

        private void AddCharacterToDisplay(Character character)
        {
            if (DisplayedRangeCharacters.TryAdd(character.Id, character) && Character is Player player)
            {
                var gameSession = player.GetGameSession();
                Packet.Builder.ShipCreateCommand(gameSession, character);
                Packet.Builder.DronesCommand(gameSession, character);

                //Send movement
                var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;
                Packet.Builder.MoveCommand(gameSession, character, (int) (character.MovementTime - timeElapsed));
                if (character.Invisible)
                {
                    Packet.Builder.LegacyModule(gameSession, "0|n|INV|" + Controller.Character.Id + "|" +
                                                            Convert.ToInt32(Controller.Character.Invisible));
                }
            }
        }

        private void RemoveCharacterFromDisplay(Character character)
        {
            Character removed;
            if (DisplayedRangeCharacters.TryRemove(character.Id, out removed))
            {
                if (Character.Selected == character)
                {
                    Character.RemoveSelection();
                }

                if (Character is Player player)
                {
                    //Console.WriteLine("Removing @POS: " + character.Position.ToPacket());
                    //Packet.Builder.LegacyModule(player.GetGameSession(), $"0|ps|png|{character.Position.X}|{character.Position.Y}");
                    Packet.Builder.ShipRemoveCommand(player.GetGameSession(), character);
                }
            }
        }

        public void ResetEntityRange()
        {
            foreach (var displayed in DisplayedRangeCharacters)
            {
                RemoveCharacterFromDisplay(displayed.Value);
            }

        }
        
        #endregion
        #region Zone related
        private void ZoneChecker()
        {
            try
            {
                foreach (var zone in Character.Spacemap.Zones.Values.ToList())
                {
                    if ((Character.Position.X >= zone.TopLeft.X && Character.Position.X <= zone.BottomRight.X) &&
                        (Character.Position.Y <= zone.TopLeft.Y && Character.Position.Y >= zone.BottomRight.Y))
                    {
                        if (!Character.Range.Zones.ContainsKey(zone.Id))
                        {
                            Character.Range.Zones.Add(zone.Id, zone);
                        }
                    }
                    else
                    {
                        if (Character.Range.Zones.ContainsKey(zone.Id)) Character.Range.Zones.Remove(zone.Id);
                    }
                }
            }
            catch (Exception e)
            {
                if (Character.Position == null || Character.Spacemap == null) return;
            }
        }
        #endregion
        #region Object related
        private void ObjectChecker()
        {
            if (!(Character is Player)) return;
            try
            {
                foreach (var obj in Character.Spacemap.Objects.Values)
                {
                    if (obj == null) continue;
                    if (Vector.IsInRange(obj.Position, Character.Position, obj.Range))
                    {
                        if (!Character.Range.Objects.ContainsKey(obj.Id))
                        {
                            Character.Range.AddObject(obj);
                            obj.execute(Character);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                    else
                    {
                        if (Character.Range.Objects.ContainsKey(obj.Id))
                        {
                            Character.Range.RemoveObject(obj);
                            (Character as Player)?.ClickableCheck(obj);
                        }
                    }
                }
                if (Character.Range.Objects.Count != Character.Spacemap.Objects.Count)
                {
                    var diff = Character.Range.Objects.Except(Character.Spacemap.Objects).Concat(Character.Spacemap.Objects.Except(Character.Range.Objects));
                    foreach (var objDiff in diff)
                    {
                        if (objDiff.Value == null) continue;
                        if (objDiff.Value.Position == null)
                        {
                            Character.Range.RemoveObject(objDiff.Value);
                            (Character as Player)?.ClickableCheck(objDiff.Value);
                        }
                    }
                }
            }
            catch (Exception)
            {
                if (Character?.Position == null || Character?.Spacemap == null) return;
                //new ExceptionLog("checkers", "Object Checker", e);
                //Error in checkers->Disconnecting player
               // World.StorageManager.GetGameSession(Character.Id)?.Disconnect(GameSession.DisconnectionType.ERROR);
            }
        }
        #endregion
    }
}
