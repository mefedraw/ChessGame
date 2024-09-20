FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChessGame2/ChessGame2.csproj", "ChessGame2/"]
COPY ["ChessLOgic/ChessLOgic.csproj", "ChessLOgic/"]
RUN dotnet restore "ChessGame2/ChessGame2.csproj"
COPY . .
WORKDIR "/src/ChessGame2"
RUN dotnet build "ChessGame2.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ChessGame2.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChessGame2.dll"]
