using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BraintreeHttp;
using Discord.Commands;
using Discord.WebSocket;
using NettyStatusBot.storage;
using PayPal;
using PayPal.Api;

namespace NettyStatusBot.modules
{
    [Group("donate")]
    class DonateModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultResponse()
        {
            await ReplyAsync("\n```" +
                             "  Would you like to support the server?\n" +
                             " We're very greatful for all of your support and we use the money we collect from donations for keeping the servers alive =)!\n" +
                             " Be a lifesaver, donate today!" +
                             "```");
            await ReplyAsync("" +
                             "`Possible choices: \ndonate premium\ndonate boosters\ndonate petfuel\ndonate design\n`\n" +
                             "***`If you've already donated: donate claim`***" +
                             "");
        }

        [Command("premium")]
        public async Task Premium()
        {
            await ReplyAsync("\n" +
                             "`Donate via PayPal for **6.99 EUR** and get`\n```cs" +
                             "\n'An extra hotkey slot'" +
                             "\n'Half the cooldown for ROCKETS'" +
                             "\n'PET'" +
                             "\nPriority Support" +
                             "\nDiscord exclusive Premium tag." +
                             "```");
        }

        [Command("claim")]
        public async Task Claim(string item, string transactionId, int userId, string username, string email)
        {
            var claimRequest = $"{Context.User.Mention}```Claim request for {item}\nTransaction ID: {transactionId}\nUsername: {username}\nPayment email: {email}" +
                               "```";

            await ReplyAsync(claimRequest);
            await ReplyAsync("Please validate to everything matches.\nSpamming requests will lead to *BAN* from our Discord.");
            await ReplyAsync("In case you've submitted wrong details about your request please make sure you delete it using `donate deleteclaim`. You can have only one claim once at a time.");
            if (Donations.ClaimRequests.ContainsKey(Context.User))
            {
                Donations.ClaimRequests[Context.User] = claimRequest;
            } else Donations.ClaimRequests.Add(Context.User, claimRequest);
        }

        [Command("claim")]
        public async Task Claim()
        {
            await ReplyAsync("For claiming your donations use the command `donate claim [item] [transactionId] [userid] [username] [paypal_email]`." +
                             "\nExample usage: `donate claim PREMIUM 7HH35086AF531700Y 1 Shock shock@univ3rse.com`");
            await ReplyAsync("**Spamming requests will lead to BAN!**");
            await ReplyAsync("In case you've submitted wrong details about your request please make sure you delete it using `donate deleteclaim`. You can have only one claim once at a time.");
        }

        [Command("deleteclaim")]
        public async Task DeleteClaim()
        {
            if (Donations.ClaimRequests.ContainsKey(Context.User))
                Donations.ClaimRequests.Remove(Context.User);

            await ReplyAsync("Your claim request have been deleted.");
        }

        [Command("claims"), RequireOwner]
        public async Task GetClaims()
        {
            await ReplyAsync($"**{Donations.ClaimRequests.Count} claims are open.**");
            foreach (var claim in Donations.ClaimRequests)
            {
                await ReplyAsync(claim.Value);
            }
        }

        [Command("removeclaim"), RequireOwner]
        public async Task RemoveClaim(string user)
        {
            var claim = Donations.ClaimRequests.FirstOrDefault(x => x.Key.Username.Contains(user));
            if (claim.Key != null && Donations.ClaimRequests.ContainsKey(claim.Key))
            {
                Donations.ClaimRequests.Remove(claim.Key);
                await ReplyAsync("Successfully removed claim: " + claim.Key.Mention);
            }
            else await ReplyAsync("Couldn't find claim key " + user);
        }
    }
}
