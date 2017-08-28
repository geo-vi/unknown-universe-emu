using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.commands.new_client
{
    class DisplaySettingsModule
    {
        public const short ID = 29050;

        private bool varN2e;
        private bool varD2I;
        private bool displayResources;
        private bool vard2k;
        private bool displayHitpointBubbles;
        private bool displayChat;
        private bool varN17;
        private bool varGx;
        private bool varA1P;
        private bool showNotOwnedItems;
        private bool var23m;
        private bool varR2C;
        private bool displayNotifications;
        private bool preloadUserShips;
        private bool varC4s;
        private bool varc4A;
        private bool vardu;
        private bool varb1H;
        private bool varVO;
        private int displaySetting3DqualityAntialias;
        private int varV4A;
        private int displaySetting3DqualityEffects;
        private int displaySetting3DqualityLights;
        private int displaySetting3DqualityTextures;
        private int varE4V;
        private int displaySetting3DsizeTextures;
        private int displaySetting3DtextureFiltering;
        private bool proActionBarEnabled;
        private bool proActionBarKeyboardInputEnabled;
        private bool proActionBarAutohideEnabled;
        private bool varIf;
        private bool varvD;

        public DisplaySettingsModule(bool varN2E, bool varD2I, bool displayResources, bool vard2K, bool displayHitpointBubbles, bool displayChat, bool varN17, bool varGx, bool varA1P, bool showNotOwnedItems, bool var23M, bool varR2C, bool displayNotifications, bool preloadUserShips, bool varC4S, bool varc4A, bool vardu, bool varb1H, bool varVo, int displaySetting3DqualityAntialias, int varV4A, int displaySetting3DqualityEffects, int displaySetting3DqualityLights, int displaySetting3DqualityTextures, int varE4V, int displaySetting3DsizeTextures, int displaySetting3DtextureFiltering, bool proActionBarEnabled, bool proActionBarKeyboardInputEnabled, bool proActionBarAutohideEnabled, bool varIf, bool varvD)
        {
            varN2e = varN2E;
            this.varD2I = varD2I;
            this.displayResources = displayResources;
            vard2k = vard2K;
            this.displayHitpointBubbles = displayHitpointBubbles;
            this.displayChat = displayChat;
            this.varN17 = varN17;
            this.varGx = varGx;
            this.varA1P = varA1P;
            this.showNotOwnedItems = showNotOwnedItems;
            var23m = var23M;
            this.varR2C = varR2C;
            this.displayNotifications = displayNotifications;
            this.preloadUserShips = preloadUserShips;
            varC4s = varC4S;
            this.varc4A = varc4A;
            this.vardu = vardu;
            this.varb1H = varb1H;
            varVO = varVo;
            this.displaySetting3DqualityAntialias = displaySetting3DqualityAntialias;
            this.varV4A = varV4A;
            this.displaySetting3DqualityEffects = displaySetting3DqualityEffects;
            this.displaySetting3DqualityLights = displaySetting3DqualityLights;
            this.displaySetting3DqualityTextures = displaySetting3DqualityTextures;
            this.varE4V = varE4V;
            this.displaySetting3DsizeTextures = displaySetting3DsizeTextures;
            this.displaySetting3DtextureFiltering = displaySetting3DtextureFiltering;
            this.proActionBarEnabled = proActionBarEnabled;
            this.proActionBarKeyboardInputEnabled = proActionBarKeyboardInputEnabled;
            this.proActionBarAutohideEnabled = proActionBarAutohideEnabled;
            this.varIf = varIf;
            this.varvD = varvD;
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(this.vard2k);
            cmd.Boolean(this.displayResources);
            cmd.Boolean(this.proActionBarKeyboardInputEnabled);
            cmd.Boolean(this.varD2I);
            cmd.Integer(this.displaySetting3DsizeTextures >> 10 | this.displaySetting3DsizeTextures << 22);
            cmd.Short(-24682);
            cmd.Boolean(this.varVO);
            cmd.Integer(this.displaySetting3DqualityAntialias << 1 | this.displaySetting3DqualityAntialias >> 31);
            cmd.Integer(this.displaySetting3DqualityLights << 4 | this.displaySetting3DqualityLights >> 28);
            cmd.Boolean(this.varN17);
            cmd.Boolean(this.proActionBarEnabled);
            cmd.Boolean(this.varR2C);
            cmd.Boolean(this.varvD);
            cmd.Boolean(this.varGx);
            cmd.Boolean(this.preloadUserShips);
            cmd.Boolean(this.varA1P);
            cmd.Integer(this.displaySetting3DqualityEffects >> 6 | this.displaySetting3DqualityEffects << 26);
            cmd.Boolean(this.varIf);
            cmd.Integer(this.displaySetting3DtextureFiltering >> 2 | this.displaySetting3DtextureFiltering << 30);
            cmd.Boolean(this.varc4A);
            cmd.Boolean(this.displayHitpointBubbles);
            cmd.Integer(this.varE4V >> 9 | this.varE4V << 23);
            cmd.Integer(this.displaySetting3DqualityTextures >> 15 | this.displaySetting3DqualityTextures << 17);
            cmd.Boolean(this.proActionBarAutohideEnabled);
            cmd.Boolean(this.vardu);
            cmd.Boolean(this.var23m);
            cmd.Boolean(this.displayChat);
            cmd.Integer(this.varV4A >> 2 | this.varV4A << 30);
            cmd.Boolean(this.varb1H);
            cmd.Short(-32657);
            cmd.Boolean(this.displayNotifications);
            cmd.Boolean(this.varC4s);
            cmd.Boolean(this.showNotOwnedItems);
            cmd.Boolean(this.varN2e);
            return cmd.Message.ToArray();
        }
    }
}
