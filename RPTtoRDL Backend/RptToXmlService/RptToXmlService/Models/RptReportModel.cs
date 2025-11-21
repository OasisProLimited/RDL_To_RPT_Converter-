using RptToXmlService.Converters;
using System;
using System.Collections.Generic;

namespace RptToXmlService.Models
{
    // ======================================================
    // MASTER REPORT MODEL — ENGINE SAFE
    // ======================================================
    public class RptReportModel
    {
        // BASIC INFO
        public string ReportName { get; set; }
        public string GeneratedDate { get; set; }
        public int ExportSequence { get; set; }

        // SUMMARY INFO
        public RptSummaryInfo SummaryInfo { get; set; }

        // DATABASE
        public List<RptTableInfo> Database { get; set; } = new List<RptTableInfo>();
        public List<RptFieldInfo> Fields { get; set; } = new List<RptFieldInfo>();
        public object DataSetPreview { get; set; }

        // PARAMETERS (ENGINE SAFE → DICTIONARY MODE)
        public List<Dictionary<string, object>> Parameters { get; set; }
            = new List<Dictionary<string, object>>();


        // =============================================================
        // FORMULAS (ENGINE-ONLY → Uses FormulaExtractionBundle fields)
        // =============================================================
        public List<FormulaModel> Formulas { get; set; } = new List<FormulaModel>();
        public List<SqlExpressionModel> SqlExpressions { get; set; } = new List<SqlExpressionModel>();

        public string RecordSelectionFormula { get; set; } = "";
        public string GroupSelectionFormula { get; set; } = "";

        public List<GroupConditionFormulaModel> GroupConditionFormulas { get; set; } = new List<GroupConditionFormulaModel>();
        public List<SortFormulaModel> SortFormulas { get; set; } = new List<SortFormulaModel>();
        public List<RunningTotalFormulaModel> RunningTotalFormulas { get; set; } = new List<RunningTotalFormulaModel>();
        public List<SummaryFormulaModel> SummaryFormulas { get; set; } = new List<SummaryFormulaModel>();

        public List<HighlightFormatModel> HighlightingFormats { get; set; } = new List<HighlightFormatModel>();

        // =============================================================
        // GROUPS & SORTS (ENGINE SAFE → DICTIONARY MODE)
        // =============================================================
        public List<Dictionary<string, object>> Groups { get; set; }
            = new List<Dictionary<string, object>>();

        public List<Dictionary<string, object>> Sorts { get; set; }
            = new List<Dictionary<string, object>>();

        // =============================================================
        // SECTIONS (Dictionary-based, because extractor returns dict)
        // =============================================================
        public List<Dictionary<string, object>> Sections { get; set; }
            = new List<Dictionary<string, object>>();

        // =============================================================
        // OBJECTS (ENGINE SAFE → DICTIONARY, matches RptObjectExtractor)
        // =============================================================
        public List<Dictionary<string, object>> Objects { get; set; }
            = new List<Dictionary<string, object>>();

        // =============================================================
        // FORMATTING (Dictionary-based — ENGINE ONLY)
        // =============================================================
        public Dictionary<string, object> Formatting { get; set; }
            = new Dictionary<string, object>();

        // =============================================================
        // CHARTS + CROSSTABS + ALERTS
        // =============================================================
        public List<RptChartInfo> Charts { get; set; } = new List<RptChartInfo>();
        public List<RptCrossTabInfo> CrossTabs { get; set; } = new List<RptCrossTabInfo>();
        public List<RptAlertInfo> Alerts { get; set; } = new List<RptAlertInfo>();
    }

    // ======================================================
    // SUMMARY INFO
    // ======================================================
    public class RptSummaryInfo
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Comments { get; set; }
    }

    // ======================================================
    // DATABASE TABLES + FIELDS
    // ======================================================
    public class RptTableInfo
    {
        public string TableName { get; set; }
        public string Location { get; set; }

        public string Server { get; set; }
        public string Database { get; set; }
        public string UserId { get; set; }
        public string Provider { get; set; }

        public List<RptFieldInfo> Fields { get; set; } = new List<RptFieldInfo>();
    }

    public class RptFieldInfo
    {
        public string Name { get; set; }
        public string NativeName { get; set; }
        public string DataType { get; set; }
        public int Length { get; set; }
    }

    // ======================================================
    // PARAMETERS
    // ======================================================
    public class RptParameterInfo
    {
        public string Name { get; set; }
        public string PromptText { get; set; }
        public string Type { get; set; }
        public bool AllowMultiple { get; set; }
        public bool HasDefault { get; set; }

        public List<string> CurrentValues { get; set; } = new List<string>();
        public List<string> DefaultValues { get; set; } = new List<string>();
        public List<RptRangeValueInfo> RangeValues { get; set; } = new List<RptRangeValueInfo>();

        public string EditMask { get; set; }
    }

    public class RptRangeValueInfo
    {
        public string StartValue { get; set; }
        public string EndValue { get; set; }
        public bool InclusiveStart { get; set; }
        public bool InclusiveEnd { get; set; }
    }

    // ======================================================
    // FORMULA MODELS (SHARED WITH EXTRACTOR)
    // ======================================================
    public class RptFormulaInfo
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public bool IsConditional { get; set; }
        public string FormulaType { get; set; }
    }
}
