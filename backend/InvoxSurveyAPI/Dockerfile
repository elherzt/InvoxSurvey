# Base image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the solution and project files
COPY ./*.sln ./
COPY ./InvoxSurveyAPI/*.csproj ./InvoxSurveyAPI/
COPY ./DataLayer/*.csproj ./DataLayer/
RUN dotnet restore

# Copy the rest of the code
COPY ./InvoxSurveyAPI/. ./InvoxSurveyAPI/
COPY ./DataLayer/. ./DataLayer/
WORKDIR /src/InvoxSurveyAPI
RUN dotnet publish -c Release -o /app/publish

# Final stage with lightweight runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InvoxSurveyAPI.dll"]
