namespace JibbR.Adapters
{
    [RobotAdapterMetadata("default-adapter")]
    public class DefaultAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"help\s*(?<command>.*)?$", (session, message, room, match) =>
            {
                session.Client.Send("@" + session.Message.User.Name + " Hey I just met you, and this is crazy, but there's no help right now; so call me later maybe! :)", room);
            });

            robot.AddListener(@"jibbe?r jabbe?r", (session, message, room, match) =>
            {
                session.Client.Send(string.Format("@{0} I got no time for the jibber-jabber!", session.Message.User.Name), room);
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
        }
    }
}