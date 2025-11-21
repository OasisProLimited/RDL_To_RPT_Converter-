// src/components/LogsViewer.jsx
import React, { useEffect, useState } from "react";
import "./LogsViewer.css"; // 👈 import CSS

const LogsViewer = () => {
  const [logs, setLogs] = useState([]);

  useEffect(() => {
    // temporary dummy logs until backend endpoint is connected
    setLogs([
      { id: 1, message: "Report A.rpt converted successfully ✅", time: "3:25 PM" },
      { id: 2, message: "Report B.rpt failed ❌ – invalid structure", time: "3:30 PM" },
    ]);
  }, []);

  return (
    <div className="logs-viewer">
      <h2>Conversion Logs</h2>
      {logs.length === 0 ? (
        <p className="no-logs">No logs available yet.</p>
      ) : (
        <ul>
          {logs.map((log) => (
            <li key={log.id}>
              <span className="log-message">{log.message}</span>
              <span className="log-time">{log.time}</span>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
};

export default LogsViewer;
