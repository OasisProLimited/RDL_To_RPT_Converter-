using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using RptToXmlService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace RptToXmlService.Converters
{
    /// <summary>
    /// ENGINE-ONLY DATA EXTRACTOR
    /// Extracts Summary Info, Database Tables, Basic Fields and Dataset.
    /// NO RAS API is used.
    /// </summary>
    public class RptDataExtractor
    {
        // ================================================================
        // SUMMARY INFO
        // ================================================================
        public RptSummaryInfo ExtractSummaryInfo(ReportDocument rpt)
        {
            return new RptSummaryInfo
            {
                Title = rpt.SummaryInfo.ReportTitle,
                Author = rpt.SummaryInfo.ReportAuthor,
                Comments = rpt.SummaryInfo.ReportComments
            };
        }

        // ================================================================
        // DATABASE TABLES (ENGINE-ONLY)
        // ================================================================
        public List<RptTableInfo> ExtractDatabase(ReportDocument rpt)
        {
            List<RptTableInfo> tables = new List<RptTableInfo>();

            foreach (Table tbl in rpt.Database.Tables)
            {
                var ci = tbl.LogOnInfo.ConnectionInfo;

                tables.Add(new RptTableInfo
                {
                    TableName = tbl.Name,
                    Location = tbl.Location,
                    Server = ci.ServerName,
                    Database = ci.DatabaseName,
                    UserId = ci.UserID,
                    Provider = ci.Type.ToString(),
                    Fields = new List<RptFieldInfo>()   // filled later
                });
            }

            return tables;
        }

        // ================================================================
        // DATABASE FIELDS (ENGINE-ONLY)
        // RAS exposes full metadata, but ENGINE provides limited info.
        // We extract what ENGINE supports.
        // ================================================================
        public List<RptFieldInfo> ExtractDatabaseFields(ReportDocument rpt)
        {
            List<RptFieldInfo> fields = new List<RptFieldInfo>();

            foreach (Table tbl in rpt.Database.Tables)
            {
                foreach (FieldDefinition field in tbl.Fields)
                {
                    fields.Add(new RptFieldInfo
                    {
                        Name = field.Name,
                        NativeName = field.FormulaName,
                        DataType = field.ValueType.ToString(),
                        Length = 0 // ENGINE does not expose length
                    });
                }
            }

            return fields;
        }

        // ================================================================
        // TABLE JOINS — ENGINE DOES NOT EXPOSE JOIN METADATA
        // We return an empty list safely.
        // ================================================================
        public List<RptJoinInfo> ExtractTableJoins(ReportDocument rpt)
        {
            return new List<RptJoinInfo>();
        }

        // ================================================================
        // FULL DATASET EXPORT using ENGINE (XML only)
        // ================================================================
        public string ExtractDataSet(ReportDocument rpt)
        {
            try
            {
                string temp = Path.GetTempFileName();

                rpt.ExportToDisk(ExportFormatType.Xml, temp);

                string xml = File.ReadAllText(temp);

                File.Delete(temp);

                return xml;
            }
            catch (Exception ex)
            {
                return "<Dataset>Error: " + ex.Message + "</Dataset>";
            }
        }
    }
}
