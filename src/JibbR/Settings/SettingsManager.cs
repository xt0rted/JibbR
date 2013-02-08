using System.IO.Abstractions;

using Newtonsoft.Json;

namespace JibbR.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private readonly IFileSystem _fileSystem;
        private string _settingsPath;

        public SettingsManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;

            if (!_fileSystem.File.Exists(SettingsPath))
            {
                CreateDefaultSettingsFile();
            }
        }

        public string SettingsPath
        {
            get
            {
                if (_settingsPath == null)
                {
                    //var path = _fileSystem.Directory.GetCurrentDirectory();
                    //_settingsPath = _fileSystem.Path.Combine(path, "jibbr.json");
                    _settingsPath = "jibbr.json";
                }

                return _settingsPath;
            }
        }

        public void CreateDefaultSettingsFile()
        {
            IRobotSettings settings = new RobotSettings();

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            _fileSystem.File.WriteAllText(SettingsPath, json);
        }

        public IRobotSettings LoadSettings()
        {
            var json = _fileSystem.File.ReadAllText(SettingsPath);
            var settings = JsonConvert.DeserializeObject<RobotSettings>(json);
            return settings;
        }
    }
}