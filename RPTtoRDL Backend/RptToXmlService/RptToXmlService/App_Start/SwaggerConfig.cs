using System.Web.Http;
using Swashbuckle.Application;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(RptToXmlService.SwaggerConfig), "Register")]

namespace RptToXmlService
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("v1", "RptToXmlService");
                    // add file upload support for /api/convert
                    c.OperationFilter<FileUploadOperation>();
                })
                .EnableSwaggerUi(); // UI at /swagger or /swagger/ui/index
        }
    }
}
