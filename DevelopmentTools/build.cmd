@ECHO OFF
SETLOCAL
SET "commandsBuildExitCode=0"

:: publish dispatches trusted publishing from committed main.
PUSHD "%~dp0..\CommandLineCommands"
IF ERRORLEVEL 1 EXIT /B 1

IF /I "%~1"=="publish" GOTO publish

dotnet build --configuration Debug -p:PlatformTarget=AnyCPU -p:Prefer32Bit=false
SET "commandsBuildExitCode=%ERRORLEVEL%"
GOTO end

:publish
WHERE gh >NUL 2>&1
SET "commandsBuildExitCode=%ERRORLEVEL%"
IF NOT "%commandsBuildExitCode%"=="0" SET "message=GitHub CLI is required. Install gh and run gh auth login."
IF NOT "%commandsBuildExitCode%"=="0" GOTO error

SET "commandsProjectVersion="
FOR /F "delims=" %%V IN (
    'dotnet msbuild -nologo -getProperty:Version'
) DO SET "commandsProjectVersion=%%V"

IF NOT DEFINED commandsProjectVersion SET "message=Unable to read the project version."
IF NOT DEFINED commandsProjectVersion GOTO error

ECHO Requesting publication of version %commandsProjectVersion% from main.
gh workflow run dotnet.yml --ref main -F publish=true -f version="%commandsProjectVersion%" --repo jamesjohnmcguire/CommandLineCommands

SET "commandsBuildExitCode=%ERRORLEVEL%"
IF NOT "%commandsBuildExitCode%"=="0" SET "message=Problem Running: gh workflow run dotnet.yml"
IF NOT "%commandsBuildExitCode%"=="0" GOTO error

ECHO Workflow requested. GitHub will build, test, and publish the package.
ECHO Use gh run watch from the CommandLineCommands repository to follow its progress.
GOTO end

:error
ECHO %message%
SET "commandsBuildExitCode=1"

:end
POPD
EXIT /B %commandsBuildExitCode%
