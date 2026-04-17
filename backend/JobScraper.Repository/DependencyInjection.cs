using JobScraper.Application.Interfaces;
using JobScraper.Repository.Data;
using JobScraper.Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobScraper.Repository;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<JobScraperDbContext>(options => 
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IJobPostingRepository, JobPostingRepository>();

        return services;
    }
}