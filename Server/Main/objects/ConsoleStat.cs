using System;
using System.Collections.Generic;

namespace Server.Main.objects
{
    class ConsoleStat
    {
        public const string DESTRUCTION_CONTROLLER_MODULE = "destruction";
        
        public const string ATTACK_CONTROLLER_MODULE = "attack";
        
        public const string DAMAGE_CONTROLLER_MODULE = "damage";
        
        public const string RANGE_CONTROLLER_MODULE = "range";

        public const string DB_CONTROLLER_MODULE = "db";

        public const string MAP_CONTROLLER_MODULE = "map";

        public const string MOVEMENT_CONTROLLER_MODULE = "move";
        
        public const string SPAWN_CONTROLLER_MODULE = "spawn";

        public const string HEAL_CONTROLLER_MODULE = "heal";
        
        public const string EXPLOSIVES_CONTROLLER_MODULE = "explode";
        
        public const string EFFECTS_CONTROLLER_MODULE = "effect";


        public Dictionary<string, UpdateTimeStatistic> UpdateTimes = new Dictionary<string, UpdateTimeStatistic>();
    }
}