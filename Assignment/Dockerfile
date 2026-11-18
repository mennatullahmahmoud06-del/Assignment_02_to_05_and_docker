# Stage 1 base image (runtime only)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Stage 2 build image
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Assignment/Assignment.csproj", "Assignment/"]
RUN dotnet restore "Assignment/Assignment.csproj"
COPY . .
WORKDIR "/src/Assignment"
RUN dotnet build "Assignment.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3 publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Assignment.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Stage 4 final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Assignment.dll"]
