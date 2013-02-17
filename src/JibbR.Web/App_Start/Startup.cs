using JibbR.Annotations;

using Owin;

using StructureMap;

namespace JibbR.Web
{
    public partial class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder app)
        {
            var bootstrapper = new Bootstrapper();

            var container = bootstrapper.Bootstrap();

            SetupNancy(app, container);

            SetupErrorHandling();
        }

        private static void SetupNancy(IAppBuilder app, IContainer container)
        {
            var bootstrapper = new JibbrBootstrapper(container);
            app.UseNancy(bootstrapper);
        }
    }
}