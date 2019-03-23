using System.IO;
using System.Threading.Tasks;
using Discord.Commands;

namespace NettyStatusBot.Modules
{
    class PublicMessageModule : ModuleBase<SocketCommandContext>
    {
        [Command("cocaine")]
        public async Task HandleCoca()
        {            
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/200.gif");
        }

        [Command("i need some")]
        public async Task HandleUrge()
        {
            await ReplyAsync(
                "Cocaine? Indeed. That's the only thing we all need. We all need that as long as we are part of the greater civilization that is ruling over " +
                "the earth right now called the 'homosapiens' which doesn't mean to we're all doomed. I need help. CALL HEISENBERG!\nNOW!");
        }

        [Command("story:apovsvalde")]
        public async Task Story1()
        {
            await ReplyAsync(
                "A long time ago... Years and years back... Before Star Wars even existed... Beastest UNO clan top Finnish fighter vs Apo ended up in massive flames..." +
                "It caused doom in the realm of the UNIVERSE... All that necessary spam... It just doomed us all... Thank you " +
                "for wasting my precious time reading this long chat... Live long Heisenberg...");
        }

        [Command("story:shock")]
        public async Task Story2()
        {
            await ReplyAsync("10 years ago Shock started as a graphic designer. During all those hard years of trying to learn to code " +
                             "for making this server come true he got help from his bestest friend Yuuki who called him Soki and Soki and Yuuki " +
                             "worked together for days until Yuuki one day said... I got bored... End of an era.");
        }

        [Command("heisenberg")]
        public async Task Story3()
        {
            var user = await Context.Channel.GetUserAsync(177418323359694848);
            await ReplyAsync("he ded bro");
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/coma.jpg");
        }

        [Command("story:centipede")]
        public async Task Story4()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/centipidor.png");
        }

        [Command("gangsta")]
        public async Task Gangsta()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/gangsta.png");
        }

        [Command("crash")]
        public async Task Crash()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/crash.png");
        }

        [Command("rip")]
        public async Task Rip()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/ripuuu.png");
        }

        [Command("connection")]
        public async Task Connection()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/connectionlost.png");
        }

        [Command("cyborg")]
        public async Task Cyborg()
        {
            await ReplyAsync("another retarded DO ship");
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/cyborg.png");
        }

        [Command("GG?")]
        public async Task GGChoice()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/shockdecision.png");
        }

        [Command("MMO")]
        public async Task WeWantU()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/wewantu.jpg");
        }

        [Command("problem")]
        public async Task Problem()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/problem.png");
        }

        [Command("shock")]
        public async Task Shock()
        {
            await Context.Channel.SendFileAsync(Directory.GetCurrentDirectory() + "/gifs/shock.jpg");
        }
    }
}
