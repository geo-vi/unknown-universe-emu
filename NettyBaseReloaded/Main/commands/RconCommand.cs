using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Game;
using NettyBaseReloaded.Game.controllers.npc;
using NettyBaseReloaded.Game.netty;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.characters;
using NettyBaseReloaded.Game.objects.world.map.collectables;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.extra.boosters;
using NettyBaseReloaded.Main.objects;
using Enum = Google.Protobuf.WellKnownTypes.Enum;

namespace NettyBaseReloaded.Main.commands
{
    class RconCommand : Command
    {
        private static GameSession RconSession;

        public RconCommand() : base("rcon", "", false, null)
        {
        }

        private static ConcurrentDictionary<int, Slave> Slaves = new ConcurrentDictionary<int, Slave>();

        public override void Execute(string[] args = null)
        {
            try
            {
                if (RconSession == null)
                {
                    if (args[1] == "set")
                    {
                        SetRcon(args[2]);
                        return;
                    }
                    Console.WriteLine("No authorized RCON Session set.");
                    return;
                }
                int id = 0;
                string msg = "";
                switch (args[1])
                {
                    case "set":
                        RconSession?.Kick();
                        SetRcon(args[2]);
                        break;
                    case "m":
                    case "move":
                        if (RconSession == null) return;
                        if (args.Length > 3)
                        {
                            var posX = int.Parse(args[2]);
                            var posY = int.Parse(args[3]);
                            RconSession.Player.SetPosition(new Vector(posX, posY));
                        }
                        else if (args.Length == 3)
                        {
                            if (args[2] == "d")
                            {
                                RconSession.Player.SetPosition(RconSession.Player.Destination);
                                return; 
                            }
                            var targetMapId = int.Parse(args[2]);
                            var map = World.StorageManager.Spacemaps[targetMapId];

                            RconSession.Player.MoveToMap(map, map.Limits[0], 0);
                        }
                        break;
                    case "moveboost":
                        var targetMoveSpeedBoost = int.Parse(args[2]);
                        RconSession.Player.BoostSpeed(targetMoveSpeedBoost);
                        break;
                    case "h":
                    case "heal":
                        RconSession.Player.Controller.Heal.Execute(RconSession.Player.Hangar.Ship.Health);
                        break;
                    case "boost":
                        //RconSession.Player.Boosters.Add(new DMGBO1(RconSession.Player, DateTime.MaxValue));
                        //RconSession.Player.Boosters.Add(new DMGBO2(RconSession.Player, DateTime.MaxValue));
                        break;
                    case "kick":
                        RconSession.Kick();
                        break;
                    case "createcubi":
                        RconSession.Player.Spacemap.CreateCubikon(RconSession.Player.Position);
                        break;
                    case "createcbs":
                        RconSession.Player.Spacemap.CreateAsteroid("nigger#1", RconSession.Player.Position);
                        break;
                    case "setclan":
                        Clan clan;
                        if (int.TryParse(args[2], out id))
                        {
                            clan = Main.Global.StorageManager.GetClan(id);
                        }
                        else clan = Main.Global.StorageManager.GetClan(args[2]);

                        if (clan == null) return;
                        msg = "Moving you to clan " + clan.Tag + "/" + clan.Name + "/" + clan.Id;
                        Packet.Builder.LegacyModule(RconSession, "0|A|STD|" + msg);
                        Console.WriteLine(msg);
                        RconSession.Player.ChangeClan(clan);
                        break;
                    case "dmg":
                        var p = RconSession.Player;
                        p.Hangar.Configurations[p.CurrentConfig - 1].TotalDamageCalculated = int.Parse(args[2]);
                        msg = "Damage set to: " + args[2];
                        Packet.Builder.LegacyModule(RconSession, "0|A|STD|" + msg);
                        Console.WriteLine(msg);
                        break;
                    case "enslave":
                        if (args[2] == "sel") id = RconSession.Player.SelectedCharacter.Id;
                        else if (args[2] == "all")
                        {
                            foreach (var entity in RconSession.Player.Spacemap.Entities.Values.Where(x => x is Npc))
                            {
                                var n = entity as Npc;
                                Enslave(n);
                            }
                        }
                        else id = int.Parse(args[2]);

                        if (RconSession.Player.Spacemap.Entities.ContainsKey(id) && RconSession.Player.Spacemap.Entities[id] is Npc npc)
                        {
                            Enslave(npc);
                        }
                        break;
                    case "slaves":
                        switch (args[2])
                        {
                            case "move":
                                foreach (var slave in Slaves.Values)
                                    slave.Move(RconSession.Player.Destination);
                                break;
                            case "attack":
                                foreach (var slave in Slaves.Values)
                                    slave.Attack(RconSession.Player.SelectedCharacter);
                                break;
                            case "come":
                                foreach (var slave in Slaves.Values)
                                    slave.MoveToMaster();
                                break;
                            case "rename":
                                int i = 0;
                                foreach (var slave in Slaves.Values)
                                {
                                    slave.SetName($"-=[ {args[3]} #{i} ]=-");
                                    i++;
                                }

                                break;
                            case "exit":
                                foreach (var slave in Slaves.Values) 
                                    slave.Exit();
                                break;
                        }
                        break;
                    case "send":
                        Packet.Builder.LegacyModule(RconSession, args[2]);
                        break;
                    case "createlootbox":
                        if (args.Length < 4)
                        {
                            if (args.Length == 3 && args[2] == "types")
                            {
                                int i = 0;
                                foreach (var record in System.Enum.GetValues(typeof(Types)))
                                {
                                    Console.WriteLine("LOOT: " + record.ToString() + " :: " + i);
                                    i++;
                                }
                            }
                            Console.WriteLine("ARGS: /rcon createlootbox [uri] [amount] [typeID]");
                            Console.WriteLine("/rcon createlootbox types");
                            return;
                        }
                        Reward reward = null;
                        switch (args[2])
                        {
                            case "uri":
                                reward = new Reward(RewardType.URIDIUM, Convert.ToInt32(args[3]));
                                break;
                            default:
                                reward = new Reward(RewardType.URIDIUM, 500);
                                break;
                        }

                        RconSession.Player.Spacemap.CreateLootBox(RconSession.Player.Position, reward, (Types)Convert.ToInt32(args[4]), 15000);
                        break;
                    case "addvisual":
                        if (args[2] == "types")
                        {
                            int i = 0;
                            foreach (var record in System.Enum.GetValues(typeof(ShipVisuals)))
                            {
                                Console.WriteLine("Visual: " + record.ToString() + " :: " + i);
                                i++;
                            }
                            return;
                        }
                        var visualId = int.Parse(args[2]);
                        IAttackable target = RconSession.Player.Selected;
                        var t = 10;
                        //if (args.Length > 2)
                        //{
                        //    if (args[3].StartsWith("t="))
                        //    {
                        //        t = int.Parse(args[3].Split('=')[1]);
                        //    }
                        //    target = RconSession.Player.Spacemap.Entities[int.Parse(args[3])];
                            
                        //}
                        var visual = new VisualEffect(target, (ShipVisuals)visualId, DateTime.Now.AddSeconds(t));
                        visual.Start();
                        break;
                    case "shield":
                        var isOn = RconSession.Player.Invincible;
                        var player = RconSession.Player;
                        VisualEffect _vis;
                        if (isOn)
                        {
                            _vis = player.Visuals[ShipVisuals.INVINCIBILITY];
                            _vis.Cancel();
                            player.Invincible = false;
                        }
                        else
                        {
                            _vis = new VisualEffect(player, ShipVisuals.INVINCIBILITY, DateTime.MaxValue);
                            _vis.Start();
                            player.Invincible = true;
                        }
                        break;
                }
            }
            catch (Exception)
            {

            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            var id = session.Player.Id;
            var sessionId = session.Player.SessionId;
            var worldSession = World.StorageManager.GetGameSession(id);
            if (worldSession != null && worldSession.Player.Id == id && worldSession.Player.SessionId == sessionId &&
                worldSession.Player.RankId == Rank.ADMINISTRATOR)
            {
                Execute(args);
            }

        }

        private void Enslave(Npc npc)
        {
            Slave slave;
            npc.Controller.Enslave(RconSession.Player, out slave);
            Packet.Builder.LegacyModule(RconSession, "0|A|STD|ENSLAVED #" + npc.Id);
            Console.WriteLine("Enslaved NPC #" + npc.Id);
            Slaves.TryAdd(npc.Id, slave);
        }

        private void SetRcon(string arg)
        {
            var playerId = 0;
            if (int.TryParse(arg, out playerId) && World.StorageManager.GetGameSession(playerId) != null)
            {
                RconSession = World.StorageManager.GetGameSession(playerId);
                Console.WriteLine("RCON SUCCESSFULLY SET TO " + RconSession.Player.Name);
                RconSession.Player.Hangar.Ship = World.StorageManager.Ships[98];
                RconSession.Player.Hangar.ShipDesign = World.StorageManager.Ships[98];
                var vsEffect = new VisualEffect(RconSession.Player, ShipVisuals.INVINCIBILITY, DateTime.MaxValue);
                vsEffect.Start();
                RconSession.Player.RankId = Rank.ADMINISTRATOR;
                RconSession.Player.Invincible = true;
                RconSession.Player.RefreshPlayersView();
                RconSession.Player.Controller.Heal.Execute(RconSession.Player.Hangar.Ship.Health);
            }
        }
    }
}
