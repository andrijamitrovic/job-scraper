namespace JobScraper.Application.Exceptions;

public class DuplicateJobPostingsException : Exception
{
    public string Url {get;}
    public DuplicateJobPostingsException(string url) 
        : base($"A job posting with the URL '{url}' already exists.")
    {
        Url = url;
    }
}