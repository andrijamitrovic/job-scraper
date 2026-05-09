using JobScraper.Application.DTOs;
using JobScraper.Application.Exceptions;
using JobScraper.Application.Interfaces;
using JobScraper.Application.Models;

namespace JobScraper.Application.Services;

public class JobApplicationService : IJobApplicationService
{
    private readonly IJobApplicationRepository _jobApplicationRepository;
    private readonly IJobPostingRepository _jobPostingRepository;

    public JobApplicationService(IJobApplicationRepository jobApplicationRepository, IJobPostingRepository jobPostingRepository)
    {
        _jobApplicationRepository = jobApplicationRepository;
        _jobPostingRepository = jobPostingRepository;
    }

    public async Task<JobApplication?> GetByJobIdAsync(Guid jobId, CancellationToken cancellationToken)
    {
        var job = await _jobPostingRepository.GetJobAsync(jobId, cancellationToken);

        if (job == null)
        {
            throw new JobNotFoundException(jobId);
        }

        return await _jobApplicationRepository.GetByJobIdAsync(jobId, cancellationToken);
    }

    public async Task<JobApplication> UpdateAsync(
    Guid jobId,
    UpdateJobApplicationRequest request,
    CancellationToken cancellationToken)
    {
        var job = await _jobPostingRepository.GetJobAsync(jobId, cancellationToken);

        if (job == null)
        {
            throw new JobNotFoundException(jobId);
        }

        if (request.Status == ApplicationStatus.NotApplied)
        {
            request.AppliedAt = null;
            request.InterviewAt = null;
            request.RejectedAt = null;
        }

        if (request.Status == ApplicationStatus.Applied && request.AppliedAt == null)
        {
            request.AppliedAt = DateTime.UtcNow;
        }

        if (request.Status == ApplicationStatus.Interview && request.InterviewAt == null)
        {
            request.InterviewAt = DateTime.UtcNow;
        }

        if (request.Status == ApplicationStatus.Rejected && request.RejectedAt == null)
        {
            request.RejectedAt = DateTime.UtcNow;
        }

        return await _jobApplicationRepository.UpdateAsync(jobId, request, cancellationToken);
    }

}