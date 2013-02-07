using System.Text.RegularExpressions;

namespace JibbR.Adapters
{
    [RobotAdapterMetadata("bing-web-adapter")]
    public class BingWebAdapter : IRobotAdapter
    {
        private readonly IBingClient _bingClient;

        public BingWebAdapter(IBingClient bingClient)
        {
            _bingClient = bingClient;
        }

        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"bing\s+(me\s+)?(?<query>.*)", (session, message, room, match) =>
            {
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                {
                    var query = match.Groups["query"].Value;

                    var result = _bingClient.WebSearch(query);

                    var resultMatch = Regex.Match(result, @"<div class=""sb_tlst""><h3><a href=""(?<result>[^""]*)");

                    string resultMessage;
                    if (resultMatch.Success)
                    {
                        resultMessage = string.Format("@{0} {1}", session.Message.User.Name, resultMatch.Groups["result"].Value);
                    }
                    else
                    {
                        resultMessage = string.Format("@{0} Sorry, Bing had zero results for '{1}'", session.Message.User.Name, query);
                    }

                    session.Client.Send(resultMessage, room);
                });
            });
        }
    }
}