# MercuryApi

## Scaffold Database Context and Entity Models
-cd into the MercuryApi folder that contains the Program.cs file.
```
dotnet ef dbcontext scaffold "Name=ConnectionStrings:MercuryDb" --context-dir Data --output-dir Data/Entities -f Microsoft.EntityFrameworkCore.SqlServer
```