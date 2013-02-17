using System.Collections.Generic;

namespace JibbR.Adapters
{
    [AdapterName("default-adapter")]
    public class DefaultAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"help\s*(?<command>.*)?$", (session, message, room, match) =>
            {
                robot.SendMessage(room, "@{0} Hey I just met you, and this is crazy, but there's no help right now; so call me later maybe! :)", session.Message.User.Name);
            });

            robot.AddListener(@"jibbe?r jabbe?r", (session, message, room, match) =>
            {
                robot.SendMessage(room, "@{0} I got no time for the jibber-jabber!", session.Message.User.Name);
            });

            robot.AddPrivateResponder(@"note:\s*(?<note>.*)$", (session, message, from, match) =>
            {
                var note = match.Groups["note"].Value;
                session.Client.SetNote(note).ContinueWith(task =>
                {
                    session.Client.SendPrivateMessage(from, "new note set");
                });
            });

            robot.AddPrivateResponder(@"flag:\s*(?<flag>.*)$", (session, message, from, match) =>
            {
                var flag = match.Groups["flag"].Value;
                session.Client.SetFlag(flag).ContinueWith(task =>
                {
                    session.Client.SendPrivateMessage(from, "new flag set");
                });
            });

            robot.AddPrivateResponder(@"where (?:are you|is (?<who>.*))\b\?*", (session, message, from, match) =>
            {
                var who = match.Groups["who"].Value;

                var toSay = "{0} is in #{1}";

                if (string.IsNullOrWhiteSpace(who))
                {
                    who = session.BotName;
                    toSay = "I'm in #{1}";
                }

                var where = string.Empty;

                List<string> rooms;
                if (robot.Settings.KnownUsers.TryGetValue(who, out rooms))
                {
                    where = string.Join(", #", rooms);
                }
                else
                {
                    toSay = "I can't find {0}";
                }

                session.Client.SendPrivateMessage(from, string.Format(toSay, who, where));
            });
        }
    }
}