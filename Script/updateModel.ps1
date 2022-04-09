# PowerShell.exe -ExecutionPolicy Bypass -File .\Script\updateModel.ps1
dotnet ef dbcontext scaffold "$env:DHEX_DATABASE" Microsoft.EntityFrameworkCore.SqlServer -o Model -f
echo 'Bitte Compiler Warnung in "DHBWExpertsdatabaseContext" beachten! (ca. Zeile 29)'
echo 'Bitte die Methode'
echo '"protected override void OnConfiguring( ... )..."'
echo 'Loeschen, da sonst Passwoerter auf GitHub veroeffentlicht werden!'
echo 'Die Konfigurierung findet in der Klasse "Startup.cs" statt.'
