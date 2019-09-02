using System;
using System.Collections.Generic;

namespace Server.Game.objects.entities.players.statistics
{
    class PlayerMapStatistics
    {
        public List<int> MapsVisited = new List<int>();
        
        public DateTime LastJumpTime = new DateTime();

        public int MilesCrossedOnCurrentMap = 0;

        public int TotalMilesCrossed = 0;
    }
}