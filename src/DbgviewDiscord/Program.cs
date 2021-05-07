using DSharpPlus;
using DSharpPlus.Entities;
using InternalPipeComm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DbgviewDiscord
{
    class Program
    {
        string token = /* Insert your Discord Bot token here */;
        DiscordClient discord;
        DiscordChannel channel;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var dbgview = Process.GetProcessesByName("dbgview");
            if(dbgview.Length == 0)
            {
                Console.WriteLine("Dbgview not running!");
                Console.ReadLine();
                Environment.Exit(0);
            }

            var process = dbgview.First();
            Injector.Inject(process);
            await Task.Delay(700);      //small delay to make sure everything is set up

            PipeClient pc = new PipeClient("dbgview");
            pc.MessageRecieved += new EventHandler<PipeMessageEventArgs>(MessageRecieved);
            

            Console.WriteLine("Pipe connected!");

            discord = new DiscordClient(new DiscordConfiguration() { Token = token, TokenType = TokenType.Bot });
            await discord.ConnectAsync();

            var guild = await discord.GetGuildAsync( /* insert the guild id here. */ );
            channel = await discord.GetChannelAsync(/* insert the channel id here. */);

            pc.StartListening();

            await Task.Delay(-1);
        }
        

        private async void MessageRecieved(object sender, PipeMessageEventArgs e)
        {
            //if you're feeling cute feel free to add in proper handling if you reach the discord rate limit
            await channel.SendMessageAsync(e.Message);
        }
    }
}
