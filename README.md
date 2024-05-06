# Simple Library Application

Simple library application backend in C# connected to external Postgres database.

## Functionality

- Get, Create and Update books
- Create, Get, and Delete users
- Get and Create borrowings for a user

## How to use

1. Add a `appsettings.json` and `appsettings.Development.json` file with contents as shown in the `application.example.json` file.
2. Create migrations.
3. Install dependencies.
4. Run with `dotnet run`.
5. Checkout `/swagger` endpoint to test the available endpoints.
