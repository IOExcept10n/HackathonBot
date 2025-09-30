# syntax=docker/dockerfile:1

ARG LAUNCHING_FROM_VS
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends clang zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файл решения (.slnx или .sln) из корня репо в образ
COPY ["*.slnx", "./"]
COPY ["*.sln", "./"]

# Копируем все csproj из src (нужно для корректного restore)
# Этот шаблон копирует все проекты в src — если у вас очень большой репо, можно перечислить конкретные .csproj
COPY src/**/*.csproj src/

# Восстанавливаем зависимости по файлу решения (dotnet restore сам найдёт slnx/sln)
RUN dotnet restore || true
# Если dotnet restore отказал из-за шаблона COPY, вторичный restore ниже после полного копирования гарантирует корректность

# Копируем весь репозиторий в контекст сборки
COPY . .

# Перейдём в папку проекта бота и соберём/опубликуем
WORKDIR /src/HackathonBot
RUN dotnet build "HackathonBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /src/HackathonBot
RUN dotnet publish "HackathonBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

FROM base AS aotdebug
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends gdb \
    && rm -rf /var/lib/apt/lists/*
USER 1000

FROM ${FINAL_BASE_IMAGE:-mcr.microsoft.com/dotnet/runtime-deps:8.0} AS final
WORKDIR /app

# Создаём каталог для sqlite и даём права на пользователя 1000
RUN mkdir -p /app/data && chown -R 1000:1000 /app/data

COPY --from=publish /app/publish .
USER 1000

ENTRYPOINT ["./HackathonBot"]
