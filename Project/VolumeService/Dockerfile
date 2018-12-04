FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /volume
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["VolumeService/VolumeService.csproj", "VolumeService/"]
RUN dotnet restore "VolumeService/VolumeService.csproj"
COPY . .
WORKDIR "/src/VolumeService"
RUN dotnet build "VolumeService.csproj" -c Release -o /volume

FROM build AS publish
RUN dotnet publish "VolumeService.csproj" -c Release -o /volume

FROM base AS final
WORKDIR /volume
COPY --from=publish /volume .
ENTRYPOINT ["dotnet", "VolumeService.dll"]