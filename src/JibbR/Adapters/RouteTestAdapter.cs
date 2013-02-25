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
            robot.AddRoute(RouteMethod.Get, "jibbr/ping", (request, response) =>
            {
                response.Status = "200 OK";
                response.ContentType = "text/html";
                response.Write("<html><body><h1>jibbr ping</h1></body></html>");

                robot.SendMessage("development", "ping/pong");
            });

            robot.AddRoute(RouteMethod.Post, "jibbr/ping", (request, response) =>
            {
                var body = GetPayload(request);
                var message = (string)body["test"];

                response.Status = "200 OK";
                response.ContentType = "text/html";
                response.Write("<html><body><h1>{0}</h1></body></html>", message);

                robot.SendMessage("development", message);
            });
        }

        // thank you kudu
        private JObject GetPayload(IRequest request)
        {
            JObject payload;

            // we don't care about content type, just let it choked
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
