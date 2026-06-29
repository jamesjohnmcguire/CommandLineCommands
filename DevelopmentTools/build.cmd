REM %2 - Version (such as 1.5.1)
REM %3 - API key

CD %~dp0
CD ..\CommandLineCommands

:default
dotnet build --configuration Release -p:PlatformTarget=AnyCPU -p:Prefer32Bit=false

IF "%1"=="publish" GOTO publish
GOTO end

:publish

if "%~2"=="" GOTO error1

dotnet pack --configuration Release --output nupkg -p:IncludeSource=true -p:IncludeSymbols=true

CD nupkg

nuget push DigitalZenWorks.CommandLine.Commands.%2.nupkg %NuGetKey% -source https://api.nuget.org/v3/index.json

cd ..
GOTO end

:error1
ECHO No version tag specified
GOTO end

:end
