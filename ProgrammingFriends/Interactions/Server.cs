using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammingFriends.Interactions;
public class Server : InteractionModuleBase<SocketInteractionContext>
{
    [RequireContext(ContextType.Guild)]
    [SlashCommand("owner", "Retreive the server owner")]
    public async Task Owner()
    {
        await RespondAsync($"{Context.Guild.Owner.DisplayName} is the owner of {Context.Guild.Name}");
    }
}
