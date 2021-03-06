﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using JibbR.Routing;
using JibbR.Settings;

namespace JibbR
{
    using MessageHandler = Action<ISession, string, string, Match>;
    using RouteHandler = Action<IRequest, IResponse>;

    public interface IRobot
    {
        string Name { get; }
        IRobotSettings Settings { get; }

        void SetupClient(Uri host);
        void Connect(string userName, string password);
        void Disconnect();

        string HelpText();
        string HelpTextFor(string command);

        void JoinRoom(string roomName);
        void JoinRooms(IEnumerable<string> roomNames);

        void LeaveRoom(string roomName);
        void LeaveRooms(IEnumerable<string> roomNames);

        void AddListener(string regex, MessageHandler function);
        void AddResponder(string regex, MessageHandler function);

        void AddPrivateResponder(string regex, MessageHandler function);

        void AddRoute(RouteMethod method, string path, RouteHandler function);

        void SendMessage(string room, string message, params object[] args);
        void SendPrivateMessage(string to, string message, params object[] args);
        void SetFlag(string country);
        void SetNote(string note);
    }
}