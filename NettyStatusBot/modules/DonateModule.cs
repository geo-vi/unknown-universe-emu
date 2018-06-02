using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BraintreeHttp;
using Discord;
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
                             "`Possible choices: \ninstructions\ndonate premium\ndonate boosters\ndonate petfuel\ndonate design\n`\n" +
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
            await ReplyAsync("", false,
                new EmbedBuilder
                {
                    Fields = new List<EmbedFieldBuilder> { new EmbedFieldBuilder { IsInline = true, Name = "Buy Premium!", Value = "Costs only 6.99 EUR / month!\nhttp://beta.univ3rse.com/buyABeer.php" } },
                    ImageUrl = "http://beta.univ3rse.com/do_img/global/events/benefitPremium.gif",
                    Url = "http://beta.univ3rse.com/buyABeer.php",
                    Color = Color.Orange,
                    Footer = new EmbedFooterBuilder { IconUrl = "http://beta.univ3rse.com/do_img/global/events/icons/benefitPremium.gif", Text = "**Make sure after you've purchased to follow the `donate instructions`!**" }
                });
        }

        [Command("boosters")]
        public async Task Boosters()
        {
            await ReplyAsync("Coming soon..");
        }

        [Command("petfuel")]
        public async Task PetFuel()
        {
            await ReplyAsync("**Please don't choose this package if you don't own a PET**\nIn order to get a PET purchase `donate premium` package!", false,
                new EmbedBuilder{ImageUrl = "http://beta.univ3rse.com/do_img/global/events/petSystem.gif",
                    Title = "Fuel your P.E.T back and ready for battle!",
                    Description = "For only 99 CENTS!\nhttp://beta.univ3rse.com/buyABeer.php",
                    Footer = new EmbedFooterBuilder { IconUrl = "http://beta.univ3rse.com/do_img/global/events/icons/petSystem.gif", Text = "**Make sure after you've purchased to follow the `donate instructions`!**" }
                });
        }

        [Command("design")]
        public async Task Design()
        {
            await ReplyAsync("Choose the design package you would like to purchase!\n" +
                             "```\n1: Skill Design Package" +
                             "\n2: --" +
                             "\n3: --" +
                             "``` `donate design [id]`");
        }

        [Command("design")]
        public async Task Design(int id)
        {
            switch (id)
            {
                case 1:
                    //await ReplyAsync("```asciidoc" +
                    //                 "\Elite Design Package" +
                    //                 "\n[Sentinel] [Spectrum] [Solace] [Venom] [Diminisher]" +
                    //                 "\n**[3.99 EUR]**" +
                    //                 "```");
                    //break;
                case 2:
                case 3:
                    await ReplyAsync("Expect soon");
                    break;
            }

            await ReplyAsync("http://beta.univ3rse.com/buyABeer.php");
        }

        [Command("instructions")]
        public async Task Instructions()
        {
            await ReplyAsync("```" +
                             "\n 1> After you've paid for the item you bought, go to your PayPal Transaction log (https://www.paypal.com/myaccount/transactions/) and look up for **Transaction ID**" +
                             "\n 2> Write to Tony Montana / Don Univ3rse (BOT) `donate claim`" +
                             "\n Follow the command instructions" +
                             "\n 3> Wait for your claim to be approved.\nPlease be patient, it's not an automated service!!" +
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
                await claim.Key.SendMessageAsync("Your claim have been processed.");
            }
            else await ReplyAsync("Couldn't find claim key " + user);
        }
    }
}
