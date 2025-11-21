import React, { useState, useEffect } from "react";
import "./App.css";
import Home from "./pages/Home";
import Settings from "./pages/Settings";

function App() {
  const [activePage, setActivePage] = useState("home");
  const [theme, setTheme] = useState("ifs-light");

  // Apply theme dynamically
  useEffect(() => {
    document.documentElement.setAttribute("data-theme", theme);
  }, [theme]);

  return (
    <div className="app-root">

      {/* === Header === */}
      <header className="app-header">
        <div className="header-title">
<span className="header-main">
  RPT → RDL Converter
</span>

<span className="header-sub">
  <br />
  Convert Crystal .rpt reports into Oasis Pro Cloud  ready RDL files
</span>

        </div>

        {/* Navigation + Theme Toggle */}
        <nav className="app-nav">
          <button
            className={`nav-link ${activePage === "home" ? "nav-link--active" : ""}`}
            onClick={() => setActivePage("home")}
          >
            Home
          </button>

          <button
            className="nav-link nav-theme"
            onClick={() =>
              setTheme(theme === "ifs-light" ? "ifs-dark" : "ifs-light")
            }
          >
            {theme === "ifs-light" ? "Dark" : "Light"}
          </button>
        </nav>
      </header>

      {/* === Hero Section === */}
      <section className="app-hero">
        <div className="hero-inner">
          <span className="hero-pill">Oasis Pro Cloud Automated Tools</span>
          <h1 className="hero-title">
            Smarter Crystal Reports → RDL Conversion
          </h1>
          <p className="hero-text">
            Automatically transform legacy Crystal Reports into fully Oasis Pro Cloud
            compatible RDL files — fast, accurate, and with zero re-development.
          </p>
        </div>
      </section>

      {/* === Main Content === */}
      <main className="app-main">
        {activePage === "home" ? <Home /> : <Settings />}
      </main>

      {/* === Footer === */}
      <footer className="app-footer">
        Built for Oasis Pro Cloud • Accelerated Reporting Modernization
      </footer>
    </div>
  );
}

export default App;
