using JobScraper.Application.DTOs;
using JobScraper.Application.Exceptions;
using JobScraper.Application.Interfaces;

namespace JobScraper.Application.Services;

public class JobImportService : IJobImportService
{
    private readonly IEnumerable<IJobSourceScraper> _scrapers;
    private readonly IJobService _jobService;

    public JobImportService(IEnumerable<IJobSourceScraper> scrapers, IJobService jobService)
    {
        _scrapers = scrapers;
        _jobService = jobService;
    }

    public async Task<IReadOnlyList<JobImportResult>> ImportAllAsync(CancellationToken cancellationToken)
    {
        var results = new List<JobImportResult>();

        foreach(IJobSourceScraper scraper in _scrapers)
        {
            JobImportResult result = await ImportAsync(scraper, cancellationToken);

            results.Add(result);
        }

        return results;
    }

    private async Task<JobImportResult> ImportAsync(IJobSourceScraper scraper, CancellationToken cancellationToken)
    {
        IReadOnlyList<ScrapedJobPosting> scrapedJobs = await scraper.ScrapeAsync(cancellationToken);

        var result = new JobImportResult
        {
            SourceName = scraper.SourceName,
            FoundCount = scrapedJobs.Count
        };

        foreach(var scrapedJob in scrapedJobs)
        {
            try
            {
                await _jobService.AddJobAsync(new CreateJobPostingRequest
                                                {
                                                    Source = scrapedJob.Source,
                                                    Company = scrapedJob.Company,
                                                    Title = scrapedJob.Title,
                                                    Url = scrapedJob.Url
                                                }, cancellationToken);

                result.AddedCount++;
            }
            catch (DuplicateJobPostingException)
            {
                result.SkippedDuplicateCount++;
            }
        }

        return result;
    }
}