using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Windows.Forms;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;

namespace NettyBaseReloaded.Game.controllers.implementable.checkers
{
    class EntityRangeChecker : RangeChecker
    {
        public Character Character => Controller.Character;
        
        public ConcurrentDictionary<int, Character> DisplayedRangeCharacters => Controller.Character.Range.Entities;

        public ConcurrentDictionary<int, Character> SpacemapEntities => Controller.Character.Spacemap.Entities;
        
        public override void Tick()
        {
            PerformCheck();
        }

        public void PerformCheck()
        {
            var allEntities = DisplayedRangeCharacters.Concat(SpacemapEntities.Where( x=> !DisplayedRangeCharacters.Keys.Contains(x.Key)));

            foreach (var entity in allEntities)
            {
                var eValue = entity.Value;
                if (eValue.InRange(Character) && !DisplayedRangeCharacters.ContainsKey(entity.Key))
                {
                    AddCharacterToDisplay(eValue);
                }
                else if (!eValue.InRange(Character) && DisplayedRangeCharacters.ContainsKey(entity.Key))
                {
                    RemoveCharacterFromDisplay(eValue);
                }
            }
        }

        public void AddCharacterToDisplay(Character character)
        {
            if (DisplayedRangeCharacters.TryAdd(character.Id, character) && Character is Player player)
            {
                var gameSession = player.GetGameSession();
                Packet.Builder.ShipCreateCommand(gameSession, character);
                Packet.Builder.DronesCommand(gameSession, character);

                //Send movement
                var timeElapsed = (DateTime.Now - character.MovementStartTime).TotalMilliseconds;
                Packet.Builder.MoveCommand(gameSession, character, (int) (character.MovementTime - timeElapsed));                
            }
        }

        public void RemoveCharacterFromDisplay(Character character)
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
                    Packet.Builder.ShipRemoveCommand(player.GetGameSession(), character);
                }
            }
        }

        public override void Reset()
        {
            foreach (var displayed in DisplayedRangeCharacters)
            {
                RemoveCharacterFromDisplay(displayed.Value);
            }
        }
    }
}