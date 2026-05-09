using JobScraper.Application.DTOs;
using JobScraper.Application.Models;

namespace JobScraper.Application.Interfaces;

public interface IJobApplicationRepository
{
    Task<JobApplication?> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken);
    Task<JobApplication> UpdateAsync(Guid jobId, UpdateJobApplicationRequest request, CancellationToken cancellationToken);
}
