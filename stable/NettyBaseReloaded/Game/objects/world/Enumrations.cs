using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world
{
    #pragma warning disable 1591
    enum RewardType
    {
        CREDITS,
        URIDIUM,
        EXPERIENCE,
        HONOR,
        ITEM,
        GALAXY_GATES_ENERGY, 
        BOOSTER,
        AMMO,
        ORE
    }
    
    /// <summary>
    /// Faction Ids
    /// </summary>
    public enum Faction
    {
        NONE = 0,
        MMO = 1,
        EIC = 2,
        VRU = 3
    }

    /// <summary>
    /// Rank Ids
    /// </summary>
    public enum Rank
    {
        NONE = 0,
        BASIC_PILOT = 1,
        SPACE_PILOT = 2,
        CHIEF_PILOT = 3,
        BASIC_SERGEANT = 4,
        SERGEANT = 5,
        CHIEF_SERGEANT = 6,
        BASIC_LIEUTENANT = 7,
        LIEUTENANT = 8,
        CHIEF_LIEUTENANT = 9,
        BASIC_CAPTAIN = 10,
        CAPTAIN = 11,
        CHIEF_CAPTAIN = 12,
        BASIC_MAJOR = 13,
        MAJOR = 14,
        CHIEF_MAJOR = 15,
        BASIC_COLONEL = 16,
        COLONEL = 17,
        CHIEF_COLONEL = 18,
        BASIC_GENERAL = 19,
        GENERAL = 20,
        ADMINISTRATOR = 21,
        WANTED = 22
    }

    /// <summary>
    /// Drone types
    /// </summary>
    public enum DroneType
    {
        UNDEFINED = 0,
        FLAX = 1,
        IRIS = 2,
        APIS = 3,
        ZEUS = 4
    }

    public enum DroneFormation
    {
        STANDARD,
        TURTLE,
        ARROW,
        LANCE,
        STAR,
        PINCER,
        DOUBLE_ARROW,
        DIAMOND,
        CHEVRON,
        MOTH,
        CRAB,
        HEART,
        BARRAGE,
        BAT
    }

    public enum DamageType
    {
        DEFINED,
        PERCENTAGE
    }

    public enum HealType
    {
        HEALTH,
        SHIELD
    }

    public enum AILevels
    {
        PASSIVE,
        AGGRESSIVE,
        MOTHERSHIP,
        DAUGHTER,
        GALAXY_GATES,
        INVASION
    }
    #pragma warning restore 1591

}
