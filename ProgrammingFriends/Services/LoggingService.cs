using Discord;
using ProgrammingFriends.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services;
public class LoggingService : ILoggingService
{
    public async Task LogAsync(LogMessage message)
    {
        await Console.Out.WriteLineAsync($"{message.Source} {message.Message}");
        if (message.Exception is not null)
        {
            await Console.Out.WriteLineAsync($"{message.Exception}");
        }
    }
}
