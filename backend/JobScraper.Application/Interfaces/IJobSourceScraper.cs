using JobScraper.Application.DTOs;

namespace JobScraper.Application.Interfaces;

public interface IJobSourceScraper
{
    string SourceName { get; }

    Task<IReadOnlyList<ScrapedJobPosting>> ScrapeAsync(CancellationToken cancellationToken);
}