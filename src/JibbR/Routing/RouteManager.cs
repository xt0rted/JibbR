using System;
using System.Collections.Generic;

namespace JibbR.Routing
{
    using RouteHandler = Action<IRequest, IResponse>;

    public class RouteManager : IRouteManager
    {
        private readonly IDictionary<string, RouteHandler> _routesGet = new Dictionary<string, RouteHandler>();
        private readonly IDictionary<string, RouteHandler> _routesPost = new Dictionary<string, RouteHandler>();
        private readonly IDictionary<string, RouteHandler> _routesPut = new Dictionary<string, RouteHandler>();
        private readonly IDictionary<string, RouteHandler> _routesDelete = new Dictionary<string, RouteHandler>();

        public void RegisterRoute(RouteMethod method, string path, RouteHandler handler)
        {
            var routes = GetRouteTable(method);

            routes.Add(path, handler);
        }

        public bool HandleRoute(IRequest request, IResponse response)
        {
            RouteMethod method;
            if (!Enum.TryParse(request.Method, true, out method))
            {
                throw new ArgumentException("Method '" + request.Method + "' is not supported.");
            }

            var routes = GetRouteTable(method);

            foreach (var route in routes)
            {
                if (request.Path == route.Key)
                {
                    route.Value(request, response);
                    return true;
                }
            }

            return false;
        }

        private IDictionary<string, RouteHandler> GetRouteTable(RouteMethod method)
        {
            IDictionary<string, RouteHandler> routes;

            switch (method)
            {
                case RouteMethod.Get:
                    routes = _routesGet;
                    break;

                case RouteMethod.Post:
                    routes = _routesPost;
                    break;

                case RouteMethod.Put:
                    routes = _routesPut;
                    break;

                case RouteMethod.Delete:
                    routes = _routesDelete;
                    break;

                default:
                    throw new ArgumentOutOfRangeException("method");
            }

            return routes;
        }
    }
}