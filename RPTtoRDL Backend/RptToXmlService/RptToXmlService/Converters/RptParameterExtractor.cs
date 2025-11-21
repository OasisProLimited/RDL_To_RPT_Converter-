using System;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// Engine-only parameter extractor.
    /// Supports:
    /// ✔ discrete values
    /// ✔ range values
    /// ✔ default + current values
    /// ✔ masks & parameter type
    /// 
    /// No RAS API → cascading, display values and descriptions are NOT available.
    /// </summary>
    public class RptParameterExtractor
    {
        // ======================================================================
        // MAIN ENTRY — Extract all parameters
        // ======================================================================
        public List<Dictionary<string, object>> ExtractParameters(ReportDocument rpt)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (ParameterFieldDefinition p in rpt.DataDefinition.ParameterFields)
            {
                // ENGINE–only way to skip system parameters
                if (p.ReportName != rpt.Name)
                    continue;

                var dict = new Dictionary<string, object>
                {
                    ["Name"] = p.Name,
                    ["PromptText"] = p.PromptText,
                    ["ValueType"] = p.ValueType.ToString(),
                    ["ParameterType"] = p.ParameterType.ToString(),
                    ["AllowMultipleValues"] = p.EnableAllowMultipleValue,
                    ["EditMask"] = p.EditMask ?? "",

                    // Values
                    ["DefaultValues"] = ExtractDiscreteDefaultValues(p),
                    ["DefaultRanges"] = ExtractRangeDefaultValues(p),
                    ["CurrentValues"] = ExtractDiscreteCurrentValues(p),
                    ["CurrentRanges"] = ExtractRangeCurrentValues(p)
                };

                list.Add(dict);
            }

            return list;
        }

        // ======================================================================
        // DEFAULT DISCRETE VALUES
        // ======================================================================
        private List<object> ExtractDiscreteDefaultValues(ParameterFieldDefinition p)
        {
            var arr = new List<object>();

            try
            {
                foreach (ParameterValue v in p.DefaultValues)
                {
                    if (v is ParameterDiscreteValue dv)
                        arr.Add(dv.Value);
                }
            }
            catch { }

            return arr;
        }

        // ======================================================================
        // DEFAULT RANGE VALUES
        // ======================================================================
        private List<Dictionary<string, object>> ExtractRangeDefaultValues(ParameterFieldDefinition p)
        {
            var arr = new List<Dictionary<string, object>>();

            try
            {
                foreach (ParameterValue v in p.DefaultValues)
                {
                    if (v is ParameterRangeValue rv)
                    {
                        arr.Add(new Dictionary<string, object>
                        {
                            ["StartValue"] = rv.StartValue,
                            ["EndValue"] = rv.EndValue,
                            ["LowerBoundType"] = rv.LowerBoundType.ToString(),
                            ["UpperBoundType"] = rv.UpperBoundType.ToString()
                        });
                    }
                }
            }
            catch { }

            return arr;
        }

        // ======================================================================
        // CURRENT DISCRETE VALUES
        // ======================================================================
        private List<object> ExtractDiscreteCurrentValues(ParameterFieldDefinition p)
        {
            var arr = new List<object>();

            try
            {
                foreach (ParameterValue v in p.CurrentValues)
                {
                    if (v is ParameterDiscreteValue dv)
                        arr.Add(dv.Value);
                }
            }
            catch { }

            return arr;
        }

        // ======================================================================
        // CURRENT RANGE VALUES
        // ======================================================================
        private List<Dictionary<string, object>> ExtractRangeCurrentValues(ParameterFieldDefinition p)
        {
            var arr = new List<Dictionary<string, object>>();

            try
            {
                foreach (ParameterValue v in p.CurrentValues)
                {
                    if (v is ParameterRangeValue rv)
                    {
                        arr.Add(new Dictionary<string, object>
                        {
                            ["StartValue"] = rv.StartValue,
                            ["EndValue"] = rv.EndValue,
                            ["LowerBoundType"] = rv.LowerBoundType.ToString(),
                            ["UpperBoundType"] = rv.UpperBoundType.ToString()
                        });
                    }
                }
            }
            catch { }

            return arr;
        }
    }
}
