using System.Collections.Concurrent;
using System.Collections.Generic;

namespace JibbR.Settings
{
    public interface IRobotSettings
    {
        IList<string> Rooms { get; set; }
        IList<AdapterDetails> Adapters { get; set; }
        ConcurrentDictionary<string, List<string>> KnownUsers { get; }

        AdapterDetails SettingsFor<T>() where T : IRobotAdapter;
        AdapterDetails SettingsFor(string adapterName);
    }
}