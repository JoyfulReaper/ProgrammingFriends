using Discord;
using Discord.WebSocket;
using ProgrammingFriends.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services;
internal class DiscordBotService : IDiscordBotService
{
    private readonly DiscordSocketClient _discordSocketClient;

    public DiscordBotService(DiscordSocketClient discordSocketClient)
    {
        _discordSocketClient = discordSocketClient;

        _discordSocketClient.Log += LogAsync;
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

    public async Task LogAsync(LogMessage message)
    {
        await Console.Out.WriteLineAsync($"{message.Source} {message.Message}");
        if (message.Exception is not null)
        {
            await Console.Out.WriteLineAsync($"{message.Exception}");
        }
    }
}
