using Newtonsoft.Json.Linq;

namespace JibbR.Settings
{
    public class AdapterDetails
    {
        public string Name { get; set; }
        public JObject Settings { get; set; }
    }
}