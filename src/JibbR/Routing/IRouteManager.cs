using System;

namespace JibbR.Routing
{
    using RouteHandler = Action<IRequest, IResponse>;

    public interface IRouteManager
    {
        void RegisterRoute(RouteMethod method, string path, RouteHandler handler);

        bool HandleRoute(IRequest request, IResponse response);
    }
}