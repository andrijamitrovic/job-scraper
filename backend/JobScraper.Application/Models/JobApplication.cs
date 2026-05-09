namespace JobScraper.Application.Models;

public class JobApplication
{
    public Guid Id { get; set; }

    public Guid JobPostingId { get; set; }
    public JobPosting JobPosting { get; set; } = null!;

    public ApplicationStatus Status { get; set; } = ApplicationStatus.Applied;

    public DateTime? AppliedAt { get; set; }
    public DateTime? InterviewAt { get; set; }
    public DateTime? RejectedAt { get; set; }

    public bool AppliedHereBefore { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
