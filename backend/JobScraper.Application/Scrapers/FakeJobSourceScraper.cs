using JobScraper.Application.DTOs;
using JobScraper.Application.Interfaces;

namespace JobScraper.Application.Scrapers;

public class FakeJobSourceScraper : IJobSourceScraper
{
    public string SourceName => "Fake";

    public Task<IReadOnlyList<ScrapedJobPosting>> ScrapeAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ScrapedJobPosting> jobs =
        [
            new ScrapedJobPosting
            {
                Source = SourceName, 
                Company = "FishingBooker",
                Title = "Backend Developer",
                Url = "https://example.com/fake/fishingbooker-backend-developer"
            },
            new ScrapedJobPosting
            {
                Source = SourceName,
                Company = "Holycode",
                Title = ".NET Developer",
                Url = "https://example.com/fake/holycode-dotnet-developer"
            }
        ];

        return Task.FromResult(jobs);
    }
}