dotnet ef dbcontext scaffold "$env:DHEX_DATABASE" Microsoft.EntityFrameworkCore.SqlServer -o Model -f
echo 'Bitte Compiler Warnung in "DHBWExpertsdatabaseContext" beachten! (ca. Zeile 29)'
echo 'Bitte die Methode'
echo '"protected override void OnConfiguring( ... )..."'
echo 'Löschen, da sonst Passwörter auf Github öffentlich gemacht werden!'
echo 'Die Konfigurierung findet in der Klasse "Startup.cs" statt.'
