using JabbR.Client;
using JabbR.Client.Models;

namespace JibbR
{
    public class Session : ISession
    {
        public Session(JabbRClient client, Message message, string botName)
        {
            Client = client;
            Message = message;
            BotName = botName;
        }

        public JabbRClient Client { get; private set; }
        public Message Message { get; private set; }
        public string BotName { get; private set; }
    }
}