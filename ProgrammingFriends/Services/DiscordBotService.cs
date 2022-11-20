using Discord;
using Discord.WebSocket;
using ProgrammingFriends.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services;
public class DiscordBotService : IDiscordBotService
{
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly ILoggingService _loggingService;
    private readonly TextCommandHandler _textCommandHandler;
    private readonly InteractionHandler _interactionHandler;

    public DiscordBotService(DiscordSocketClient discordSocketClient,
        ILoggingService loggingService,
        TextCommandHandler textCommandHandler,
        InteractionHandler interactionHandler)
    {
        _discordSocketClient = discordSocketClient;
        _loggingService = loggingService;
        _textCommandHandler = textCommandHandler;
        _interactionHandler = interactionHandler;
        _discordSocketClient.Log += _loggingService.LogAsync;
        _discordSocketClient.Ready += OnReady;
    }

    public async Task StartAsync()
    {
        await _discordSocketClient.LoginAsync(TokenType.Bot, "");
        await _discordSocketClient.StartAsync();
    }

    public async Task StopAsync()
    {
        await _discordSocketClient.LogoutAsync();
        await _discordSocketClient.StopAsync();
        await _discordSocketClient.DisposeAsync();
    }

    private async Task OnReady()
    {
        await _textCommandHandler.InitializeAsync();
        await _interactionHandler.InitializeAsync();
    }
}
