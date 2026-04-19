using JobScraper.Application.Interfaces;
using JobScraper.Application.Services;
using JobScraper.Repository;
using JobScraper.Repository.Data;
using JobScraper.Scrapers.HelloWorld;
using JobScraper.Scrapers.Infostud;
using JobScraper.Scrapers.Joberty;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IJobService, JobService>();
//builder.Services.AddScoped<IJobSourceScraper, FakeJobSourceScraper>();
builder.Services.AddHttpClient<IJobSourceScraper, HelloWorldJobSourceScraper>();
builder.Services.AddHttpClient<IJobSourceScraper, InfostudJobSourceScraper>();
builder.Services.AddHttpClient<IJobSourceScraper, JobertyJobSourceScraper>();
builder.Services.AddScoped<IJobImportService, JobImportService>();
builder.Services.AddRepositoryLayer(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<JobScraperDbContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
