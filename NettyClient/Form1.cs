using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AxShockwaveFlashObjects;

namespace NettyClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var tabPageBuilder = new TabPage();
            tabPageBuilder.Text = textBox1.Text;
            var flashObj = new AxShockwaveFlash();
            tabPageBuilder.Controls.Add(flashObj);
            tabControl1.TabPages.Add(tabPageBuilder);
            flashObj.Dock = DockStyle.Fill;
            flashObj.WMode = "Direct";
            flashObj.LoadMovie(0, "http://ge1.dev.univ3rse.com/spacemap/loadingscreen.swf");
            flashObj.FlashVars = "lang=en&loadingClaim=UNIV3RSE&userID=1001&sessionID=opk7fv0rq8lnev2n4vrts1u9i3&basePath=spacemap&pid=390&resolutionID=0&boardLink=?boardLink&helpLink=?helpLink&chatHost=dev.univ3rse.com&cdn=http://ge1.dev.univ3rse.com/&useHash=1&host=dev.univ3rse.com&localGS=0&browser=Chrome&fullscreen=1&errortracking=1&gameXmlHash=8308af173e550899b2c22cc7a7334f00&resourcesXmlHash=2bb6860545fe461c6607203218288a00&profileXmlHash=e6c5f6627f9a7b9bb7bf5471e08a1500&loadingscreenHash=ddc84a3e9bd358b9af65859114631900&gameclientHash=none&gameclientPath=spacemap&loadingscreenAssetsXmlHash=1c540d399333ca7cc1755735a6082100&gameclientAllowedInitDelay=100&eventStreamContext=eyJwaWQiOjM5MCwidWlkIjoxNjAwNTI5NjUsInRpZCI6IjMyYjY5Y2FhYzg3MWU1MmVhNTg4ZmIyZjA4NjY0ZmIzIiwiaWlkIjoiN2VjYzhjYjQxZTg5M2EwMTE1YzQ5NzhhNTM5OWY5ODMiLCJzaWQiOiI3YjA2MmIxNTQ2OGE1ZWNlMmU2NTAzMzI5ODNkNDYwOCIsImN0aW1lIjoxNDAzNTM0NDQ5NjE3fQ&useDeviceFonts=1&factionID=1&autoStartEnabled=0&mapID=255&allowChat=1";
        }
    }
}
