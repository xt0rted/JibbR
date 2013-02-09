using System;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json.Linq;

namespace JibbR.Settings
{
    public class RobotSettings : IRobotSettings
    {
        public RobotSettings()
        {
            Rooms = new List<string>();
            Adapters = new List<AdapterDetails>();
        }

        public IList<string> Rooms { get; set; }
        public IList<AdapterDetails> Adapters { get; set; }

        public AdapterDetails SettingsFor<TAdapter>() where TAdapter : IRobotAdapter
        {
            var metadata = typeof (TAdapter).GetCustomAttributes(typeof (RobotAdapterMetadataAttribute), false);
            var data = (RobotAdapterMetadataAttribute) metadata[0];

            var name = data.GetAdapterName();
            return SettingsFor(name);
        }

        public AdapterDetails SettingsFor(string adapterName)
        {
            var details = Adapters.SingleOrDefault(a => string.Equals(a.Name, adapterName, StringComparison.OrdinalIgnoreCase));
            if (details == null)
            {
                details = new AdapterDetails
                {
                    Name = adapterName,
                    Settings = new JObject()
                };
            }

            return details;
        }
    }
}