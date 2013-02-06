using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JabbR.Client;
using JabbR.Client.Models;

namespace JibbR
{
    using MessageHandler = Action<ISession, string, string, Match>;
    using PrivateMessageHandler = Action<ISession, string, string, Match>;

    public class Robot : IRobot
    {
        protected const RegexOptions DefaultRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled;

        private readonly IAdapterManager _adapterManager;
        private JabbRClient _client;
        private bool isSetup;

        private readonly List<string> _currentrooms = new List<string>();

        private readonly Dictionary<string, MessageHandler> _listenerHandlers = new Dictionary<string, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _responderHandlers = new Dictionary<string, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _privateResponderHandlers = new Dictionary<string, PrivateMessageHandler>();

        public Robot(IAdapterManager adapterManager, ISettingsManager settingsManager)
        {
            _adapterManager = adapterManager;

            Settings = settingsManager.LoadSettings();
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
                handlers = _responderHandlers;
                messageBody = match.Groups["message"].Value;
            }
            else
            {
                handlers = _listenerHandlers;
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

        private void ClientOnPrivateMessage(string from, string to, string message)
        {
            // we never want to respond to ourself...
            if (string.Equals(from, Name, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            Console.WriteLine("private message received");

            ISession session = new Session(_client, null, Name);

            foreach (var handler in _privateResponderHandlers)
            {
                var result = HandlePrivateCommand(session, message, from, handler.Key, handler.Value);
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

        private bool HandlePrivateCommand(ISession session, string message, string from, string regex, PrivateMessageHandler callback)
        {
            var match = Regex.Match(message, regex, DefaultRegexOptions);
            if (match.Success)
            {
                callback(session, message, from, match);

                return true;
            }

            return false;
        }

        public void SetupClient(Uri host)
        {
            if (isSetup)
            {
                Console.Error.WriteLine("Client is already setup.");
                return;
            }

            isSetup = true;

            _client = new JabbRClient(host);

            _client.MessageReceived += ClientOnMessageReceived;
            _client.PrivateMessage += ClientOnPrivateMessage;

            _adapterManager.SetupAdapters(this);
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
            _listenerHandlers.Add(regex, function);
        }

        public void AddResponder(string regex, MessageHandler function)
        {
            _responderHandlers.Add(regex, function);
        }

        public void AddPrivateResponder(string regex, PrivateMessageHandler function)
        {
            _privateResponderHandlers.Add(regex, function);
        }
    }
}