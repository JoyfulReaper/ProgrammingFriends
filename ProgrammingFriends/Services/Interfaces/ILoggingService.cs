using Discord;

namespace ProgrammingFriends.Services.Interfaces;
public interface ILoggingService
{
    Task LogAsync(LogMessage message);
}