FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FreeDOW.API.Core/FreeDOW.API.Core.csproj", "FreeDOW.API.Core/"]
COPY ["FreeDOW.API.DataAccess/FreeDOW.API.DataAccess.csproj", "FreeDOW.API.DataAccess/"]
COPY ["FreeDOW.API.WebHost/FreeDOW.API.WebHost.csproj", "FreeDOW.API.WebHost/"]
RUN dotnet restore "FreeDOW.API.WebHost/FreeDOW.API.WebHost.csproj"
COPY . . 
WORKDIR "/src/FreeDOW.API.WebHost"
RUN dotnet build "FreeDOW.API.WebHost.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "FreeDOW.API.WebHost.csproj" -c Debug -o /app/publish

FROM publish AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FreeDOW.API.WebHost.dll"]