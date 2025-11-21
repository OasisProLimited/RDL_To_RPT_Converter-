using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using RptToXmlService.Converters;

namespace RptToXmlService.Apis
{
    [RoutePrefix("api/xml")]
    public class RptXmlApiController : ApiController
    {
        private readonly RptConverter _converter = new RptConverter();

        [HttpPost]
        [Route("convert")]
        public HttpResponseMessage ConvertRptToXml()
        {
            try
            {
                var file = HttpContext.Current.Request.Files["file"];
                if (file == null || file.ContentLength == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "No file uploaded.");

                string reportName = Path.GetFileNameWithoutExtension(file.FileName);

                string tempFolder = HttpContext.Current.Server.MapPath("~/App_Data/Temp/");
                Directory.CreateDirectory(tempFolder);

                string rptPath = Path.Combine(tempFolder, reportName + "_" + Guid.NewGuid() + ".rpt");
                file.SaveAs(rptPath);

                string exportFolder = HttpContext.Current.Server.MapPath("~/App_Data/Exports/");
                Directory.CreateDirectory(exportFolder);

                int sequence = 1;
                string finalFilePath = Path.Combine(exportFolder, $"{reportName}_{sequence:D3}.xml");
                while (File.Exists(finalFilePath))
                {
                    sequence++;
                    finalFilePath = Path.Combine(exportFolder, $"{reportName}_{sequence:D3}.xml");
                }

                string xmlOutput = _converter.ConvertRptToXml(rptPath, reportName, sequence);

                File.WriteAllText(finalFilePath, xmlOutput);

                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(System.Text.Encoding.UTF8.GetBytes(xmlOutput));

                response.Content.Headers.ContentDisposition =
                    new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = $"{reportName}_{sequence:D3}.xml"
                    };

                response.Content.Headers.ContentType =
                    new MediaTypeHeaderValue("application/xml");

                response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
