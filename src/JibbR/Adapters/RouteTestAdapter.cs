using System;
using System.IO;
using System.Linq;

using JibbR.Routing;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// Remove: this adapter is just for testing & building out the route functionality, if it seems hacky...that's because it is :)
namespace JibbR.Adapters
{
    [AdapterName("routetest-adapter")]
    public class RouteTestAdapter : IRobotAdapter
    {
        public void Setup(IRobot robot)
        {
            var settings = robot.Settings.SettingsFor<RouteTestAdapter>();

            var room = (string) settings.Settings.Room;

            robot.AddRoute(RouteMethod.Get, "/jibbr/ping", (request, response) =>
            {
                response.ContentType = "text/html";
                response.StatusCode = 200;
                response.Write("<html>" +
                                   "<head>" +
                                       "<title>JibbR</title>" +
                                   "</head>" +
                                   "<body>" +
                                       "<p>jibbr ping</p>" +
                                   "</body>" +
                               "</html>");

                robot.SendMessage(room, "syn/ack");
            });

            robot.AddRoute(RouteMethod.Post, "/jibbr/ping", (request, response) =>
            {
                var body = GetPayload(request);

                var message = string.Format("New push by {0} with {1} commit[s]. {2}",
                                            body["head_commit"]["author"]["username"],
                                            body["commits"].Count(),
                                            body["compare"]);

                response.StatusCode = 200;

                robot.SendMessage(room, message);
            });
        }

        // thank you kudu
        private JObject GetPayload(IRequest request)
        {
            JObject payload;

            if (request.HasFormData)
            {
                var form = request.ReadForm();
                string json = form["payload"];
                if (String.IsNullOrEmpty(json))
                {
                    json = form.First().Value;
                }

                payload = JsonConvert.DeserializeObject<JObject>(json);
            }
            else
            {
                using (JsonTextReader reader = new JsonTextReader(new StreamReader(request.Body)))
                {
                    payload = JObject.Load(reader);
                }
            }

            if (payload == null)
            {
                throw new FormatException("The json payload is empty.");
            }

            return payload;
        }
    }
}
