using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY Object Extractor
    /// Supports: FieldObject, TextObject, LineObject, BoxObject,
    /// PictureObject, SubreportObject, ChartObject, CrossTabObject
    /// 
    /// IMPORTANT:
    /// Crystal Reports ENGINE API does NOT support:
    ///  - HorizontalAlignment
    ///  - BackgroundColor
    ///  - GraphicLocation / GraphicType
    ///  - FormatConditions (Highlighting)
    /// </summary>
    public class RptObjectExtractor
    {
        public RptObjectExtractor() { }

        // ==============================================================
        // Extract a single object
        // ==============================================================
        public Dictionary<string, object> ExtractObject(dynamic obj, dynamic section)
        {
            var result = new Dictionary<string, object>
            {
                ["Name"] = obj.Name,
                ["ObjectType"] = obj.Kind.ToString(),
                ["SectionName"] = section.Name,
                ["Top"] = obj.Top,
                ["Left"] = obj.Left,
                ["Width"] = obj.Width,
                ["Height"] = obj.Height,
                ["Border"] = ExtractBorder(obj)
            };

            // ==========================================================
            // FIELD OBJECT (ENGINE)
            // ==========================================================
            if (obj is CrystalDecisions.CrystalReports.Engine.FieldObject f)
            {
                result["Type"] = "FieldObject";
                result["FieldName"] = f.DataSource?.Name;
                result["Font"] = ExtractFont(f.Font);
                result["Color"] = f.Color.ToString();
            }

            // ==========================================================
            // TEXT OBJECT (ENGINE)
            // ==========================================================
            else if (obj is CrystalDecisions.CrystalReports.Engine.TextObject t)
            {
                result["Type"] = "TextObject";
                result["Text"] = t.Text;
                result["Font"] = ExtractFont(t.Font);
                result["Color"] = t.Color.ToString();
            }

            // ==========================================================
            // LINE OBJECT (ENGINE)
            // ==========================================================
            else if (obj is CrystalDecisions.CrystalReports.Engine.LineObject line)
            {
                result["Type"] = "LineObject";
                result["X1"] = line.Left;
                result["Y1"] = line.Top;
                result["X2"] = line.Left + line.Width;
                result["Y2"] = line.Top + line.Height;
            }

            // ==========================================================
            // BOX OBJECT (ENGINE)
            // ==========================================================
            else if (obj is CrystalDecisions.CrystalReports.Engine.BoxObject box)
            {
                result["Type"] = "BoxObject";
                result["LineStyle"] = box.LineStyle.ToString();
            }

            // ==========================================================
            // PICTURE OBJECT (ENGINE — limited)
            // ==========================================================
            else if (obj is CrystalDecisions.CrystalReports.Engine.PictureObject)
            {
                result["Type"] = "PictureObject";
                result["Note"] = "GraphicLocation/GraphicType not available in ENGINE API.";
            }

            // ==========================================================
            // SUBREPORT OBJECT (ENGINE ONLY)
            // ==========================================================
            else if (obj is CrystalDecisions.CrystalReports.Engine.SubreportObject sub)
            {
                result["Type"] = "SubreportObject";
                result["SubreportName"] = sub.SubreportName;

                // ENGINE DOES NOT SUPPORT SUBREPORT LINKS
                result["Links"] = "NotAvailableInEngineAPI";
            }


            // ==========================================================
            // CHART OBJECT
            // ==========================================================
            if (obj.Kind == ReportObjectKind.ChartObject)
            {
                result["ChartInfo"] = "Chart extraction must be done at ReportDocument level.";
            }

            // ==========================================================
            // CROSSTAB OBJECT
            // ==========================================================
            if (obj.Kind == ReportObjectKind.CrossTabObject)
            {
                result["CrossTabInfo"] = "CrossTab extraction must be done at ReportDocument level.";
            }

            return result;
        }

        // ==============================================================
        // BORDER (ENGINE supports ONLY border styles)
        // ==============================================================
        private Dictionary<string, object> ExtractBorder(dynamic obj)
        {
            var b = obj.Border;

            return new Dictionary<string, object>
            {
                ["Left"] = b.LeftLineStyle.ToString(),
                ["Right"] = b.RightLineStyle.ToString(),
                ["Top"] = b.TopLineStyle.ToString(),
                ["Bottom"] = b.BottomLineStyle.ToString(),
                ["Color"] = b.BorderColor.ToString()
            };
        }

        // ==============================================================
        // FONT
        // ==============================================================
        private Dictionary<string, object> ExtractFont(Font f)
        {
            if (f == null)
                return new Dictionary<string, object>();

            return new Dictionary<string, object>
            {
                ["FontName"] = f.Name,
                ["Size"] = f.Size,
                ["Bold"] = f.Bold,
                ["Italic"] = f.Italic,
                ["Underline"] = f.Underline
            };
        }
    }
}
