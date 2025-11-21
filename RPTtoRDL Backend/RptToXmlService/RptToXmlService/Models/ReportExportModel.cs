using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RptToXmlService.Models
{
    /// <summary>
    /// Represents a report export model containing input and output
    /// file paths for different transformation formats such as
    /// XML, JSON, XSD, XSL, and RDL.
    /// </summary>
    public class ReportExportModel
    {
        /// <summary>
        /// Gets or sets the full file path of the input report file (.rpt, .xml, .json).
        /// </summary>
        public string InputFilePath { get; set; }

        /// <summary>
        /// Gets or sets the generated XML file path after RPT conversion.
        /// </summary>
        public string OutputXmlPath { get; set; }

        /// <summary>
        /// Gets or sets the generated JSON file path after RPT conversion.
        /// </summary>
        public string OutputJsonPath { get; set; }

        /// <summary>
        /// Gets or sets the generated XSD (XML Schema Definition) file path.
        /// </summary>
        public string OutputXsdPath { get; set; }

        /// <summary>
        /// Gets or sets the generated XSL (Extensible Stylesheet Language) file path.
        /// </summary>
        public string OutputXslPath { get; set; }

        /// <summary>
        /// Gets or sets the generated RDL (Report Definition Language) file path.
        /// </summary>
        public string OutputRdlPath { get; set; }

        /// <summary>
        /// Gets or sets the name of the report (without extension).
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the target folder where all generated files will be saved.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets an optional description or notes about the report.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the time the export process was executed.
        /// </summary>
        public DateTime ExportedOn { get; set; } = DateTime.Now;

        /// <summary>
        /// Gets or sets a list of any validation or error messages produced during conversion.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
}
