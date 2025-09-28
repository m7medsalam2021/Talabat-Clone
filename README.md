<img width="1211" height="634" alt="12" src="https://github.com/user-attachments/assets/dbfbe2c2-03f6-4069-9bd7-e230d262b6ae" />

#Backend
Backend API for a Talabat-style food delivery platform, built with ASP.NET Core. Supports user auth, restaurant management, and order processing using clean architecture.

# Talabat APIs
This is a Talabat Clone Project built in Onion Architecture Based on the following Design Patterns:
    
    Repository Design Pattern.
    
    Specification Design Pattern.
    
    UnitOfWork Design Pattern.
    
    Builder Design Pattern.

# Libraries Used And Packages
    StackExchange.Redis.
    
    Microsoft.EntityFrameworkCore.Tools.
    
    Microsoft.EntityFrameworkCore.SqlServer.
    
    Microsoft.AspNetCore.Identity.EntityFrameworkCore.
    
    AutoMapper.Extensions.Microsoft.DependencyInjection.
    
    Microsoft.AspNetCore.Authentication.JwtBearer.
    
    Stripe.net.

# Getting Started
    Prerequisites
    .NET 9 SDK or higher
    SQL Server (local)
    Redis (local)
    Stripe account for payment integration

#Setup
# Clone the repository
    git clone https://github.com/Talabat-Project/Back-End
    cd talabat-api
# Apply database migrations
    dotnet ef database update --project Repository
# Run the application
    dotnet run --project API
