using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class Skilltree
    {
        #region Defensive skills
        /// <summary>
        /// 5,000 Extra HP (+ 5,000 to base hitpoints)
        /// 10,000 Extra HP (+5,000 to hitpoints)
        /// 15,000 Extra HP (+5,000 to hitpoints)
        /// 25,000 Extra HP (+10,000 to hitpoints)
        /// 50,000 Extra HP (+25,000 to hitpoints)
        /// </summary>
        public int ShipHull { get; set; }

        /// <summary>
        /// 5% Faster Repairs
        /// 10% Faster Repairs
        /// 15% Faster Repairs
        /// 20% Faster Repairs
        /// 30% Faster Repairs
        /// </summary>
        public int Engineering { get; set; }

        /// <summary>
        /// 4% increase in shield strength
        /// 8% increase in shield strength
        /// 12% increase in shield strength
        /// 18% increase in shield strength
        /// 25% increase in shield strength
        /// </summary>
        public int ShieldEngineering { get; set; }

        /// <summary> 
        /// 2% less likely to be hit
        /// 4% less likely to be hit
        /// 6% less likely to be hit
        /// 8% less likely to be hit
        /// 12% less likely to be hit
        /// </summary>
        public int EvasiveManeuvers { get; set; }

        /// <summary>
        /// Shields are 2% more resilient to damage.
        /// Shields are 4% more resilient to damage.
        /// Shields are 6% more resilient to damage.
        /// Shields are 8% more resilient to damage.
        /// Shields are 12% more resilient to damage.(Special visual effect that looks like a force field when attacked by an enemy. Mordon alien ship attacks do not activate this visual effect when player is attacked by one.)
        /// </summary>
        public int ShieldMechanics { get; set; }
        #endregion

        #region Loot / Rank Bonus skills
        /// <summary>
        /// More EXP per alien kill
        /// 2%	4%	6%	8%	12%
        /// </summary>
        public int Tactics { get; set; }

        /// <summary>
        /// Expands your cargo bay by___
        /// 4%	8%	12%	16%	25%	
        /// </summary>
        public int Logistics { get; set; }

        /// <summary>
        /// How much more bonus box Uridium
        /// 2%	4%	6%	8%	12%
        /// </summary>
        public int Luck { get; set; }

        /// <summary>
        /// How much extra honour you get
        /// 4%	8%	12%	18%	25%	
        /// </summary>
        public int Cruelty { get; set; }

        /// <summary>
        /// How much extra cargo you get from boxes (1-5)
        /// 1%	2%	3%	4%	6%	
        /// Increase loot from bonus boxes (6-10)
        /// 2%	6%	10%	15%	20%	
        /// </summary>
        public int TractorBeam { get; set; }

        /// <summary>
        /// How many extra credits you earn per alien kill
        /// 4%	8%	12%	18%	25%
        /// </summary>
        public int Greed { get; set; }
        #endregion

        #region Offensive skills
        public int Detonation { get; set; }
        public int Explosives { get; set; }
        public int HeatSeekingMissles { get; set; }
        public int BountyHunter { get; set; }
        public int RocketFusion { get; set; }
        public int AlienHunter { get; set; }
        public int ElectroOptics { get; set; }
        #endregion

        private Character Character { get; set; }

        public Skilltree(Character character)
        {
            Character = character;

            if (Character is Player player)
            {
                World.DatabaseManager.LoadSkilltree(player, this);
            }
        }

        public bool HasFatLasers()
        {
            return true;
        }

        public double GetLaserDamageBonus(bool isNpc)
        {
            var baseValue = 0.0;
            switch (BountyHunter)
            {
                case 1:
                    baseValue += 0.02;
                    break;
                case 2:
                    baseValue += 0.04;
                    break;
                case 3:
                    baseValue += 0.06;
                    break;
                case 4:
                    baseValue += 0.08;
                    break;
                case 5:
                    baseValue += 0.12;
                    break;
            }

            if (isNpc)
            {
                switch (AlienHunter)
                {
                    case 1:
                        baseValue += 0.02;
                        break;
                    case 2:
                        baseValue += 0.04;
                        break;
                    case 3:
                        baseValue += 0.06;
                        break;
                    case 4:
                        baseValue += 0.08;
                        break;
                    case 5:
                        baseValue += 0.12;
                        break;
                }
            }

            return baseValue;
        }
    }
}
