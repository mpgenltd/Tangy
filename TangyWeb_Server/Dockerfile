FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TangyWeb_Server/TangyWeb_Server.csproj", "TangyWeb_Server/"]
RUN dotnet restore "TangyWeb_Server/TangyWeb_Server.csproj"
COPY . .
WORKDIR "/src/TangyWeb_Server"
RUN dotnet build "TangyWeb_Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TangyWeb_Server.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TangyWeb_Server.dll"]
