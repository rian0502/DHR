using Microsoft.AspNetCore.Mvc.Rendering;
using System;
namespace Presensi360.Helper
{
    public class UrlActive
    {
        public static string IsActive(HttpContext httpContext, string controller)
        {
            var routeData = httpContext.GetRouteData();
            var currentController = routeData.Values["controller"]?.ToString();

            return (currentController != null && currentController.Equals(controller, StringComparison.OrdinalIgnoreCase) ? "active" : "");
        }
        public static string IsActive(HttpContext httpContext, params string[] controllers)
        {
            var routeData = httpContext.GetRouteData();
            var currentController = routeData.Values["controller"]?.ToString();
            return controllers.Contains(currentController, StringComparer.OrdinalIgnoreCase) ? "active" : "";
        }
    }
}
