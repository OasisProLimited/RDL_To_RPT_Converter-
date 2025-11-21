import React, { useState } from "react";
import { uploadRPTFile } from "../api/conversionApi";
import ConversionProgress from "./ConversionProgress";
import "./FileUploader.css";

const FileUploader = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [progress, setProgress] = useState(0);
  const [status, setStatus] = useState("");

  const handleFileChange = (e) => {
    const file = e.target.files[0];

    if (file && file.name.endsWith(".rpt")) {
      setSelectedFile(file);
      setStatus("");
    } else {
      setStatus("Invalid file. Please upload a .rpt file.");
      setSelectedFile(null);
    }
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      setStatus("No file selected.");
      return;
    }

    setStatus("Uploading...");
    setProgress(0);

    try {
      const response = await uploadRPTFile(selectedFile, setProgress);

      const blob = new Blob([response.data], { type: "application/xml" });
      const downloadURL = URL.createObjectURL(blob);

      const link = document.createElement("a");
      link.href = downloadURL;
      link.download = selectedFile.name.replace(".rpt", ".xml");
      link.click();

      URL.revokeObjectURL(downloadURL);
      setStatus("Conversion Successful!");
    } catch (err) {
      console.error(err);
      setStatus("Conversion Failed. Please try again.");
    }
  };

  return (
    <div className="uploader-wrapper">
      <div className="uploader-card animate-fade-up">

        <h2 className="uploader-title">RPT → XML Converter</h2>
        <p className="uploader-subtitle">
          Upload a Crystal Report (.rpt) and convert instantly.
        </p>

        <input
          type="file"
          accept=".rpt"
          onChange={handleFileChange}
          className="file-input"
        />

        {selectedFile && (
          <p className="selected-file">
            Selected: <strong>{selectedFile.name}</strong>
          </p>
        )}

        <button className="upload-btn" onClick={handleUpload}>
          Upload & Convert
        </button>

        {status && <p className="status-text">{status}</p>}

        {progress > 0 && progress < 100 && (
          <ConversionProgress progress={progress} />
        )}
      </div>
    </div>
  );
};

export default FileUploader;
