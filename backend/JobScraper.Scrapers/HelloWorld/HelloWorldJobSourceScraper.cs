using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using JobScraper.Application.DTOs;
using JobScraper.Application.Interfaces;

namespace JobScraper.Scrapers.HelloWorld;

public class HelloWorldJobSourceScraper : IJobSourceScraper
{
    private readonly HttpClient _httpClient;
    private const string Source = "HelloWorld";
    private const string JobsUrl = "https://helloworld.rs/oglasi-za-posao?sort=p_vreme_postavljanja_sort";
    private const string SearchResultsSelector = ".__search-results";
    private const string JobTitleSelector = ".__ga4_job_title";
    private const string JobCompanySelector = ".__ga4_job_company";

    private static readonly Uri BaseUri = new("https://helloworld.rs");

    public HelloWorldJobSourceScraper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string SourceName => Source;

    public async Task<IReadOnlyList<ScrapedJobPosting>> ScrapeAsync(CancellationToken cancellationToken)
    {
        var html = await _httpClient.GetStringAsync(JobsUrl, cancellationToken);

        var parser = new HtmlParser();
        var document = await parser.ParseDocumentAsync(html, cancellationToken);

        var jobs = new List<ScrapedJobPosting>();

        if (document.QuerySelector(SearchResultsSelector) is not IElement container)
        {
            return [];
        }

        foreach (var card in container.Children)
        {
            if (!TryExtractTitle(card, out var title, out var href) ||
                !TryExtractCompany(card, out var company))
            {
                continue;
            }

            jobs.Add(new ScrapedJobPosting
            {
                Source = SourceName,
                Company = company,
                Title = title,
                Url = new Uri(BaseUri, href).ToString()
            });
        }

        return jobs;
    }

    private static bool TryExtractTitle(IElement card, out string title, out string href)
    {
        title = string.Empty;
        href = string.Empty;

        var titleLink = card.QuerySelector(JobTitleSelector)
            ?? card.QuerySelector("h3 a[href]");

        if (titleLink is null)
        {
            return false;
        }

        title = titleLink.TextContent.Trim();
        href = titleLink.GetAttribute("href")?.Trim() ?? string.Empty;

        return !string.IsNullOrWhiteSpace(title) &&
               !string.IsNullOrWhiteSpace(href);
    }

    private static bool TryExtractCompany(IElement card, out string company)
    {
        company = string.Empty;

        var companyLink = card.QuerySelector(JobCompanySelector)
            ?? card.QuerySelector("h4 a[data-job-id]")
            ?? card.QuerySelector("h4 a");

        if (companyLink is null)
        {
            return false;
        }

        company = companyLink.TextContent.Trim();

        return !string.IsNullOrWhiteSpace(company);
    }
}
