using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.managers
{
    class PortalSystemManager
    {
        public List<int> GetOpenMapsList(Player player)
        {
            List<int> openMaps = new List<int>(); // 0
            if (player.Information.Level.Id >= 1)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 1, 2 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 5, 6 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 9, 10 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 2)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 3 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 7 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 11 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 3)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 4 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 8 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 12 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 5)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 8, 12, 7, 11 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 4, 12, 3, 11 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 4, 8, 3, 7 });
                        break;
                }
            }
            if (player.Information.Level.Id >= 8)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 13, 14, 15 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 13, 14, 15 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 13, 14, 15 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 9)
            {
                openMaps.AddRange(new List<int> { 16 });
            }

            if (player.Information.Level.Id >= 10)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 17 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 21 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 25 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 11)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 18, 19 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 22, 23 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 26, 27 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 12)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 20 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 24 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 28 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 13)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 6,10 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 2,10 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 2,6 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 14)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 21,25 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 17,25 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 17,21 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 15)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 22, 23, 26, 27 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 18, 19, 26, 27 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 18, 19, 22, 23});
                        break;
                }
            }

            if (player.Information.Level.Id >= 16)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 5,9 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 1,9 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 1,5 });
                        break;
                }
            }

            if (player.Information.Level.Id >= 17)
            {
                switch (player.FactionId)
                {
                    case Faction.MMO:
                        openMaps.AddRange(new List<int> { 24, 28 });
                        break;
                    case Faction.EIC:
                        openMaps.AddRange(new List<int> { 20, 28 });
                        break;
                    case Faction.VRU:
                        openMaps.AddRange(new List<int> { 20, 24 });
                        break;
                }
            }
            return openMaps;
        }

        public int CalculateDistance(int mapId, int destinationMapId)
        {
            return Math.Abs(destinationMapId - mapId);
        }
    }
}
