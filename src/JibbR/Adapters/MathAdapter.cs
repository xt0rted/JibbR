using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using RestSharp;

namespace JibbR.Adapters
{
    [RobotAdapterMetadata("math-adapter")]
    public class MathAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"(calc|calculate|convert|math)( me)? (?<query>.*)", (session, message, room, match) =>
            {
                Task.Factory.StartNew(() =>
                {
                    var query = match.Groups["query"].Value;

                    var restClient = new RestClient(@"https://www.google.com/ig/calculator");

                    var request = new RestRequest(Method.GET);
                    request.AddParameter("hl", "en");
                    request.AddParameter("q", query);

                    var response = restClient.Execute(request);

                    var result = JObject.Parse(response.Content).Value<string>("rhs");

                    var solution = string.IsNullOrWhiteSpace(result) ? "Could not compute." : result;

                    session.Client.Send(string.Format("@{0} {1}", session.Message.User.Name, solution), room);
                });
            });
        }
    }
}