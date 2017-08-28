using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;

namespace NettyBaseReloaded.Game.objects.world.players
{
    class RocketLauncher
    {
        // TODO: Recode

        public const int NO_LAUNCHER = 0;
        public const int HST_1 = 1;
        public const int HST_2 = 2;


        public int Id { get; set; }

        public List<int> Launchers { get; set; }
        public int LoadedRockets { get; set; }

        public short SelectedRocketType;

        public RocketLauncher(int id, List<int> launchers, int loadedRockets, short selectedRocketType)
        {
            Id = id;

            Launchers = launchers;
            LoadedRockets = loadedRockets;
            SelectedRocketType = selectedRocketType;
        }

        public int GetMaxLoad()
        {
            int max = 0;
            foreach (var launcher in Launchers)
            {
                switch (launcher)
                {
                    case HST_1:
                        max += 3;
                        break;
                    case HST_2:
                        max += 5;
                        break;
                }
            }
            return max;
        }

        private bool Loading = false;
        public bool Running = false;
        public bool ReloadCooldown = false;

        public void Load()
        {
            if (Loading || LoadedRockets == GetMaxLoad())
            {
                if (Running && LoadedRockets == GetMaxLoad())
                    Running = false;

                return;
            }

            if (!Running)
            {
                Running = true;
                LoadLoop();
                return;
            }

            if (ReloadCooldown) return;

            LoadedRockets += 1;
            Update();
        }

        public void Cooldown()
        {
//            ReloadCooldown = true;
//            World.StorageManager.GetGameSession(Id).Player.Controller.CooldownStorage.HellstormCooldownEnd = DateTime.Now.AddSeconds(3);
        }

        async void LoadLoop()
        {
            while (Running)
            {
                Load();
                Loading = true;
                await Task.Delay(1000);
                Loading = false;
            }
        }

        public void Update()
        {
            World.StorageManager.GetGameSession(Id).Client.Send(GetBytes());
        }

        public void Empty()
        {
            LoadedRockets = 0;
            Update();
        }

        public void ChangeRocketType(short newRocket)
        {
            SelectedRocketType = newRocket;
            Empty();
        }

        public byte[] GetBytes()
        {
            return null;
            //return HellstormStatusCommand.write(Launchers, new AmmunitionTypeModule(SelectedRocketType), LoadedRockets);
        }
    }
}
