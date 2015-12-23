mkdir  ../payload

.\..\.nuget\nuget.exe pack .\..\Marioneta\Marioneta\Marioneta.csproj -OutputDirectory .\payload -build -Properties Configuration=Release

REM .\.nuget\NuGet.exe push .\payload\Marioneta*.nupkg -ConfigFile .\.nuget\NuGet.Config %1

rm -r ../payload
