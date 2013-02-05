using System.Collections.Generic;

namespace JibbR
{
    public class RobotSettings : IRobotSettings
    {
        public RobotSettings()
        {
            Adapters = new List<string>();
        }

        public List<string> Adapters { get; set; }
    }
}