using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using RestSharp;

namespace JibbR.Adapters
{
    [AdapterName("math-adapter")]
    [AdapterDescription("math", "Provides basic calculations and conversions"),
     AdapterUsage("math", "jibbr calc [me] 10 + 3"),
     AdapterUsage("math", "jibbr calculate [me] 10 + 3"),
     AdapterUsage("math", "jibbr convert [me] 3 pounds to grams"),
     AdapterUsage("math", "jibbr math [me] 10 + 3")]
    public class MathAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"(calc|calculate|convert|math)( me)? (?<query>.*)", (session, message, room, match) =>
            {
                Task.Factory.StartNew(() =>
                {
                    var query = match.ValueFor("query");

                    var restClient = new RestClient(@"https://www.google.com/ig/calculator");

                    var request = new RestRequest(Method.GET);
                    request.AddParameter("hl", "en");
                    request.AddParameter("q", query);

                    var response = restClient.Execute(request);

                    var result = JObject.Parse(response.Content).Value<string>("rhs");

                    var solution = string.IsNullOrWhiteSpace(result) ? "Could not compute." : result.Replace("\uFFFD", string.Empty);

                    robot.SendMessage(room, "@{0} {1}", session.Message.User.Name, solution);
                });
            });
        }
    }
}