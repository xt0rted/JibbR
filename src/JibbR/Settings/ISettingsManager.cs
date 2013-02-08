namespace JibbR.Settings
{
    public interface ISettingsManager
    {
        string SettingsPath { get; }

        void CreateDefaultSettingsFile();
        IRobotSettings LoadSettings();
    }
}