using System.Collections.Generic;

namespace JibbR.Settings
{
    public interface IRobotSettings
    {
        IList<string> Rooms { get; set; }
        IList<AdapterDetails> Adapters { get; set; }

        AdapterDetails SettingsFor<T>() where T : IRobotAdapter;
        AdapterDetails SettingsFor(string adapterName);
    }
}