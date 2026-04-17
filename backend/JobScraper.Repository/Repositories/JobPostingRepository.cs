using JobScraper.Application.Interfaces;
using JobScraper.Application.Models;
using JobScraper.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace JobScraper.Repository.Repositories;

public class JobPostingRepository : IJobPostingRepository
{
    private readonly JobScraperDbContext _dbContext;

    public JobPostingRepository(JobScraperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<JobPosting>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.JobPostings
            .OrderByDescending(jobPosting => jobPosting.ScrapedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByUrlAsync(string url, CancellationToken cancellationToken)
    {
        return await _dbContext.JobPostings
            .AnyAsync(jobPosting => jobPosting.Url == url, cancellationToken);
    }

    public async Task AddAsync(JobPosting jobPosting, CancellationToken cancellationToken)
    {
        _dbContext.JobPostings.Add(jobPosting);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<JobPosting?> GetJobAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _dbContext.JobPostings.FirstOrDefaultAsync(jobPosting => jobPosting.Id == id, cancellationToken);
    }
}