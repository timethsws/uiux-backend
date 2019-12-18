# update the database
dotnet ef database drop
rm -r Migrations
dotnet ef migrations add "Initial Migration"
dotnet ef database update
