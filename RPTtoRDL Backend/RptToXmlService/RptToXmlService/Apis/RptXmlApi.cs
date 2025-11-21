//using System;
//using System.IO;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Headers;
//using System.Web;
//using System.Web.Http;
//using RptToXmlService.Converters;

//namespace RptToXmlService.Apis
//{
//    [RoutePrefix("api/xml")]
//    public class RptXmlApiController : ApiController
//    {
//        private readonly RptConverter _converter = new RptConverter();

//        // ======================================================================
//        // 🔵 POST: /api/xml/convert
//        // ======================================================================
//        [HttpPost]
//        [Route("convert")]
//        public HttpResponseMessage ConvertRptToXml()
//        {
//            try
//            {
//                // --------------------------------------------------------------
//                // 1️⃣ Validate upload
//                // --------------------------------------------------------------
//                var file = HttpContext.Current.Request.Files["file"];
//                if (file == null || file.ContentLength == 0)
//                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
//                        "No RPT file uploaded.");

//                string reportName = Path.GetFileNameWithoutExtension(file.FileName);

//                // --------------------------------------------------------------
//                // 2️⃣ Save uploaded .rpt to a temporary location
//                // --------------------------------------------------------------
//                string tempFolder = HttpContext.Current.Server.MapPath("~/App_Data/Temp/");
//                Directory.CreateDirectory(tempFolder);

//                string rptPath = Path.Combine(tempFolder, reportName + "_" + Guid.NewGuid() + ".rpt");
//                file.SaveAs(rptPath);

//                // --------------------------------------------------------------
//                // 3️⃣ Output folder (App_Data/Exports)
//                // --------------------------------------------------------------
//                string exportFolder = HttpContext.Current.Server.MapPath("~/App_Data/Exports/");
//                Directory.CreateDirectory(exportFolder);

//                // --------------------------------------------------------------
//                // 4️⃣ Auto-increment filename
//                // --------------------------------------------------------------
//                int sequence = 1;
//                string finalFilePath = Path.Combine(exportFolder, $"{reportName}_{sequence:D3}.xml");

//                while (File.Exists(finalFilePath))
//                {
//                    sequence++;
//                    finalFilePath = Path.Combine(exportFolder, $"{reportName}_{sequence:D3}.xml");
//                }

//                // --------------------------------------------------------------
//                // 5️⃣ Convert RPT → XML
//                // --------------------------------------------------------------
//                string xmlOutput = _converter.ConvertRptToXml(rptPath, reportName, sequence);

//                // --------------------------------------------------------------
//                // 6️⃣ Save XML to export folder
//                // --------------------------------------------------------------
//                File.WriteAllText(finalFilePath, xmlOutput);

//                // --------------------------------------------------------------
//                // 7️⃣ Return file as download
//                // --------------------------------------------------------------
//                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
//                response.Content = new StringContent(xmlOutput);
//                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
//                {
//                    FileName = $"{reportName}_{sequence:D3}.xml"
//                };
//                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

//                return response;
//            }
//            catch (Exception ex)
//            {
//                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
//            }
//        }
//    }
//}
