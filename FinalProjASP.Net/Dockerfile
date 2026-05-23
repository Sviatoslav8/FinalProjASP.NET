
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src


COPY ["FinalProjASP.Net/FinalProjASP.Net.csproj", "FinalProjASP.Net/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Storage/Storage.csproj", "Storage/"]


RUN dotnet restore "FinalProjASP.Net/FinalProjASP.Net.csproj"


COPY . .

WORKDIR "/src/FinalProjASP.Net"
RUN dotnet publish "FinalProjASP.Net.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "FinalProjASP.Net.dll"]