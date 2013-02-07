using System;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace JibbR.Adapters
{
    [RobotAdapterMetadata("bing-image-adapter")]
    public class BingImageAdapter : IRobotAdapter
    {
        private readonly IBingClient _bingClient;
        private readonly BingAdapterSettings _settings;

        public BingImageAdapter(ISettingsManager settingsManager, IBingClient bingClient)
        {
            _bingClient = bingClient;

            _settings = settingsManager.LoadSettings()
                                       .For<BingImageAdapter>()
                                       .ToObject<BingAdapterSettings>();

            if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            {
                Console.Error.WriteLine("No API key was found for bing. If you do not have one you can get one by signing up here http://www.bing.com/developers/");
            }
        }

        public void Setup(IRobot robot)
        {
            robot.AddListener(@"^(bing )?image( me)? (?<query>.*)", (session, message, room, match) =>
            {
                var query = match.Groups["query"].Value;

                var result = _bingClient.ImageSearch(_settings.ApiKey, query);

                if (result.StartsWith("{"))
                {
                    var results = JObject.Parse(result);
                    var imageUrl = results["d"]["results"].Select(x => (string)x["MediaUrl"]).RandomElement(new Random());

                    session.Client.Send(imageUrl, room);
                }
            });
        }
    }
}