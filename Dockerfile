FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os ficheiros e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia todo o código e faz o publish
COPY . ./
RUN dotnet publish -c Release -o /app/out

# Imagem de execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/out .

# Troque "SeuProjeto.dll" pelo nome exato da DLL da sua aplicação
ENTRYPOINT ["dotnet", "agendamento_api.dll"]