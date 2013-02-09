using Newtonsoft.Json.Linq;

namespace JibbR.Settings
{
    public class AdapterDetails
    {
        public AdapterDetails()
        {
            Settings = new JObject();
        }

        public string Name { get; set; }
        public bool Enabled { get; set; }
        public dynamic Settings { get; set; }
    }
}