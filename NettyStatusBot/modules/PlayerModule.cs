using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace NettyStatusBot.modules
{
    [Group("player")]
    class PlayerModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task Base()
        {
            await ReplyAsync("```" +
                             "\nadditem [userId] [itemId]" +
                             "\ngetitemlist" +
                             "\nsetdamage [userId] [damage]" +
                             "\nsetshield [userId] [shield]" +
                             "\nsetspeed [userId] [speed]" +
                             "\nsethp [userId] [hp]" +
                             "\nsetnano [userId] [nano]" +
                             "```");
        }

        [Command("additem")]
        public async Task AddItem(int userId, string itemId)
        {
            switch (itemId)
            {
                case "premiumpack":
                    break;
                case "designpack":
                    break;
                case "boosterpack":
                    break;
                default:
                    await ReplyAsync("player getitemlist");
                    break;
            }
        }

        [Command("getitemlist")]
        public async Task GetItemList()
        {
            await ReplyAsync("```" +
                             "\npremiumpack" +
                             "\ndesignpack" +
                             "\nboosterpack" +
                             "```");
        }

        [Command("setdamage")]
        public async Task SetDamage(int userId, int damage)
        {

        }

        [Command("setshield")]
        public async Task SetShield(int userId, int hp)
        {

        }

        [Command("setspeed")]
        public async Task SetSpeed(int userId, int speed)
        {

        }

        [Command("sethp")]
        public async Task SetHp(int userId, int hp)
        {

        }

        [Command("setnano")]
        public async Task SetNano(int userId, int nano)
        {

        }

        [Command("setpos")]
        public async Task SetPosition(int userid, int x, int y)
        {

        }
    }
}
