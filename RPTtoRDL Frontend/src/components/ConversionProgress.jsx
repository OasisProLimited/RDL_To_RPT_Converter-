// src/components/ConversionProgress.jsx
import React from "react";
import "./ConversionProgress.css"; //  import its own style file

const ConversionProgress = ({ progress }) => {
  return (
    <div className="progress-container">
      <div className="progress-bar" style={{ width: `${progress}%` }}></div>
      <p className="progress-text">{progress}%</p>
    </div>
  );
};

export default ConversionProgress;
