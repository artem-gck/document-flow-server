FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source
COPY . .

RUN dotnet restore "MicroServices/UserInfoService/UsersServiceApi/UsersServiceApi.csproj"
RUN dotnet publish "MicroServices/UserInfoService/UsersServiceApi/UsersServiceApi.csproj" -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
COPY --from=build /app ./

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "UsersServiceApi.dll"]