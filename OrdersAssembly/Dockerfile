# Базовый образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8085

# Этап сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем ВСЕ .csproj файлы
COPY ["OrdersAssembly/FastFoodBackend.OrdersAssembly/FastFoodBackend.OrdersAssembly.csproj", "OrdersAssembly/FastFoodBackend.OrdersAssembly/"]

# Восстанавливаем зависимости основного проекта
RUN dotnet restore "OrdersAssembly/FastFoodBackend.OrdersAssembly/FastFoodBackend.OrdersAssembly.csproj"

# Копируем ВЕСЬ исходный код
COPY . .

# Сборка основного проекта
WORKDIR "/src/OrdersAssembly/FastFoodBackend.OrdersAssembly"
RUN dotnet build "FastFoodBackend.OrdersAssembly.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этап публикации
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FastFoodBackend.OrdersAssembly.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastFoodBackend.OrdersAssembly.dll"]