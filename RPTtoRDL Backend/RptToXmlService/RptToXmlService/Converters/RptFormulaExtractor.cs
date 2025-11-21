using System;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY Formula Extractor.
    /// Extracts what CrystalDecisions.CrystalReports.Engine supports.
    /// NO RAS API CALLS.
    /// </summary>
    public class RptFormulaExtractor
    {
        private readonly ReportDocument _rpt;

        public RptFormulaExtractor(ReportDocument rpt)
        {
            _rpt = rpt;
        }

        // ================================================================
        // MAIN ENTRY (returns a bundle of all formula-related metadata)
        // ================================================================
        public FormulaExtractionBundle ExtractAll()
        {
            return new FormulaExtractionBundle
            {
                ReportFormulas = ExtractReportFormulas(),
                SqlExpressions = ExtractSqlExpressions(),
                RecordSelectionFormula = ExtractRecordSelection(),
                GroupSelectionFormula = ExtractGroupSelection(),
                GroupConditionFormulas = ExtractGroupConditions(),
                SortFormulas = ExtractSortFormulas(),
                HighlightingFormats = new List<HighlightFormatModel>(),   // ENGINE CANNOT EXTRACT CONDITIONAL FORMATTING
                RunningTotalFormulas = ExtractRunningTotalFormulas(),
                SummaryFormulas = ExtractSummaryFormulas()
            };
        }

        // ================================================================
        // 1. Report Formula Fields
        // ================================================================
        public List<FormulaModel> ExtractReportFormulas()
        {
            var list = new List<FormulaModel>();

            foreach (FormulaFieldDefinition f in _rpt.DataDefinition.FormulaFields)
            {
                list.Add(new FormulaModel
                {
                    Name = f.Name,
                    Text = f.Text
                });
            }

            return list;
        }

        // ================================================================
        // 2. SQL Expression Fields
        // ================================================================
        public List<SqlExpressionModel> ExtractSqlExpressions()
        {
            var list = new List<SqlExpressionModel>();

            foreach (SQLExpressionFieldDefinition sql in _rpt.DataDefinition.SQLExpressionFields)
            {
                list.Add(new SqlExpressionModel
                {
                    Name = sql.Name,
                    Text = sql.Text
                });
            }

            return list;
        }

        // ================================================================
        // 3. Record Selection Formula
        // ================================================================
        public string ExtractRecordSelection()
        {
            return _rpt.RecordSelectionFormula ?? "";
        }

        // ================================================================
        // 4. Group Selection Formula
        // ================================================================
        public string ExtractGroupSelection()
        {
            return _rpt.DataDefinition.GroupSelectionFormula ?? "";
        }

        // ================================================================
        // 5. Group Condition Metadata (ENGINE)
        // ================================================================
        public List<GroupConditionFormulaModel> ExtractGroupConditions()
        {
            var list = new List<GroupConditionFormulaModel>();

            foreach (Group g in _rpt.DataDefinition.Groups)
            {
                list.Add(new GroupConditionFormulaModel
                {
                    GroupName = g.ConditionField?.FormulaName,
                    GroupPath = "",
                    ConditionField = g.ConditionField?.FormulaName
                });
            }

            return list;
        }

        // ================================================================
        // 6. Sort Formulas (ENGINE)
        // ================================================================
        public List<SortFormulaModel> ExtractSortFormulas()
        {
            var list = new List<SortFormulaModel>();

            foreach (SortField sf in _rpt.DataDefinition.SortFields)
            {
                list.Add(new SortFormulaModel
                {
                    Field = sf.Field?.FormulaName,
                    Direction = sf.SortDirection.ToString()
                });
            }

            return list;
        }

        // ====================================================================
        // 7. Running Total Fields (Engine-Safe)
        // ====================================================================
        public List<RunningTotalFormulaModel> ExtractRunningTotalFormulas()
        {
            var list = new List<RunningTotalFormulaModel>();

            foreach (RunningTotalFieldDefinition rt in _rpt.DataDefinition.RunningTotalFields)
            {
                // Crystal Engine is buggy for ResetCondition & EvaluationConditionType
                // Not safe to read. They randomly null ref inside COM layer.
                string operation = "";
                string eval = "";
                string reset = "";

                try { operation = rt.Operation != null ? rt.Operation.ToString() : ""; }
                catch { operation = ""; }

                // DO NOT CALL rt.EvaluationConditionType or rt.ResetCondition
                // These cause ComCrash / NullRef inside CR13 runtime

                list.Add(new RunningTotalFormulaModel
                {
                    Name = rt.Name ?? "",
                    Operation = operation,          // SAFE
                    EvaluateCondition = "",         // NOT SAFE TO READ → empty
                    ResetCondition = ""             // NOT SAFE TO READ → empty
                });
            }

            return list;
        }



        // ====================================================================
        // 8. Summary Fields  (ENGINE ONLY — limited metadata)
        // ====================================================================
        public List<SummaryFormulaModel> ExtractSummaryFormulas()
        {
            var list = new List<SummaryFormulaModel>();

            foreach (SummaryFieldDefinition s in _rpt.DataDefinition.SummaryFields)
            {
                list.Add(new SummaryFormulaModel
                {
                    Name = s.Name,
                    SummaryType = s.Operation.ToString(),                  // ENGINE property
                    Field = s.SummarizedField?.FormulaName ?? ""           // FIXED ✔
                });
            }

            return list;
        }

    }

    // ================================================================
    // DATA MODELS (ENGINE SAFE)
    // ================================================================

    public class FormulaExtractionBundle
    {
        public List<FormulaModel> ReportFormulas { get; set; }
        public List<SqlExpressionModel> SqlExpressions { get; set; }
        public string RecordSelectionFormula { get; set; }
        public string GroupSelectionFormula { get; set; }
        public List<GroupConditionFormulaModel> GroupConditionFormulas { get; set; }
        public List<SortFormulaModel> SortFormulas { get; set; }
        public List<HighlightFormatModel> HighlightingFormats { get; set; }
        public List<RunningTotalFormulaModel> RunningTotalFormulas { get; set; }
        public List<SummaryFormulaModel> SummaryFormulas { get; set; }
    }

    public class FormulaModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }

    public class SqlExpressionModel
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }

    public class GroupConditionFormulaModel
    {
        public string GroupName { get; set; }
        public string GroupPath { get; set; }
        public string ConditionField { get; set; }
    }

    public class SortFormulaModel
    {
        public string Field { get; set; }
        public string Direction { get; set; }
    }

    public class HighlightFormatModel
    {
        public string ObjectName { get; set; }
        public string Condition { get; set; }
        public string BackColor { get; set; }
        public string FontColor { get; set; }
        public bool Applied { get; set; }
    }

    public class RunningTotalFormulaModel
    {
        public string Name { get; set; }
        public string Operation { get; set; }
        public string EvaluateCondition { get; set; }
        public string ResetCondition { get; set; }
    }

    public class SummaryFormulaModel
    {
        public string Name { get; set; }
        public string SummaryType { get; set; }
        public string Field { get; set; }
    }
}
