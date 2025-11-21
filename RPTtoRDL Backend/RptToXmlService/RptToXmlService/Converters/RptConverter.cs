using CrystalDecisions.CrystalReports.Engine;
using Newtonsoft.Json.Linq;
using RptToXmlService.Models;
using System;
using System.Collections.Generic;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// MASTER ORCHESTRATOR
    /// Loads report → Extracts all metadata → Builds unified model → Sends to builders
    /// ENGINE-ONLY (NO RAS)
    /// </summary>
    public class RptConverter
    {
        // ======================================================
        // ENGINE EXTRACTORS
        // ======================================================
        private readonly RptDataExtractor _dataExtractor = new RptDataExtractor();
        private readonly RptFormatExtractor _formatExtractor = new RptFormatExtractor();
        private readonly RptSectionExtractor _sectionExtractor = new RptSectionExtractor();
        private readonly RptObjectExtractor _objectExtractor = new RptObjectExtractor();
        private readonly RptGroupSortExtractor _groupExtractor = new RptGroupSortExtractor();
        private readonly RptParameterExtractor _parameterExtractor = new RptParameterExtractor();

        // Formula extractor must receive rpt at runtime → cannot be constructed here
        private RptFormulaExtractor _formulaExtractor;

        // Chart, Crosstab, Alerts → need rpt in constructor
        private RptChartExtractor _chartExtractor;
        private RptCrossTabExtractor _crossTabExtractor;
        private RptAlertExtractor _alertExtractor;

        // BUILDERS
        private readonly RptJsonBuilder _jsonBuilder = new RptJsonBuilder();
        private readonly RptXmlBuilder _xmlBuilder = new RptXmlBuilder();

        public RptConverter() { }

        // ======================================================
        // LOAD REPORT
        // ======================================================
        private ReportDocument LoadReport(string path)
        {
            var rpt = new ReportDocument();
            rpt.Load(path);
            return rpt;
        }

        // ======================================================
        // BUILD MODEL (ENGINE-ONLY)
        // ======================================================
        private RptReportModel BuildModel(ReportDocument rpt, string reportName, int sequence)
        {
            // instantiate extractors requiring rpt
            _formulaExtractor = new RptFormulaExtractor(rpt);
            _chartExtractor = new RptChartExtractor(rpt);
            _crossTabExtractor = new RptCrossTabExtractor(rpt);
            _alertExtractor = new RptAlertExtractor();

            // formula bundle
            var formulaBundle = _formulaExtractor.ExtractAll();

            // groups + sorts come in one dictionary bundle
            var groupSortBundle = _groupExtractor.ExtractGroupAndSortInfo(rpt);

            return new RptReportModel
            {
                // BASIC
                ReportName = reportName,
                ExportSequence = sequence,
                GeneratedDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),

                // SUMMARY & DATABASE
                SummaryInfo = _dataExtractor.ExtractSummaryInfo(rpt),
                Database = _dataExtractor.ExtractDatabase(rpt),
                Fields = _dataExtractor.ExtractDatabaseFields(rpt),
                DataSetPreview = _dataExtractor.ExtractDataSet(rpt),

                // PARAMETERS
                Parameters = _parameterExtractor.ExtractParameters(rpt),

                // FORMULAS
                Formulas = formulaBundle.ReportFormulas,
                SqlExpressions = formulaBundle.SqlExpressions,
                RecordSelectionFormula = formulaBundle.RecordSelectionFormula,
                GroupSelectionFormula = formulaBundle.GroupSelectionFormula,
                GroupConditionFormulas = formulaBundle.GroupConditionFormulas,
                SortFormulas = formulaBundle.SortFormulas,
                RunningTotalFormulas = formulaBundle.RunningTotalFormulas,
                SummaryFormulas = formulaBundle.SummaryFormulas,
                HighlightingFormats = formulaBundle.HighlightingFormats,

                // GROUPS
                Groups = groupSortBundle.ContainsKey("Groups")
                    ? (List<Dictionary<string, object>>)groupSortBundle["Groups"]
                    : new List<Dictionary<string, object>>(),

                // SORTS
                Sorts = groupSortBundle.ContainsKey("Sorts")
                    ? (List<Dictionary<string, object>>)groupSortBundle["Sorts"]
                    : new List<Dictionary<string, object>>(),

                // SECTIONS
                Sections = _sectionExtractor.ExtractSections(rpt),

                // OBJECTS — engine cannot extract → empty list
                Objects = new List<Dictionary<string, object>>(),

                // FORMATTING — engine-only → empty dictionary
                Formatting = new Dictionary<string, object>(),

                // CHARTS + CROSSTABS
                Charts = _chartExtractor.ExtractCharts(),
                CrossTabs = _crossTabExtractor.ExtractCrossTabs(),

                // ALERTS
                Alerts = _alertExtractor.ExtractAlerts(rpt)
            };
        }


        // ======================================================
        // PUBLIC API → XML
        // ======================================================
        public string ConvertRptToXml(string rptPath, string reportName, int sequence)
        {
            var rpt = LoadReport(rptPath);
            var model = BuildModel(rpt, reportName, sequence);
            return _xmlBuilder.BuildXml(model);
        }

        // ======================================================
        // PUBLIC API → JSON
        // ======================================================
        public JObject ConvertRptToJson(string rptPath, string reportName, int sequence)
        {
            var rpt = LoadReport(rptPath);
            var model = BuildModel(rpt, reportName, sequence);
            return _jsonBuilder.BuildJson(model);
        }
    }
}
