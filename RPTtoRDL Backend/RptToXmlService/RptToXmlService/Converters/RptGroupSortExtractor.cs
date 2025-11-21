using System;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY GROUP & SORT EXTRACTOR
    /// Limitations:
    ///  - Group header/footer sections cannot be mapped in Engine API.
    ///  - No SectionKind, No GroupNumber, No RAS properties.
    /// Provides:
    ///  ✔ Group condition fields
    ///  ✔ Synthetic GroupIndex (1..n)
    ///  ✔ Sort fields
    /// </summary>
    public class RptGroupSortExtractor
    {
        // =====================================================================
        // MAIN ENTRY
        // =====================================================================
        public Dictionary<string, object> ExtractGroupAndSortInfo(ReportDocument rpt)
        {
            return new Dictionary<string, object>
            {
                ["Groups"] = ExtractGroups(rpt),
                ["Sorts"] = ExtractSorts(rpt)
            };
        }

        // =====================================================================
        // GROUPS (ENGINE SAFE)
        // =====================================================================
        private List<Dictionary<string, object>> ExtractGroups(ReportDocument rpt)
        {
            var list = new List<Dictionary<string, object>>();

            try
            {
                Groups groups = rpt.DataDefinition.Groups;

                for (int i = 0; i < groups.Count; i++)
                {
                    Group g = groups[i];

                    var info = new Dictionary<string, object>
                    {
                        ["GroupIndex"] = i + 1,                               // synthetic only
                        ["ConditionField"] = g.ConditionField?.FormulaName,    // only valid property
                        ["HeaderSection"] = "Not Available in Engine API",
                        ["FooterSection"] = "Not Available in Engine API"
                    };

                    list.Add(info);
                }
            }
            catch
            {
                // ignore engine-only limitations
            }

            return list;
        }

        // =====================================================================
        // SORT FIELDS (ENGINE SAFE)
        // =====================================================================
        private List<Dictionary<string, object>> ExtractSorts(ReportDocument rpt)
        {
            var list = new List<Dictionary<string, object>>();

            try
            {
                SortFields sorts = rpt.DataDefinition.SortFields;

                for (int i = 0; i < sorts.Count; i++)
                {
                    SortField sf = sorts[i];

                    list.Add(new Dictionary<string, object>
                    {
                        ["SortIndex"] = i + 1,
                        ["FieldName"] = sf.Field?.FormulaName,
                        ["SortDirection"] = sf.SortDirection.ToString()
                    });
                }
            }
            catch
            {
                // ignore
            }

            return list;
        }
    }
}
