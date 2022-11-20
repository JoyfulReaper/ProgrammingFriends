using Microsoft.Extensions.DependencyInjection;
using ProgrammingFriends;
using ProgrammingFriends.Services;
using ProgrammingFriends.Services.Interfaces;

IServiceProvider serviceProvider = Bootstrap.Initialize(args);
IDiscordBotService bot = serviceProvider.GetRequiredService<IDiscordBotService>();

await bot.StartAsync();
await Task.Delay(-1);