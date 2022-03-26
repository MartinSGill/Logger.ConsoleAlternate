FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ExampleProject/ExampleProject.csproj", "ExampleProject/"]
RUN dotnet restore "src/ExampleProject/ExampleProject.csproj"
COPY . .
WORKDIR "/src/ExampleProject"
RUN dotnet build "ExampleProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExampleProject.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExampleProject.dll"]
