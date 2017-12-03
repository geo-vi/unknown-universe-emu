using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.controllers.implementable;
using NettyBaseReloaded.Game.objects.world;

namespace NettyBaseReloaded.Game.controllers.player
{
    class CPU : IChecker
    {
        public enum Types
        {
            ROBOT,
            CLOAK,
            JCPU
        }

        private PlayerController baseController;

        public CPU(PlayerController controller)
        {
            baseController = controller;
            foreach (var type in Enum.GetValues(typeof(Types))) Initiate((Types)type);
        }

        DateTime LastTimeCheckedCpus = new DateTime(2017, 1, 18, 0, 0, 0);

        public void Check()
        {
            if (LastTimeCheckedCpus.AddSeconds(1) > DateTime.Now) return;
            LastTimeCheckedCpus = DateTime.Now;

            if (baseController.Repairing) Robot();
        }

        public void Initiate(Types type)
        {
            var client = World.StorageManager.GetGameSession(baseController.Character.Id).Client;
            switch (type)
            {
                case Types.CLOAK:
                    //client.Send(Builder.LegacyModule("0|A|CPU|C|" + baseController.Player.GetCpuUsesLeft(type)).Bytes);
                    break;
            }
        }

        public void Activate(Types type)
        {
            switch (type)
            {
                case Types.ROBOT:
                    baseController.Repairing = true;
                    break;
            }
        }

        public void DeActivate(Types type)
        {
            switch (type)
            {
                case Types.ROBOT:
                    baseController.Repairing = false;
                    break;
            }
        }

        void Robot()
        {
            if (baseController.Character.Moving || !baseController.Repairing ||
                baseController.Character.LastCombatTime.AddSeconds(3) > DateTime.Now)
            {
                baseController.Repairing = false;
                return;
            }


            var player = (Player)baseController.Character;
            if (player.CurrentHealth == player.MaxHealth)
            {
                baseController.Repairing = false;
                return;
            }

            var repAmount = (player.MaxHealth / 100) * player.Equipment.GetRobotLevel();

            baseController.Attack.Heal(repAmount);
        }

        void Cloak()
        {

        }

        void AutoRocketLauncher()
        {

        }

        void DroneRepair()
        {

        }

        void JCPU()
        {

        }
    }
}
