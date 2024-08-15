dotnet publish -c Release -r linux-x64 --self-contained false
if (-Not (Test-Path -Path .\dist)) {
    New-Item -ItemType Directory -Path .\dist
}
Compress-Archive -Path .\bin\Release\net8.0\linux-x64\publish\* -DestinationPath .\dist\ExternalLogicTemplating.zip -Force