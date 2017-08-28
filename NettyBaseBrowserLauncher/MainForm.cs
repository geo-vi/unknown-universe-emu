using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;

namespace NettyBaseBrowserLauncher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            TopMost = true;
            Opacity = 0.65;
            CenterToScreen();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LookForUpdate();
        }

        private async void LookForUpdate()
        {
            label1.Text = "Updating... Please wait.";
            while (progressBar1.Value != 100)
            {
                progressBar1.Value++;
                await Task.Delay(5);
            }
            progressBar1.Hide();
            IdleLoop();
        }

        public async void IdleLoop()
        {
            label1.Text = "Waiting for map launch on website...";
            while (GetCurrentPage() != "unknownuniverse.pw/index.php?action=internalMapRevolution")
            {
                await Task.Delay(150);
            }

            StartProcess();
            CloseForm();
        }

        private string GetCurrentPage()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");

            if (procsChrome.Length <= 0)
                return null;

            foreach (Process proc in procsChrome)
            {
                // the chrome process must have a window 
                if (proc.MainWindowHandle == IntPtr.Zero)
                    continue;

                // to find the tabs we first need to locate something reliable - the 'New Tab' button 
                AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);

                var SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));

                if (SearchBar != null)
                    return (string)SearchBar.GetCurrentPropertyValue(ValuePatternIdentifiers.ValueProperty);
            }

            return null;

        }

        private void StartProcess()
        {
            ProcessStartInfo proc = new ProcessStartInfo();
            proc.Arguments = "-SID \"sessionIdGoesHere\" -UID \"250\"";
            proc.FileName = "NettyBaseReloadedBrowser.exe";
            try
            {
                Process.Start(proc);
            }
            catch (Exception) { Debug.WriteLine("errorinio"); }
        }

        private void CloseForm()
        {
            TopMost = false;
            Close();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
        }
    }
}
