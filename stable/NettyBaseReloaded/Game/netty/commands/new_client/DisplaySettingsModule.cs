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
        public const short ID = 31318;

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

        public DisplaySettingsModule(bool varN2E, bool varD2I, bool displayResources, bool vard2K, bool displayHitpointBubbles, bool displayChat, bool varN17, bool varGx, bool varA1P, bool showNotOwnedItems, bool var23M, bool varR2C, bool displayNotifications, bool preloadUserShips, bool varC4S, bool varc4A, bool vardu, bool varb1H, bool varVo, int displaySetting3DqualityAntialias, int varV4A, int displaySetting3DqualityEffects, int displaySetting3DqualityLights, int displaySetting3DqualityTextures, int varE4V, int displaySetting3DsizeTextures, int displaySetting3DtextureFiltering, bool proActionBarEnabled, bool proActionBarKeyboardInputEnabled, bool proActionBarAutohideEnabled, bool varIf)
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
        }

        public byte[] write()
        {
            var cmd = new ByteArray(ID);
            cmd.Boolean(varC4s);
            cmd.Boolean(var23m);
            cmd.Boolean(displayNotifications);
            cmd.Boolean(showNotOwnedItems);
            cmd.Integer(varE4V << 3 | varE4V >> 29);
            cmd.Boolean(varN2e);
            cmd.Boolean(vardu);
            cmd.Integer(displaySetting3DsizeTextures >> 3 | displaySetting3DsizeTextures << 29);
            cmd.Integer(displaySetting3DqualityAntialias << 1 | displaySetting3DqualityAntialias >> 31);
            cmd.Boolean(vard2k);
            cmd.Integer(displaySetting3DtextureFiltering << 10 | displaySetting3DtextureFiltering >> 22);
            cmd.Boolean(varGx);
            cmd.Boolean(varR2C);
            cmd.Boolean(proActionBarKeyboardInputEnabled);
            cmd.Boolean(proActionBarAutohideEnabled);
            cmd.Integer(varV4A << 7 | varV4A >> 25);
            cmd.Integer(displaySetting3DqualityTextures << 16 | displaySetting3DqualityTextures >> 16);
            cmd.Boolean(varc4A);
            cmd.Integer(displaySetting3DqualityEffects << 9 | displaySetting3DqualityEffects >> 23);
            cmd.Boolean(varVO);
            cmd.Boolean(varD2I);
            cmd.Boolean(preloadUserShips);
            cmd.Boolean(varIf);
            cmd.Boolean(varN17);
            cmd.Integer(displaySetting3DqualityLights << 15 | displaySetting3DqualityLights >> 17);
            cmd.Boolean(varb1H);
            cmd.Boolean(displayChat);
            cmd.Boolean(displayHitpointBubbles);
            cmd.Boolean(proActionBarEnabled);
            cmd.Boolean(varA1P);
            cmd.Boolean(displayResources);
            return cmd.Message.ToArray();
        }
    }
}
