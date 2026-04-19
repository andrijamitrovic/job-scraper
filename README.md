# JobScraper

JobScraper is a simple job scraping app for searching jobs across multiple websites from one place.

It currently supports:

- HelloWorld
- Poslovi Infostud
- Joberty

The project uses a .NET backend, a React frontend, and Docker for easier local running and deployment.

## Tech Stack

- .NET / ASP.NET Core Web API
- Entity Framework Core
- SQLite
- React
- Vite
- Docker
- Docker Compose

## Features

- Import jobs from multiple job boards
- Store scraped jobs in a local database
- Skip duplicate jobs by URL
- Display saved jobs in a simple React frontend
- Mark newly imported jobs after running an import

## Running With Docker

From the project root, run:

```bash
docker compose up --build -d
```

The frontend will be available at:

```txt
http://localhost:3000
```

The backend API will be available at:

```txt
http://localhost:8080
```

## Running Locally

Start the backend:

```bash
cd backend
dotnet run --project .\JobScraper.Api\
```

Start the frontend:

```bash
cd frontend
npm install
npm run dev
```

The Vite frontend usually runs at:

```txt
http://localhost:5173
```

## Usage

Open the frontend in your browser and click **Import jobs**.

The backend will scrape the supported sources, save new jobs, skip duplicates, and return an import summary. The jobs list will then refresh and show the saved jobs.

## Project Structure

The project is split into a backend and frontend.

- `backend/` contains the .NET solution.
- `JobScraper.Api/` exposes the HTTP API used by the frontend.
- `JobScraper.Application/` contains the application services and shared interfaces.
- `JobScraper.Repository/` contains the database context, migrations, and repository code.
- `JobScraper.Scrapers/` contains the individual job source scrapers.
- `frontend/` contains the Vite React app.
- `docker-compose.yml` runs the backend and frontend together with Docker.

job-scraper/
├── README.md
├── docker-compose.yml
├── backend/
│   ├── Dockerfile
│   ├── .dockerignore
│   ├── job-scraper.sln
│   ├── JobScraper.Api/
│   │   ├── Controllers/
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   └── appsettings.Development.json
│   ├── JobScraper.Application/
│   │   ├── Interfaces/
│   │   ├── Models/
│   │   ├── Requests/
│   │   └── Services/
│   ├── JobScraper.Repository/
│   │   ├── Data/
│   │   ├── Migrations/
│   │   └── Repositories/
│   └── JobScraper.Scrapers/
│       ├── HelloWorld/
│       ├── Infostud/
│       └── Joberty/
└── frontend/
    ├── Dockerfile
    ├── .dockerignore
    ├── package.json
    ├── vite.config.js
    ├── index.html
    └── src/
        ├── App.jsx
        └── index.css


## API Endpoints

```txt
GET  /api/jobs
POST /api/jobs
GET  /api/jobs/{id}
POST /api/job-imports/run
```

## Notes

The SQLite database is stored locally and ignored by Git.

When running with Docker, the database is stored in a Docker volume so scraped jobs can persist between container restarts.
