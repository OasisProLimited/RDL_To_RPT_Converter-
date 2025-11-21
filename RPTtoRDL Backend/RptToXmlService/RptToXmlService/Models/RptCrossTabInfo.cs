using System.Collections.Generic;

namespace RptToXmlService.Models
{
    /// <summary>
    /// ENGINE-SAFE CrossTab model.
    /// Matches XML builder requirements.
    /// </summary>
    public class RptCrossTabInfo
    {
        public string Name { get; set; }
        public string SectionName { get; set; }

        public int Top { get; set; }
        public int Left { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public List<RptCrossTabGroup> Rows { get; set; } = new List<RptCrossTabGroup>();
        public List<RptCrossTabGroup> Columns { get; set; } = new List<RptCrossTabGroup>();
        public List<RptCrossTabSummary> Summaries { get; set; } = new List<RptCrossTabSummary>();
        public List<RptCrossTabCell> Cells { get; set; } = new List<RptCrossTabCell>();
    }

    public class RptCrossTabGroup
    {
        public string Field { get; set; }
        public string SortDirection { get; set; }
        public string GroupType { get; set; }
        public string CustomLabel { get; set; }
    }

    public class RptCrossTabSummary
    {
        public string Field { get; set; }
        public string SummaryType { get; set; }
        public string CustomLabel { get; set; }
        public string FormatString { get; set; }
        public string HighlightFormula { get; set; }
    }

    public class RptCrossTabCell
    {
        public int RowIndex { get; set; }
        public int ColumnIndex { get; set; }

        public string ValueFormula { get; set; }

        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public string BorderStyle { get; set; }

        public string FontName { get; set; }
        public int FontSize { get; set; }

        public bool Bold { get; set; }
        public bool Italic { get; set; }
    }
}
