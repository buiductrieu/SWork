# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["SWork.API/SWork.API.csproj", "SWork.API/"]
COPY ["SWork.Service/SWork.Service.csproj", "SWork.Service/"]
COPY ["SWork.ServiceContract/SWork.ServiceContract.csproj", "SWork.ServiceContract/"]
COPY ["SWork.Repository/SWork.Repository.csproj", "SWork.Repository/"]
COPY ["SWork.RepositoryContract/SWork.RepositoryContract.csproj", "SWork.RepositoryContract/"]
COPY ["SWork.Data/SWork.Data.csproj", "SWork.Data/"]
COPY ["SWork.Common/SWork.Common.csproj", "SWork.Common/"]

RUN dotnet restore "SWork.API/SWork.API.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet build "SWork.API/SWork.API.csproj" -c Release -o /app/build
RUN dotnet publish "SWork.API/SWork.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Set environment variables
ENV ASPNETCORE_URLS=http://+:80
ENV ASPNETCORE_ENVIRONMENT=Production

# Copy the published app
COPY --from=build /app/publish .

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "SWork.API.dll"] 