using System;

namespace Server.Game.controllers.players
{
    class LogoutController : PlayerSubController
    {
        private DateTime LogoutProcessStart { get; set; }

        private DateTime EstimatedProcessFinish { get; set; }

        public bool InLogoutProcess { get; set; }

        public bool LoggingOut { get; set; }

        public LogoutController()
        {
            InLogoutProcess = false;
            LoggingOut = false;
        }
        
        public void CreateLogoutProcess(int time)
        {
        }

        private void InitiateLogout()
        {
            
        }

        public void AbortLogoutProcess()
        {
            if (LoggingOut)
            {
                return;
            }
        }
    }
}
