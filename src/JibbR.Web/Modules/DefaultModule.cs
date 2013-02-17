﻿using System;
using System.Globalization;

using JibbR.Queuing;

using Nancy;

namespace JibbR.Web.Modules
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule(IEventBus eventBus)
        {
            Get["/"] = _ => "hi.";

            Get["/ping"] = _ =>
            {
                string time = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);

                eventBus.Push(new JabbrMessage
                {
                    Room = "development",
                    Message = string.Format("It is now {0}", time)
                });

                return time;
            };
        }
    }
}