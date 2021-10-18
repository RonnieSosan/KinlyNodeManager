#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["KinleyNodeManager.API/KinleyNodeManager.API.csproj", "KinleyNodeManager.API/"]
COPY ["KinlyNodeManager.Persistence/KinlyNodeManager.Persistence.csproj", "KinlyNodeManager.Persistence/"]
COPY ["KinleyNodeManager.Core/KinlyNodeManager.Core.csproj", "KinleyNodeManager.Core/"]
COPY ["KinlyNodeManagerService/KinlyNodeManagerService.csproj", "KinlyNodeManagerService/"]
RUN dotnet restore "KinleyNodeManager.API/KinleyNodeManager.API.csproj"
COPY . .
WORKDIR "/src/KinleyNodeManager.API"
RUN dotnet build "KinleyNodeManager.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KinleyNodeManager.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KinleyNodeManager.API.dll"]