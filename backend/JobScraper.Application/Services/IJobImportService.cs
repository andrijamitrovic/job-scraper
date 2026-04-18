using JobScraper.Application.DTOs;

namespace JobScraper.Application.Services;

public interface IJobImportService
{
    Task<IReadOnlyList<JobImportResult>> ImportAllAsync(
        CancellationToken cancellationToken);
}
