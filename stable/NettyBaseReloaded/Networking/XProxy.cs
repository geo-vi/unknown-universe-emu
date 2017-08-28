using System;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Fiddler;

namespace UOV3_Stealth
{
    internal class XProxy
    {
        #region Variables
        public int Port { get; }
        public bool DecryptSsl { get; set; }
        #endregion

        public event EventHandler<EventArgs> MainReplaced;

        public XProxy(int port)
        {
            Port = port;
            DecryptSsl = false; //By default
        }

        /// <summary>
        /// Starts the proxy
        /// </summary>
        public void Start()
        {
            //Stops everything before start
            Stop();

            FiddlerApplication.BeforeRequest += delegate (Session session)
            {
                //Console.WriteLine(session.host);
                if (session.host == "v3.uberorbit.net")
                {
                    session.bBufferResponse = true;
                }
            };

            FiddlerApplication.BeforeResponse += delegate (Session session)
            {
                if (session.host == "v3.uberorbit.net" && session.PathAndQuery.Contains("main.swf"))
                {
                    session.utilDecodeResponse();
                    session["x-replywithfile"] = Application.StartupPath + @"\main.swf";
                    OnMainReplaced();
                    Console.WriteLine("Successfuly replaced main.swf");
                }
            };

            FiddlerApplication.Startup(Port, true, DecryptSsl);
        }

        protected virtual void OnMainReplaced()
        {
            MainReplaced?.Invoke(this, EventArgs.Empty);
        }

        public void Stop()
        {
            FiddlerApplication.Shutdown();
        }
    }
}