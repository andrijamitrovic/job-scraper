using System.Text.Json;
using System.Text.RegularExpressions;
using JobScraper.Application.DTOs;
using JobScraper.Application.Interfaces;

namespace JobScraper.Scrapers.Joberty;

public class JobertyJobSourceScraper : IJobSourceScraper
{
    private readonly HttpClient _httpClient;

    private const string Source = "Joberty";
    private const string JobsUrl = "https://backend.joberty.com/api/v1/jobs?page=0&pageSize=10&sort=created_desc";

    private static readonly Uri BaseUri = new("https://www.joberty.com");

    public JobertyJobSourceScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string SourceName => Source;

    public async Task<IReadOnlyList<ScrapedJobPosting>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var jsonText = await _httpClient.GetStringAsync(JobsUrl, cancellationToken);

        using var json = JsonDocument.Parse(jsonText);

        if (!json.RootElement.TryGetProperty("items", out var items) ||
            items.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        var jobs = new List<ScrapedJobPosting>();

        foreach (var item in items.EnumerateArray())
        {
            var title = GetString(item, "jobTitle");
            var company = GetString(item, "companyName");
            var companySlug = GetString(item, "companyUrlName");

            if (!item.TryGetProperty("id", out var idElement) ||
                !idElement.TryGetInt32(out var id) ||
                string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(company))
            {
                continue;
            }

            companySlug = string.IsNullOrWhiteSpace(companySlug)
                ? ToSlug(company)
                : companySlug.Trim();

            var titleSlug = ToSlug(title);

            jobs.Add(new ScrapedJobPosting
            {
                Source = SourceName,
                Company = company.Trim(),
                Title = title.Trim(),
                Url = new Uri(BaseUri, $"/job/{companySlug}/{titleSlug}/{id}").ToString()
            });
        }

        return jobs;
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        return element.TryGetProperty(propertyName, out var property) &&
               property.ValueKind == JsonValueKind.String
            ? property.GetString()
            : null;
    }

    private static string ToSlug(string value)
    {
        var slug = value
            .Replace("#", "")
            .Replace("/", "-")
            .Replace("\\", "-")
            .Trim();

        slug = Regex.Replace(slug, @"\s+", "-");

        return slug.ToLowerInvariant();
    }
}