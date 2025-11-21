using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using RptToXmlService.Converters;

namespace RptToXmlService.Apis
{
    [RoutePrefix("api/json")]
    public class RptJsonApiController : ApiController
    {
        private readonly RptConverter _converter = new RptConverter();

        // ======================================================================
        // 🔵 POST: /api/json/convert
        // ======================================================================
        [HttpPost]
        [Route("convert")]
        public HttpResponseMessage ConvertRptToJson()
        {
            try
            {
                // --------------------------------------------------------------
                // 1️⃣ Validate upload
                // --------------------------------------------------------------
                var file = HttpContext.Current.Request.Files["file"];
                if (file == null || file.ContentLength == 0)
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest,
                        "No RPT file uploaded.");

                string reportName = Path.GetFileNameWithoutExtension(file.FileName);

                // --------------------------------------------------------------
                // 2️⃣ Save uploaded .rpt temporarily
                // --------------------------------------------------------------
                string tempFolder = HttpContext.Current.Server.MapPath("~/App_Data/Temp/");
                Directory.CreateDirectory(tempFolder);

                string rptPath = Path.Combine(tempFolder, reportName + "_" + Guid.NewGuid() + ".rpt");
                file.SaveAs(rptPath);

                // --------------------------------------------------------------
                // 3️⃣ Ensure export folder exists
                // --------------------------------------------------------------
                string exportFolder = HttpContext.Current.Server.MapPath("~/App_Data/Exports/");
                Directory.CreateDirectory(exportFolder);

                // --------------------------------------------------------------
                // 4️⃣ Auto-increment JSON file number
                // --------------------------------------------------------------
                int sequence = 1;
                string finalFilePath = Path.Combine(exportFolder, $"{reportName}_{sequence:D3}.json");

                while (File.Exists(finalFilePath))
                {
                    sequence++;
                    finalFilePath = Path.Combine(exportFolder, $"{reportName}_{sequence:D3}.json");
                }

                // --------------------------------------------------------------
                // 5️⃣ Convert RPT → JSON (JObject)
                // --------------------------------------------------------------
                var jsonObj = _converter.ConvertRptToJson(rptPath, reportName, sequence);

                // Convert JObject → formatted JSON text
                string jsonOutput = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);

                // --------------------------------------------------------------
                // 6️⃣ Save JSON file
                // --------------------------------------------------------------
                File.WriteAllText(finalFilePath, jsonOutput);

                // --------------------------------------------------------------
                // 7️⃣ Return JSON to user as download
                // --------------------------------------------------------------
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(jsonOutput);
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = $"{reportName}_{sequence:D3}.json"
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                return response;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
