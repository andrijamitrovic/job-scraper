using JobScraper.Application.DTOs;
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
        return await _dbContext.JobPostings
            .Include(jobPosting => jobPosting.Application)
            .FirstOrDefaultAsync(jobPosting => jobPosting.Id == id, cancellationToken);
    }

    public async Task<PagedResult<JobPosting>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var totalCount = await _dbContext.JobPostings.CountAsync(cancellationToken);

        var jobs = await _dbContext.JobPostings
            .Include(job => job.Application)
            .OrderByDescending(job => job.ScrapedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<JobPosting>
        {
            Items = jobs,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
        };
    }
}