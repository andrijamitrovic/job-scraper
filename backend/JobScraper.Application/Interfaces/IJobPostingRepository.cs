using JobScraper.Application.Models;

namespace JobScraper.Application.Interfaces;

public interface IJobPostingRepository
{
    Task<IReadOnlyList<JobPosting>> GetAllAsync( CancellationToken cancellationToken);
    
    Task<bool> ExistsByUrlAsync(string url, CancellationToken cancellationToken);

    Task AddAsync(JobPosting jobPosting, CancellationToken cancellationToken);
}