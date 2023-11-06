# MercuryApi

## Scaffold Database Context and Entity Models
```
dotnet ef dbcontext scaffold "Name=ConnectionStrings:MercuryDb" --context-dir Data --output-dir Data/Entities -f Microsoft.EntityFrameworkCore.SqlServer
```