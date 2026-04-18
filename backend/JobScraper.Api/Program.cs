using JobScraper.Application.Interfaces;
using JobScraper.Application.Services;
using JobScraper.Repository;
using JobScraper.Scrapers.HelloWorld;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddScoped<IJobService, JobService>();
//builder.Services.AddScoped<IJobSourceScraper, FakeJobSourceScraper>();
builder.Services.AddHttpClient<IJobSourceScraper, HelloWorldJobSourceScraper>();
builder.Services.AddScoped<IJobImportService, JobImportService>();
builder.Services.AddRepositoryLayer(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
