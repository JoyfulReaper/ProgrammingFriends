﻿using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using ProgrammingFriends;
using ProgrammingFriends.Services;
using ProgrammingFriends.Services.Interfaces;

IServiceProvider serviceProvider = Bootstrap.Initialize(args);
DiscordSocketClient client = serviceProvider.GetRequiredService<DiscordSocketClient>();
IDiscordBotService bot = serviceProvider.GetRequiredService<IDiscordBotService>();

await bot.StartAsync();
await Task.Delay(-1);