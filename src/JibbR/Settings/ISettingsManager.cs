namespace JibbR.Settings
{
    public interface ISettingsManager
    {
        string SettingsPath { get; }
        IRobotSettings Settings { get; }

        void LoadSettings();
        void SaveSettings();
    }
}