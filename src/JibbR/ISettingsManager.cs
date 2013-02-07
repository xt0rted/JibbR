namespace JibbR
{
    public interface ISettingsManager
    {
        string SettingsPath { get; }

        void CreateDefaultSettingsFile();
        IRobotSettings LoadSettings();
    }
}