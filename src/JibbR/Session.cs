using JabbR.Client.Models;

namespace JibbR
{
    public class Session : ISession
    {
        public Session(Message message, string botName)
        {
            Message = message;
            BotName = botName;
        }

        public Message Message { get; private set; }
        public string BotName { get; private set; }
    }
}