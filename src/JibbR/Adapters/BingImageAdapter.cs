using System;

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

                var searchResult = _bingClient.ImageSearch((string) settings.Settings.ApiKey, query);

                if (searchResult.StartsWith("{", StringComparison.OrdinalIgnoreCase))
                {
                    var results = JObject.Parse(searchResult);
                    var urls = results["d"]["results"].SelectList(x => (string) x["MediaUrl"]);

                    string messageResult;
                    if (urls.Count > 0)
                    {
                        messageResult = urls.RandomElement(new Random());
                    }
                    else
                    {
                        messageResult = string.Format("@{0} I couldn't find any images for '{1}'", session.Message.User.Name, query);
                    }

                    robot.SendMessage(room, messageResult);
                }
            });
        }
    }
}