using JobScraper.Application.Models;

namespace JobScraper.Application.DTOs;

public class UpdateJobApplicationRequest
{
    public ApplicationStatus Status { get; set; }
    public DateTime? AppliedAt { get; set; }
    public DateTime? InterviewAt { get; set; }
    public DateTime? RejectedAt { get; set; }
    public bool AppliedHereBefore { get; set; }
    public string? Notes { get; set; }
}
