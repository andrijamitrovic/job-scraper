namespace JobScraper.Application.DTOs;

public class JobPostingResponse
{
    public Guid Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateTime ScrapedAt { get; set; }

    public JobApplicationResponse? Application { get; set; }
}
