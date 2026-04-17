using JobScraper.Application.Interfaces;
using JobScraper.Application.Models;

namespace JobScraper.Application.Services;

public class JobService : IJobService
{
    private readonly IJobPostingRepository _jobPostingRepository;

    public JobService(IJobPostingRepository jobPostingRepository)
    {
        _jobPostingRepository = jobPostingRepository;
    }    

    public Task<IReadOnlyList<JobPosting>> GetJobsAsync(CancellationToken cancellationToken)
    {
        return _jobPostingRepository.GetAllAsync(cancellationToken);
    }

    public Task AddJobAsync(JobPosting jobPosting, CancellationToken cancellationToken)
    {
        jobPosting.Id = Guid.NewGuid();
        jobPosting.ScrapedAt = DateTime.UtcNow;

        return _jobPostingRepository.AddAsync(jobPosting, cancellationToken);
    }
}