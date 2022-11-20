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
    private readonly ITextCommandHandler _textCommandHandler;
    private readonly IInteractionHandler _interactionHandler;

    public DiscordBotService(DiscordSocketClient discordSocketClient,
        ILoggingService loggingService,
        ITextCommandHandler textCommandHandler,
        IInteractionHandler interactionHandler)
    {
        _discordSocketClient = discordSocketClient;
        _loggingService = loggingService;
        _textCommandHandler = textCommandHandler;
        _interactionHandler = interactionHandler;
        _discordSocketClient.Log += _loggingService.LogAsync;
        _discordSocketClient.Ready += OnReady;
        _discordSocketClient.UserLeft += OnUserLeft;
        _discordSocketClient.UserJoined += OnUserJoined;
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

    private Task OnUserJoined(SocketGuildUser user)
    {
        // Runs when user joins
        return Task.CompletedTask;
    }

    private Task OnUserLeft(SocketGuild guild, SocketUser user)
    {
        //Runs when user leaves
        return Task.CompletedTask;
    }
}
