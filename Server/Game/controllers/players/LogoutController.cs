using System;
using Server.Game.managers;
using Server.Game.objects.stateMachine;

namespace Server.Game.controllers.players
{
    class LogoutController : PlayerSubController
    {
        private DateTime LogoutProcessStart { get; set; }

        private DateTime EstimatedProcessFinish { get; set; }

        //IF CURRENTLY SESSION IS LOGGING OUT
        public bool InLogoutProcess { get; set; }
        
        //TIME NEEDED FOR DISCONNECT IN MILLISECONDS
        private const int INACTIVITY_TIME_NEEDED = 3000;
        
        public LogoutController()
        {
            InLogoutProcess = false;
        }
        
        /// <summary>
        /// Creating a logout process, which will kill session within time
        /// </summary>
        /// <param name="time">Time in MS</param>
        public void CreateLogoutProcess(int time)
        {
            EstimatedProcessFinish = DateTime.Now.AddMilliseconds(time);
            LogoutProcessStart = DateTime.Now;
            InLogoutProcess = true;
        }

        /// <summary>
        /// Each tick it will check if the session is in logout process
        /// if it is, check if time is finished and disconnect
        /// </summary>
        public override void OnTick()
        {
            if (!InLogoutProcess) return;
            
            var session = GameStorageManager.Instance.FindSession(Player);
            if (session == null || session.LastCombatTime.AddMilliseconds(INACTIVITY_TIME_NEEDED) < DateTime.Now && session.LastMovementTime.AddMilliseconds(INACTIVITY_TIME_NEEDED) < DateTime.Now
                && EstimatedProcessFinish < DateTime.Now)
            {
                Player.Controller.GetInstance<SessionController>().Kill();
            }
        }

        /// <summary>
        /// Will abort the current logout sequence
        /// </summary>
        public void AbortLogoutProcess()
        {
            if (InLogoutProcess)
            {
                EstimatedProcessFinish = DateTime.MaxValue;
                InLogoutProcess = false;
            }
        }
    }
}
