using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.netty.commands.old_client;
using NettyBaseReloaded.Game.objects.world.map;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NettyBaseReloaded.Game.objects.world.players.killscreen
{
    class Killscreen
    {
        /// <summary>
        /// If user connects for the 2nd time we should send him the option to repair at base only
        /// Otherwise if player is fresh killed (didn't disconnect since death) we just send him the killscreen with the killer's details.
        /// </summary>
        // TODO: Recode it

        public int Id { get; set; }

        public Player KilledPlayer { get; set; }

        public string KillerName { get; set; }
        public string KillerLink { get; set; }

        public DeathType DeathType { get; set; }

        public string Alias => "MISC"; // TEMPORARY until we find out what it is

        public DateTime TimeOfDeath { get; set; }

        public Tuple<short, int> Price => CalculatePrice(SelectedOption);
        public int SelectedOption { get; set; } = 1;

        public Killscreen(Player killedPlayer, Character killer, DeathType deathType)
        {
            DeathType = deathType;
            KilledPlayer = killedPlayer;
            ParseKiller(killer);
            TimeOfDeath = DateTime.Now;
            SaveToDb();
            SendToPlayer();
        }

        /// <summary>
        /// Emergency constructor
        /// </summary>
        public Killscreen() { }

        /// <summary>
        /// Parsing the details of the player who killed me
        /// </summary>
        /// <param name="killer">Character of killer - can be null</param>
        private void ParseKiller(Character killer)
        {
            if (killer == null)
            {
                KillerName = "Unknown";
                KillerLink = "null";
                // TODO
                // if player is invalid what should you drop?
                // for instance if killed by radiation player would be null
                // if killed by CBS killer would be null as well
                return;
            }
            KillerLink = GetKillerLink(killer.Name);
            if (killer.Id == KilledPlayer.Id)
            {
                KillerName = "You";
                // TODO
                // If I killed myself somehow
                // mine? 
                // radiation maybe?
                return;
            }
            KillerName = killer.Name;

            // TODO
            // Actually something killed me
            // A Player, npc or whatever that is defined as : Character
            // Pet contains Owner which is Player so it will display the Pet.GetOwner() and if it's null it drops
            // the normal killscreen with killer as Pet
        }

        /// <summary>
        /// Gets the killer link from website
        /// ag. http://univ3rse.com/profiles/base64(killerName)
        /// NOT DONE SO RETURNING AUTO NULL
        /// </summary>
        /// <param name="killerName">Name of player who killed me</param>
        /// <returns>Returns full link</returns>
        private string GetKillerLink(string killerName) => "null"; // => is like {get;} only returns - can't be setted
                                                                   // example GetKillerLink() = ""; won't work
        private void SaveToDb()
        {
            World.DatabaseManager.AddKillScreen(this);
        }

        private void SendToPlayer()
        {
            var killedSession = KilledPlayer.GetGameSession();
            if (killedSession != null)
                Packet.Builder.KillScreenCommand(killedSession, this); // TODO
        }

        public short GetDestructionType()
        {
            return (short) this.DeathType;
        }

        public List<netty.commands.old_client.KillScreenOptionModule> GetOldOptions()
        {
            var price = CalculatePrice(1);
            var options = new List<netty.commands.old_client.KillScreenOptionModule>();
            var optionModule = new netty.commands.old_client.KillScreenOptionModule(
                new netty.commands.old_client.KillScreenOptionTypeModule(netty.commands.old_client.KillScreenOptionTypeModule.BASIC_REPAIR),
                new netty.commands.old_client.PriceModule(price.Item1, price.Item2),
                true,
                0,
        //        new netty.commands.old_client.MessageLocalizedWildcardCommand("desc_killscreen_repair_for_money", new List<netty.commands.old_client.MessageWildcardReplacementModule> {
        //            new netty.commands.old_client.MessageWildcardReplacementModule("%COUNT%", price.Item2.ToString()),
        //            new netty.commands.old_client.MessageWildcardReplacementModule("%CURRENCY%", price.Item1 == PriceModule.URIDIUM ? "U." : "C.")
        //        }),
                new netty.commands.old_client.MessageLocalizedWildcardCommand("ttip_killscreen_basic_repair", new List<netty.commands.old_client.MessageWildcardReplacementModule>()),
                new netty.commands.old_client.MessageLocalizedWildcardCommand("", new List<netty.commands.old_client.MessageWildcardReplacementModule>()),
                new netty.commands.old_client.MessageLocalizedWildcardCommand("ttip_killscreen_basic_repair", new List<netty.commands.old_client.MessageWildcardReplacementModule>()),
                new netty.commands.old_client.MessageLocalizedWildcardCommand("btn_killscreen_repair_for_money", new List<netty.commands.old_client.MessageWildcardReplacementModule> {
                    new netty.commands.old_client.MessageWildcardReplacementModule("%COUNT%", price.Item2.ToString()),
                    new netty.commands.old_client.MessageWildcardReplacementModule("%CURRENCY%", price.Item1 == PriceModule.URIDIUM ? "U." : "C.")
                }));
            options.Add(optionModule);

            return options;
        }

        public List<netty.commands.new_client.KillScreenOptionModule> GetNewOptions()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tuple of PriceModule type as Item1 and value as Item2
        /// Possible options 1,2,3
        /// Regular, Portal, Place
        /// </summary>
        /// <returns></returns>
        public Tuple<short,int> CalculatePrice(int option)
        {
            if (KilledPlayer == null) return new Tuple<short, int>(PriceModule.URIDIUM, 0);
            if (KilledPlayer.Information.Premium.Active) return new Tuple<short, int>(PriceModule.URIDIUM, 0);
            switch (KilledPlayer.Spacemap.Id)
            {
                case 1:
                case 5:
                case 9:
                    return new Tuple<short,int>(PriceModule.CREDITS, 1000 * option * KilledPlayer.Hangar.Ship.Id); // 100 cre
                default:
                    return new Tuple<short, int>(PriceModule.URIDIUM, 250 * option); // default 500 uri
            }
        }

        /// <summary>
        /// Loads the killscreen from Database *if any*
        /// </summary>
        /// <param name="killedPlayer">Player value of the killed player (me)</param>
        /// <returns>Either a killscreen or null if player is not technically dead (just respawn)</returns>
        public static Killscreen Load(Player killedPlayer)
        {
            return World.DatabaseManager.GetLastKillscreen(killedPlayer);
        }
    }
}
