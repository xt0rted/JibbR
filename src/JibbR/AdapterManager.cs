using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using StructureMap;

namespace JibbR
{
    public class AdapterManager : IAdapterManager
    {
        private static readonly Regex BotNameReplacer = new Regex(@"\bjibbr\b", RegexOptions.IgnoreCase);

        private readonly IContainer _container;

        private Dictionary<string, HelpDetails> _helpCahe;
        private bool _adaptersLoaded;

        public AdapterManager(IContainer container)
        {
            _container = container;

            Adapters = new List<IRobotAdapter>();
        }

        public IList<IRobotAdapter> Adapters { get; private set; }

        public void SetupAdapters(IRobot robot)
        {
            if (_adaptersLoaded)
            {
                // should we toss an exception instead?
                return;
            }

            _adaptersLoaded = true;

            foreach (var adapter in robot.Settings.Adapters.Where(a => a.Enabled).Select(a => a.Name))
            {
                Console.WriteLine("Trying to load adapter named '{0}'", adapter);

                var instance = _container.TryGetInstance<IRobotAdapter>(adapter);
                if (instance == null)
                {
                    Console.WriteLine("No adapter found named '{0}'", adapter);
                    continue;
                }

                instance.Setup(robot);
                Adapters.Add(instance);
            }

            InitializeHelpText();
        }

        public string HelpText(string botName)
        {
            var output = new StringBuilder();

            foreach (var adapter in _helpCahe.OrderBy(a => a.Key))
            {
                output.AppendFormat("{0} - {1}", adapter.Key, adapter.Value.Description);
                output.AppendLine();
            }

            return CleanText(botName, output.ToString());
        }

        public string HelpTextFor(string botName, string command)
        {
            if (!_helpCahe.ContainsKey(command))
            {
                return null;
            }

            var output = new StringBuilder();

            foreach (var usage in _helpCahe[command].Usages)
            {
                output.AppendLine(usage);
            }

            return CleanText(botName, output.ToString());
        }

        private void InitializeHelpText()
        {
            if (_helpCahe != null)
            {
                return;
            }

            _helpCahe = new Dictionary<string, HelpDetails>(StringComparer.OrdinalIgnoreCase);

            foreach (var adapter in Adapters)
            {
                var summaries = adapter.GetType().GetCustomAttributes(typeof(AdapterDescriptionAttribute), false);
                var usages = adapter.GetType().GetCustomAttributes(typeof(AdapterUsageAttribute), false);

                foreach (var summary in summaries.Cast<AdapterDescriptionAttribute>())
                {
                    var details = new HelpDetails
                    {
                        Description = summary.Description
                    };

                    foreach (var usage in usages.Cast<AdapterUsageAttribute>().Where(u => string.Equals(u.Name, summary.Name, StringComparison.OrdinalIgnoreCase)))
                    {
                        details.Usages.Add(usage.Example);
                    }

                    _helpCahe.Add(summary.Name, details);
                }
            }
        }

        private static string CleanText(string botName, string input)
        {
            if (string.Equals(botName, Helpers.BotName, StringComparison.OrdinalIgnoreCase))
            {
                input = BotNameReplacer.Replace(input, botName);
            }

            return input;
        }
    }
}