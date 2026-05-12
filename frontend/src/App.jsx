import { useEffect, useState } from "react";
import "./index.css";
import { Button } from "@/components/ui/button";
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import {
  Select,
  SelectContent,
  SelectTrigger,
  SelectValue,
  SelectItem,
} from "@/components/ui/select";

export default function App() {
  const API_BASE_URL = import.meta.env.VITE_API_BASE_URL ?? "";
  const [jobs, setJobs] = useState([]);
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState("");
  const [isImporting, setIsImporting] = useState(false);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);

  async function importJobs() {
    setIsImporting(true);
    setError("");

    try {
      const response = await fetch(API_BASE_URL + "/api/job-imports/run", {
        method: "POST",
      });

      if (!response.ok) {
        throw new Error("Failed to import jobs.");
      }

      await response.json();

      const firstPage = 1;
      setPage(firstPage);

      const jobsResponse = await fetch(
        `${API_BASE_URL}/api/jobs?page=${firstPage}&pageSize=${pageSize}`,
      );

      if (!jobsResponse.ok) {
        throw new Error("Failed to load jobs.");
      }

      const freshPage = await jobsResponse.json();

      setJobs(freshPage.items);
      setTotalPages(freshPage.totalPages);
      setTotalCount(freshPage.totalCount);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsImporting(false);
    }
  }

  async function updateApplicationStatus(jobId, status) {
    const response = await fetch(
      `${API_BASE_URL}/api/jobs/${jobId}/application`,
      {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          status,
          appliedAt: status === "Applied" ? new Date().toISOString() : null,
          interviewAt: status === "Interview" ? new Date().toISOString() : null,
          rejectedAt: status === "Rejected" ? new Date().toISOString() : null,
          appliedHereBefore: false,
          notes: "",
        }),
      },
    );

    if (!response.ok) {
      throw new Error("Failed to update application status.");
    }

    await loadJobs(page);
  }

  async function loadJobs(pageToLoad = page) {
    setIsLoading(true);
    setError("");

    try {
      const response = await fetch(
        `${API_BASE_URL}/api/jobs?page=${pageToLoad}&pageSize=${pageSize}`,
      );

      const data = await response.json();

      setJobs(data.items);
      setTotalPages(data.totalPages);
      setTotalCount(data.totalCount);
    } catch (error) {
      setError(error.message);
    } finally {
      setIsLoading(false);
    }
  }

  useEffect(() => {
    loadJobs();
  }, [page]);

  return (
    <main className="min-h-screen bg-background px-4 py-8 text-foreground">
      <div className="mx-auto flex w-full max-w-4xl flex-col gap-6">
        <header className="flex flex-col gap-4 border-b pb-6 sm:flex-row sm:items-end sm:justify-between">
          <div className="space-y-1">
            <h1 className="text-3xl font-semibold tracking-tight">
              Job Scraper
            </h1>
            <p className="text-sm text-muted-foreground">
              {totalCount} jobs saved
            </p>
          </div>
          <Button
            onClick={importJobs}
            disabled={isImporting}
            className="w-full sm:w-auto"
          >
            {isImporting ? "Importing..." : "Import jobs"}
          </Button>
        </header>

        {isLoading && (
          <p className="text-sm text-muted-foreground">Loading jobs...</p>
        )}

        {error && (
          <p className="rounded-lg border border-destructive/30 bg-destructive/10 px-3 py-2 text-sm text-destructive">
            {error}
          </p>
        )}

        <section className="flex flex-col gap-4">
          {jobs.map((job) => (
            <Card key={job.id}>
              <CardHeader>
                <div className="flex items-start justify-between gap-4">
                  <div className="space-y-1">
                    <CardTitle>{job.title}</CardTitle>
                    <CardDescription>{job.company}</CardDescription>
                  </div>

                  <div className="flex shrink-0 flex-col items-end gap-1 text-xs text-muted-foreground">
                    <span>{job.source}</span>
                    <span>
                      {new Date(job.scrapedAt).toISOString().split("T")[0]}
                    </span>
                  </div>
                </div>
              </CardHeader>

              <CardContent>
                <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:justify-between">
                  <a
                    href={job.url}
                    target="_blank"
                    rel="noreferrer"
                    className="text-sm font-medium text-primary underline-offset-4 hover:underline"
                  >
                    Open Job
                  </a>
                  <div className="flex flex-col gap-2 sm:flex-row sm:items-center sm:gap-3">
                    <label
                      htmlFor={`application-status-${job.id}`}
                      className="text-sm font-medium text-muted-foreground"
                    >
                      Status
                    </label>

                    <Select
                      value={job.application?.status ?? "NotApplied"}
                      onValueChange={(value) =>
                        updateApplicationStatus(job.id, value)
                      }
                    >
                      <SelectTrigger
                        id={`application-status-${job.id}`}
                        className="w-full sm:w-[180px]"
                      >
                        <SelectValue placeholder="Select Status" />
                      </SelectTrigger>

                      <SelectContent>
                        <SelectItem value="NotApplied">Not applied</SelectItem>
                        <SelectItem value="Applied">Applied</SelectItem>
                        <SelectItem value="Interview">Interview</SelectItem>
                        <SelectItem value="Rejected">Rejected</SelectItem>
                        <SelectItem value="Offer">Offer</SelectItem>
                        <SelectItem value="Withdrawn">Withdrawn</SelectItem>
                      </SelectContent>
                    </Select>
                  </div>
                </div>
              </CardContent>
            </Card>
          ))}
        </section>
        <div className="flex flex-col gap-3 border-t pt-4 sm:flex-row sm:items-center sm:items-center">
          <span className="order-first text-center text-sm text-muted-foreground sm:order-none">
            Page {page} of {totalPages}
          </span>

          <div className="flex gap-2 sm:order-first">
            <Button
              variant="outline"
              className="flex-1 sm:flex-none"
              disabled={page === 1}
              onClick={() => setPage((currentPage) => currentPage - 1)}
            >
              Previous
            </Button>

            <Button
              variant="outline"
              className="flex-1 sm:flex-none"
              disabled={page === totalPages}
              onClick={() => setPage((currentPage) => currentPage + 1)}
            >
              Next
            </Button>
          </div>
        </div>
      </div>
    </main>
  );
}
