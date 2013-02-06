﻿using System;
using System.Linq;
using System.Text.RegularExpressions;

using Newtonsoft.Json.Linq;

namespace JibbR.Adapters
{
    [RobotAdapterMetadata("bing-adapter")]
    public class BingAdapter : IRobotAdapter
    {
        private readonly IBingClient _bingClient;
        private readonly string _bingApiKey;

        public BingAdapter(IBingClient bingClient)
        {
            _bingClient = bingClient;

            _bingApiKey = System.Configuration.ConfigurationManager.AppSettings["jibbr:bingadapter:apikey"];

            if (string.IsNullOrWhiteSpace(_bingApiKey))
            {
                Console.Error.WriteLine("No API key was found for bing. If you do not have one you can get one by signing up here http://www.bing.com/developers/");
            }
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
                    if (resultMatch.Success)
                    {
                        session.Client.Send(string.Format("@{0} {1}", session.Message.User.Name, resultMatch.Groups["result"].Value), room);
                    }
                    else
                    {
                        session.Client.Send(string.Format("@{0} Sorry, Bing had zero results for '{1}'", session.Message.User.Name, query), query);
                    }
                });
            });

            robot.AddListener(@"^(bing )?image( me)? (?<query>.*)", (session, message, room, match) =>
            {
                var query = match.Groups["query"].Value;

                var result = _bingClient.ImageSearch(_bingApiKey, query);

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