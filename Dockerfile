# Stage 1: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

# Copy the .csproj file and restore dependencies
COPY DogBlog/DogBlog.csproj DogBlog/
RUN dotnet restore DogBlog/DogBlog.csproj

# Copy the rest of the application code and build it
COPY DogBlog/ DogBlog/
WORKDIR /app/DogBlog
RUN dotnet build DogBlog.csproj -c Release -o /app/build

# Stage 2: Publish the application
FROM build AS publish
RUN dotnet publish DogBlog.csproj -c Release -o /app/publish

# Stage 3: Set up the runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development
EXPOSE 8080
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "DogBlog.dll" ]
