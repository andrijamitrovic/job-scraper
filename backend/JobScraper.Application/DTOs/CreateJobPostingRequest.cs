using System.ComponentModel.DataAnnotations;

namespace JobScraper.Application.DTOs;

public class CreateJobPostingRequest
{
    [Required]
    [MaxLength(100)]
    public string Source { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string Company { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    [Url]
    public string Url { get; set; } = string.Empty;
}