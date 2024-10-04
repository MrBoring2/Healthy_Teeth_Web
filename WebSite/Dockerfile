#FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
#WORKDIR /WebSite

# We copy the .csproj of our app to root and 
# restore the dependencies of the project.
#COPY /WebSite/WebSite.csproj .
#RUN dotnet restore "WebSite.csproj"

# We proceed by copying all the contents in
# the main project folder to root and build it
#COPY /WebSite .
#RUN dotnet build "WebSite.csproj" -c Release -o /build

# Once we're done building, we'll publish the project
# to the publish folder 
#FROM build-env AS publish
#RUN dotnet publish "WebSite.csproj" -c Release -o /publish

# We then get the base image for Nginx and set the 
# work directory 
#FROM nginx:alpine AS final
#WORKDIR /usr/share/nginx/html

# We'll copy all the contents from wwwroot in the publish
# folder into nginx/html for nginx to serve. The destination
# should be the same as what you set in the nginx.conf.
#COPY --from=publish /publish/wwwroot /usr/local/webapp/nginx/html
#COPY WebSite/nginx.conf /etc/nginx/nginx.conf

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