FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Presentation/Appointments.API/Appointments.API.csproj", "src/Presentation/Appointments.API/"]
COPY ["src/Core/Appointments.Application/Appointments.Application.csproj", "src/Core/Appointments.Application/"]
COPY ["src/Core/Appointments.Domain/Appointments.Domain.csproj", "src/Core/Appointments.Domain/"]
COPY ["src/Infrastructure/Appointments.Infrastructure/Appointments.Infrastructure.csproj", "src/Infrastructure/Appointments.Infrastructure/"]
RUN dotnet restore "src/Presentation/Appointments.API/Appointments.API.csproj"
COPY . .
WORKDIR "/src/src/Presentation/Appointments.API"
RUN dotnet build "./Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Appointments.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Appointments.API.dll"]
