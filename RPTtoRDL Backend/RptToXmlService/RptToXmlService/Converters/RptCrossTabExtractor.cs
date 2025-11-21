using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using RptToXmlService.Models;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY CROSS TAB EXTRACTOR
    /// Crystal Reports ENGINE exposes only geometry (Top, Left, Width, Height)
    /// and object name + section.  
    /// 
    /// Crosstab structure (rows/cols/summaries/cells) is NOT available
    /// without RAS API.
    /// 
    /// Therefore we produce a safe, minimal RptCrossTabInfo with
    /// empty collections, keeping XML builder structure consistent.
    /// </summary>
    public class RptCrossTabExtractor
    {
        private readonly ReportDocument _rpt;

        public RptCrossTabExtractor(ReportDocument rpt)
        {
            _rpt = rpt;
        }

        // =====================================================================
        // MAIN ENTRY — Extract Crosstabs (Engine only)
        // =====================================================================
        public List<RptCrossTabInfo> ExtractCrossTabs()
        {
            var list = new List<RptCrossTabInfo>();

            foreach (Section sec in _rpt.ReportDefinition.Sections)
            {
                foreach (ReportObject obj in sec.ReportObjects)
                {
                    if (obj.Kind == ReportObjectKind.CrossTabObject)
                    {
                        CrossTabObject ct = obj as CrossTabObject;
                        if (ct != null)
                            list.Add(ConvertEngineCrossTab(ct, sec));
                    }
                }
            }

            return list;
        }

        // =====================================================================
        // ENGINE-SAFE CROSSTAB CONVERSION
        // =====================================================================
        private RptCrossTabInfo ConvertEngineCrossTab(CrossTabObject ct, Section sec)
        {
            return new RptCrossTabInfo
            {
                Name = ct.Name,
                SectionName = sec.Name,

                Top = ct.Top,
                Left = ct.Left,
                Width = ct.Width,
                Height = ct.Height,

                // ENGINE exposes NO row/column/summary/cell metadata.
                Rows = new List<RptCrossTabGroup>(),
                Columns = new List<RptCrossTabGroup>(),
                Summaries = new List<RptCrossTabSummary>(),
                Cells = new List<RptCrossTabCell>()
            };
        }
    }
}
