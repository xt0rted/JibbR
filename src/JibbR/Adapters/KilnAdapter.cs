// Note: none of these actually do anything... they're just stubs for testing at the moment
namespace JibbR.Adapters
{
    public class KilnAdapter : IRobotAdapter
    {
        public string Name { get { return "kiln-adapter"; } }

        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"push\s+(?<project>.*)\s+from\s+(?<from>.*)\s+to\s+(?<to>.*)", (session, message, room, match) =>
            {
                var project = match.Groups["project"].Value;
                var from = match.Groups["from"].Value;
                var to = match.Groups["to"].Value;

                session.Client.Send(string.Format("{0} was just pushed from ({1}) to ({2}) successfully.", project, from, to), room);
            });

            robot.AddResponder(@"push-alias\s+(?<from>.*)\s+is\s+(?<to>.*)", (session, message, room, match) =>
            {
                var from = match.Groups["from"].Value;
                var to = match.Groups["to"].Value;

                session.Client.Send(string.Format("({0}) is now an alis for ({1})", from, to), room);
            });

            robot.AddResponder(@"push-alias\s+list", (session, message, room, match) =>
            {
                session.Client.Send(string.Format("@{0} listing all user aliases", session.Message.User.Name), room);
            });
        }
    }
}