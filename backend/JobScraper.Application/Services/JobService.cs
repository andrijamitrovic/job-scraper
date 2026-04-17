using JobScraper.Application.DTOs;
using JobScraper.Application.Exceptions;
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

    public async Task<JobPosting> AddJobAsync(CreateJobPostingRequest request, CancellationToken cancellationToken)
    {
        var exists = await _jobPostingRepository.ExistsByUrlAsync(request.Url, cancellationToken);

        if(exists)
        {
            throw new DuplicateJobPostingException(request.Url);
        }

        var jobPosting = new JobPosting
        {
            Id = Guid.NewGuid(),
            Source = request.Source,
            Company = request.Company, 
            Title = request.Title, 
            Url = request.Url,
            ScrapedAt = DateTime.UtcNow
        };

        await _jobPostingRepository.AddAsync(jobPosting, cancellationToken);
        
        return jobPosting;
    }

    public async Task<JobPosting> GetJobAsync(Guid id, CancellationToken cancellationToken)
    {
        var job = await _jobPostingRepository.GetJobAsync(id, cancellationToken);

        if(job == null)
        {
            throw new JobNotFoundException(id);
        }
        else
        {
            return job;
        }
    }
}