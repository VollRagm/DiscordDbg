# DiscordDbg
Application to route DbgView 4.90 output to a Discord bot.

## Compiling and running

Clone the repository and run a nuget restore on the C# project and fill in your Bot token, guild and channel id.  
Make sure you compile both projects in the x86 configuration.  
Place the dbgviewhook.dll into the same directory as the DbgviewDiscord.exe, run DbgView, run DbgviewDiscord.exe.  

The hook is placed after DbgView's filters, so you can apply the filters in DbgView.

![](https://i.imgur.com/86HCNQ2.png "Demo")
