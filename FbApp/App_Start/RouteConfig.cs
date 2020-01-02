using System.Web.Mvc;
using System.Web.Routing;

namespace FbApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Friends",
               url: "{controller}/{action}/{senderId}/{receiverId}",
               defaults: new { controller = "FriendRequests", action = "AddFriend"}
           );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
