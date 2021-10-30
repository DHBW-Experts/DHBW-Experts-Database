# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /source/api
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "DatabaseAPI.dll"]

EXPOSE 80
ENV DHEX_DATABASE ""

#BUILD:
#docker build -t databaseAPI .

#RUN:
#docker run -it --rm -p 5000:80 --name databaseAPI_sample -e "STRING" databaseAPI
