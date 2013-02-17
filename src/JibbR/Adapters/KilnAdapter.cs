// Note: none of these actually do anything... they're just stubs for testing at the moment
namespace JibbR.Adapters
{
    [AdapterName("kiln-adapter")]
    public class KilnAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"push\s+(?<project>.*)\s+from\s+(?<from>.*)\s+to\s+(?<to>.*)", (session, message, room, match) =>
            {
                var project = match.ValueFor("project");
                var from = match.ValueFor("from");
                var to = match.ValueFor("to");

                robot.SendMessage(room, "{0} was just pushed from ({1}) to ({2}) successfully.", project, from, to);
            });

            robot.AddResponder(@"push-alias\s+(?<from>.*)\s+is\s+(?<to>.*)", (session, message, room, match) =>
            {
                var from = match.ValueFor("from");
                var to = match.ValueFor("to");

                robot.SendMessage(room, "({0}) is now an alis for ({1})", from, to);
            });

            robot.AddResponder(@"push-alias\s+list", (session, message, room, match) =>
            {
                robot.SendMessage(room, "@{0} listing all user aliases", session.Message.User.Name);
            });
        }
    }
}