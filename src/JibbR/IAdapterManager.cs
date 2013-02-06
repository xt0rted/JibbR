namespace JibbR
{
    using System.Collections.Generic;

    public interface IAdapterManager
    {
        IList<IRobotAdapter> Adapters { get; }

        void SetupAdapters(IRobot robot);
    }
}