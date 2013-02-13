using System;
using System.Globalization;

using JibbR.Web.App_Start;

using Nancy;

namespace JibbR.Web.Modules
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/"] = _ => "hi.";

            Get["/ping"] = _ =>
            {
                JibbrSetup.Robot.HeartBeat();
                return DateTime.UtcNow.ToString(CultureInfo.InvariantCulture);
            };
        }
    }
}