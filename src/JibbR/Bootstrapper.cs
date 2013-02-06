using System.IO.Abstractions;

using JibbR.Adapters;

using StructureMap;

namespace JibbR
{
    public class Bootstrapper
    {
        public void Bootstrap()
        {
            ObjectFactory.Initialize(kernel =>
            {
                kernel.Scan(scanner =>
                {
                    // basics
                    scanner.TheCallingAssembly();
                    //scanner.AssembliesFromApplicationBaseDirectory(a => a.GetName().Name.StartsWith("JibbR.", StringComparison.OrdinalIgnoreCase));
                    scanner.LookForRegistries();

                    // just for us
                    scanner.Convention<AdapterScanner>();
                });

                kernel.For<IFileSystem>()
                      .Use<FileSystem>();

                kernel.For<ISettingsManager>()
                      .Singleton()
                      .Use<SettingsManager>();

                kernel.For<IRobot>()
                      .Use<Robot>();

                kernel.For<IBingClient>()
                      .Use<BingClient>();
            });
        }

        public IContainer Container
        {
            get { return ObjectFactory.Container; }
        }
    }
}