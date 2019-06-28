using System;

namespace Server.Main.commands.chat
{
    class ChatUsersCommand : GlobalCommand
    {
        public ChatUsersCommand() : base("users", "Displays amount of currently connected users")
        {
        }

        public override void Execute(string[] args = null)
        {
            Console.WriteLine("Currently 0 chat users online");
        }
    }
}
