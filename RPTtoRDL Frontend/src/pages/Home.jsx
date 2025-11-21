// src/pages/Home.jsx
import React from "react";
import FileUploader from "../components/FileUploader";
// import LogsViewer from "../components/LogsViewer";

const Home = () => {
  return (
    <div className="home-layout">
      <div className="home-column">
        <FileUploader />
      </div>

      <div className="home-column home-column--logs">
        {/* <LogsViewer /> */}
      </div>
    </div>
  );
};

export default Home;
