namespace JobScraper.Application.Exceptions;

public class DuplicateJobPostingException : Exception
{
    public string Url { get; }
    public DuplicateJobPostingException(string url)
        : base($"A job posting with the URL '{url}' already exists.")
    {
        Url = url;
    }
}