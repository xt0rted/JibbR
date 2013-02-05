using System.Collections.Generic;

namespace JibbR
{
    public interface IRobotSettings
    {
        List<string> Adapters { get; set; }
    }
}