## 🚀 Quick Start

### 1. Clone Repository
```bash
git clone https://github.com/breyg/DeploymentTracker.git
cd GP.ADQ.DeploymentTracker
```

### 2. Install Entity Framework Tools
```bash
dotnet tool install --global dotnet-ef
```

### 3. Setup Database

#### Option A: Local PostgreSQL
```sql
-- Connect to PostgreSQL
psql -U postgres

-- Create database
CREATE DATABASE "DeploymentTracker";

-- Exit
\q
```

#### Option B: Docker PostgreSQL
```bash
docker run --name postgres-deployment \
  -e POSTGRES_PASSWORD=mypassword \
  -e POSTGRES_DB=DeploymentTracker \
  -p 5432:5432 \
  -d postgres:15
```

### 4. Configure Connection String
Edit `GP.ADQ.DeploymentTracker.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=DeploymentTracker;Username=postgres;Password=your_password_here;Port=5432"
  }
}
```

### 5. Setup Application
```bash
# Navigate to WebAPI project
cd GP.ADQ.DeploymentTracker.API

# Restore NuGet packages
dotnet restore

# Apply database migrations
dotnet ef database update --startup-project . --project ../GP.ADQ.DeploymentTracker.Infrastructure

# Run the application
dotnet run
```
