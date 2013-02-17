using System;
using System.IO.Abstractions;

using JibbR.Adapters;
using JibbR.Queuing;
using JibbR.Settings;

using StructureMap;

namespace JibbR
{
    public class Bootstrapper
    {
        public IContainer Bootstrap()
        {
            ObjectFactory.Initialize(kernel =>
            {
                kernel.Scan(scanner =>
                {
                    // basics
                    scanner.TheCallingAssembly();
                    scanner.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("JibbR", StringComparison.OrdinalIgnoreCase));
                    scanner.LookForRegistries();

                    // just for us
                    scanner.Convention<AdapterScanner>();
                });

                kernel.For<IFileSystem>()
                      .Use<FileSystem>();

                kernel.For<ISettingsManager>()
                      .Singleton()
                      .Use<SettingsManager>();

                kernel.For<IAdapterManager>()
                      .Use<AdapterManager>();

                kernel.For<IEventBus>()
                      .Singleton()
                      .Use<EventBus>();

                kernel.For<IRobot>()
                      .Use<Robot>();

                kernel.For<IBingClient>()
                      .Use<BingClient>();
            });

            return ObjectFactory.Container;
        }
    }
}