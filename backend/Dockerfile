FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY job-scraper.sln ./
COPY JobScraper.Api/JobScraper.Api.csproj JobScraper.Api/
COPY JobScraper.Application/JobScraper.Application.csproj JobScraper.Application/
COPY JobScraper.Repository/JobScraper.Repository.csproj JobScraper.Repository/
COPY JobScraper.Scrapers/JobScraper.Scrapers.csproj JobScraper.Scrapers/

RUN dotnet restore job-scraper.sln

COPY . ./

RUN dotnet publish JobScraper.Api/JobScraper.Api.csproj \
    -c Release \
    -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish ./

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ConnectionStrings__DefaultConnection="Data Source=/data/jobscraper.db"

EXPOSE 8080

ENTRYPOINT ["dotnet", "JobScraper.Api.dll"]
