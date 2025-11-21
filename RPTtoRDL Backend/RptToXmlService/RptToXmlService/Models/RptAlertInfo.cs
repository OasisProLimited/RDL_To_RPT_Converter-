using System.Collections.Generic;

namespace RptToXmlService.Models
{
    /// <summary>
    /// ENGINE-SAFE Alert Model.
    /// Crystal Reports Engine API does NOT expose alert metadata.
    /// </summary>
    public class RptAlertInfo
    {
        public string AlertName { get; set; }
        public bool Enabled { get; set; }
        public string Severity { get; set; }
        public string Message { get; set; }
        public string ConditionFormula { get; set; }
        public List<string> InvolvedFields { get; set; } = new List<string>();
    }
}
