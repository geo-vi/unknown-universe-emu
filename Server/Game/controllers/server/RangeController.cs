using Server.Main.objects;

namespace Server.Game.controllers.server
{
    class RangeController : ITick
    {
        public int TickId { get; set; }

        public void Tick()
        {

        }

        public void GameObjectRangeTicker()
        {

        }

        public void EntityRangeTicker()
        {
            //todo: improve
            //foreach (var entity in Spacemap.Entities)
            //{
            //    if (!Spacemap.RangeDisabled)
            //    {
            //        var entitiesInRange =
            //            Spacemap.Entities.Where(x => x.Value != entity.Value && x.Value.Position.DistanceTo(entity.Value.Position));
            //        foreach (var rangeEntityHit in entitiesInRange)
            //        {
            //            AddToEntityRange(entity.Value, rangeEntityHit);
            //        }

            //    }
            //    else
            //    {
            //        var entities = Spacemap.Entities.Where(x => x.Key != entity.Key);
            //        foreach (var rangeEntityHit in entities)
            //        {
            //            AddToEntityRange(entity.Value, rangeEntityHit);
            //        }
            //    }
            //}
        }
    }
}
