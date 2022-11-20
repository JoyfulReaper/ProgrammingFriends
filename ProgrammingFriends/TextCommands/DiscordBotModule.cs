using Discord.Commands;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ContextType = Discord.Commands.ContextType;
using RequireContextAttribute = Discord.Commands.RequireContextAttribute;
using RequireOwnerAttribute = Discord.Commands.RequireOwnerAttribute;
using SummaryAttribute = Discord.Commands.SummaryAttribute;

namespace ProgrammingFriends.TextCommands;


public class DiscordBotModule : ModuleBase<SocketCommandContext>
{
    private readonly InteractionService _interactionService;

    public DiscordBotModule(InteractionService interactionService)
    {
        _interactionService = interactionService;
    }


    [Command("registerSlashGlobal")]
    [Summary("Registers slash commands")]
    [RequireOwner]
    [Alias("rsg")]
    public async Task RegisterSlashCommandsGlobal()
    {
        await _interactionService.RegisterCommandsGloballyAsync(true);
        await ReplyAsync("Registered slash commands globally!");
    }

    [Command("registerSlashDev")]
    [Summary("Registers slash commands to developmnent guild")]
    [RequireOwner]
    [RequireContext(ContextType.Guild)]
    [Alias("rsd")]
    public async Task RegisterSlashCommandsDev()
    {
        await _interactionService.RegisterCommandsToGuildAsync(820787797682159616, true);
        await ReplyAsync("Registered slash commands to development guild!");
    }

    [Command("unregisterSlashDev")]
    [Summary("Un-registers slash commands to developmnent guild")]
    [RequireOwner]
    [Alias("usd")]
    public async Task UnRegisterSlashCommandsDev()
    {
        var interactionModules = from type in Assembly.GetEntryAssembly().GetTypes()
                                 where typeof(InteractionModuleBase<SocketInteractionContext>).IsAssignableFrom(type)
                                 select type;

        foreach (var type in interactionModules)
        {
            await _interactionService.RemoveModuleAsync(type);
        }

        await _interactionService.RegisterCommandsToGuildAsync(820787797682159616, true);
        await ReplyAsync("un-Registered slash commands to development guild!");
    }
}
