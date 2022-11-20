using Discord;
using Discord.Commands;
using Discord.WebSocket;
using ProgrammingFriends.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services;
public class TextCommandHandler : IDisposable
{
    private readonly DiscordSocketClient _client;
    private readonly CommandService _commandService;
    private readonly IServiceProvider _services;
    private readonly ILoggingService _loggingService;

    private bool _initialized;

    public TextCommandHandler(DiscordSocketClient client,
        CommandService commandService,
        IServiceProvider services,
        ILoggingService loggingService)
    {
        _client = client;
        _commandService = commandService;
        _services = services;
        _loggingService = loggingService;
    }

    public async Task InitializeAsync()
    {
        if(_initialized)
        {
            return;
        }

        _client.MessageReceived += HandleCommandAsync;
        _commandService.Log += _loggingService.LogAsync;
        _commandService.CommandExecuted += CommandExecutedAsync;

        await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(),
             _services);


        _initialized = true;
    }

    private async Task HandleCommandAsync(SocketMessage messageParam)
    {
        var message = messageParam as SocketUserMessage;
        if (message == null)
        {
            return;
        }

        var context = new SocketCommandContext(_client, message);

        SocketTextChannel? channel = message.Channel as SocketTextChannel;
        string prefix = "%";

        int argPos = 0;
        if (!(message.HasStringPrefix(prefix, ref argPos) ||
            message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
        {
            return;
        }

        if (message.Author.IsBot)
        {
            return;
        }

        await _commandService.ExecuteAsync(
            context: context,
            argPos: argPos,
            services: _services);
    }

    private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, Discord.Commands.IResult result)
    {
        // Runs when text command executes
    }

    public void Dispose()
    {
        _client.MessageReceived -= HandleCommandAsync;
        _commandService.Log -= _loggingService.LogAsync;
        _commandService.CommandExecuted -= CommandExecutedAsync;
    }
}
