using System;
using System.Collections.Generic;
using System.Xml;
using RptToXmlService.Models;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// FINAL ENGINE-SAFE XML BUILDER
    /// Matches the NEW RptReportModel (dictionary-based)
    /// </summary>
    public class RptXmlBuilder
    {
        public string BuildXml(RptReportModel model)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);

            XmlElement root = doc.CreateElement("CrystalReport");
            doc.AppendChild(root);

            AppendMetadata(doc, root, model);
            AppendSummary(doc, root, model.SummaryInfo);

            AppendDatabase(doc, root, model.Database);
            AppendAllFields(doc, root, model.Fields);
            AppendDataSetPreview(doc, root, model.DataSetPreview);

            AppendParameters(doc, root, model.Parameters);

            // FORMULA BUNDLE
            AppendFormulaList(doc, root, "ReportFormulas", model.Formulas);
            AppendSqlExpressionList(doc, root, "SqlExpressions", model.SqlExpressions);
            AppendTextNode(doc, root, "RecordSelectionFormula", model.RecordSelectionFormula);
            AppendTextNode(doc, root, "GroupSelectionFormula", model.GroupSelectionFormula);
            AppendGroupConditionFormulas(doc, root, model.GroupConditionFormulas);
            AppendSortFormulaList(doc, root, model.SortFormulas);
            AppendRunningTotalList(doc, root, model.RunningTotalFormulas);
            AppendSummaryFormulaList(doc, root, model.SummaryFormulas);
            AppendHighlightFormats(doc, root, model.HighlightingFormats);

            // SECTIONS / OBJECTS / GROUPS / SORTS (DICTIONARIES)
            AppendDictionaryList(doc, root, "Sections", model.Sections, "Section");
            AppendDictionaryList(doc, root, "Objects", model.Objects, "Object");
            AppendDictionaryList(doc, root, "Groups", model.Groups, "Group");
            AppendDictionaryList(doc, root, "Sorts", model.Sorts, "Sort");

            // GLOBAL FORMATTING (dictionary)
            AppendFormatting(doc, root, model.Formatting);

            // CHARTS & CROSSTABS
            AppendCharts(doc, root, model.Charts);
            AppendCrossTabs(doc, root, model.CrossTabs);

            // ALERTS
            AppendAlerts(doc, root, model.Alerts);

            return Beautify(doc);
        }

        private void AppendSqlExpressionList(XmlDocument doc, XmlElement root, string label, List<SqlExpressionModel> list)
        {
            XmlElement fmNode = doc.CreateElement(label);
            root.AppendChild(fmNode);

            if (list == null) return;

            foreach (var sql in list)
            {
                XmlElement node = doc.CreateElement("SqlExpression");
                fmNode.AppendChild(node);

                AddText(doc, node, "Name", sql.Name);
                AddText(doc, node, "Text", sql.Text);
            }
        }


        // ============================================================
        // METADATA
        // ============================================================
        private void AppendMetadata(XmlDocument doc, XmlElement root, RptReportModel m)
        {
            XmlElement meta = doc.CreateElement("ReportMetadata");
            root.AppendChild(meta);

            AddText(doc, meta, "ReportName", m.ReportName);
            AddText(doc, meta, "GeneratedDate", m.GeneratedDate);
            AddText(doc, meta, "ExportSequence", m.ExportSequence.ToString());
        }

        // ============================================================
        // SUMMARY INFO
        // ============================================================
        private void AppendSummary(XmlDocument doc, XmlElement root, RptSummaryInfo info)
        {
            if (info == null) return;

            XmlElement node = doc.CreateElement("SummaryInfo");
            root.AppendChild(node);

            AddText(doc, node, "Title", info.Title);
            AddText(doc, node, "Author", info.Author);
            AddText(doc, node, "Comments", info.Comments);
        }

        // ============================================================
        // DATABASE TABLES
        // ============================================================
        private void AppendDatabase(XmlDocument doc, XmlElement root, List<RptTableInfo> tables)
        {
            XmlElement db = doc.CreateElement("Database");
            root.AppendChild(db);

            if (tables == null) return;

            foreach (var t in tables)
            {
                XmlElement tbl = doc.CreateElement("Table");
                db.AppendChild(tbl);

                AddText(doc, tbl, "TableName", t.TableName);
                AddText(doc, tbl, "Location", t.Location);

                XmlElement conn = doc.CreateElement("Connection");
                tbl.AppendChild(conn);

                AddText(doc, conn, "Server", t.Server);
                AddText(doc, conn, "Database", t.Database);
                AddText(doc, conn, "User", t.UserId);
                AddText(doc, conn, "Provider", t.Provider);

                AppendFieldList(doc, tbl, t.Fields);
            }
        }

        // ============================================================
        // ALL FIELDS
        // ============================================================
        private void AppendAllFields(XmlDocument doc, XmlElement root, List<RptFieldInfo> fields)
        {
            XmlElement fRoot = doc.CreateElement("AllFields");
            root.AppendChild(fRoot);

            AppendFieldList(doc, fRoot, fields);
        }

        private void AppendFieldList(XmlDocument doc, XmlElement parent, List<RptFieldInfo> fields)
        {
            if (fields == null) return;

            foreach (var f in fields)
            {
                XmlElement n = doc.CreateElement("Field");
                parent.AppendChild(n);

                AddText(doc, n, "Name", f.Name);
                AddText(doc, n, "NativeName", f.NativeName);
                AddText(doc, n, "DataType", f.DataType);
                AddText(doc, n, "Length", f.Length.ToString());
            }
        }

        // ============================================================
        // DATASET PREVIEW
        // ============================================================
        private void AppendDataSetPreview(XmlDocument doc, XmlElement root, object preview)
        {
            if (preview == null) return;

            XmlElement n = doc.CreateElement("DataSetPreview");
            n.InnerText = preview.ToString();
            root.AppendChild(n);
        }

        // ============================================================
        // PARAMETERS
        // ============================================================
        private void AppendParameters(XmlDocument doc, XmlElement root, List<Dictionary<string, object>> list)
        {
            XmlElement prmNode = doc.CreateElement("Parameters");
            root.AppendChild(prmNode);

            if (list == null) return;

            foreach (var dict in list)
            {
                XmlElement paramNode = doc.CreateElement("Parameter");
                prmNode.AppendChild(paramNode);

                foreach (var kv in dict)
                {
                    XmlElement item = doc.CreateElement(kv.Key);
                    item.InnerText = kv.Value?.ToString() ?? "";
                    paramNode.AppendChild(item);
                }
            }
        }


        private void AppendValueList(XmlDocument doc, XmlElement parent, string label, List<string> values)
        {
            XmlElement vNode = doc.CreateElement(label);
            parent.AppendChild(vNode);

            if (values == null) return;

            foreach (var v in values)
                AddText(doc, vNode, "Value", v);
        }

        // ============================================================
        // FORMULA BUNDLE (NEW MODEL)
        // ============================================================
        private void AppendFormulaList(XmlDocument doc, XmlElement root, string label, List<FormulaModel> list)
        {
            XmlElement fRoot = doc.CreateElement(label);
            root.AppendChild(fRoot);

            if (list == null) return;

            foreach (var f in list)
            {
                XmlElement n = doc.CreateElement("Formula");
                fRoot.AppendChild(n);

                AddText(doc, n, "Name", f.Name);
                AddText(doc, n, "Text", f.Text);
            }
        }

        private void AppendGroupConditionFormulas(XmlDocument doc, XmlElement root, List<GroupConditionFormulaModel> list)
        {
            XmlElement gRoot = doc.CreateElement("GroupConditionFormulas");
            root.AppendChild(gRoot);

            if (list == null) return;

            foreach (var g in list)
            {
                XmlElement n = doc.CreateElement("GroupCondition");
                gRoot.AppendChild(n);

                AddText(doc, n, "GroupName", g.GroupName);
                AddText(doc, n, "GroupPath", g.GroupPath);
                AddText(doc, n, "ConditionField", g.ConditionField);
            }
        }

        private void AppendSortFormulaList(XmlDocument doc, XmlElement root, List<SortFormulaModel> list)
        {
            XmlElement sRoot = doc.CreateElement("SortFormulas");
            root.AppendChild(sRoot);

            if (list == null) return;

            foreach (var s in list)
            {
                XmlElement n = doc.CreateElement("SortFormula");
                sRoot.AppendChild(n);

                AddText(doc, n, "Field", s.Field);
                AddText(doc, n, "Direction", s.Direction);
            }
        }

        private void AppendRunningTotalList(XmlDocument doc, XmlElement root, List<RunningTotalFormulaModel> list)
        {
            XmlElement rtRoot = doc.CreateElement("RunningTotalFormulas");
            root.AppendChild(rtRoot);

            if (list == null) return;

            foreach (var rt in list)
            {
                XmlElement n = doc.CreateElement("RunningTotal");
                rtRoot.AppendChild(n);

                AddText(doc, n, "Name", rt.Name);
                AddText(doc, n, "Operation", rt.Operation);
                AddText(doc, n, "EvaluateCondition", rt.EvaluateCondition);
                AddText(doc, n, "ResetCondition", rt.ResetCondition);
            }
        }

        private void AppendSummaryFormulaList(XmlDocument doc, XmlElement root, List<SummaryFormulaModel> list)
        {
            XmlElement smRoot = doc.CreateElement("SummaryFormulas");
            root.AppendChild(smRoot);

            if (list == null) return;

            foreach (var s in list)
            {
                XmlElement n = doc.CreateElement("SummaryFormula");
                smRoot.AppendChild(n);

                AddText(doc, n, "Name", s.Name);
                AddText(doc, n, "SummaryType", s.SummaryType);
                AddText(doc, n, "Field", s.Field);
            }
        }

        private void AppendHighlightFormats(XmlDocument doc, XmlElement root, List<HighlightFormatModel> list)
        {
            XmlElement hRoot = doc.CreateElement("HighlightingFormats");
            root.AppendChild(hRoot);

            if (list == null) return;
        }

        // ============================================================
        // DICTIONARY-BASED MODELS (NEW)
        // ============================================================
        private void AppendDictionaryList(XmlDocument doc, XmlElement root, string rootName, List<Dictionary<string, object>> list, string itemName)
        {
            XmlElement r = doc.CreateElement(rootName);
            root.AppendChild(r);

            if (list == null) return;

            foreach (var dict in list)
            {
                XmlElement n = doc.CreateElement(itemName);
                r.AppendChild(n);

                foreach (var pair in dict)
                    AddText(doc, n, pair.Key, pair.Value?.ToString() ?? "");
            }
        }

        // ============================================================
        // GLOBAL FORMATTING (dictionary)
        // ============================================================
        private void AppendFormatting(XmlDocument doc, XmlElement root, Dictionary<string, object> fmt)
        {
            if (fmt == null) return;

            XmlElement fNode = doc.CreateElement("ReportFormatting");
            root.AppendChild(fNode);

            foreach (var kv in fmt)
                AddText(doc, fNode, kv.Key, kv.Value?.ToString() ?? "");
        }

        // ============================================================
        // CHARTS
        // ============================================================
        private void AppendCharts(XmlDocument doc, XmlElement root, List<RptChartInfo> charts)
        {
            XmlElement cRoot = doc.CreateElement("Charts");
            root.AppendChild(cRoot);

            if (charts == null) return;

            foreach (var c in charts)
            {
                XmlElement n = doc.CreateElement("Chart");
                cRoot.AppendChild(n);

                AddText(doc, n, "Name", c.Name);
                AddText(doc, n, "ChartType", c.ChartType);
                AddText(doc, n, "SectionName", c.SectionName);

                AddText(doc, n, "Is3D", c.Is3D.ToString());
                AddText(doc, n, "Palette", c.Palette);

                // Title
                if (c.Title != null)
                {
                    XmlElement t = doc.CreateElement("Title");
                    AddText(doc, t, "Text", c.Title.Text);
                    AddText(doc, t, "Font", c.Title.Font);
                    AddText(doc, t, "Size", c.Title.Size.ToString());
                    AddText(doc, t, "Bold", c.Title.Bold.ToString());
                    AddText(doc, t, "Color", c.Title.Color);
                    n.AppendChild(t);
                }

                // Legend
                if (c.Legend != null)
                {
                    XmlElement l = doc.CreateElement("Legend");
                    AddText(doc, l, "Visible", c.Legend.Visible.ToString());
                    AddText(doc, l, "Position", c.Legend.Position);
                    AddText(doc, l, "Font", c.Legend.Font);
                    AddText(doc, l, "Size", c.Legend.Size.ToString());
                    AddText(doc, l, "Color", c.Legend.Color);
                    n.AppendChild(l);
                }

                // Series
                XmlElement series = doc.CreateElement("Series");
                n.AppendChild(series);

                foreach (var s in c.Series)
                {
                    XmlElement si = doc.CreateElement("SeriesItem");
                    AddText(doc, si, "Name", s.Name);
                    AddText(doc, si, "Field", s.Field);
                    AddText(doc, si, "Color", s.Color);
                    AddText(doc, si, "MarkerShape", s.MarkerShape);
                    series.AppendChild(si);
                }
            }
        }

        // ============================================================
        // CROSSTABS
        // ============================================================
        private void AppendCrossTabs(XmlDocument doc, XmlElement root, List<RptCrossTabInfo> tabs)
        {
            XmlElement ctRoot = doc.CreateElement("CrossTabs");
            root.AppendChild(ctRoot);

            if (tabs == null) return;

            foreach (var ct in tabs)
            {
                XmlElement n = doc.CreateElement("CrossTab");
                ctRoot.AppendChild(n);

                AddText(doc, n, "Name", ct.Name);
                AddText(doc, n, "SectionName", ct.SectionName);
                AddText(doc, n, "Top", ct.Top.ToString());
                AddText(doc, n, "Left", ct.Left.ToString());
                AddText(doc, n, "Width", ct.Width.ToString());
                AddText(doc, n, "Height", ct.Height.ToString());
            }
        }

        // ============================================================
        // ALERTS
        // ============================================================
        private void AppendAlerts(XmlDocument doc, XmlElement root, List<RptAlertInfo> alerts)
        {
            XmlElement aRoot = doc.CreateElement("Alerts");
            root.AppendChild(aRoot);

            if (alerts == null) return;

            foreach (var a in alerts)
            {
                XmlElement n = doc.CreateElement("Alert");
                aRoot.AppendChild(n);

                AddText(doc, n, "AlertName", a.AlertName);
                AddText(doc, n, "Enabled", a.Enabled.ToString());
                AddText(doc, n, "Severity", a.Severity);
                AddText(doc, n, "Message", a.Message);
                AddText(doc, n, "ConditionFormula", a.ConditionFormula);

                XmlElement fNode = doc.CreateElement("InvolvedFields");
                n.AppendChild(fNode);

                if (a.InvolvedFields != null)
                {
                    foreach (var f in a.InvolvedFields)
                        AddText(doc, fNode, "Field", f);
                }
            }
        }

        // ============================================================
        // HELPERS
        // ============================================================
        private void AddText(XmlDocument doc, XmlElement parent, string label, string value)
        {
            XmlElement el = doc.CreateElement(label);
            el.InnerText = value ?? "";
            parent.AppendChild(el);
        }

        private void AppendTextNode(XmlDocument doc, XmlElement parent, string label, string value)
        {
            XmlElement el = doc.CreateElement(label);
            el.InnerText = value ?? "";
            parent.AppendChild(el);
        }

        private string Beautify(XmlDocument doc)
        {
            using (var sw = new System.IO.StringWriter())
            {
                using (var xw = new XmlTextWriter(sw) { Formatting = Formatting.Indented })
                {
                    doc.WriteTo(xw);
                    return sw.ToString();
                }
            }
        }
    }
}
