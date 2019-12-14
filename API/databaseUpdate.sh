# update the database
dotnet ef database drop
dotnet ef migrations remove
dotnet ef migrations add "Initial Migration"
dotnet ef database update
