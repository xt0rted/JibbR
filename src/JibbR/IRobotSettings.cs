using System.Collections.Generic;

namespace JibbR
{
    public interface IRobotSettings
    {
        List<string> Rooms { get; set; }
        List<string> EnabledAdapters { get; set; }
        List<AdapterDetails> Adapters { get; set; }

        dynamic For<TAdapter>() where TAdapter : IRobotAdapter;
        dynamic For(string adapterName);
    }
}