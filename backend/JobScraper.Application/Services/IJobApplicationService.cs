using JobScraper.Application.DTOs;
using JobScraper.Application.Models;

namespace JobScraper.Application.Services;

public interface IJobApplicationService
{
    Task<JobApplication> UpdateAsync(Guid jobId, UpdateJobApplicationRequest request, CancellationToken cancellationToken);
    Task<JobApplication?> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken);
}
