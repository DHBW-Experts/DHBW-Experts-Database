#!/usr/bin/env bash
# Wait for database to startup 
sleep 20
./opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i createDatabase.sql
./opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i configureDatabase.sql
./opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -i injectTestdata.sql