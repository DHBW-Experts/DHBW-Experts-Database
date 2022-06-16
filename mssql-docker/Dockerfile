FROM mcr.microsoft.com/mssql/server:2022-latest

COPY ./Script/createDatabase.sql createDatabase.sql
COPY ./Script/configureDatabase.sql configureDatabase.sql
COPY ./Script/injectTestdata.sql injectTestdata.sql

COPY ./mssql-docker/setup_database.sh setup_database.sh
COPY ./mssql-docker/entrypoint.sh entrypoint.sh

CMD /bin/bash ./entrypoint.sh