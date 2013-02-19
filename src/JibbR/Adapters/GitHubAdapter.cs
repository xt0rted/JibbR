using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

using RestSharp;

namespace JibbR.Adapters
{
    [AdapterName("github-adapter")]
    [AdapterDescription("github", "Provides status information about github.com"),
     AdapterUsage("github", "jibbr github status"),
     AdapterUsage("github", "jibbr github status last"),
     AdapterUsage("github", "jibbr github status messages")]
    public class GitHubAdapter : IRobotAdapter
    {
        private const string GitHubApiUrl = "https://status.github.com/api";

        public void Setup(IRobot robot)
        {
            robot.AddResponder(@"github status$", (session, message, room, match) =>
            {
                robot.SendMessage(room, "GitHub {0}", Status());
            });

            robot.AddResponder(@"github status last$", (session, message, room, match) =>
            {
                robot.SendMessage(room, "GitHub {0}", LastStatus());
            });

            robot.AddResponder(@"github status messages$", (session, message, room, match) =>
            {
                robot.SendMessage(room, string.Join("\n\n", StatusMessages()));
            });
        }

        private static string FormatStatus(JToken data)
        {
            var createdOn = (DateTime) data["created_on"];
            var status = (string) data["status"];
            var body = (string) data["body"];

            return string.Format("Status: {0}\n"
                                 + "Message: {1}\n"
                                 + "Date: {2} UTC", status, body, createdOn);
        }

        private static string Status()
        {
            var client = new RestClient(GitHubApiUrl);

            var request = new RestRequest("status.json", Method.GET);
            var result = client.Execute(request);

            var data = JObject.Parse(result.Content);

            var lastUpdated = (DateTime) data["last_updated"];
            var status = (string) data["status"];
            var secondsAgo = DateTime.UtcNow - lastUpdated;

            return string.Format("Status: {0} ({1:%s} seconds ago)", status, secondsAgo);
        }

        private static string LastStatus()
        {
            var client = new RestClient(GitHubApiUrl);

            var request = new RestRequest("last-message.json", Method.GET);
            var result = client.Execute(request);

            var data = JObject.Parse(result.Content);

            return FormatStatus(data);
        }

        private static IEnumerable<string> StatusMessages()
        {
            var client = new RestClient(GitHubApiUrl);

            var request = new RestRequest("messages.json", Method.GET);
            var result = client.Execute(request);

            var data = JArray.Parse(result.Content);

            return data.Select(FormatStatus);
        }
    }
}