using System.Collections.Generic;

namespace RptToXmlService.Models
{
    /// <summary>
    /// ENGINE-SAFE chart model used by RptXmlBuilder and RptJsonBuilder.
    /// Only contains fields accessible in CrystalDecisions.CrystalReports.Engine.
    /// </summary>
    public class RptChartInfo
    {
        public string Name { get; set; }
        public string ChartType { get; set; }
        public string SectionName { get; set; }

        public bool Is3D { get; set; }
        public string Palette { get; set; }

        public RptChartTitleInfo Title { get; set; }
        public RptChartLegendInfo Legend { get; set; }

        public RptAxisInfo XAxis { get; set; }
        public RptAxisInfo YAxis { get; set; }

        public List<RptChartSeriesInfo> Series { get; set; } = new List<RptChartSeriesInfo>();
    }

    // ============================================================
    //  TITLE
    // ============================================================
    public class RptChartTitleInfo
    {
        public string Text { get; set; }
        public string Font { get; set; }
        public int Size { get; set; }
        public bool Bold { get; set; }
        public string Color { get; set; }
    }

    // ============================================================
    //  LEGEND
    // ============================================================
    public class RptChartLegendInfo
    {
        public bool Visible { get; set; }
        public string Position { get; set; }
        public string Font { get; set; }
        public int Size { get; set; }
        public string Color { get; set; }
    }

    // ============================================================
    //  SERIES
    // ============================================================
    public class RptChartSeriesInfo
    {
        public string Name { get; set; }
        public string Field { get; set; }
        public string Color { get; set; }
        public string MarkerShape { get; set; }
    }

    // ============================================================
    //  AXIS — FINAL SINGLE DEFINITION (REMOVE DUPLICATE FILE)
    // ============================================================
    public class RptAxisInfo
    {
        public string Title { get; set; }

        public string LabelFont { get; set; }
        public int LabelSize { get; set; }
        public string LabelColor { get; set; }

        public string MinValue { get; set; }
        public string MaxValue { get; set; }

        public string AxisType { get; set; }   // Numeric / Category / Date
    }
}
