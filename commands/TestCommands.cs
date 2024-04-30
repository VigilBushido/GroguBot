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

namespace GroguBot.commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task BasicCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync($"Grogu Instantiated.. I am at your command {ctx.User.Username}");
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