namespace Server.Main.commands
{
    abstract class GlobalCommand
    {
        public class SubHelp
        {
            public string Name { get; }
            public string Desc { get; }
            public bool Display { get; }
            public SubHelp(string name, string desc, bool display = true)
            {
                Name = name;
                Desc = desc;
                Display = display;
            }
        }

        /// <summary>
        /// Command name which will be used in order to execute the command
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Description of what it does
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// This will be used when /help is executed and according to it's value it will display it in /help
        /// </summary>
        public bool Display { get; }

        /// <summary>
        /// Array containing all the sub commands of the certain cmd which will be displayed on /help
        /// \\ Name \\ Desc
        /// </summary>
        public SubHelp[] HelpingParams { get; }

        public string[] Args { get; set; }

        public GlobalCommand(string name, string desc, bool display = true, SubHelp[] subHelps = null)
        {
            Name = name;
            Description = desc;
            Display = display;
            HelpingParams = subHelps;
        }

        public abstract void Execute(string[] args = null);

        //public abstract void Execute(ChatSession session, string[] args = null);

    }
}
