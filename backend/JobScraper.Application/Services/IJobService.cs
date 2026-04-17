using JobScraper.Application.Models;

namespace JobScraper.Application.Services;

public interface IJobService
{
    Task<IReadOnlyList<JobPosting>> GetJobsAsync(CancellationToken cancellationToken);

    Task AddJobAsync(JobPosting jobPosting, CancellationToken cancellationToken);
}