using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace JibbR
{
    public class RobotSettings : IRobotSettings
    {
        public RobotSettings()
        {
            Rooms = new List<string>();
            EnabledAdapters = new List<string>();
            Adapters = new List<AdapterDetails>();
        }

        public List<string> Rooms { get; set; }
        public List<string> EnabledAdapters { get; set; }
        public List<AdapterDetails> Adapters { get; set; }

        public dynamic For<TAdapter>() where TAdapter : IRobotAdapter
        {
            var metadata = typeof (TAdapter).GetCustomAttributes(typeof (RobotAdapterMetadataAttribute), false);
            var data = (RobotAdapterMetadataAttribute) metadata[0];

            var name = data.GetAdapterName();
            return For(name);
        }

        public dynamic For(string adapterName)
        {
            var settings = Adapters.SingleOrDefault(a => string.Equals(a.Name, adapterName, StringComparison.OrdinalIgnoreCase));
            if (settings == null)
            {
                settings = new AdapterDetails
                {
                    Name = adapterName,
                    Settings = new JObject()
                };

                Adapters.Add(settings);
            }

            return settings.Settings;
        }
    }
}