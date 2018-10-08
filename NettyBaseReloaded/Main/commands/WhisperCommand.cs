using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NettyBaseReloaded.Chat.controllers;
using NettyBaseReloaded.Chat.objects;
using NettyBaseReloaded.Chat.objects.chat;
using NettyBaseReloaded.Chat.packet;

namespace NettyBaseReloaded.Main.commands
{
    class WhisperCommand : Command
    {
        public static bool WHISPERS_MUTED = false;

        public WhisperCommand() : base("w", "Whisper")
        {
        }

        public override void Execute(string[] args = null)
        {
            try
            {
                if (args.Length == 2)
                {
                    if (args[1] == "mute")
                    {
                        WHISPERS_MUTED = !WHISPERS_MUTED;
                        Console.WriteLine("Whispers muted: " + WHISPERS_MUTED);
                        return;
                    }
                }
                var final = string.Join(" ", args).Replace(args[0] + " " + args[1] + " ", "");
                MessageController.Whisper(args[1], "SYSTEM", final);
            }
            catch (Exception)
            {

            }
        }

        public override void Execute(ChatSession session, string[] args = null)
        {
            try
            {
                var final = string.Join(" ", args).Replace(args[0] + " " + args[1] + " ", "");
                if (args[1] == "SYSTEM")
                {
                    if (WHISPERS_MUTED)
                    {
                        Packet.Builder.UserNotExist(session);
                        return;
                    }
                    Console.WriteLine(session.Player.Name + " whispered " + final);
                    Packet.Builder.WhisperTo(session, "SYSTEM", final);
                    return;
                }
                MessageController.Whisper(args[1], session.Player.Name, final);
            }
            catch (Exception)
            {

            }
        }
    }
}
