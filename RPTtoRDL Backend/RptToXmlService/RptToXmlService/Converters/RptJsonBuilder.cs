using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using RptToXmlService.Models;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// FINAL JSON BUILDER matching the new RptReportModel (dictionary-based)
    /// </summary>
    public class RptJsonBuilder
    {
        public JObject BuildJson(RptReportModel model)
        {
            JObject root = new JObject();

            root["ReportName"] = model.ReportName;
            root["GeneratedDate"] = model.GeneratedDate;
            root["ExportSequence"] = model.ExportSequence;

            root["SummaryInfo"] = BuildSummary(model.SummaryInfo);

            root["Database"] = BuildTables(model.Database);
            root["Fields"] = BuildFields(model.Fields);

            root["DataSetPreview"] = JToken.FromObject(model.DataSetPreview ?? "");

            root["Parameters"] = BuildParameters(model.Parameters);

            // FORMULAS (NEW MODEL)
            root["ReportFormulas"] = BuildFormulaList(model.Formulas);
            root["SqlExpressions"] = BuildSqlExpressions(model.SqlExpressions);
            root["RecordSelectionFormula"] = model.RecordSelectionFormula ?? "";
            root["GroupSelectionFormula"] = model.GroupSelectionFormula ?? "";
            root["GroupConditionFormulas"] = BuildGroupConditionFormulas(model.GroupConditionFormulas);
            root["SortFormulas"] = BuildSortFormulaList(model.SortFormulas);
            root["RunningTotalFormulas"] = BuildRunningTotals(model.RunningTotalFormulas);
            root["SummaryFormulas"] = BuildSummaryFormulaList(model.SummaryFormulas);
            root["HighlightingFormats"] = BuildHighlightFormats(model.HighlightingFormats);

            root["Sections"] = BuildDictionaryList(model.Sections);
            root["Objects"] = BuildDictionaryList(model.Objects);
            root["Groups"] = BuildDictionaryList(model.Groups);
            root["Sorts"] = BuildDictionaryList(model.Sorts);

            root["Formatting"] = JObject.FromObject(model.Formatting ?? new Dictionary<string, object>());

            root["Charts"] = BuildCharts(model.Charts);
            root["CrossTabs"] = BuildCrossTabs(model.CrossTabs);

            root["Alerts"] = BuildAlerts(model.Alerts);

            return root;
        }

        // ============================================================
        // SUMMARY
        // ============================================================
        private JObject BuildSummary(RptSummaryInfo info)
        {
            if (info == null) return new JObject();

            return new JObject
            {
                ["Title"] = info.Title,
                ["Author"] = info.Author,
                ["Comments"] = info.Comments
            };
        }

        // ============================================================
        // DATABASE
        // ============================================================
        private JArray BuildTables(List<RptTableInfo> tables)
        {
            JArray arr = new JArray();
            if (tables == null) return arr;

            foreach (var t in tables)
            {
                JObject tbl = new JObject
                {
                    ["TableName"] = t.TableName,
                    ["Location"] = t.Location,
                    ["Connection"] = new JObject
                    {
                        ["Server"] = t.Server,
                        ["Database"] = t.Database,
                        ["UserId"] = t.UserId,
                        ["Provider"] = t.Provider
                    },
                    ["Fields"] = BuildFields(t.Fields)
                };

                arr.Add(tbl);
            }

            return arr;
        }

        // ============================================================
        // FIELDS
        // ============================================================
        private JArray BuildFields(List<RptFieldInfo> fields)
        {
            JArray arr = new JArray();
            if (fields == null) return arr;

            foreach (var f in fields)
            {
                arr.Add(new JObject
                {
                    ["Name"] = f.Name,
                    ["NativeName"] = f.NativeName,
                    ["DataType"] = f.DataType,
                    ["Length"] = f.Length
                });
            }

            return arr;
        }

        // ============================================================
        // PARAMETERS
        // ============================================================
        private JArray BuildParameters(List<Dictionary<string, object>> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var dict in list)
            {
                JObject obj = new JObject();

                foreach (var kv in dict)
                {
                    obj[kv.Key] = kv.Value != null
                        ? JToken.FromObject(kv.Value)
                        : null;
                }

                arr.Add(obj);
            }

            return arr;
        }


        private JArray BuildRanges(List<RptRangeValueInfo> ranges)
        {
            JArray arr = new JArray();
            if (ranges == null) return arr;

            foreach (var r in ranges)
            {
                arr.Add(new JObject
                {
                    ["Start"] = r.StartValue,
                    ["End"] = r.EndValue,
                    ["InclusiveStart"] = r.InclusiveStart,
                    ["InclusiveEnd"] = r.InclusiveEnd
                });
            }

            return arr;
        }

        // ============================================================
        // FORMULAS (NEW ENGINE MODEL)
        // ============================================================
        private JArray BuildFormulaList(List<FormulaModel> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var f in list)
            {
                arr.Add(new JObject
                {
                    ["Name"] = f.Name,
                    ["Text"] = f.Text
                });
            }

            return arr;
        }

        private JArray BuildSqlExpressions(List<SqlExpressionModel> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var sql in list)
            {
                arr.Add(new JObject
                {
                    ["Name"] = sql.Name,
                    ["Text"] = sql.Text
                });
            }

            return arr;
        }

        private JArray BuildGroupConditionFormulas(List<GroupConditionFormulaModel> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var g in list)
            {
                arr.Add(new JObject
                {
                    ["GroupName"] = g.GroupName,
                    ["GroupPath"] = g.GroupPath,
                    ["ConditionField"] = g.ConditionField
                });
            }

            return arr;
        }

        private JArray BuildSortFormulaList(List<SortFormulaModel> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var s in list)
            {
                arr.Add(new JObject
                {
                    ["Field"] = s.Field,
                    ["Direction"] = s.Direction
                });
            }

            return arr;
        }

        private JArray BuildRunningTotals(List<RunningTotalFormulaModel> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var rt in list)
            {
                arr.Add(new JObject
                {
                    ["Name"] = rt.Name,
                    ["Operation"] = rt.Operation,
                    ["EvaluateCondition"] = rt.EvaluateCondition,
                    ["ResetCondition"] = rt.ResetCondition
                });
            }

            return arr;
        }

        private JArray BuildSummaryFormulaList(List<SummaryFormulaModel> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var s in list)
            {
                arr.Add(new JObject
                {
                    ["Name"] = s.Name,
                    ["SummaryType"] = s.SummaryType,
                    ["Field"] = s.Field
                });
            }

            return arr;
        }

        private JArray BuildHighlightFormats(List<HighlightFormatModel> list)
        {
            return list == null ? new JArray() : JArray.FromObject(list);
        }

        // ============================================================
        // DICTIONARY-BASED MODELS
        // ============================================================
        private JArray BuildDictionaryList(List<Dictionary<string, object>> list)
        {
            JArray arr = new JArray();
            if (list == null) return arr;

            foreach (var dict in list)
                arr.Add(JObject.FromObject(dict));

            return arr;
        }

        // ============================================================
        // CHARTS
        // ============================================================
        private JArray BuildCharts(List<RptChartInfo> charts)
        {
            JArray arr = new JArray();
            if (charts == null) return arr;

            foreach (var c in charts)
            {
                JObject obj = new JObject
                {
                    ["Name"] = c.Name,
                    ["ChartType"] = c.ChartType,
                    ["SectionName"] = c.SectionName,
                    ["Is3D"] = c.Is3D,
                    ["Palette"] = c.Palette,
                    ["Series"] = JArray.FromObject(c.Series ?? new List<RptChartSeriesInfo>())
                };

                if (c.Title != null) obj["Title"] = JObject.FromObject(c.Title);
                if (c.Legend != null) obj["Legend"] = JObject.FromObject(c.Legend);
                if (c.XAxis != null) obj["XAxis"] = JObject.FromObject(c.XAxis);
                if (c.YAxis != null) obj["YAxis"] = JObject.FromObject(c.YAxis);

                arr.Add(obj);
            }

            return arr;
        }

        // ============================================================
        // CROSSTABS
        // ============================================================
        private JArray BuildCrossTabs(List<RptCrossTabInfo> tabs)
        {
            JArray arr = new JArray();
            if (tabs == null) return arr;

            foreach (var ct in tabs)
            {
                arr.Add(new JObject
                {
                    ["Name"] = ct.Name,
                    ["SectionName"] = ct.SectionName,
                    ["Rows"] = JArray.FromObject(ct.Rows),
                    ["Columns"] = JArray.FromObject(ct.Columns),
                    ["Summaries"] = JArray.FromObject(ct.Summaries),
                    ["Cells"] = JArray.FromObject(ct.Cells)
                });
            }

            return arr;
        }

        // ============================================================
        // ALERTS
        // ============================================================
        private JArray BuildAlerts(List<RptAlertInfo> alerts)
        {
            JArray arr = new JArray();
            if (alerts == null) return arr;

            foreach (var a in alerts)
            {
                arr.Add(new JObject
                {
                    ["AlertName"] = a.AlertName,
                    ["Enabled"] = a.Enabled,
                    ["Severity"] = a.Severity,
                    ["Message"] = a.Message,
                    ["ConditionFormula"] = a.ConditionFormula,
                    ["InvolvedFields"] = new JArray(a.InvolvedFields ?? new List<string>())
                });
            }

            return arr;
        }
    }
}
