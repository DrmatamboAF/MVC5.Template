using System.Web.Routing;

namespace MvcTemplate.Controllers
{
    public interface IRouteConfig
    {
        void RegisterRoutes(RouteCollection routes);
    }
}
