namespace JobScraper.Application.DTOs;

public class JobImportResult
{
    public string SourceName { get; set; } = string.Empty;

    public int FoundCount { get; set; }

    public int AddedCount { get; set; }

    public int SkippedDuplicateCount { get; set; }
}
