using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JibbR.Settings
{
    public class RobotSettings : IRobotSettings
    {
        public RobotSettings()
        {
            Rooms = new List<string>();
            Adapters = new List<AdapterDetails>();
            KnownUsers = new ConcurrentDictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        }

        public IList<string> Rooms { get; set; }
        public IList<AdapterDetails> Adapters { get; set; }

        [JsonIgnore]
        public ConcurrentDictionary<string, List<string>> KnownUsers { get; private set; }

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