# ������� ����� ��� runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8084

# ���� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# �������� ��� .csproj �����
COPY ["HotDishes/FastFoodBackend.HotDishes/FastFoodBackend.HotDishes.csproj", "HotDishes/FastFoodBackend.HotDishes/"]

# ��������������� ����������� ��������� �������
RUN dotnet restore "HotDishes/FastFoodBackend.HotDishes/FastFoodBackend.HotDishes.csproj"

# �������� ���� �������� ���
COPY . .

# ������ ��������� �������
WORKDIR "/src/HotDishes/FastFoodBackend.HotDishes"
RUN dotnet build "FastFoodBackend.HotDishes.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ���� ����������
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FastFoodBackend.HotDishes.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ��������� �����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastFoodBackend.HotDishes.dll"]