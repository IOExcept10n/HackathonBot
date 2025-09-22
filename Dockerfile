FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/MyBots.Common/MyBots.Common.csproj", "src/MyBots.Common/"]
COPY ["src/HackathonBot/HackathonBot.csproj", "src/HackathonBot/"]
RUN dotnet restore "src/HackathonBot/HackathonBot.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "src/HackathonBot/HackathonBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/HackathonBot/HackathonBot.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create volume for SQLite database
VOLUME /app/data

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production

ENTRYPOINT ["dotnet", "HackathonBot.dll"]