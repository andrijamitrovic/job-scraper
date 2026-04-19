using System.Text.Json;
using AngleSharp.Html.Parser;
using JobScraper.Application.DTOs;
using JobScraper.Application.Interfaces;

namespace JobScraper.Scrapers.Infostud;

public class InfostudJobSourceScraper : IJobSourceScraper
{
    private readonly HttpClient _httpClient;

    private const string Source = "Infostud";
    private const string JobsUrl = "https://poslovi.infostud.com/oglasi-za-posao?category%5B0%5D=5&sort=online_view_date";

    private static readonly Uri BaseUri = new("https://poslovi.infostud.com");

    public InfostudJobSourceScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string SourceName => Source;


    public async Task<IReadOnlyList<ScrapedJobPosting>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var html = await _httpClient.GetStringAsync(JobsUrl, cancellationToken);
        
        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(html, cancellationToken);

        var nextDataScript = document.QuerySelector("script#__NEXT_DATA__");

        if (nextDataScript is null)
        {
            return Array.Empty<ScrapedJobPosting>();
        }

        using var json = JsonDocument.Parse(nextDataScript.TextContent);

        var jobs = json.RootElement
            .GetProperty("props")
            .GetProperty("pageProps")
            .GetProperty("initialSearchResults")
            .GetProperty("jobs")
            .GetProperty("primary");

        var scrapedJobs = new List<ScrapedJobPosting>();

        foreach(var job in jobs.EnumerateArray())
        {
            var title = job.GetProperty("title").GetString();
            var company = job.GetProperty("companyName").GetString();
            var url = job.GetProperty("url").GetString();

            if(string.IsNullOrWhiteSpace(title) || 
                string.IsNullOrWhiteSpace(company) || 
                string.IsNullOrWhiteSpace(url))
            {
                continue;
            }

            scrapedJobs.Add(new ScrapedJobPosting
            {
                Source = SourceName,
                Title = title.Trim(),
                Company = company.Trim(),
                Url = new Uri(BaseUri, url.Trim()).ToString()
            });
        }

        return scrapedJobs;
    }
}