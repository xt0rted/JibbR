using System.Threading.Tasks;

using Gate;

using JibbR.Annotations;
using JibbR.Routing;
using JibbR.Web.Routing;

using Owin;

using StructureMap;

namespace JibbR.Web
{
    public partial class Startup
    {
        [UsedImplicitly]
        public void Configuration(IAppBuilder appBuilder)
        {
            var bootstrapper = new Bootstrapper();

            var container = bootstrapper.Bootstrap();

            var routeManager = container.GetInstance<IRouteManager>();

            var app = appBuilder.UseShowExceptions()
                                .UseContentType()
                                .Map("/adapters/", env =>
                                {
                                    // we wrap these simply to remove the owin & gate dependencies elsewhere
                                    // but maybe we won't need it...for now I kind of like it
                                    IRequest request = new RequestWrapper(env);
                                    IResponse response = new ResponseWrapper(env);

                                    var result = routeManager.HandleRoute(request, response);
                                    if (!result)
                                    {
                                        response.Status = "404 Route Not Found";
                                    }

                                    return TaskHelpers.Completed();
                                });
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