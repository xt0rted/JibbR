namespace JibbR
{
    using System;
    using System.Collections.Generic;
    using StructureMap;

    public class AdapterManager : IAdapterManager
    {
        private readonly IContainer _container;
        private readonly IRobotSettings _settings;

        private bool _adaptersLoaded;

        public AdapterManager(IContainer container, ISettingsManager settingsManager)
        {
            _container = container;
            _settings = settingsManager.LoadSettings();

            Adapters = new List<IRobotAdapter>();
        }

        public IList<IRobotAdapter> Adapters { get; private set; }

        public void SetupAdapters(IRobot robot)
        {
            if (_adaptersLoaded)
            {
                return;
            }

            _adaptersLoaded = true;

            foreach (var adapter in _settings.Adapters)
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
        }
    }
}