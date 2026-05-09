using JobScraper.Application.DTOs;
using JobScraper.Application.Models;
using JobScraper.Application.Interfaces;
using JobScraper.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace JobScraper.Repository.Repositories;

public class JobApplicationRepository : IJobApplicationRepository
{

    private readonly JobScraperDbContext _dbContext;

    public JobApplicationRepository(JobScraperDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<JobApplication?> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken)
    {
        return await _dbContext.JobApplications.FirstOrDefaultAsync(a => a.JobPostingId == jobId, cancellationToken);
    }

    public async Task<JobApplication> UpdateAsync(Guid jobId, UpdateJobApplicationRequest request, CancellationToken cancellationToken)
    {
        var updatedJobApplication = await _dbContext.JobApplications.FirstOrDefaultAsync(application => application.JobPostingId == jobId, cancellationToken);
        if (updatedJobApplication == null)
        {
            updatedJobApplication = new JobApplication
            {
                Id = Guid.NewGuid(),
                JobPostingId = jobId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.JobApplications.AddAsync(updatedJobApplication, cancellationToken);
        }

        updatedJobApplication.Status = request.Status;
        updatedJobApplication.AppliedAt = request.AppliedAt;
        updatedJobApplication.InterviewAt = request.InterviewAt;
        updatedJobApplication.RejectedAt = request.RejectedAt;
        updatedJobApplication.AppliedHereBefore = request.AppliedHereBefore;
        updatedJobApplication.Notes = request.Notes;
        updatedJobApplication.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return updatedJobApplication;
    }
}