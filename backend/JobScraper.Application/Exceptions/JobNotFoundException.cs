namespace JobScraper.Application.Exceptions;

public class JobNotFoundException : Exception
{
    public Guid Id { get; }
    public JobNotFoundException(Guid id)
        : base($"A job posting with the id '{id}' does not exist.")
    {
        Id = id;
    }
}