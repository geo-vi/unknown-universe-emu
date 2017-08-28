using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world.map;
using Newtonsoft.Json;

namespace NettyBaseReloaded.Game.objects.world.storages.playerStorages
{
    class StatsStorage
    {
        public int Id { get; }

        // Key => npcId, Value => count
        public Dictionary<int, int> KilledShipsDictionary { get; set; }
        // Key => boxType, Value => count
        public Dictionary<int, int> CollectedBoxesDictionary { get; set; }

        public StatsStorage(int id, Dictionary<int, int> killedShipsDictionary, Dictionary<int, int> collectedBoxesDictionary)
        {
            Id = id;
            KilledShipsDictionary = killedShipsDictionary;
            CollectedBoxesDictionary = collectedBoxesDictionary;
        }

        public void CollectBox(Collectable box)
        {
            //if (CollectedBoxesDictionary.ContainsKey(box.Type))
            //    CollectedBoxesDictionary[box.Type] = CollectedBoxesDictionary[box.Type] += 1;
            //else CollectedBoxesDictionary.Add(box.Type, 1);
        }

        public void Destroy(Ship ship)
        {
            if (KilledShipsDictionary.ContainsKey(ship.Id))
                KilledShipsDictionary[ship.Id] = KilledShipsDictionary[ship.Id] += 1;
            else KilledShipsDictionary.Add(ship.Id, 1);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
