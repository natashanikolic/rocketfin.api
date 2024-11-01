# RocketFin Project Setup

This document provides setup instructions for the RocketFin API and Client projects.

## Table of Contents
1. [Clone the Repositories](#clone-the-repositories)
2. [Client Setup](#client-setup)
3. [API Setup](#api-setup)

---

## Clone the Repositories

1.
  **Clone the API repository:**
   git clone https://github.com/your-repo/rocketfin-api.git
   cd rocketfin-api

 **Clone the Client repository:**
   git clone https://github.com/your-repo/rocketfin-api.git
   cd rocketfin-client

## API Setup

1. **Navigate to the API project directory**:

   ```bash
   cd ../rocketfin-api
   
   dotnet build
   
   cd RocketFinInfrastructure
   
   dotnet ef --startup-project ../RocketFinApi/ migrations add InitialCreate --context PortfolioDbContext
   
   dotnet run --project RocketFinApi
