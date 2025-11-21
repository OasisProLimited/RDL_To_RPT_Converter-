using System;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY FORMAT EXTRACTOR
    /// Extracts only properties supported by CrystalDecisions.CrystalReports.Engine.
    /// No RAS API calls.
    /// </summary>
    public class RptFormatExtractor
    {
        // ====================================================================
        // MAIN ENTRY — Extract formatting for a single ReportObject
        // ====================================================================
        public Dictionary<string, object> ExtractFormatting(ReportObject obj)
        {
            var result = new Dictionary<string, object>();

            // Common properties
            result["Top"] = obj.Top;
            result["Left"] = obj.Left;
            result["Width"] = obj.Width;
            result["Height"] = obj.Height;

            // Object type-specific
            if (obj is TextObject text)
            {
                result["Type"] = "TextObject";
                result["Text"] = text.Text;
                result["Font"] = ExtractFont(text);
                result["Alignment"] = ExtractAlignment(text);
                result["Color"] = text.Color.ToString();

                // ⭐ ADDED → ENGINE property
                result["CanGrow"] = text.ObjectFormat?.EnableCanGrow ?? false;
            }
            else if (obj is FieldObject field)
            {
                result["Type"] = "FieldObject";
                result["FieldName"] = field.DataSource?.Name;
                result["Font"] = ExtractFont(field);
                result["Alignment"] = ExtractAlignment(field);

                // Extract base field ObjectFormat properties
                var fieldFmt = ExtractFieldObjectFormat(field);
                result["ObjectFormat"] = fieldFmt;

                // ⭐ No SuppressIfDuplicated property exists in this CR Engine version.
                result["SuppressIfDuplicated"] = "NotAvailableInEngineAPI";
            }


            else if (obj is LineObject line)
            {
                result["Type"] = "LineObject";
                result["LineStyle"] = line.LineStyle.ToString();
            }
            else if (obj is BoxObject box)
            {
                result["Type"] = "BoxObject";
                result["LineStyle"] = box.LineStyle.ToString();
            }
            else if (obj is PictureObject pic)
            {
                result["Type"] = "PictureObject";

                // ENGINE cannot extract GraphicLocation / Type
                result["GraphicLocation"] = "NotAvailableInEngineAPI";
                result["GraphicType"] = "NotAvailableInEngineAPI";
            }
            else
            {
                result["Type"] = obj.Kind.ToString();
            }

            // Conditional formatting (ENGINE DOES NOT SUPPORT IT — return empty)
            result["ConditionalFormatting"] = new List<Dictionary<string, string>>();

            return result;
        }

        // ====================================================================
        // FONT EXTRACTION
        // ====================================================================
        private Dictionary<string, object> ExtractFont(dynamic obj)
        {
            try
            {
                var f = obj.Font;

                return new Dictionary<string, object>
                {
                    { "FontName", f.Name },
                    { "Size", f.Size },
                    { "Bold", f.Bold },
                    { "Italic", f.Italic },
                    { "Underline", f.Underline },
                    { "Strikeout", f.Strikeout },
                    { "Color", obj.Color.ToString() }
                };
            }
            catch
            {
                return new Dictionary<string, object>();
            }
        }

        // ====================================================================
        // ALIGNMENT
        // ====================================================================
        private string ExtractAlignment(dynamic obj)
        {
            try
            {
                return obj.HorizontalAlignment.ToString();
            }
            catch
            {
                return "Default";
            }
        }

        // ====================================================================
        // FIELD OBJECT FORMAT (ENGINE ONLY)
        // ====================================================================
        private Dictionary<string, object> ExtractFieldObjectFormat(FieldObject field)
        {
            var fmt = field.ObjectFormat;

            return new Dictionary<string, object>
            {
                { "CanGrow", fmt.EnableCanGrow },
                { "Suppress", fmt.EnableSuppress },
                { "KeepTogether", fmt.EnableKeepTogether }
                // ⭐ Note: SuppressIfDuplicated extracted above separately 
            };
        }
    }
}
