using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using NettyBaseReloadedBrowser.Utils;

namespace NettyBaseReloadedBrowser
{
    public partial class MainForm : Form
    {
        public static string Status { get; set; }

        public Form Overlay { get; set; }
        public Form Esc { get; set; }

        private int CurrentPing { get; set; }

        public MainForm()
        {
            KeyPreview = true;
            InitializeComponent();
            Init();
        }

        public async void Ticker()
        {
            while (true)
            {
                UpdatePing();
                await Task.Delay(1000 / 64);
            }
        }

        private void Init()
        {
            KeyDown += OnKeyDown;
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            homePanel.Dock = DockStyle.Fill;
            mapPanel.Dock = DockStyle.Fill;
            homePanel.Hide();
            mapPanel.Show();
            mapPanel.Controls.Add(axShockwaveFlash1);
            this.Controls.Remove(axShockwaveFlash1);
            axShockwaveFlash1.Dock = DockStyle.Fill;
            // TODO: Move stuff that don't belong to here
            //axShockwaveFlash1.FlashVars = 
            //    "lang=en&userID=250&sessionID=6h8bi6pegkk8uradb9sjj0lo51&basePath=spacemap&pid=390&resolutionID=2&boardLink=?boardLink&helpLink=?helpLink&chatHost=unknownuniverse.pw&cdn=http://unknownuniverse.pw/&useHash=1&host=unknownuniverse.pw&browser=Chrome&fullscreen=1&errortracking=1&gameXmlHash=8308af173e550899b2c22cc7a7334f00&resourcesXmlHash=2bb6860545fe461c6607203218288a00&profileXmlHash=e6c5f6627f9a7b9bb7bf5471e08a1500&loadingscreenHash=ddc84a3e9bd358b9af65859114631900&gameclientHash=none&gameclientPath=spacemap&loadingscreenAssetsXmlHash=1c540d399333ca7cc1755735a6082100&showAdvertisingHint=&gameclientAllowedInitDelay=100&eventStreamContext=eyJwaWQiOjM5MCwidWlkIjoxNjAwNTI5NjUsInRpZCI6IjMyYjY5Y2FhYzg3MWU1MmVhNTg4ZmIyZjA4NjY0ZmIzIiwiaWlkIjoiN2VjYzhjYjQxZTg5M2EwMTE1YzQ5NzhhNTM5OWY5ODMiLCJzaWQiOiI3YjA2MmIxNTQ2OGE1ZWNlMmU2NTAzMzI5ODNkNDYwOCIsImN0aW1lIjoxNDAzNTM0NDQ5NjE3fQ&useDeviceFonts=1&factionID=1&mapID=12&allowChat=1";
            axShockwaveFlash1.WMode = "Direct";
            axShockwaveFlash1.FlashVars =
                "lang=us&userID=495&sessionID=oqasddp1b4ervgimem1g5jts76&basePath=spacemap&pid=89&boardLink=&helpLink=&loadingClaim=LOADING&chatHost=unknownuniverse.pw&cdn=&useHash=1&host=dev-shock.unknownuniverse.pw/browser/gamefiles&browser=Chrome&fullscreen=1&gameXmlHash=&resourcesXmlHash=252475c240ec76a4e1e0ea41861f4200&profileXmlHash=d77d1a04740e7a5b23d0602dd1c30300&languageXmlHash=47342756a7914febc0a3562435d2ab00&loadingscreenHash=9f1d0689a62675eb04e5edc65788c900&gameclientHash=900ad0c95299418c7138aec826259200&gameclientPath=spacemap&loadingscreenAssetsXmlHash=1c540d399333ca7cc1755735a6082100&showAdvertisingHint=&gameclientAllowedInitDelay=10&eventStreamContext=&useDeviceFonts=0&display2d=0&autoStartEnabled=0&mapID=1&allowChat=1";
            axShockwaveFlash1.LoadMovie(0, "http://dev-shock.unknownuniverse.pw/browser/gamefiles/spacemap/preloader.swf");
            //axShockwaveFlash1.LoadMovie(0, "http://unknownuniverse.pw/spacemap/loadingscreen.swf");     // 7.5.3    
        }

        private void OnKeyDown(object sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Oemtilde)
            {
                if (Overlay != null)
                {
                    if (!Overlay.Visible) Overlay = new Overlay(this);
                    else Overlay.Close();
                }
                else Overlay = new Overlay(this);
            }
            else if (keyEventArgs.KeyCode == Keys.Escape)
            {
                if (Esc != null)
                {
                    if (!Esc.Visible) Esc = new Esc(this);
                    else Esc.Close();
                }
                else Esc = new Esc(this);
            }

        }

        private Bitmap Snap()
        {
            Bitmap bmp = Screenshot.TakeSnapshot(this);
            return bmp;
        }


        public Rectangle GetScreen()
        {
            return Screen.PrimaryScreen.WorkingArea;
        }

        private DateTime LastPingCheck = new DateTime(2016, 12, 24, 0, 0, 0);
        private void UpdatePing()
        {
            if (LastPingCheck.AddSeconds(1) < DateTime.Now)
            {
                var ping = new Ping();
                PingReply reply = ping.Send("213.32.95.48");
                CurrentPing = (int)reply.RoundtripTime;
                LastPingCheck = DateTime.Now;
            }
        }

        public void Minimize()
        {
            KillOverlays();
            WindowState = FormWindowState.Minimized;
        }

        public void Exit()
        {
            KillOverlays();
            Application.Exit(); 
        }

        private void KillOverlays()
        {
            if (Esc != null)
            {
                if (Esc.Visible) Esc.Close();
            }
            if (Overlay != null)
            {
                if (Overlay.Visible) Overlay.Close();
            }
        }
    }
}
