using System;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace JibbR.Adapters
{
    [AdapterName("bing-image-adapter")]
    [AdapterDescription("bing images", "Returns a random image from bing's image search"),
     AdapterUsage("bing images", "image [me] something amazing"),
     AdapterUsage("bing images", "bing image [me] something amazing")]
    public class BingImageAdapter : IRobotAdapter
    {
        private readonly IBingClient _bingClient;

        public BingImageAdapter(IBingClient bingClient)
        {
            _bingClient = bingClient;
        }

        public void Setup(IRobot robot)
        {
            var settings = robot.Settings.SettingsFor<BingImageAdapter>();

            // for now there's no point in registering this listener if there's no api key
            if (string.IsNullOrWhiteSpace((string) settings.Settings.ApiKey))
            {
                Console.Error.WriteLine("No API key was found for bing image search. If you do not have one you can get one by signing up here http://www.bing.com/developers/");
                return;
            }

            robot.AddListener(@"^(bing )?image( me)? (?<query>.*)", (session, message, room, match) =>
            {
                var query = match.ValueFor("query");

                var result = _bingClient.ImageSearch((string) settings.Settings.ApiKey, query);

                if (result.StartsWith("{"))
                {
                    var results = JObject.Parse(result);
                    var imageUrl = results["d"]["results"].Select(x => (string) x["MediaUrl"]).RandomElement(new Random());

                    robot.SendMessage(room, imageUrl);
                }
            });
        }
    }
}