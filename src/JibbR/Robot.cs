using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JabbR.Client;
using JabbR.Client.Models;

namespace JibbR
{
    using MessageHandler = Action<ISession, string, string, Match>;

    public class Robot : IRobot
    {
        protected const RegexOptions DefaultRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled;

        private readonly IAdapterManager _adapterManager;
        private readonly JabbRClient _client;

        private readonly List<string> _currentrooms = new List<string>();

        private readonly Dictionary<string, MessageHandler> _listenHandler = new Dictionary<string, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _respondHandler = new Dictionary<string, MessageHandler>();

        public Robot(IAdapterManager adapterManager, ISettingsManager settingsManager, Uri host)
        {
            _adapterManager = adapterManager;

            Settings = settingsManager.LoadSettings();

            _client = new JabbRClient(host);

            _client.MessageReceived += ClientOnMessageReceived;

            // this is a total mess, but it works for now :)
            _client.PrivateMessage += (from, to, message) =>
            {
                if (message.StartsWith("note:", StringComparison.OrdinalIgnoreCase))
                {
                    var note = message.Substring(5).Trim();
                    _client.SetNote(note).ContinueWith(task =>
                    {
                        _client.SendPrivateMessage(from, "new note set");
                    });
                }
                else if (message.StartsWith("flag:", StringComparison.OrdinalIgnoreCase))
                {
                    var flag = message.Substring(5).Trim();
                    _client.SetFlag(flag).ContinueWith(task =>
                    {
                        _client.SendPrivateMessage(from, "new flag set");
                    });
                }
            };

            _adapterManager.SetupAdapters(this);
        }

        public string Name { get; private set; }
        public IRobotSettings Settings { get; private set; }

        private void ClientOnMessageReceived(Message message, string room)
        {
            // we never want to respond to ourself...
            if (string.Equals(message.User.Name, Name, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Console.WriteLine("message received");
            var match = Regex.Match(message.Content, string.Format(@"^(?<name>@?{0}\b)\s+(?<message>\b.*\b)", Name), DefaultRegexOptions);

            ISession session = new Session(_client, message, Name);

            Dictionary<string, MessageHandler> handlers;
            string messageBody;
            if (match.Success)
            {
                handlers = _respondHandler;
                messageBody = match.Groups["message"].Value;
            }
            else
            {
                handlers = _listenHandler;
                messageBody = message.Content;
            }

            foreach (var handler in handlers)
            {
                var result = HandleCommand(session, messageBody, room, handler.Key, handler.Value);
                if (result)
                {
                    break;
                }
            }
        }

        private bool HandleCommand(ISession session, string messageBody, string room, string regex, MessageHandler callback)
        {
            var match = Regex.Match(messageBody, regex, DefaultRegexOptions);
            if (match.Success)
            {
                callback(session, messageBody, room, match);

                return true;
            }

            return false;
        }

        public void Connect(string userName, string password)
        {
            Name = userName;
            _client.Connect(userName, password).Wait();
        }

        public void Disconnect()
        {
            System.Threading.Tasks.Task.WaitAll(_currentrooms.Select(room => _client.LeaveRoom(room)).ToArray());
            _currentrooms.Clear();

            _client.LogOut().ContinueWith(_ => _client.Disconnect()).Wait();
        }

        public void JoinRoom(string roomName)
        {
            _client.JoinRoom(roomName)
                   .ContinueWith(_ => _currentrooms.Add(roomName))
                   .Wait();
        }
        public void JoinRooms(string[] roomNames)
        {
            foreach (var roomName in roomNames)
            {
                JoinRoom(roomName);
            }
        }

        public void AddListener(string regex, MessageHandler function)
        {
            _listenHandler.Add(regex, function);
        }

        public void AddResponder(string regex, MessageHandler function)
        {
            _respondHandler.Add(regex, function);
        }
    }
}