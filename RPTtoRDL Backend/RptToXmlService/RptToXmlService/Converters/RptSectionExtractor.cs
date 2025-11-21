using System;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY section extractor.
    /// Extracts:
    /// ✔ Section metadata
    /// ✔ SectionFormat flags (Suppress, KeepTogether, etc.)
    /// ✔ Objects inside each section
    /// ✔ Background color & height
    /// 
    /// NOTE:
    /// Conditional section formulas require RAS API
    /// and are NOT available in the Crystal ENGINE.
    /// They return empty strings here.
    /// </summary>
    public class RptSectionExtractor
    {
        private readonly RptObjectExtractor _objectExtractor;
        private readonly RptFormatExtractor _formatExtractor;

        public RptSectionExtractor()
        {
            _objectExtractor = new RptObjectExtractor();
            _formatExtractor = new RptFormatExtractor();
        }

        // ====================================================================
        // Extract all sections
        // ====================================================================
        public List<Dictionary<string, object>> ExtractSections(ReportDocument rpt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (Section sec in rpt.ReportDefinition.Sections)
            {
                list.Add(ExtractSection(sec));
            }

            return list;
        }

        // ====================================================================
        // Extract a single section
        // ====================================================================
        private Dictionary<string, object> ExtractSection(Section sec)
        {
            var fmt = sec.SectionFormat;

            var result = new Dictionary<string, object>
            {
                ["Name"] = sec.Name,
                ["Kind"] = sec.Kind.ToString(),
                ["Height"] = sec.Height,

                // SectionFormat flags supported by ENGINE
                ["Suppress"] = fmt.EnableSuppress,
                ["KeepTogether"] = fmt.EnableKeepTogether,
                ["PrintAtBottom"] = fmt.EnablePrintAtBottomOfPage,
                ["UnderlayFollowingSection"] = fmt.EnableUnderlaySection,
                ["BackgroundColor"] = fmt.BackgroundColor.ToString(),

                // ⭐ ADDED — ENGINE supports these two flags
                ["NewPageBefore"] = fmt.EnableNewPageBefore,
                ["NewPageAfter"] = fmt.EnableNewPageAfter,

                // ENGINE cannot extract conditional formulas → return empty
                ["XPositionFormula"] = "",
                ["YPositionFormula"] = "",
                ["SuppressFormula"] = "",
                ["VisibilityFormula"] = "",

                // No RAS conditional formatting
                ["ConditionalFormatting"] = new List<Dictionary<string, string>>()
            };

            // Extract objects inside the section
            var objList = new List<Dictionary<string, object>>();

            foreach (ReportObject obj in sec.ReportObjects)
            {
                var objData = _objectExtractor.ExtractObject(obj, sec);
                objData["Formatting"] = _formatExtractor.ExtractFormatting(obj);
                objList.Add(objData);
            }

            result["Objects"] = objList;

            return result;
        }
    }
}
