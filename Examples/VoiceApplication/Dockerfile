FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 7380
ENV ASPNETCORE_URLS=http://+:7380

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["aspvoice.csproj", "./"]
RUN dotnet restore "./aspvoice.csproj"
COPY . .
WORKDIR /src/.
RUN dotnet build "aspvoice.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "aspvoice.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "aspvoice.dll"]
