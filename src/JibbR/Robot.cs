using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using JabbR.Client;
using JabbR.Client.Models;

using JibbR.Settings;

namespace JibbR
{
    using MessageHandler = Action<ISession, string, string, Match>;
    using PrivateMessageHandler = Action<ISession, string, string, Match>;

    public class Robot : IRobot
    {
        private const RegexOptions DefaultRegexOptions = RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase | RegexOptions.Compiled;

        private readonly IAdapterManager _adapterManager;
        private readonly ISettingsManager _settingsManager;
        private JabbRClient _client;
        private bool _isSetup;

        private readonly List<string> _currentRooms = new List<string>();

        private readonly Dictionary<string, MessageHandler> _listenerHandlers = new Dictionary<string, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _responderHandlers = new Dictionary<string, MessageHandler>();
        private readonly Dictionary<string, MessageHandler> _privateResponderHandlers = new Dictionary<string, PrivateMessageHandler>();

        public Robot(IAdapterManager adapterManager, ISettingsManager settingsManager)
        {
            _adapterManager = adapterManager;
            _settingsManager = settingsManager;

            settingsManager.LoadSettings();
        }

        public string Name { get; private set; }

        public IRobotSettings Settings { get { return _settingsManager.Settings; } }

        private void ClientOnUserJoined(User user, string room, bool isOwner)
        {
            List<string> rooms;
            var isUpdating = true;
            if (!Settings.KnownUsers.TryGetValue(user.Name, out rooms))
            {
                isUpdating = false;
                rooms = new List<string>();
            }

            if (rooms.Contains(room))
            {
                // no need to track them, for some reason we already are
                return;
            }

            rooms.Add(room);

            if (!isUpdating)
            {
                Settings.KnownUsers.TryAdd(user.Name, rooms);
            }
        }

        private void ClientOnUserLeft(User user, string room)
        {
            List<string> rooms;
            if (!Settings.KnownUsers.TryGetValue(user.Name, out rooms))
            {
                return;
            }

            rooms.Remove(room);

            if (rooms.Count != 0)
            {
                return;
            }

            if (!Settings.KnownUsers.TryRemove(user.Name, out rooms))
            {
                Console.Error.WriteLine("There was an error removing '{0}' from the user list.", user.Name);
            }
        }

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
            if (_isSetup)
            {
                Console.Error.WriteLine("Client is already setup.");
                return;
            }

            _isSetup = true;

            _client = new JabbRClient(host);

            _client.MessageReceived += ClientOnMessageReceived;
            _client.PrivateMessage += ClientOnPrivateMessage;
            _client.UserJoined += ClientOnUserJoined;
            _client.UserLeft += ClientOnUserLeft;

            _adapterManager.SetupAdapters(this);
        }

        public void Connect(string userName, string password)
        {
            Name = userName;

            var currentInfo = _client.Connect(userName, password).Result;

            // check if we're in rooms we shouldn't be, and leave them if we are
            var roomsToLeave = currentInfo.Rooms.Select(r => r.Name).Except(Settings.Rooms);
            LeaveRooms(roomsToLeave);

            // join any rooms we're not already in
            var roomsToJoin = Settings.Rooms.Except(currentInfo.Rooms.Select(r => r.Name));
            JoinRooms(roomsToJoin);

            foreach (var room in _currentRooms)
            {
                var roomInfo = _client.GetRoomInfo(room).Result;
                foreach (var user in roomInfo.Users)
                {
                    List<string> rooms;
                    if (!Settings.KnownUsers.TryGetValue(user.Name, out rooms))
                    {
                        rooms = new List<string>();
                    }

                    rooms.Add(room);

                    Settings.KnownUsers[user.Name] = rooms;
                }
            }
        }

        public void Disconnect()
        {
            LeaveRooms(_currentRooms);

            _client.LogOut().ContinueWith(_ => _client.Disconnect()).Wait();

            _settingsManager.SaveSettings();
        }

        public void HeartBeat()
        {
            var room = _currentRooms.FirstOrDefault();
            if (room != null)
            {
                _client.SetTyping(room);
            }
        }

        public void JoinRoom(string roomName)
        {
            Console.WriteLine("joining room '{0}'", roomName);
            _client.JoinRoom(roomName)
                   .ContinueWith(_ => _currentRooms.Add(roomName))
                   .Wait();
        }

        public void JoinRooms(IEnumerable<string> roomNames)
        {
            foreach (var roomName in roomNames)
            {
                JoinRoom(roomName);
            }
        }

        public void LeaveRoom(string roomName)
        {
            Console.WriteLine("leaving room '{0}'", roomName);
            _client.LeaveRoom(roomName)
                   .ContinueWith(_ => _currentRooms.Remove(roomName))
                   .Wait();
        }

        public void LeaveRooms(IEnumerable<string> roomNames)
        {
            // gotta reverse the rooms since we remove each one from the current room list in LeaveRoom()
            foreach (var roomName in roomNames.Reverse())
            {
                LeaveRoom(roomName);
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