dotnet ef dbcontext scaffold "$env:DHEX_DATABASE" Microsoft.EntityFrameworkCore.SqlServer -o Model -f
echo 'Bitte Compiler Warnung in "DHBWExpertsdatabaseContext" beachten! (ca. Zeile 29)'
echo '"optionsBuilder.UseSqlServer( (...) );"'
echo 'ersetzen durch'
echo '"optionsBuilder.UseSqlServer(Environment.GetEnvironmentVariable("DHEX_DATABASE"));"'
