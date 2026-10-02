FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia os ficheiros e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código e realiza o publish
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Imagem final de execução para .NET 10.0
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "agendamentoApi.dll"]