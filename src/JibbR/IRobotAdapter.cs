namespace JibbR
{
    public interface IRobotAdapter
    {
        string Name { get; }

        void Setup(IRobot robot);
    }
}