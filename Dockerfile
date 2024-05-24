FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ic13-shchehlov-vla.sln ./

RUN dotnet sln ic13-shchehlov-vla.sln remove Tests/Tests.csproj

COPY DataInCloud.Api/DataInCloud.csproj DataInCloud.Api/
COPY DataInCloud.Dal/DataInCloud.Dal.csproj DataInCloud.Dal/
COPY DataInCloud.Model/DataInCloud.Model.csproj DataInCloud.Model/
COPY DataInCloud.Orchestrators/DataInCloud.Orchestrators.csproj DataInCloud.Orchestrators/

RUN dotnet restore

COPY . .
WORKDIR /src/DataInCloud.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:8080

ENTRYPOINT ["dotnet", "DataInCloud.dll"]

EXPOSE 8080
