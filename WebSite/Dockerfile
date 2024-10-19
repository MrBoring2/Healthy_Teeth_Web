
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["WebSite/WebSite.csproj", "WebSite/"]
RUN dotnet restore "./WebSite/./WebSite.csproj"
COPY . .
WORKDIR "/src/WebSite"
RUN dotnet build "./WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./WebSite.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY WebSite/nginx.conf /etc/nginx/nginx.conf