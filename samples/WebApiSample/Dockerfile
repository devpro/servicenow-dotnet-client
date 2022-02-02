FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/", "./"]
COPY ["samples/", "./"]
RUN dotnet restore "WebApiSample/WebApiSample.csproj"
COPY . .
WORKDIR "/src/WebApiSample"
RUN dotnet build "WebApiSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiSample.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RabbidsIncubator.Samples.ServiceNowWebApiSample.dll"]
