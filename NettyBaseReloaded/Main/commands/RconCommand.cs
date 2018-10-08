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
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Game.objects.world.players.extra.boosters;

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
                    case "h":
                    case "heal":
                        RconSession.Player.Controller.Heal.Execute(RconSession.Player.Hangar.Ship.Health);
                        break;
                    case "boost":
                        RconSession.Player.Boosters.Add(new DMGBO1(RconSession.Player, DateTime.MaxValue));
                        RconSession.Player.Boosters.Add(new DMGBO2(RconSession.Player, DateTime.MaxValue));
                        break;
                    case "kick":
                        RconSession.Kick();
                        break;
                    case "createcubi":
                        RconSession.Player.Spacemap.CreateCubikon(RconSession.Player.Position);
                        break;
                    case "dmg":
                        var p = RconSession.Player;
                        p.Hangar.Configurations[p.CurrentConfig - 1].Damage = int.Parse(args[2]);
                        break;
                    case "enslave":
                        int id = 0;
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
                RconSession.Player.Visuals.Add(new VisualEffect(RconSession.Player, ShipVisuals.INVINCIBILITY, DateTime.MaxValue));
                RconSession.Player.RankId = Rank.ADMINISTRATOR;
                RconSession.Player.Invincible = true;
                RconSession.Player.Refresh();
                RconSession.Player.Controller.Heal.Execute(RconSession.Player.Hangar.Ship.Health);
            }
        }
    }
}
