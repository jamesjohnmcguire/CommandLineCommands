REM %1 - Version (such as 1.0.5)
REM %2 - API key

CD %~dp0
CD ..\SourceCode\CommandLineCommands

msbuild -property:Configuration=Release;OutputPath=Bin\Release\Nuget;Platform="Any CPU" -restore CommandLineCommands.csproj
msbuild -property:Configuration=Release;OutputPath=Bin\Release\Nuget;Platform="Any CPU" CommandLineCommands.csproj

CD Bin\Release\Nuget
dotnet nuget push DigitalZenWorks.CommandLine.Commands.%1.nupkg --api-key %2 --source https://api.nuget.org/v3/index.json
