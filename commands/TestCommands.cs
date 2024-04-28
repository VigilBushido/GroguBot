using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;

namespace GroguBot.commands
{
    public class TestCommands : BaseCommandModule
    {
        [Command("test")]
        public async Task BasicCommand(CommandContext ctx)
        {
            await ctx.Channel.SendMessageAsync("Grogu Instantiated");
        }
    }
}