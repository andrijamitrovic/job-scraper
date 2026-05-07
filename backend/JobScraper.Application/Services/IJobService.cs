using JobScraper.Application.DTOs;
using JobScraper.Application.Models;

namespace JobScraper.Application.Services;

public interface IJobService
{
    Task<IReadOnlyList<JobPosting>> GetJobsAsync(CancellationToken cancellationToken);

    Task<JobPosting> AddJobAsync(CreateJobPostingRequest request, CancellationToken cancellationToken);
    Task<JobPosting> GetJobAsync(Guid id, CancellationToken cancellationToken);

    Task<PagedResult<JobPosting>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
}