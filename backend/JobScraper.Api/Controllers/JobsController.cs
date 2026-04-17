using JobScraper.Application.DTOs;
using JobScraper.Application.Exceptions;
using JobScraper.Application.Models;
using JobScraper.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobScraper.Api.Controllers;

[ApiController]
[Route("api/jobs")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobService;

    public JobsController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<JobPosting>>> GetJobs(CancellationToken cancellationToken)
    {
        var jobs = await _jobService.GetJobsAsync(cancellationToken);

        return Ok(jobs);
    }

    [HttpPost]
    public async Task<ActionResult<JobPosting>> AddJob(CreateJobPostingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var jobPosting = await _jobService.AddJobAsync(request, cancellationToken);

            return Created(jobPosting.Url, jobPosting);
        }
        catch (DuplicateJobPostingsException exception)
        {
            return Conflict(new
            {
                message = exception.Message,
                url = exception.Url
            });
        }
    }
}