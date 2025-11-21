namespace RptToXmlService.Models
{
    /// <summary>
    /// ENGINE-ONLY JOIN MODEL
    /// Crystal Engine cannot extract join metadata,
    /// but this model exists so ExtractTableJoins() compiles.
    /// Always returns an empty list.
    /// </summary>
    public class RptJoinInfo
    {
        public string LeftTable { get; set; } = "";
        public string LeftField { get; set; } = "";

        public string RightTable { get; set; } = "";
        public string RightField { get; set; } = "";

        public string JoinType { get; set; } = "Unknown";
    }
}
