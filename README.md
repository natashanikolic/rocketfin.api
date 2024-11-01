# RocketFin API

RocketFin API is a .NET backend service for handling portfolio management

## Table of Contents
1. [Getting Started](#getting-started)
2. [Prerequisites](#prerequisites)
3. [Installation](#installation)
4. [Configuration](#configuration)
5. [Running the API](#running-the-api)
6. [API Endpoints](#api-endpoints)
7. [Project Structure](#project-structure)
8. [Testing](#testing)
9. [Deployment](#deployment)
10. [Contributing](#contributing)
11. [License](#license)

---

## Getting Started

These instructions will help you set up the RocketFin API on your local machine for development and testing.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server) (local or Docker container)
- [Docker](https://www.docker.com/) (for containerized deployment)
- [Node.js 18](https://nodejs.org/) (for the Angular frontend)

## Installation

1. **Clone the repositories:**

   ```bash
   git clone https://github.com/your-repo/rocketfin-api.git
   cd rocketfin-api

     ```bash
   git clone https://github.com/your-repo/rocketfin-client.git
   cd rocketfin-api

2. **Build projects:**

   client -
   1. npm install
   2. ng serve
  
   api -
   1. dotnet build
   2. change to RocketFinInfrastructure to run migrations dotnet ef --startup-project ../RocketFinApi/ migrations add InitialCreate --context PortfolioDbContext
   3. API > dotnet run

   
