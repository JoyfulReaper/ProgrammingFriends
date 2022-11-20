using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services.Interfaces;

internal interface IDiscordBotService
{
    Task StartAsync();
    Task StopAsync();
}
