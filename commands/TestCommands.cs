using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using GroguBot.util;
using Microsoft.VisualBasic;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.EventArgs;

namespace GroguBot.commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("interactive")]
        public async Task BasicCommand(CommandContext ctx)
        {
            var interactivity = Program.Client.GetInteractivity();

            var messageToRetrieve = await interactivity.WaitForMessageAsync(message => message.Content == "activate");
            if (messageToRetrieve.Result.Content == "activate")
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} started a new environment");
            }
            //await ctx.Channel.SendMessageAsync($"Grogu Instantiated.. I am at your command {ctx.User.Username}");
        }

        [Command("awake")]
        public async Task ActiveAwake(CommandContext ctx)
        {
            var interactivity = Program.Client.GetInteractivity();

            var messageToReact = await interactivity.WaitForReactionAsync(message => message.Message.Id == 1240175731472859136);
            if (messageToReact.Result.Message.Id == 1240175731472859136)
            {
                await ctx.Channel.SendMessageAsync($"{ctx.User.Username} used the emoji with name {messageToReact.Result.Emoji.Name}");
            }
        }

        [Command("poll")]
        public async Task PollCreator(CommandContext ctx, string option1, string option2, string option3, string option4, [RemainingText] string pollTitle)
        {
            var interactivity = Program.Client.GetInteractivity();
            var pollTime = TimeSpan.FromSeconds(10);

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                            DiscordEmoji.FromName(Program.Client, ":two:"),
                                            DiscordEmoji.FromName(Program.Client, ":three:"),
                                            DiscordEmoji.FromName(Program.Client, ":four:")};

            string optionsDescription = $"{emojiOptions[0]} | {option1} \n" +
                                        $"{emojiOptions[1]} | {option2} \n" +
                                        $"{emojiOptions[2]} | {option3} \n" +
                                        $"{emojiOptions[3]} | {option4}";

            var pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Red,
                Title = pollTitle,
                Description = optionsDescription
            };

            var sentPoll = await ctx.Channel.SendMessageAsync(embed: pollMessage);
            foreach (var emoji in emojiOptions)
            {
                await sentPoll.CreateReactionAsync(emoji);
            }

            var totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);

            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            int count4 = 0;

            foreach (var emoji in totalReactions)
            {
                if (emoji.Emoji == emojiOptions[0])
                {
                    count1++;
                }
                if (emoji.Emoji == emojiOptions[1])
                {
                    count2++;
                }
                if (emoji.Emoji == emojiOptions[2])
                {
                    count3++;
                }
                if (emoji.Emoji == emojiOptions[3])
                {
                    count4++;
                }
            }

            int totalVotes = count1 + count2 + count3 + count4;

            string resultDescription = $"{emojiOptions[0]}: {count1} Votes \n" +
                                       $"{emojiOptions[1]}: {count2} Votes \n" +
                                       $"{emojiOptions[2]}: {count3} Votes \n" +
                                       $"{emojiOptions[3]}: {count4} Votes \n\n" +
                                       $"Total Votes: {totalVotes}";

            var resultEmbed = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Green,
                Title = "Results of the Poll",
                Description = resultDescription
            };

            await ctx.Channel.SendMessageAsync(embed: resultEmbed);
        }

        [Command("addhex")]
        public async Task AddHex(CommandContext ctx, string hex_one, string hex_two)
        {
            BigInteger result = HexLiteralToLong(hex_one) + HexLiteralToLong(hex_two);
            await ctx.Channel.SendMessageAsync(result.ToString());
        }

        private static long HexLiteralToLong(string hex)
        {
            if (string.IsNullOrEmpty(hex)) throw new ArgumentException("hex");

            int i = hex.Length > 1 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X') ? 2 : 0;
            long value = 0;

            while (i < hex.Length)
            {
                int x = hex[i++];

                if
                    (x >= '0' && x <= '9') x = x - '0';
                else if
                    (x >= 'A' && x <= 'F') x = (x - 'A') + 10;
                else if
                    (x >= 'a' && x <= 'f') x = (x - 'a') + 10;
                else
                    throw new ArgumentOutOfRangeException("hex");

                value = 16 * value + x;
            }

            return value;
        }

        [Command("embed")]
        public async Task EmbedMessage(CommandContext ctx)
        {
            /*             var message = new DiscordMessageBuilder()
                            .AddEmbed(new DiscordEmbedBuilder()
                                .WithTitle("Breaking Notice")
                                .WithDescription($"Alert {ctx.User.Username}, you have been notified of an action"));

                        await ctx.Channel.SendMessageAsync(message); */
            var message = new DiscordEmbedBuilder
            {
                Title = "Breaking Notice",
                Description = $"Alert {ctx.User.Username}, you have been notified of an action",
                Color = DiscordColor.IndianRed
            };

            await ctx.Channel.SendMessageAsync(embed: message);
        }

        [Command("cardgame")]
        public async Task CardGame(CommandContext ctx)
        {
            var userCard = new CardSystem();

            var userCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"Your card is {userCard.SelectedCard}",
                Color = DiscordColor.Lilac
            };

            await ctx.Channel.SendMessageAsync(embed: userCardEmbed);

            var botCard = new CardSystem();

            var botCardEmbed = new DiscordEmbedBuilder
            {
                Title = $"The Bot drew a {botCard.SelectedCard}",
                Color = DiscordColor.Orange
            };

            await ctx.Channel.SendMessageAsync(embed: botCardEmbed);

            if (userCard.SelectedNumber > botCard.SelectedNumber)
            {
                //User Wins 
                var winMessage = new DiscordEmbedBuilder
                {
                    Title = "Congratulations, you WIN!!",
                    Color = DiscordColor.SapGreen
                };
                await ctx.Channel.SendMessageAsync(embed: winMessage);
            }
            else
            {
                //Bot Wins 
                var loseMessage = new DiscordEmbedBuilder
                {
                    Title = "You Lost the Game!!!",
                    Color = DiscordColor.Red
                };
                await ctx.Channel.SendMessageAsync(embed: loseMessage);
            }
        }
    }
}