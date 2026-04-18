using JobScraper.Application.DTOs;
using JobScraper.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace JobScraper.Api.Controllers;

[ApiController]
[Route("api/job-imports")]
public class JobImportsController : ControllerBase
{
    private readonly IJobImportService _jobImportService;

    public JobImportsController(IJobImportService jobImportService)
    {
        _jobImportService = jobImportService;
    }

    [HttpPost("run")]
    public async Task<ActionResult<IReadOnlyList<JobImportResult>>> RunImport(
        CancellationToken cancellationToken)
    {
        var results = await _jobImportService.ImportAllAsync(cancellationToken);

        return Ok(results);
    }
}
