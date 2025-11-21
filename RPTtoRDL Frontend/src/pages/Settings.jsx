// src/pages/Settings.jsx
import React, { useState } from "react";

const Settings = () => {
  // For now this is only UI – not wired to conversionApi yet.
  const [apiBaseUrl, setApiBaseUrl] = useState("http://localhost:5000");

  const handleSubmit = (e) => {
    e.preventDefault();
    // Later you can save this to localStorage or a config file.
    alert(`(Demo only) API base URL saved: ${apiBaseUrl}`);
  };

  return (
    <div className="settings-card">
      <h2 className="settings-title">Settings</h2>
      <p className="settings-subtitle">
        Here you can later control things like backend API URL or environment.
        Right now this is just a UI placeholder.
      </p>

      <form onSubmit={handleSubmit} className="settings-form">
        <label className="settings-label">
          Backend API base URL
          <input
            type="text"
            value={apiBaseUrl}
            onChange={(e) => setApiBaseUrl(e.target.value)}
            className="settings-input"
          />
        </label>

        <button type="submit" className="settings-button">
          Save (demo)
        </button>
      </form>
    </div>
  );
};

export default Settings;
