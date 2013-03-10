﻿using System.Collections.Generic;
using System.Threading.Tasks;

using Gate;

using JibbR.Annotations;
using JibbR.Routing;

using Owin;

using StructureMap;

namespace JibbR
{
    public partial class Startup
    {
        private IContainer _container;

        public Startup()
        {
        }

        public Startup(IContainer container)
        {
            _container = container;
        }

        [UsedImplicitly]
        public void Configuration(IAppBuilder appBuilder)
        {
            var container = GetApplicationContainer();
            var routeManager = container.GetInstance<IRouteManager>();

            appBuilder.UseShowExceptions()
                      .UseContentType()
                      .Map("/", IndexRoute)
                      .Map("/robots.txt", RobotsRoute)
                      .Map("/adapters/", env => AdapterRoutes(routeManager, env));

            SetupErrorHandling();
        }

        public static Task IndexRoute(IDictionary<string, object> env)
        {
            IResponse response = new ResponseWrapper(env);

            response.StatusCode = 200;
            response.Write("<html>" +
                               "<head>" +
                                   "<title>JibbR</title>" +
                               "</head>" +
                               "<body>hi.</body>" +
                           "</html>");

            return TaskHelpers.Completed();
        }

        public static Task RobotsRoute(IDictionary<string, object> env)
        {
            IResponse response = new ResponseWrapper(env);

            response.ContentType = "text/plain";
            response.Write("User-agent: *\nDisallow: /");

            return TaskHelpers.Completed();
        }

        public static Task AdapterRoutes(IRouteManager routeManager, IDictionary<string, object> env)
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
        }

        private IContainer GetApplicationContainer()
        {
            if (_container == null)
            {
                var bootstrapper = new Bootstrapper();
                _container = bootstrapper.Bootstrap();
            }

            return _container;
        }
    }
}