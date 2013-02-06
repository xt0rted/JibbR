using System;
using System.Text.RegularExpressions;

namespace JibbR
{
    using MessageHandler = Action<ISession, string, string, Match>;

    public interface IRobot
    {
        string Name { get; }
        IRobotSettings Settings { get; }

        void SetupClient(Uri host);
        void Connect(string userName, string password);
        void Disconnect();
        void JoinRoom(string roomName);
        void JoinRooms(string[] roomNames);

        void AddListener(string regex, MessageHandler function);
        void AddResponder(string regex, MessageHandler function);

        void AddPrivateResponder(string regex, MessageHandler function);
    }
}