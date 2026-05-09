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
    private readonly IJobApplicationService _jobApplicationService;

    public JobsController(IJobService jobService, IJobApplicationService jobApplicationService)
    {
        _jobService = jobService;
        _jobApplicationService = jobApplicationService;
    }

    [HttpPost]
    public async Task<ActionResult<JobPosting>> AddJob(CreateJobPostingRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var jobPosting = await _jobService.AddJobAsync(request, cancellationToken);

            return CreatedAtAction(nameof(GetJob), new { id = jobPosting.Id }, jobPosting);
        }
        catch (DuplicateJobPostingException exception)
        {
            return Conflict(new
            {
                message = exception.Message,
                url = exception.Url
            });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobPosting>> GetJob(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var job = await _jobService.GetJobAsync(id, cancellationToken);

            return Ok(job);
        }
        catch (JobNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message,
                id = exception.Id
            });
        }
    }

    [HttpGet]
    public async Task<ActionResult<PagedResult<JobPosting>>> GetPagedJobs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var jobs = await _jobService.GetPagedAsync(page, pageSize, cancellationToken);

        return Ok(jobs);
    }

    [HttpGet("{id}/application")]
    public async Task<ActionResult<JobApplication>> GetApplication(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var application = await _jobApplicationService.GetByJobIdAsync(id, cancellationToken);

            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }
        catch (JobNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message,
                id = exception.Id
            });
        }
    }

    [HttpPut("{id}/application")]
    public async Task<ActionResult<JobApplication>> UpdateApplication(
        Guid id,
        UpdateJobApplicationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var application = await _jobApplicationService.UpdateAsync(id, request, cancellationToken);

            return Ok(application);
        }
        catch (JobNotFoundException exception)
        {
            return NotFound(new
            {
                message = exception.Message,
                id = exception.Id
            });
        }
    }

}