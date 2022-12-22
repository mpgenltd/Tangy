FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["TangyWeb_API/TangyWeb_API.csproj", "TangyWeb_API/"]
RUN dotnet restore "TangyWeb_API/TangyWeb_API.csproj"
COPY . .
WORKDIR "/src/TangyWeb_API"
RUN dotnet build "TangyWeb_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TangyWeb_API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TangyWeb_API.dll"]
