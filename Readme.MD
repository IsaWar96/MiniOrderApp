# MiniOrderApp

MiniOrderApp is a small console-based order management system built with C#, .NET and SQLite.

The application is structured into separate projects for Domain, Infrastructure, Presentation(UI) and Tests. It provides basic order handling with customers, orders, order items and returns, using SQLite and Dapper.

## Features

- Manage customers (create, list, update, delete).
- Manage orders and order items, including total amounts.
- Mark orders as returned and store return information (reason, refunded amount).
- Text-based console UI and integration tests for repositories.

## Build and Run (from source)

Requirements:
- .NET SDK installed.
- Git (if you are cloning via git).

Clone or download the repository:

```bash
# 1. Restore dependencies
dotnet restore

# 2. Build the solution
dotnet build

# 3. Run the console application
dotnet run --project MiniOrderApp.Presentation

# 4. (Optional) Run tests
dotnet test
