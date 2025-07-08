#### Initial steps

`dotnet new sln -n SmartTaskManager` 
- Solution （vs code recoginize） record .csproj how they connect
- `dotnet new sln` create a solution
- `-n SmartTaskManager` name


`dotnet new console -n SmartTaskApp`
it will create 
- Program.cs - entrance
- SmartTaskApp.csproj


`dotnet sln add SmartTaskApp/SmartTaskApp.csproj`


run `dotnet run --project SmartTaskApp`


#### create models 
`mkdir SmartTaskApp/Models`
