using System;
using System.Threading.Tasks;
using Server.Game.managers;
using Server.Game.netty.packet.prebuiltCommands;
using Server.Game.objects;
using Server.Game.objects.enums;
using Server.Main.objects;
using Server.Networking.clients;
using Server.Utils;

namespace Server.Game.controllers.players
{
    class SessionController : PlayerSubController
    {
        protected GameSession Session
        {
            get
            {
                var session = GameStorageManager.Instance.FindSession(Player);
                return session;
            }
        }
        
        public override void OnTick()
        {
            CalculateActivity();
        }

        private void CalculateActivity()
        {
            if (Session == null)
            {
                Out.QuickLog("Something went wrong with session", LogKeys.ERROR_LOG);
                throw new NullReferenceException("Session is undefined");
            }

            var loginTimePoints = (int)((DateTime.Now - Session.LoginTime).TotalMilliseconds) * 0.0001;
            var movementTimePoints = (int)((DateTime.Now - Session.LastMovementTime).TotalMilliseconds) * 0.001;
            var combatTimePoints = (int)((DateTime.Now - Session.LastCombatTime).TotalMilliseconds) * 0.001;

            if (loginTimePoints < 0)
            {
                //yet waiting to login
                return;
            }

            if (loginTimePoints > 100 && movementTimePoints + combatTimePoints < 0)
            {
                InactivityKick();
            }
            else if (loginTimePoints + movementTimePoints + combatTimePoints > 1000)
            {
                if (movementTimePoints < 100 || combatTimePoints < 50)
                {
                    //wrong calculation, was probably flying only lmao and if he just escaped from a fight?
                    return;
                }
                // high tolerance kick
                InactivityKick();
            }
        }

        protected void InactivityKick()
        {
            Out.WriteLog("Kicking player for inactivity", LogKeys.PLAYER_LOG, Player.Id);
            PrebuiltLegacyCommands.Instance.ServerMessage(Player, "Inactivity detected, you're kicked from the server");
            Kill();
        }

        public void SessionTakeover(GameClient pirateClient)
        {
            if (Session == null || !pirateClient.Initialized)
            {
                Out.WriteLog("Something went wrong when trying to takeover session", LogKeys.PLAYER_LOG, Player.Id);
                throw new Exception("Something went wrong when trying to takeover session");
            }

            Task.Run(Session.GameClient.Disconnect);
            Session.GameClient = pirateClient;
            pirateClient.UserId = -1;
        }

        public void Kill()
        {
            CharacterStateManager.Instance.RequestStateChange(Player, CharacterStates.FULLY_DISCONNECTED, out var stateChanged);
            if (!stateChanged)
            {
                throw new Exception("Something went wrong when trying to fully disconnect player");
            }
            
            Player.Controller.Dispose();
            Task.Run(Session.GameClient.Disconnect);
        }
    }
}
