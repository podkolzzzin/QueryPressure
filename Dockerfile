FROM node:19.8 as fe-build

COPY ./frontend/ /frontend/

WORKDIR /frontend

RUN npm i && npm run build

FROM mcr.microsoft.com/dotnet/sdk:7.0

COPY ./src/ /src/
COPY --from=fe-build /frontend/dist/ /src/QueryPressure.UI/dist/

WORKDIR /
ENTRYPOINT dotnet publish src/QueryPressure.UI/QueryPressure.UI.csproj -c Release -o .out -r win-x64 -p:PublishSingleFile=true -p:PublishReadyToRun=true --self-contained true
