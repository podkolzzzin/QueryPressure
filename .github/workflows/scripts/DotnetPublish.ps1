param (
  [string] $OutputSubfolder,
  [string] $Runtime
)

dotnet publish src/QueryPressure.UI/QueryPressure.UI.csproj `
  -c Release `
  --self-contained true `
  -p:PublishSingleFile=true `
  -p:PublishReadyToRun=true `
  -p:PublishTrimmed=true `
  -p:DebugType=None `
  -p:DebugSymbols=false `
  -p:IncludeNativeLibrariesForSelfExtract=true `
  --output src/QueryPressure.UI/.out/$OutputSubfolder `
  --runtime $Runtime

if ($LastExitCode -ne 0) {
  throw
}
