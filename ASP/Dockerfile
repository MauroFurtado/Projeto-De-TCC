FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar projeto e restaurar dependências
COPY ResrvaDeSala_API/*.csproj ./ResrvaDeSala_API/
RUN dotnet restore ResrvaDeSala_API/ReservaDeSala_API.csproj

# Copiar todo o código
COPY ResrvaDeSala_API/. ./ResrvaDeSala_API/

# Build da aplicação
WORKDIR /src/ResrvaDeSala_API
RUN dotnet build ReservaDeSala_API.csproj -c Release -o /app/build

# Publicar a aplicação
FROM build AS publish
RUN dotnet publish ReservaDeSala_API.csproj -c Release -o /app/publish


FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 80

ENTRYPOINT ["dotnet", "ReservaDeSala_API.dll"]
