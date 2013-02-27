using System;
using System.IO.Abstractions;

using JibbR.Adapters.Bing;
using JibbR.Queuing;
using JibbR.Routing;
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

                // Note: should this be a singleton?
                kernel.For<IAdapterManager>()
                      .Use<AdapterManager>();

                kernel.For<IRobot>()
                      .Use<Robot>();

                kernel.For<IBingClient>()
                      .Use<BingClient>();

                // things shared between the bot & web host need to be singletons
                kernel.For<ISettingsManager>()
                      .Singleton()
                      .Use<SettingsManager>();

                kernel.For<IRouteManager>()
                      .Singleton()
                      .Use<RouteManager>();

                kernel.For<IEventBus>()
                      .Singleton()
                      .Use<EventBus>();
            });

            return ObjectFactory.Container;
        }
    }
}