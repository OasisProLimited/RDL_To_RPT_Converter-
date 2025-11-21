using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using RptToXmlService.Models;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY Chart Extractor.
    /// 
    /// Crystal Reports ENGINE exposes only:
    ///   • Name
    ///   • Top
    ///   • Left
    ///   • Width
    ///   • Height
    ///   • Section
    /// 
    /// No ChartType, No Series, No Axes, No Title/Legend.
    /// Those are RAS-only features.
    /// 
    /// Therefore we populate ENGINE-SAFE RptChartInfo with
    /// default values so XML builder always has a stable schema.
    /// </summary>
    /// 
    public class RptChartExtractor
    {
        private readonly ReportDocument _rpt;

        public RptChartExtractor(ReportDocument rpt)
        {
            _rpt = rpt;
        }

        // =====================================================================
        // MAIN ENTRY
        // =====================================================================
        public List<RptChartInfo> ExtractCharts()
        {
            List<RptChartInfo> charts = new List<RptChartInfo>();

            foreach (Section section in _rpt.ReportDefinition.Sections)
            {
                foreach (ReportObject obj in section.ReportObjects)
                {
                    if (obj.Kind == ReportObjectKind.ChartObject)
                    {
                        ChartObject ch = obj as ChartObject;
                        if (ch != null)
                            charts.Add(ConvertEngineChart(ch, section));
                    }
                }
            }

            return charts;
        }

        // =====================================================================
        // ENGINE-SAFE CHART CONVERSION
        // =====================================================================
        private RptChartInfo ConvertEngineChart(ChartObject ch, Section sec)
        {
            return new RptChartInfo
            {
                Name = ch.Name,
                SectionName = sec.Name,

                // Crystal engine only gives geometry
                Is3D = false,
                Palette = "NotAvailable",
                ChartType = "NotAvailable",

                Title = null,
                Legend = null,
                XAxis = null,
                YAxis = null,

                // ENGINE cannot expose series at all
                Series = new List<RptChartSeriesInfo>()
            };
        }
    }
}
