# syntax=docker/dockerfile:1

ARG LAUNCHING_FROM_VS
ARG FINAL_BASE_IMAGE=${LAUNCHING_FROM_VS:+aotdebug}

# ---- base runtime ----
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# ---- build image ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
RUN apt-get update \
    && apt-get install -y --no-install-recommends clang zlib1g-dev \
    && rm -rf /var/lib/apt/lists/*

ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файл решения и исходники в контейнер (контекст сборки - корень репо)
COPY ["HackathonBot.slnx", "./"]
COPY src/ src/

# Сборка и publish проекта бота
WORKDIR /src/src/HackathonBot
RUN dotnet restore "HackathonBot.csproj"
RUN dotnet build "HackathonBot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
WORKDIR /src/src/HackathonBot
RUN dotnet publish "HackathonBot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=true

# ---- aotdebug stage (for VS debugging) ----
FROM base AS aotdebug
USER root
RUN apt-get update \
    && apt-get install -y --no-install-recommends gdb \
    && rm -rf /var/lib/apt/lists/*
USER 1000

# ---- final runtime image ----
FROM ${FINAL_BASE_IMAGE:-mcr.microsoft.com/dotnet/runtime-deps:8.0} AS final
WORKDIR /app

# Создаём каталог для sqlite и даём права на запись пользователю 1000
RUN mkdir -p /app/data && chown -R 1000:1000 /app/data

# Копируем опубликованные файлы (включая appsettings.Secrets.json, если он входит в publish)
COPY --from=publish /app/publish .

# Запуск от непользователя 1000
USER 1000

ENTRYPOINT ["./HackathonBot"]
