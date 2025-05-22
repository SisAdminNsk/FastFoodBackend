# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8084

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем ВСЕ .csproj файлы
COPY ["HotDishes/FastFoodBackend.HotDishes/FastFoodBackend.HotDishes.csproj", "HotDishes/FastFoodBackend.HotDishes/"]

# Восстанавливаем зависимости основного проекта
RUN dotnet restore "HotDishes/FastFoodBackend.HotDishes/FastFoodBackend.HotDishes.csproj"

# Копируем ВЕСЬ исходный код
COPY . .

# Сборка основного проекта
WORKDIR "/src/HotDishes/FastFoodBackend.HotDishes"
RUN dotnet build "FastFoodBackend.HotDishes.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап публикации
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FastFoodBackend.HotDishes.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastFoodBackend.HotDishes.dll"]