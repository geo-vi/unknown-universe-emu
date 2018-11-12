using System.Collections.Concurrent;
using System.Linq;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;

namespace NettyBaseReloaded.Game.controllers.implementable.checkers
{
    class ObjectRangeChecker : RangeChecker
    {
        public Character Character => Controller.Character;

        public ConcurrentDictionary<int, Object> DisplayedRangeObjects => Character.Range.Objects;

        public ConcurrentDictionary<int, Object> SpacemapObjects => Character.Spacemap.Objects;
        
        public override void Tick()
        {
            PerformCheck();
        }

        public void PerformCheck()
        {
            var allObjects = DisplayedRangeObjects.Concat(SpacemapObjects.Where( x=> !DisplayedRangeObjects.Keys.Contains(x.Key)));

            foreach (var spaceObject in allObjects)
            {
                var eValue = spaceObject.Value;
                
                if (Vector.IsInRange(eValue.Position, Character.Position, eValue.Range) && !DisplayedRangeObjects.ContainsKey(spaceObject.Key))
                {
                    AddObjectToRange(eValue);
                }
                else if (!Vector.IsInRange(eValue.Position, Character.Position, eValue.Range) && DisplayedRangeObjects.ContainsKey(spaceObject.Key))
                {
                    RemoveObjectFromRange(eValue);
                }
            }
        }

        public void AddObjectToRange(Object spaceObject)
        {
            //todo: add
        }

        public void RemoveObjectFromRange(Object spaceObject)
        {
            //todo: remove
        }
        
        
        public override void Reset()
        {
            //todo: reset
        }
    }
}