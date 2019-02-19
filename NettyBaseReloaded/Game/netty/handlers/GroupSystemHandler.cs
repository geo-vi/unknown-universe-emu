using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Game.netty.commands;
using NettyBaseReloaded.Game.objects;
using NettyBaseReloaded.Game.objects.world;
using NettyBaseReloaded.Game.objects.world.players;
using NettyBaseReloaded.Utils;

namespace NettyBaseReloaded.Game.netty.handlers
{
    class GroupSystemHandler : ILegacyHandler
    {
        public void execute(GameSession gameSession, string[] param)
        {
            switch (param[1])
            {
                case ServerCommands.GROUPSYSTEM_GROUP_INVITE:
                    switch (param[2])
                    {
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_BY_NAME:
                            AssembleInvite(gameSession, GetPlayerByName(Decode.DecodeFrom64(param[3]))?.GetGameSession());
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_REVOKE:
                            Revoke(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[3])));
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ACKNOWLEDGE:
                            AssembleAcceptedInvitation(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[3])));
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_REJECT:
                            Reject(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[3])));
                            break;
                    }
                    break;
                case ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_KICK:
                    Kick(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[3])));
                    break;
                case ServerCommands.GROUPSYSTEM_GROUP_EVENT_MEMBER_LEAVES_SUB_LEAVE:
                    Leave(gameSession);
                    break;
                case ClientCommands.GROUPSYSTEM_PING:
                    switch (param[2])
                    {
                        case ClientCommands.GROUPSYSTEM_PING_POSITION:
                            if (param.Length < 4)
                            {
                                Ping(gameSession, gameSession.Player.Spacemap.Entities[int.Parse(param[3])]?.Position);
                                break;
                            }
                            Ping(gameSession, new Vector(int.Parse(param[3]), int.Parse(param[4])));
                            break;
                        case ClientCommands.GROUPSYSTEM_PING_USER:
                            Ping(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[3])));
                            break;
                    }
                    break;
                case ClientCommands.GROUPSYSTEM_FOLLOW:
                    Follow(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[2])));
                    break;
                case ClientCommands.GROUPSYSTEM_PROMOTE:
                    ChangeLeader(gameSession, World.StorageManager.GetGameSession(Convert.ToInt32(param[2])));
                    break;
                case ClientCommands.GROUPSYSTEM_SET_REMOTE:
                    switch (param[2])
                    {
                        case ClientCommands.GROUPSYSTEM_CHANGE_INVITATON_BEHAVIOUR:
                            ChangeGroupBehaviour(gameSession, Convert.ToInt32(param[3]));
                            break;
                        case "delete":
                            World.DatabaseManager.Reset(gameSession, param[3]);
                            break;
                    }
                    break;
            }
        }

        public Player GetPlayerByName(string name)
        {
            return World.StorageManager.GameSessions.FirstOrDefault(x => x.Value.Player.Name == name).Value?.Player;
        }

        public void AssembleInvite(GameSession gameSession, GameSession invited)
        {
            if (invited == null)
            {
                Error(gameSession, ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NON_EXISTANT);
                return;
            }
            if (invited.Player.Group != null)
            {
                Error(gameSession, ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_IN_GROUP);
                return;
            }
            if (invited.Player.Storage.BlockedGroupInvites || invited == gameSession)
            {
                Error(gameSession, ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NOT_AVAILABLE);
                return;
            }
            if (invited.Player.Storage.GroupInvites.ContainsKey(gameSession.Player.Id) && World.StorageManager.Groups.Contains(gameSession.Player.Group))
            {
                Error(gameSession, ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_DUPLICATE);
                return;
            }
            invited.Player.Storage.GroupInvites.TryAdd(gameSession.Player.Id, gameSession.Player.Group);
            Packet.Builder.GroupInviteCommand(gameSession, invited);
        }

        public void AssembleAcceptedInvitation(GameSession gameSession, GameSession inviterSession)
        {
            if (inviterSession == null ||
                !gameSession.Player.Storage.GroupInvites.ContainsKey(inviterSession.Player.Id))
            {
                Error(gameSession, ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_INVITER_NONEXISTENT);
                return;
            }
            var inviter = inviterSession.Player;
            if (inviter.Group == null)
            {
                new Group(inviter, gameSession.Player);
            }
            else if (inviter.Group.Members.Count < Group.DEFAULT_MAX_GROUP_SIZE)
            {
                inviter.Group.Accept(inviter, gameSession.Player);
            }
            foreach (var invitation in gameSession.Player.Storage.GroupInvites)
            {
                 DeleteInvitation(World.StorageManager.GetGameSession(invitation.Key)?.Player, gameSession.Player);
            }
            gameSession.Player.Storage.GroupInvites.Clear();
        }

        public void Reject(GameSession gameSession, GameSession inviterSession)
        {
            if (gameSession.Player.Storage.GroupInvites.ContainsKey(inviterSession.Player.Id))
            {
                gameSession.Player.Storage.GroupInvites.TryRemove(inviterSession.Player.Id, out Group output);
                Packet.Builder.LegacyModule(inviterSession, "0|ps|inv|del|rj|" + gameSession.Player.Id + "|" + inviterSession.Player.Id);
                Packet.Builder.LegacyModule(gameSession, "0|ps|inv|del|none|" + inviterSession.Player.Id);
                Error(inviterSession, ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_NOT_AVAILABLE);
            }
        }

        public void Revoke(GameSession gameSession, GameSession inviterSession)
        {
            if (inviterSession == null) return;
            if (inviterSession.Player.Storage.GroupInvites.ContainsKey(gameSession.Player.Id))
            {
                inviterSession.Player.Storage.GroupInvites.TryRemove(gameSession.Player.Id, out Group output);
                Packet.Builder.LegacyModule(gameSession, "0|ps|inv|del|rv|" + gameSession.Player.Id);
                Packet.Builder.LegacyModule(gameSession, "0|ps|inv|del|rv|" + gameSession.Player.Id + "|" + inviterSession.Player.Id);
            }
        }

        public void Kick(GameSession gameSession, GameSession inviterSession)
        {
            if (inviterSession != null)
                gameSession.Player.Group?.Kick(inviterSession.Player);
        }

        public void Leave(GameSession gameSession)
        {
            gameSession.Player.Group?.Leave(gameSession.Player);
        }

        public void DeleteInvitation(Player inviter, Player player)
        {
            try
            {
                Packet.Builder.GroupDeleteInvitationCommand(inviter.GetGameSession(), player);
                Packet.Builder.GroupDeleteInvitationCommand(player.GetGameSession(), inviter);
            }
            catch (Exception)
            {

            }
        }

        public void Error(GameSession gameSession, string error)
        {
            Packet.Builder.LegacyModule(gameSession, $"0|ps|inv|err|{ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ERROR_CANDIDATE_IN_GROUP}");
        }

        public void Ping(GameSession gameSession, GameSession pingedUser)
        {
            if (pingedUser == null || !pingedUser.Player.Controller.Active) return;
            if (pingedUser.Player.Spacemap != gameSession.Player.Spacemap) return;
            gameSession.Player.Group?.Ping(pingedUser.Player.Position);
        }

        public void Ping(GameSession gameSession, Vector position)
        {
            gameSession.Player.Group?.Ping(position);
        }

        public void Follow(GameSession gameSession, GameSession followedSession)
        {
            if (followedSession.Player.Controller.Active && gameSession.Player.Spacemap == followedSession.Player.Spacemap)
                gameSession.Player.Group?.Follow(gameSession.Player, followedSession.Player);
        }

        public void ChangeLeader(GameSession gameSession, GameSession leaderSession)
        {
            if (leaderSession.Player.Controller.Active)
                gameSession.Player.Group?.ChangeLeader(gameSession, leaderSession);
        }

        public void ChangeGroupBehaviour(GameSession gameSession, int newBehaviour)
        {
            gameSession.Player.Group?.ChangeBehavior(gameSession, newBehaviour);
        }
    }
}
