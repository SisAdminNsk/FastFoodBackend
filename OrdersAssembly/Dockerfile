# ������� ����� ��� runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8085

# ���� ������
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# �������� ��� .csproj �����
COPY ["OrdersAssembly/FastFoodBackend.OrdersAssembly/FastFoodBackend.OrdersAssembly.csproj", "OrdersAssembly/FastFoodBackend.OrdersAssembly/"]

# ��������������� ����������� ��������� �������
RUN dotnet restore "OrdersAssembly/FastFoodBackend.OrdersAssembly/FastFoodBackend.OrdersAssembly.csproj"

# �������� ���� �������� ���
COPY . .

# ������ ��������� �������
WORKDIR "/src/OrdersAssembly/FastFoodBackend.OrdersAssembly"
RUN dotnet build "FastFoodBackend.OrdersAssembly.csproj" -c $BUILD_CONFIGURATION -o /app/build

# ���� ����������
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FastFoodBackend.OrdersAssembly.csproj" \
    -c $BUILD_CONFIGURATION \
    -o /app/publish \
    /p:UseAppHost=false

# ��������� �����
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FastFoodBackend.OrdersAssembly.dll"]