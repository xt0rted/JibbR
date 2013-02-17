using Nancy.Bootstrappers.StructureMap;
using Nancy.Conventions;

using StructureMap;

namespace JibbR.Web
{
    public class JibbrBootstrapper : StructureMapNancyBootstrapper
    {
        private readonly IContainer _container;

        public JibbrBootstrapper(IContainer container)
        {
            _container = container;
        }

        protected override IContainer GetApplicationContainer()
        {
            return _container;
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddFile("/robots.txt", "/robots.txt"));
        }
    }
}