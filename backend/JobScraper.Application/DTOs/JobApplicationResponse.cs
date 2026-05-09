using JobScraper.Application.Models;

namespace JobScraper.Application.DTOs;

public class JobApplicationResponse
{
    public Guid Id { get; set; }
    public Guid JobPostingId { get; set; }
    public ApplicationStatus Status { get; set; }
    public DateTime? AppliedAt { get; set; }
    public DateTime? InterviewAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public bool AppliedHereBefore { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
