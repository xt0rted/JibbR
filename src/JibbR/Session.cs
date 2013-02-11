using System.Collections.Concurrent;
using System.Collections.Generic;

using JabbR.Client;
using JabbR.Client.Models;

namespace JibbR
{
    public class Session : ISession
    {
        public Session(JabbRClient client, Message message, string botName, ConcurrentDictionary<string, List<string>> knownUsers)
        {
            Client = client;
            Message = message;
            BotName = botName;
            KnownUsers = knownUsers;
        }

        public JabbRClient Client { get; private set; }
        public Message Message { get; private set; }
        public string BotName { get; private set; }
        public ConcurrentDictionary<string, List<string>> KnownUsers { get; private set; }
    }
}