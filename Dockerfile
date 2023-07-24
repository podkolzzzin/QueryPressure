FROM node:19.8 as fe-build

COPY ./frontend/ /frontend/

WORKDIR /frontend

RUN npm i && npm run build

FROM mcr.microsoft.com/dotnet/sdk:7.0 as build-env

COPY ./src/ /src/
COPY --from=fe-build /frontend/dist/ /src/QueryPressure.UI/dist/

WORKDIR /
RUN dotnet publish src/QueryPressure.UI/QueryPressure.UI.csproj -c Release -o .out

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /App
COPY --from=build-env /.out .
ENTRYPOINT ["dotnet", "QueryPressure.UI.dll"]