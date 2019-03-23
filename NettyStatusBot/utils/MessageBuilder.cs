using Discord;

namespace NettyStatusBot.Utils
{
    static class MessageBuilder
    {
        public static EmbedBuilder GetOfflineMsg()
        {
            var builder = new EmbedBuilder();
            builder.Title = "Current server status";
            builder.Description = "Server is OFFLINE";
            builder.Color = Color.Red;
            builder.AddField("Please PM the @La Familia", "Expect us back on soon, running!", true);
            return builder;
        }

        public static EmbedBuilder GetOnlineMsg()
        {
            var builder = new EmbedBuilder();
            //builder.Title = "Current server status";
            //builder.Description = $"Server is ONLINE, Running for ~{Program.ServerStatus.Runtime:g}.";
            //builder.Color = Color.Green;
            //builder.AddField("Currently online players", Program.ServerStatus.PlayersOnline.ToString(), true);
            return builder;
        }

        public static EmbedBuilder GetEventsRunning()
        {
            var builder = new EmbedBuilder();
            builder.Title = "Events running:";
            builder.Description = "No events running.";
            builder.Color = Color.DarkGrey;
            return builder;
        }

        public static EmbedBuilder GetMaintenanceMsg()
        {
            var builder = new EmbedBuilder();
            builder.AddField("Currently server is undergoing maintenance.",
                "Expect restarts, perhaps a bit unstable to play now", true);
            builder.AddField("Unknown Universe team suggests",
                "not to play now, rather wait a while until maintenance is over.");
            builder.Color = Color.LightOrange;
            return builder;
        }
    }
}
