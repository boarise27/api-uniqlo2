# Use official .NET 8 SDK image for build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore WepApi.csproj
RUN dotnet publish WepApi.csproj -c Release -o /app --no-restore

# Use official .NET 8 ASP.NET runtime image for run
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .
# EXPOSE 80
# EXPOSE 443
ENTRYPOINT ["dotnet", "WepApi.dll"]
