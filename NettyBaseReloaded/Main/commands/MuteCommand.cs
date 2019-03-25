using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;

namespace NettyBaseReloaded.Main.commands
{
    class MuteCommand : Command
    {
        public MuteCommand() : base("mute", "Mutes the user", false, null)
        {
        }

        public override void Execute(string[] args = null)
        {
            
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            try
            {
                if (session.Player is Moderator)
                {
                    var targetName = args[1];
                    Player chatUser;
                    if (!Chat.Chat.StorageManager.FindUserByName(targetName, out chatUser))
                    {
                        MessageController.System(session.Player, "Tried muting an invalid user");
                        return;
                    }
                    if (chatUser is Moderator) MessageController.System(session.Player, "Can't mute moderators");
                    else
                    {
                        DateTime expiry = DateTime.Now;
                        var value = int.Parse(args[3]);
                        switch (args[2])
                        {
                            case "minutes":
                                expiry = expiry.AddMinutes(value);
                                break;
                            case "hours":
                                expiry = expiry.AddHours(value);
                                break;
                            case "days":
                                expiry = expiry.AddDays(value);
                                break;
                            case "months":
                                expiry = expiry.AddMonths(value);
                                break;
                            case "years":
                                expiry = expiry.AddYears(value);
                                break;
                        }
                        chatUser.GetSession().Mute(string.Join(" ", args.Skip(4)), expiry, session.Player.Id);
                    }
                }
                else
                {
                    MessageController.System(session.Player, "Unauthorized to use that command.");
                }
            }
            catch (Exception)
            {
                MessageController.System(session.Player, "Unable to mute");
                if (session.Player is Moderator)
                {
                    MessageController.System(session.Player, "Usage: /mute [name] [minutes/hours/days/months/years] [value] [reason]");
                }
            }
        }
    }
}
