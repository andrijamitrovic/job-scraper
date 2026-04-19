import { useEffect, useMemo, useState } from "react";
import "./index.css";

export default function App() {
  const [jobs, setJobs] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const [isImporting, setIsImporting] = useState(false);
  const [importResult, setImportResult] = useState([]);
  const [newJobIds, setNewJobIds] = useState(new Set());

  async function importJobs() {
    setIsImporting(true);
    setError("");

    const beforeIds = new Set(jobs.map((job) => job.id));

    try {
      const response = await fetch("/api/job-imports/run", {
        method: "POST"
      });

      if (!response.ok) {
        throw new Error("Failed to import jobs.");
      }

      const result = await response.json();
      setImportResult(result);

      const jobsResponse = await fetch("/api/jobs");
      const freshJobs = await jobsResponse.json();

      setJobs(freshJobs);
      setNewJobIds(
        new Set(
          freshJobs
            .filter((job) => !beforeIds.has(job.id))
            .map((job) => job.id)
        )
      );
    } catch (error) {
      setError(error.message);
    } finally {
      setIsImporting(false);
    }
  }


  async function loadJobs() {
    setIsLoading(true);
    setError("");

    try {
      const response = await fetch("/api/jobs");

      if(!response.ok) {
        throw new Error("Failed to load jobs.");
      }

      const data = await response.json();
      setJobs(data);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    loadJobs();
  }, [])

  return (
    <main className="app">
      <header className="header">
        <h1>Job Scraper</h1>
        <p>{jobs.length} jobs saved</p>
      </header>
      <button onClick={importJobs} disabled={isImporting} className="button">
        {isImporting ? "Importing..." : "Import jobs"}
      </button>


      {isLoading && <p>Loading jobs...</p>}

      {error && <p className="error">{error}</p>}

      {importResult.length > 0 && (
        <section className="import-summary">
          {importResult.map((result) => (
            <div key={result.sourceName}>
              <strong>{result.sourceName}:  </strong>
              <span>{result.foundCount} found  </span>
              <span>{result.addedCount} added </span>
              <span>{result.skippedDuplicateCount} duplicates</span>
            </div>
          ))}
        </section>
      )}


      <section className="jobs">
        {jobs.map((job) => (
          <article key={job.id} className="job-card">
            <div className="job-meta">
              {newJobIds.has(job.id) && <span className="new-badge">New</span>}
              <span>{job.source}</span>
              <span>{new Date(job.scrapedAt).toISOString().split("T")[0]}</span>
            </div>

            <h2>{job.title}</h2>
            <p>{job.company}</p>

            <a href={job.url} target="_blank" rel="noreferrer">
              Open job
            </a>
          </article>
        ))}
      </section>
    </main>
  );
}
