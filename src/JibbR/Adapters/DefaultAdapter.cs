using System.Collections.Generic;

namespace JibbR.Adapters
{
    [AdapterName("default-adapter")]
    [AdapterDescription("help", "Provides help about the loaded adapters"),
     AdapterUsage("help", "jibbr help"),
     AdapterUsage("help", "jibbr help math")]
    [AdapterDescription("stalker", "Tells you the list of rooms a user is in which jibbr is also in"),
     AdapterUsage("stalker", "/msg jibbr where are you"),
     AdapterUsage("stalker", "/msg jibbr where is mr-t")]
    public class DefaultAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            robot.AddPrivateResponder(@"help\s*(?<command>.*)?$", (session, message, from, match) =>
            {
                var command = match.ValueFor("command");
                if (string.IsNullOrWhiteSpace(command))
                {
                    robot.SendPrivateMessage(@from, "Help\n\n{0}", robot.HelpText());
                    return;
                }

                var helpMessage = "Help for {0}\n\n{1}";
                var helpText = robot.HelpTextFor(command);
                if (string.IsNullOrWhiteSpace(helpText))
                {
                    helpMessage = "No help for '{0}'";
                }

                robot.SendPrivateMessage(@from, helpMessage, command, helpText);
            });

            robot.AddListener(@"jibbe?r jabbe?r", (session, message, room, match) =>
            {
                robot.SendMessage(room, "@{0} I got no time for the jibber-jabber!", session.Message.User.Name);
            });

            robot.AddPrivateResponder(@"note (?<note>.*)$", (session, message, from, match) =>
            {
                // ToDo: add permission check
                var note = match.ValueFor("note");

                robot.SetNote(note);
            });

            robot.AddPrivateResponder(@"flag (?<country>.*)$", (session, message, from, match) =>
            {
                // ToDo: add permission check
                var countryCode = match.ValueFor("country");

                robot.SetFlag(countryCode);
            });

            robot.AddPrivateResponder(@"where (?:are you|is (?<who>.*))\b\?*", (session, message, from, match) =>
            {
                var who = match.ValueFor("who");

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

                robot.SendPrivateMessage(from, toSay, who, where);
            });
        }
    }
}