namespace JobScraper.Application.Models;

public class JobPosting
{
    public Guid Id { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;

    public DateTime ScrapedAt { get; set; }
}