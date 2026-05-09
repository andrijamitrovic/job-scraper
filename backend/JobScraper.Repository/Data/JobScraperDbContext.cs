using JobScraper.Application.Models;
using Microsoft.EntityFrameworkCore;

namespace JobScraper.Repository.Data;

public class JobScraperDbContext : DbContext
{
    public JobScraperDbContext(DbContextOptions<JobScraperDbContext> options) : base(options)
    {

    }

    public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<JobApplication>(entity =>
        {
            entity.HasKey(application => application.Id);

            entity.Property(application => application.Status)
                .HasConversion<string>()
                .HasMaxLength(50);

            entity.Property(application => application.Notes)
                .HasMaxLength(2000);

            entity.HasOne(application => application.JobPosting)
                .WithOne(job => job.Application)
                .HasForeignKey<JobApplication>(application => application.JobPostingId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(application => application.JobPostingId)
                .IsUnique();
        });
    }
}