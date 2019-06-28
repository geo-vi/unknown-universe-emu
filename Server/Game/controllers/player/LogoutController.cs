using System;

namespace Server.Game.controllers.player
{
    class LogoutController
    {
        private DateTime LogoutProcessStart;

        private DateTime EstimatedProcessFinish;

        public bool InLogoutProcess = false;

        public bool LoggingOut = false;
        
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
