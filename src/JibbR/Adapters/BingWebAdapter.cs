using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JibbR.Adapters
{
    [AdapterName("bing-web-adapter")]
    [AdapterDescription("bing search", "Returns the first search result from bing"),
     AdapterUsage("bing search", "jibbr bing [me] something amazing")]
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
                Task.Factory.StartNew(() =>
                {
                    var query = match.ValueFor("query");

                    var result = _bingClient.WebSearch(query);

                    var resultMatch = Regex.Match(result, @"<div class=""sb_tlst""><h3><a href=""(?<result>[^""]*)");

                    string resultMessage;
                    if (resultMatch.Success)
                    {
                        resultMessage = string.Format("@{0} {1}", session.Message.User.Name, resultMatch.ValueFor("result"));
                    }
                    else
                    {
                        resultMessage = string.Format("@{0} Sorry, Bing had zero results for '{1}'", session.Message.User.Name, query);
                    }

                    robot.SendMessage(room, resultMessage);
                });
            });
        }
    }
}