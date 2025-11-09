using System;
using System.Collections.Generic;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace RptToXmlService
{
    // Makes Swagger show a "Choose File" for our POST /api/convert
    public class FileUploadOperation : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // match the route used in ReportController
            if (!apiDescription.RelativePath.Equals("api/convert", StringComparison.OrdinalIgnoreCase))
                return;

            operation.consumes = new List<string> { "multipart/form-data" };
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "file",
                @in = "formData",
                description = ".rpt file",
                required = true,
                type = "file"
            });
        }
    }
}
