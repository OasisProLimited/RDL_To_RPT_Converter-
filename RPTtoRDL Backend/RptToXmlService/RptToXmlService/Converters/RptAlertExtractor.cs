using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using RptToXmlService.Models;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY Alert Extractor.
    /// CrystalDecisions.CrystalReports.Engine does NOT support Alerts.
    /// </summary>
    /// 

    public class RptAlertExtractor
    {
        public List<RptAlertInfo> ExtractAlerts(ReportDocument rpt)
        {
            return new List<RptAlertInfo>
            {
                new RptAlertInfo
                {
                    AlertName = "NotSupportedInEngineAPI",
                    Enabled = false,
                    Severity = "N/A",
                    Message = "Crystal Reports Engine API does not expose alert information.",
                    ConditionFormula = "",
                    InvolvedFields = new List<string>()
                }
            };
        }
    }
}
