# ������� ����� ��� runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8083

# ���� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# �������� ��� .csproj �����
COPY ["OrderAcception/FastFoodBackend.OrderAcception/FastFoodBackend.OrderAcception.csproj", "OrderAcception/FastFoodBackend.OrderAcception/"]

# ��������������� ����������� ��������� �������
RUN dotnet restore "OrderAcception/FastFoodBackend.OrderAcception/FastFoodBackend.OrderAcception.csproj"

# �������� ���� �������� ���
COPY . .

# ������ ��������� �������
WORKDIR "/src/OrderAcception/FastFoodBackend.OrderAcception"
RUN dotnet build "FastFoodBackend.OrderAcception.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ���� ����������
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FastFoodBackend.OrderAcception.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ��������� �����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastFoodBackend.OrderAcception.dll"]