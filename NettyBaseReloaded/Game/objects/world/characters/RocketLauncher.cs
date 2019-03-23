using System;
using System.Collections.Generic;
using System.Linq;
using NettyBaseReloaded.Game.controllers.player;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects.world.players;

namespace NettyBaseReloaded.Game.objects.world.characters
{
    class RocketLauncher
    {
        private Character Character;

        public RocketLaunchers[] RocketLaunchers;

        public int LoadedRockets;

        public bool ReadyForLaunch => LoadedRockets == MaxLoadableRockets;

        private int MaxLoadableRockets
        {
            get
            {
                var value = RocketLaunchers.Count(x => x == world.RocketLaunchers.HST_01) * 3 +
                            RocketLaunchers.Count(x => x == world.RocketLaunchers.HST_02) * 5;
                return value;
            }
        }

        public string LoadLootId;

        public bool Loading;

        private Ammunition LoadedAmmunition
        {
            get
            {
                if (Character is Player player && player.Information.Ammunitions.ContainsKey(LoadLootId))
                {
                    return player.Information.Ammunitions[LoadLootId];
                }

                return null;
            }
        }
        
        public RocketLauncher(Character character, RocketLaunchers[] launchers = null)
        {
            Character = character;
            RocketLaunchers = launchers;
            LoadLootId = "ammunition_rocketlauncher_eco-10";
            if (character is Player player)
            {
                LoadLootId = player.Settings.CurrentHellstorm.LootId;
            }
        }

        public void Tick()
        {
            if (Character is Player player)
            {
                if (player.Controller.CPUs.Active.Contains(CPU.Types.AUTO_ROCKLAUNCHER))
                    Loading = true;
            }
            else Loading = true;
            AddRocket();
        }

        private DateTime _lastLoadedRocketTime;
        private void AddRocket()
        {
            if (!Loading || LoadedRockets == MaxLoadableRockets || _lastLoadedRocketTime.AddSeconds(1) >= DateTime.Now) return;
            _lastLoadedRocketTime = DateTime.Now;
            if (Character is Player player)
            {
                if (LoadedAmmunition.Shoot() == 0)
                {
                    Loading = false;
                }
                else LoadedRockets++;
                Packet.Builder.HellstormStatusCommand(player.GetGameSession());
            }
            else LoadedRockets++;
            if (LoadedRockets == MaxLoadableRockets) Loading = false;
        }

        public void Shoot(int loadedAmount)
        {
            LoadedRockets -= loadedAmount;
            Loading = false;
        }

        public void ChangeLoad(string lootId)
        {
            GameSession gameSession = null;
            if (Character is Player player)
            {
                LoadedAmmunition.Add(LoadedRockets);
                gameSession = player.GetGameSession();
            }
            
            LoadedRockets = 0;
            LoadLootId = lootId;
            if (gameSession != null)
                Packet.Builder.HellstormStatusCommand(gameSession);
        }

        public List<int> GetLaunchersInt()
        {
            return RocketLaunchers.Select(launcher => (int) launcher).ToList();
        }
    }
}
