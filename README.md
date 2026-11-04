# DigitalZenWorks.CommandLine.Commands

[![NuGet version][version-badge]][nuget]
[![NuGet downloads][downloads-badge]][nuget]
[![Build and tests][build-badge]][workflow]
[![License: MIT][license-badge]][license]
[![GitHub issues][issues-badge]][issues]

A lightweight C# library for named commands, short and long options,
option values, positional arguments, JSON command definitions, and help
output. The application retains control of command execution.

## Installation

Add the package to your application:

```shell
dotnet add package DigitalZenWorks.CommandLine.Commands
```

Or use Visual Studio's Package Manager Console:

```powershell
Install-Package DigitalZenWorks.CommandLine.Commands
```

## Quick start

In a console application, define the accepted commands and options, then
validate the supplied arguments:

```csharp
using System;
using System.Collections.Generic;
using DigitalZenWorks.CommandLine.Commands;

Command help = new("help");
help.Description = "Show available commands";

CommandOption encoding = new("e", "encoding", true);
CommandOption dryRun = new("n", "dryrun");
List<CommandOption> options = new();
options.Add(encoding);
options.Add(dryRun);

Command convert = new("convert", options, 1, "Convert a data file");
List<Command> commands = new();
commands.Add(help);
commands.Add(convert);

CommandLineInstance commandLine = new(commands, args);

if (commandLine.ValidArguments == false)
{
	Console.Error.WriteLine(commandLine.ErrorMessage);
	commandLine.ShowHelp();
	return;
}

Command command = commandLine.Command;

if (command.Name == "help")
{
	commandLine.ShowHelp();
	return;
}

string inputPath = command.Parameters[0];
CommandOption encodingOption = command.GetOption("e", "encoding");
string encodingName = "utf-8";

if (encodingOption != null)
{
	encodingName = encodingOption.Parameter;
}

bool dryRunRequested = command.DoesOptionExist("n", "dryrun");
Console.WriteLine("Input: " + inputPath);
Console.WriteLine("Encoding: " + encodingName);
Console.WriteLine("Dry run: " + dryRunRequested);
```

Example invocations:

```shell
dotnet run -- convert --encoding utf-8 --dryrun input.txt
dotnet run -- convert -e utf-8 -n input.txt
dotnet run -- help
```

The `1` in the `Command` constructor is the minimum number of positional
arguments. Setting `requiresParameter` to `true` on a `CommandOption` makes
it consume the following argument as its value. Supply values separately,
as in `--encoding utf-8`.

## JSON command definitions

Commands can also be defined in a JSON file, such as `commands.json`:

```json
[
	{
		"command": "help",
		"description": "Show available commands"
	},
	{
		"command": "convert",
		"description": "Convert a data file",
		"parameterCount": 1,
		"options":
		[
			{
				"shortName": "e",
				"longName": "encoding",
				"requiresParameter": true
			},
			{
				"shortName": "n",
				"longName": "dryrun",
				"requiresParameter": false
			}
		]
	}
]
```

Load the definitions instead of constructing the command list in code:

```csharp
using System.IO;
using DigitalZenWorks.CommandLine.Commands;

string jsonText = File.ReadAllText("commands.json");
CommandsSet commandSet = new(jsonText);
CommandLineInstance commandLine = new(commandSet, args);
```

Check `ValidArguments` before accessing the selected command, its options,
or its positional arguments. See the XML documentation in the
[library source][source] for individual API contracts.

## Target frameworks

The current Release configuration targets .NET Standard 2.0, .NET Framework
4.7.2, and .NET 5, 6, 7, 8, 9, and 10. Debug builds target .NET 10 only.
See the [package page][nuget] for a published version's frameworks and
its dependencies.

## Build and test

Install the .NET 10 SDK, then clone and build the repository:

```shell
git clone https://github.com/jamesjohnmcguire/CommandLineCommands.git
cd CommandLineCommands
dotnet build CommandLineCommands.slnx --configuration Debug
dotnet test CommandLineCommands.slnx --configuration Debug
```

See the [development notes][development-notes] for Release builds,
versioning, trusted publishing, and the `build.cmd` helper.

## Contributing

Report bugs and propose improvements through [GitHub issues][issues].

 For a pull request:

1. Fork the Project
2. Create your focused branch
3. Add your code changes
3. Add or update tests to validate your changes
3. Commit and Push your Changes
4. Open a Pull Request. Include the description of the change, its validation, and any related issue in the PR.

### Coding style
Please match the current coding style.  Most notably:
1. One operation per line
2. Use complete, self-documenting English words in variable and method names
3. Attempt to declare variable and method names in a self-documenting manner
4. Use tabs and Allman braces

Keep public API compatibility in mind: this library is consumed as NuGet package.

## License

Distributed under the [MIT License][license].

## Contact

James John McGuire — jamesjohnmcguire@gmail.com

[Project repository][repository]. If the library is useful to you, consider
giving it a star.

[version-badge]:
  https://img.shields.io/nuget/v/DigitalZenWorks.CommandLine.Commands.svg
[downloads-badge]:
  https://img.shields.io/nuget/dt/DigitalZenWorks.CommandLine.Commands.svg
[license-badge]:
  https://img.shields.io/badge/license-MIT-blue.svg
[build-badge]:
  https://img.shields.io/github/actions/workflow/status/jamesjohnmcguire/CommandLineCommands/dotnet.yml?branch=main
[workflow]:
  https://github.com/jamesjohnmcguire/CommandLineCommands/actions/workflows/dotnet.yml
[issues-badge]:
  https://img.shields.io/github/issues/jamesjohnmcguire/CommandLineCommands
[nuget]: https://www.nuget.org/packages/DigitalZenWorks.CommandLine.Commands
[license]:
  https://github.com/jamesjohnmcguire/CommandLineCommands/blob/main/LICENSE
[issues]: https://github.com/jamesjohnmcguire/CommandLineCommands/issues
[source]:
  https://github.com/jamesjohnmcguire/CommandLineCommands/tree/main/CommandLineCommands
[repository]: https://github.com/jamesjohnmcguire/CommandLineCommands
[development-notes]:
  https://github.com/jamesjohnmcguire/CommandLineCommands/blob/main/Support/DevelopmentNotes.md
