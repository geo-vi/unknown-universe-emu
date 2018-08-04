using System;
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
    class Checkers : IAbstractCharacter, ITick
    {
        public int VisibilityRange { get; set; }

        public Checkers(AbstractCharacterController controller) : base(controller)
        {
            VisibilityRange = 2000;//900
            Character.Spacemap.EntityAdded += AddedToSpacemap;
            Character.Spacemap.EntityRemoved += RemovedFromSpacemap;
        }

        private void RemovedFromSpacemap(object sender, CharacterArgs e)
        {
            //throw new NotImplementedException();
        }

        private void AddedToSpacemap(object sender, CharacterArgs e)
        {
            //throw new NotImplementedException();
        }


        public override void Tick()
        {
            PerformSpacemapCheck();
        }

        public override void Stop()
        {
            
        }

        private Dictionary<int, Character> _rangeEntries = new Dictionary<int, Character>();
        private void PerformSpacemapCheck()
        {
            var spacemap = Character.Spacemap;
            foreach (var mapEntry in spacemap.Entities)
            {
                if (mapEntry.Value.Position.DistanceTo(Character.Position) < VisibilityRange)
                {
                    if (!_rangeEntries.ContainsKey(mapEntry.Key))
                        _rangeEntries.Add(mapEntry.Key, mapEntry.Value);
                }
                else if (_rangeEntries.ContainsKey(mapEntry.Key))
                {
                    _rangeEntries.Remove(mapEntry.Key);
                }
            }
        }
    }
}
