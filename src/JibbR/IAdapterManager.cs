using System.Collections.Generic;

namespace JibbR
{
    public interface IAdapterManager
    {
        IList<IRobotAdapter> Adapters { get; }

        void SetupAdapters(IRobot robot);

        string HelpText(string botName);
        string HelpTextFor(string botName, string command);
    }
}