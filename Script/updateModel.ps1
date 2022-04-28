# PowerShell.exe -ExecutionPolicy Bypass -File .\Script\updateModel.ps1
dotnet ef dbcontext scaffold "$env:DHEX_DATABASE" Microsoft.EntityFrameworkCore.SqlServer -o Model -f --no-pluralize --force --no-onconfiguring
