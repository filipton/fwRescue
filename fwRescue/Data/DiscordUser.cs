using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using fwRescue.Pages;
using Microsoft.AspNetCore.Components;

namespace fwRescue
{
    [Serializable]
    public struct RescueDmChats
	{
        public string name;
        public List<DiscordMessage> messages;

        public RescueDmChats(string nick, List<DiscordMessage> dmMessages)
		{
            name = nick;
            messages = dmMessages;
		}
	}

	public class DiscordUser
	{
        public static DiscordClient Client { get; set; }
        public static List<RescueDmChats> RescueDmChats = new List<RescueDmChats>();

        public static ulong ActiveChatId;
        public static bool UserOnline;

        public async Task RunBotAsync()
        {
            WriteLine("STARTING BOT!");

            string fileTokenPath = Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location.Replace("fwRescue.dll", ""), "token.cfg");
            WriteLine(fileTokenPath);
            if(!File.Exists(fileTokenPath))
			{
                File.Create(fileTokenPath);
			}
			else
			{
                var cfg = new DiscordConfiguration
                {
                    Token = File.ReadAllText(fileTokenPath),
                    TokenType = TokenType.User,

                    AutoReconnect = true,
                    LogLevel = LogLevel.Debug,
                    UseInternalLogHandler = true
                };

                Client = new DiscordClient(cfg);

                Client.Ready += this.Client_Ready;
                Client.ClientErrored += this.Client_ClientError;
                Client.MessageCreated += Message_Sent;

                await Client.ConnectAsync();

                await Task.Delay(-1);
            }
        }

        private Task Message_Sent(MessageCreateEventArgs e)
        {
            if (e.Channel.IsPrivate)
            {
                foreach (fwRescue.Pages.Index c in fwRescue.Pages.Index.indexes)
                {
                    c.Refresh();
                }
            }

            return Task.CompletedTask;
        }

        private Task Client_Ready(ReadyEventArgs e)
        {
            WriteLine("Client is ready to process events.");
            UserOnline = true;
            Pages.Index.RefreshPage();
            return Task.CompletedTask;
        }

        private Task Client_ClientError(ClientErrorEventArgs e)
        {
            WriteLine($"Exception occured: {e.Exception.GetType()}: {e.Exception.Message}");

            return Task.CompletedTask;
        }

        public static void WriteLine(string content)
		{
            File.AppendAllText(@"C:\Users\filip\Desktop\logs.txt", $"[{DateTime.Now.ToString()}] {content}" + Environment.NewLine);
		}
    }

    public class MessageSendModel
    {
        [Required]
        [StringLength(2000, ErrorMessage = "Message is too long.")]
        public string Message { get; set; }
    }
}
