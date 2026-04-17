using JobScraper.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace JobScraper.Repository.Data;

public class JobScraperDbContext : DbContext
{
    public JobScraperDbContext(DbContextOptions<JobScraperDbContext> options) : base(options)
    {
        
    }

    public DbSet<JobPosting> JobPostings => Set<JobPosting>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobPosting>()
            .HasIndex(JobPosting => JobPosting.Url)
            .IsUnique();
    }
}