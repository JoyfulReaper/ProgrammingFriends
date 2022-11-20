using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using ProgrammingFriends.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services;
public class InteractionHandler : IDisposable
{
    private readonly InteractionService _interactionService;
    private readonly DiscordSocketClient _client;
    private readonly ILoggingService _loggingService;
    private readonly IServiceProvider _serviceProvider;
    private bool _initialized;

    public InteractionHandler(InteractionService interactionService,
        DiscordSocketClient client,
        ILoggingService loggingService,
        IServiceProvider serviceProvider)
	{
        _interactionService = interactionService;
        _client = client;
        _loggingService = loggingService;
        _serviceProvider = serviceProvider;
    }

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        _client.InteractionCreated += HandleInteractionAsync;
        //_client.UserCommandExecuted += UserCommandExcuted;
        _interactionService.SlashCommandExecuted += SlashCommandExcuted;
        //_interactionService.ContextCommandExecuted += ContextCommandExcuted;
        //_interactionService.ComponentCommandExecuted += ComponentCommentExecuted;
        //_interactionService.InteractionExecuted += InteractionExcuted;
        //_interactionService.Log += _loggingService.LogAsync;


        await _interactionService.AddModulesAsync(Assembly.GetEntryAssembly(), _serviceProvider);
        _initialized = true;
    }

    private async Task HandleInteractionAsync(SocketInteraction interactionParam)
    {
        try
        {
            var ctx = new SocketInteractionContext(_client, interactionParam);
            await _interactionService.ExecuteCommandAsync(ctx, _serviceProvider);
        }
        catch (Exception ex)
        {
            await _loggingService.LogAsync(new LogMessage(Discord.LogSeverity.Warning, "InteractionHandler", $"Error handling interaction: {ex.Message}", ex));

            if (interactionParam.Type == InteractionType.ApplicationCommand)
            {
                await interactionParam.GetOriginalResponseAsync()
                    .ContinueWith(async (msg) => await msg.Result.DeleteAsync());
            }
        }
    }

    private async Task SlashCommandExcuted(SlashCommandInfo slashInfo,
        IInteractionContext context,
        IResult result)
    {
        //if (!result.IsSuccess)
        //{
        //    switch (result.Error)
        //    {
        //        case InteractionCommandError.UnmetPrecondition:
        //            // implement
        //            break;
        //        case InteractionCommandError.UnknownCommand:
        //            _logger.LogInformation("{user}#{discriminator} attempted to use an unknown slash command: {slashCommand} on {guild}/{channel}",
        //                context.User.Username, context.User.Discriminator, slashInfo?.Name ?? "(unknown slash command)", context.Guild?.Name ?? "DM", context.Channel.Name);

        //            return;
        //        case InteractionCommandError.BadArgs:
        //            // implement
        //            break;
        //        case InteractionCommandError.Exception:
        //            _logger.LogInformation(((ExecuteResult)result).Exception, "An exception occured in {slashCommand} for {user}#{discriminator}  on {guild}/{channel}",
        //                slashInfo?.Name ?? "(unknown slash command)", context.User.Username, context.User.Discriminator, context.Guild?.Name ?? "DM", context.Channel.Name);

        //            await context.Channel.SendMessageAsync($"The slash command threw an exception :(\nError: {result.ErrorReason}");
        //            return;
        //        case InteractionCommandError.Unsuccessful:
        //            _logger.LogDebug("Slash command {slashcommand} was unsuccessful for {user}#{discriminator} on {guild}/{channel}: {error}",
        //                slashInfo?.Name ?? "(unknown slash command)", context.User.Username, context.User.Discriminator, context.Guild?.Name ?? "DM", context.Channel.Name, result.ErrorReason);

        //            await context.Channel.SendMessageAsync($"The slash command {slashInfo?.Name ?? "(unknown slash command)"} was unsuccessful :(\nError: {result.ErrorReason}");
        //            return;
        //        default:
        //            break;
        //    }

        //    _logger.LogInformation("{user}#{discriminator} failed to execute an {interaction} on {guild}/{channel}: {reason}",
        //        context.User.Username, context.User.Discriminator, slashInfo.Name, context.Guild?.Name ?? "DM", context.Channel.Name, result.ErrorReason);

        //    await context.Channel.SendMessageAsync($"Something went wrong :(\nError: {result.ErrorReason}");
        //}
    }

    public void Dispose()
    {
        _client.InteractionCreated -= HandleInteractionAsync;
        //_client.UserCommandExecuted += UserCommandExcuted;
        _interactionService.SlashCommandExecuted -= SlashCommandExcuted;
        //_interactionService.ContextCommandExecuted += ContextCommandExcuted;
        //_interactionService.ComponentCommandExecuted += ComponentCommentExecuted;
        //_interactionService.InteractionExecuted += InteractionExcuted;
        _interactionService.Log -= _loggingService.LogAsync;
    }
}
