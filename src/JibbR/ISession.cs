using System.Collections.Concurrent;
using System.Collections.Generic;

using JabbR.Client;
using JabbR.Client.Models;

namespace JibbR
{
    public interface ISession
    {
        JabbRClient Client { get; }
        Message Message { get; }
        string BotName { get; }
        ConcurrentDictionary<string, List<string>> KnownUsers { get; }
    }
}