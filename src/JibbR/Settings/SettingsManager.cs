using System.IO.Abstractions;
using System.Text;

using Newtonsoft.Json;

namespace JibbR.Settings
{
    public class SettingsManager : ISettingsManager
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        private readonly IFileSystem _fileSystem;
        private string _settingsPath;

        public SettingsManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
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

        public IRobotSettings Settings { get; private set; }

        public void LoadSettings()
        {
            var json = _fileSystem.File.ReadAllText(SettingsPath);
            Settings = JsonConvert.DeserializeObject<RobotSettings>(json, SerializerSettings);
        }

        public void SaveSettings()
        {
            var json = JsonConvert.SerializeObject(Settings, Formatting.Indented, SerializerSettings);
            _fileSystem.File.WriteAllText(SettingsPath, json, new UTF8Encoding(false));
        }
    }
}