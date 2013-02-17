using JabbR.Client.Models;

namespace JibbR
{
    public interface ISession
    {
        Message Message { get; }
        string BotName { get; }
    }
}