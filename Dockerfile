# Base image for building the application
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

# Set the working directory
WORKDIR /app

# Copy the source code
COPY . .

# Restore dependencies and build the application
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime

# Set the working directory
WORKDIR /app

# Copy the published output from the build image
COPY --from=build /app/out .

# Change the ownership of the working directory to a non-root user
RUN chown -R 1000:1000 /app

# Switch to a non-root user
USER 1000

# Expose the port
EXPOSE 8080

# Set the entry point for the container
ENTRYPOINT ["dotnet", "YourAppName.dll"]

