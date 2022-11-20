using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Interactions;
using Discord.Commands;
using ProgrammingFriends.Services.Interfaces;
using ProgrammingFriends.Services;

namespace ProgrammingFriends;
internal class Bootstrap
{
    internal static IServiceProvider Initialize(string[] args)
    {
        IServiceCollection services = new ServiceCollection();

        var interactionServiceConfig = new InteractionServiceConfig
        {
            DefaultRunMode = Discord.Interactions.RunMode.Async,
            LogLevel = LogSeverity.Verbose,
        };

        var discordSocketConfig = new DiscordSocketConfig
        {
            LogLevel = LogSeverity.Verbose,
            MessageCacheSize = 500,
            AlwaysDownloadUsers = true,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent,
        };

        var commandServiceConfig = new CommandServiceConfig
        {
            LogLevel = LogSeverity.Verbose,
            DefaultRunMode = Discord.Commands.RunMode.Async,
            CaseSensitiveCommands = false,
        };

        DiscordSocketClient client = new DiscordSocketClient(discordSocketConfig);
        CommandService commandService = new CommandService(commandServiceConfig);
        InteractionService interactionService = new InteractionService(client, interactionServiceConfig);

        services.AddSingleton(client);
        services.AddSingleton(commandService);
        services.AddSingleton(interactionService);
        services.AddSingleton<IDiscordBotService, DiscordBotService>();
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<TextCommandHandler>();
        services.AddSingleton<InteractionHandler>();

        return services.BuildServiceProvider();
    }
}
