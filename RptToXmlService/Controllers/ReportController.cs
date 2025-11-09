using System;
using System.IO;
using System.Web;
using System.Web.Http;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace RptToXmlService.Controllers
{
    public class ReportController : ApiController
    {
        [HttpPost]
        [Route("api/convert")]
        public IHttpActionResult ConvertRptToXml()
        {
            try
            {
                var file = HttpContext.Current.Request.Files["file"];
                if (file == null)
                    return BadRequest("No .rpt file uploaded.");

                string tempRpt = Path.GetTempFileName() + ".rpt";
                file.SaveAs(tempRpt);

                using (ReportDocument rpt = new ReportDocument())
                {
                    rpt.Load(tempRpt);
                    string xmlPath = Path.ChangeExtension(tempRpt, ".xml");
                    rpt.ExportToDisk(ExportFormatType.Xml, xmlPath);

                    byte[] data = File.ReadAllBytes(xmlPath);
                    var response = new System.Net.Http.HttpResponseMessage
                    {
                        Content = new System.Net.Http.ByteArrayContent(data)
                    };
                    response.Content.Headers.ContentType =
                        new System.Net.Http.Headers.MediaTypeHeaderValue("application/xml");
                    response.Content.Headers.ContentDisposition =
                        new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                        {
                            FileName = Path.GetFileName(xmlPath)
                        };
                    return ResponseMessage(response);
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
