using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters.cooldowns;
using NettyBaseReloaded.Game.objects.world.map;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.map.objects.assets;
using NettyBaseReloaded.Game.objects.world.map.objects.stations;
using NettyBaseReloaded.Game.objects.world.map.pois;
using NettyBaseReloaded.Game.objects.world.players.extra.techs;
using NettyBaseReloaded.Game.objects.world.players.informations;
using NettyBaseReloaded.Game.objects.world.players.settings;
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
            GameSession.Player.UpdateConfig();
            SendLegacy(GameSession);
            SendCooldowns(GameSession);
        }

        private void SendCooldowns(GameSession gameSession)
        {
            foreach (var cooldown in gameSession.Player.Cooldowns.CooldownDictionary.Values)
            {
                cooldown.Send(gameSession);
            }
        }

        public static void SendLegacy(GameSession GameSession)
        {
            try
            {
                Packet.Builder.DronesCommand(GameSession, GameSession.Player);
                //Packet.Builder.LegacyModule(GameSession, "0|n|t|" + GameSession.Player.Id + "|222|most_wanted");
                Packet.Builder.LegacyModule(GameSession, "0|A|CC|" + GameSession.Player.CurrentConfig); // Config
                GameSession.Player.Information.DisplayBootyKeys();
                Packet.Builder.PetInitializationCommand(GameSession, GameSession.Player.Pet); // PET
                Packet.Builder.HellstormStatusCommand(GameSession); // Rocket launcher

                Packet.Builder.LegacyModule(GameSession, "0|A|ITM|" + GameSession.Player.Equipment.GetConsumablesPacket(), true);
                GameSession.Player.Controller.CPUs.LoadCpus();
              
                GameSession.Player.Settings.Slotbar.HideMenu(MenuButtons.QUICK_BUY);
                GameSession.Player.Settings.Slotbar.HideButton(Buttons.SELECTION_LASER_CBO100);
                GameSession.Player.Settings.Slotbar.HideButton(Buttons.SELECTION_LASER_JOB100);

                if (GameSession.Player.Group != null)
                    Packet.Builder.GroupInitializationCommand(GameSession); // group

                if (GameSession.Player.Information.Title != null)
                    Packet.Builder.TitleCommand(GameSession, GameSession.Player); // title
                
                GameSession.Player.Information.Premium.Login(GameSession); // Premium notification
                
                Packet.Builder.QuestInitializationCommand(GameSession); // Quests

                CreateFormations(GameSession); // Drone Formations
                
                //CreateTechs(GameSession); // Techs
                
                CreateAbilities(GameSession); // Abilities
                
                Packet.Builder.AttributeOreCountUpdateCommand(GameSession, GameSession.Player.Information.Cargo); // Cargo

                UpdateClanWindow(GameSession); // Clan Window
                
                //Packet.Builder.EventActivationStateCommand(GameSession, 0, true); // Event Christmas 0
                //Packet.Builder.EventActivationStateCommand(GameSession, 1, true); // Event Christmas 1
                
                LoadShipEffects(GameSession);

                GameSession.Player.Spacemap.CreateGalaxyGates(GameSession.Player);
            }
            catch (Exception e)
            {
                Console.WriteLine("legacy:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        public static void UpdateClanWindow(GameSession gameSession)
        {
            foreach (var member in gameSession.Player.Clan.Members)
            {
                var memberPlayer = member.Value.Player;
                if (memberPlayer != null)
                {
                    var memberSession = memberPlayer.GetGameSession();
                    Packet.Builder.ClanWindowInitCommand(memberSession);
                }
            }
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
            session.Player.Techs.TryAdd(Techs.ROCKET_PRECISSION, new RocketPrecission(session.Player));
            session.Player.Techs.TryAdd(Techs.SHIELD_BUFF, new ShieldBuff(session.Player));
            session.Player.Techs.TryAdd(Techs.BATTLE_REPAIR_ROBOT, new BattleRepairRobot(session.Player));
            session.Player.Techs.TryAdd(Techs.ENERGY_LEECH, new EnergyLeech(session.Player));
            session.Player.Techs.TryAdd(Techs.CHAIN_IMPULSE, new ChainImpulse(session.Player));

            Packet.Builder.TechStatusCommand(session);
        }

        private static void CreateFormations(GameSession session)
        {
            Packet.Builder.DroneFormationAvailableFormationsCommand(session);
        }

        private static void CreateAbilities(GameSession session)
        {
            Packet.Builder.AbilityStatusFullCommand(session, session.Player.Abilities.Values.ToList());
        }

        private static void LoadShipEffects(GameSession session)
        {
            var player = session.Player;
            if (player.Visuals.Any(x => x.Key == ShipVisuals.RED_GLOW || x.Key == ShipVisuals.GENERIC_GLOW || x.Key == ShipVisuals.RED_GLOW)) return;
            
            if (player.State.IsOnHomeMap() && player.Hangar.Ship.Id == 3)
            {
                var visualEffect = new VisualEffect(player, ShipVisuals.GENERIC_GLOW, DateTime.MaxValue);
                visualEffect.Start();
            }

            if (player.RankId == Rank.ADMINISTRATOR)
            {
                var visualEffect = new VisualEffect(player, ShipVisuals.RED_GLOW, DateTime.MaxValue);
                visualEffect.Start();
            }
        }
    } 
}
