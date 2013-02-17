using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using JibbR.Settings;

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
        void HeartBeat();

        void JoinRoom(string roomName);
        void JoinRooms(IEnumerable<string> roomNames);

        void LeaveRoom(string roomName);
        void LeaveRooms(IEnumerable<string> roomNames);

        void AddListener(string regex, MessageHandler function);
        void AddResponder(string regex, MessageHandler function);

        void AddPrivateResponder(string regex, MessageHandler function);

        void SendMessage(string room, string message, params object[] args);
    }
}