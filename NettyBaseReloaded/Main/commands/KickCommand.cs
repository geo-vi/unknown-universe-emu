using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Game;

namespace NettyBaseReloaded.Main.commands
{
    class KickCommand : Command
    {
        public KickCommand() : base("kick", "Kicking user", true, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                if (args == null || args.Length < 2)
                {
                    Console.WriteLine("/kick [game/chat/{id}] [id/reason] [reason]");
                    return;
                }

                int userId = 0;
                var reason = "";
                switch (args[1])
                {
                    default:
                        if (int.TryParse(args[1], out userId))
                        {
                            var player = World.StorageManager.GetGameSession(userId);
                            var chatUser = Chat.Chat.StorageManager.GetChatSession(userId);
                            player.Kick();
                            chatUser.Kick(string.Join(" ", args.Skip(2)));
                        }

                        break;
                    case "game":
                        if (int.TryParse(args[2], out userId))
                        {
                            var player = World.StorageManager.GetGameSession(userId);
                            player.Kick();
                        }

                        break;
                    case "chat":
                        if (int.TryParse(args[2], out userId))
                        {
                            var chatUser = Chat.Chat.StorageManager.GetChatSession(userId);
                            chatUser.Kick(string.Join(" ", args.Skip(3)));
                        }

                        break;
                }
            }
            catch
            {

            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            try
            {
                if (args == null)
                {
                    MessageController.System(session.Player, "Unable to kick");
                    return;
                }

                if (session.Player is Moderator moderator)
                {
                    var targetName = args[1];
                    Player chatUser;
                    if (!Chat.Chat.StorageManager.FindUserByName(targetName, out chatUser))
                    {
                        MessageController.System(session.Player, "Tried kicking an invalid user");
                        return;
                    }
                    if (chatUser is Moderator) MessageController.System(session.Player, "Can't kick moderators");
                    else
                    {
                        chatUser.GetSession().Kick(string.Join(" ", args.Skip(2)), session.Player.Id);
                    }
                }
                else MessageController.System(session.Player, "Unauthorized to use that command.");
            }
            catch (Exception)
            {
                MessageController.System(session.Player, "Unable to kick");
                if (session.Player is Moderator)
                {
                    MessageController.System(session.Player, "Usage: /kick [name] [reason]");
                }
            }
        }
    }
}
