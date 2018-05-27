using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.players.extra.techs;
using Types = NettyBaseReloaded.Game.objects.world.map.pois.Types;

namespace NettyBaseReloaded.Game.controllers.login
{
    abstract class ILogin
    {
        public GameSession GameSession;


        protected ILogin(GameSession gameSession)
        {
            GameSession = gameSession;
        }

        /// <summary>
        /// Executable for login
        /// </summary>
        public abstract void Execute();

        public void SendSettings()
        {
            Packet.Builder.HotkeysCommand(GameSession);
            Packet.Builder.UserSettingsCommand(GameSession);
            Packet.Builder.SendUserSettings(GameSession);
        }

        public void SendLegacy()
        {
            SendLegacy(GameSession);
            SendCooldowns(GameSession);
        }

        private void SendCooldowns(GameSession gameSession)
        {
            foreach (var cooldown in gameSession.Player.Cooldowns.Cooldowns)
            {
                cooldown.Send(gameSession);
            }
        }

        public static void SendLegacy(GameSession GameSession)
        {
            Packet.Builder.DronesCommand(GameSession, GameSession.Player);
            //Packet.Builder.LegacyModule(GameSession, "0|n|t|" + GameSession.Player.Id + "|222|most_wanted");

            Packet.Builder.LegacyModule(GameSession, "0|A|BK|0"); //green booty
            Packet.Builder.LegacyModule(GameSession, "0|A|BKR|0"); //red booty
            Packet.Builder.LegacyModule(GameSession, "0|A|BKB|0"); //blue booty
            Packet.Builder.LegacyModule(GameSession, "0|TR");
            Packet.Builder.LegacyModule(GameSession, "0|A|CC|" + GameSession.Player.CurrentConfig);
            Packet.Builder.LegacyModule(GameSession, "0|ps|nüscht|");
            Packet.Builder.LegacyModule(GameSession, "0|ps|blk|0");
            Packet.Builder.LegacyModule(GameSession, "0|g|a|b,1000,1,10000.0,C,2,500.0,U,3,1000.0,U,5,4000.0,U|r,100,1,10000,C,2,50000,C,3,500.0,U,4,700.0,");
            GameSession.Player.LoadExtras();
            //Packet.Builder.VideoWindowCreateCommand(GameSession, 1, "c", true, new List<string> { "login_dialog_1", "login_dialog_2" }, 0, 1);
            //Packet.Builder.MineCreateCommand(GameSession, "asdf", 6, GameSession.Player.Position, false);

            Packet.Builder.PetInitializationCommand(GameSession, GameSession.Player.Pet);
            Packet.Builder.HellstormStatusCommand(GameSession);

            Packet.Builder.LegacyModule(GameSession, "0|n|w|0");

            //MBA -> MenuButtonAccess
            //DB -> Disable button
            //EB -> Enable button
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|7");
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|6");
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|2");
            Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|4");
            //Packet.Builder.LegacyModule(GameSession, "0|UI|MBA|DB|5");

            Packet.Builder.LegacyModule(GameSession
                , "0|A|CC|" + GameSession.Player.CurrentConfig);

            if (GameSession.Player.Group != null)
                Packet.Builder.GroupInitializationCommand(GameSession);

            if (GameSession.Player.Information.Title != null)
                Packet.Builder.TitleCommand(GameSession, GameSession.Player);
            GameSession.Player.Information.Premium.Login(GameSession);

            Packet.Builder.QuestInitializationCommand(GameSession);

            CreateFormations(GameSession);
            CreateTechs(GameSession);
            CreateAbilities(GameSession);
            //Packet.Builder.EventActivationStateCommand(GameSession, EventActivationStateCommand.APRIL_FOOLS, true);
        }

        public void InitiateEvents()
        {
            foreach (var gameEvent in World.StorageManager.Events)
            {
                if (gameEvent.Value.Active)
                    World.DatabaseManager.LoadEventForPlayer(gameEvent.Key, GameSession.Player);
            }
            foreach (var gameEvent in GameSession.Player.EventsPraticipating)
                gameEvent.Value.Start();
        }

        private static void CreateTechs(GameSession session)
        {
            session.Player.Techs.Add(new RocketPrecission(session.Player));
            session.Player.Techs.Add(new ShieldBuff(session.Player));
            session.Player.Techs.Add(new BattleRepairRobot(session.Player));
            session.Player.Techs.Add(new EnergyLeech(session.Player));
            session.Player.Techs.Add(new ChainImpulse(session.Player));

            Packet.Builder.TechStatusCommand(session);
        }

        private static void CreateFormations(GameSession session)
        {
            Packet.Builder.DroneFormationAvailableFormationsCommand(session);
        }

        private static void CreateAbilities(GameSession session)
        {
            Packet.Builder.AbilityStatusFullCommand(session, session.Player.Abilities);
        }
    } 
}
