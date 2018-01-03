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
            //ps|inv|name|{name} - invitation
            switch (param[1])
            {
                case ServerCommands.GROUPSYSTEM_GROUP_INVITE:
                    switch (param[2])
                    {
                        //pacet[1] = "inv"
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_BY_NAME:
                            //Declaration
                            Packet.Builder.GroupInviteCommand(gameSession, GetPlayerByName(Decode.DecodeFrom64(param[3]))?.GetGameSession());
                            //Invite(packet[3]);
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_REVOKE:
                            //Declaration
                            //RevokeInvitation(Integer.parseInt(packet[3]));
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_ACKNOWLEDGE:
                            //Declaration
                            AssembleAcceptedInvitation(gameSession.Player, World.StorageManager.GetGameSession(Convert.ToInt32(param[3]))?.Player);
                            break;
                        case ServerCommands.GROUPSYSTEM_GROUP_INVITE_SUB_REJECT:
                            //Declaration
                            //RejectInvitation(Integer.parseInt(packet[3]));
                            break;
                    }
                    break;
            }
        }

        public Player GetPlayerByName(string name)
        {
            return World.StorageManager.GameSessions.FirstOrDefault(x => x.Value.Player.Name == name).Value?.Player;
        }

        public void AssembleAcceptedInvitation(Player player, Player inviter)
        {
            if (inviter.Group == null)
            {
                inviter.Group = new Group(inviter, player);
            }
            else
            {
                inviter.Group.Accept(player);
            }
        }
    }
}
