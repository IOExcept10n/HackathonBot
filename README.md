# HackathonBot

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](./LICENSE)

Simple cross-platform Telegram bot for hackathon management.

## Quick start (Docker)

Assumes Linux host, repository checked out, and host directory /srv/hackathonbot/data mounted into the container as /app/data.

1. Build image:
```sh
docker build -f src/HackathonBot/Dockerfile -t hackathonbot:release .
```
2. Run container:
```sh
docker run -d --name hackathonbot \
  -v /srv/hackathonbot/data:/app/data \
  -e ASPNETCORE_ENVIRONMENT=Production \
  --restart unless-stopped \
  hackathonbot:release
```

## Configuration

Create appsettings.Secrets.json in src/HackathonBot (example):
```json
{
  "BotStartupConfig": {
    "Token": "<your_token_here>",
    "BotCreator": "<your_telegram_username>"
  }
}
```
For debugging locally, create appsettings.Development.json. You can copy contents from appsettings.json and adjust values as needed.

## Build from source

Project is cross-platform (.NET) and can run in Docker. There are no prebuilt releases — build from sources using the included Dockerfile or your preferred dotnet tooling.

## Project layout (important parts)

    src/HackathonBot — main application
        Migrations, Models, Modules, Repository, Services
        Dockerfile, appsettings*.json, hackbot.db (SQLite)
    src/MyBots.* — shared libraries (Common, Persistence, Analyzers)

(Repository contains additional test and analyzer projects; some subprojects are not used by runtime.)

## Notes

    Quartz scheduling code exists under MyBots.Scheduling but is not used by the main bot (left as TODO).
    Keep configuration minimal; the app reads appsettings*.json from src/HackathonBot.

## License

MIT — see LICENSE file in repository.