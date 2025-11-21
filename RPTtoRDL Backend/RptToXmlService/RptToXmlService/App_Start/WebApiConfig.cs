//using System.Web.Http;
//using System.Web.Http.Cors;

//namespace RptToXmlService
//{
//    public static class WebApiConfig
//    {
//        public static void Register(HttpConfiguration config)
//        {
//            // Enable CORS globally
//            var cors = new EnableCorsAttribute("http://localhost:5173", "*", "*");
//            config.EnableCors(cors);

//            // Enable attribute-based routing
//            config.MapHttpAttributeRoutes();

//            // Fallback conventional route (no {action})
//            config.Routes.MapHttpRoute(
//                name: "DefaultApi",
//                routeTemplate: "api/{controller}/{id}",
//                defaults: new { id = RouteParameter.Optional }
//            );

//            // Force JSON response formatting
//            config.Formatters.Remove(config.Formatters.XmlFormatter);
//            config.Formatters.JsonFormatter.SerializerSettings.Formatting =
//                Newtonsoft.Json.Formatting.Indented;

//            // Always show detailed errors (for development)
//            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
//        }
//    }
//}


using System.Web.Http;
using System.Web.Http.Cors;

namespace RptToXmlService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Enable CORS globally (ONLY HERE)
            var cors = new EnableCorsAttribute("http://localhost:5173", "*", "*");
            config.EnableCors(cors);

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // JSON only
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}

