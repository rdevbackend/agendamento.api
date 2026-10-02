FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia o ficheiro .csproj e restaura dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código-fonte e compila em Release
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Imagem final de execução do ASP.NET Core 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/out .

# Executa a DLL gerada pelo projeto agendamento.api
ENTRYPOINT ["dotnet", "agendamento.api.dll"]