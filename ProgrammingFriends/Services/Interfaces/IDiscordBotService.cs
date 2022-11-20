using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Services.Interfaces;

public interface IDiscordBotService
{
    Task StartAsync();
    Task StopAsync();
}
