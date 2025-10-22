# DigitalZenWorks.CommandLine.Commands

This C# client library that provides light weight functionality to help process command line options.

Please :star: star this project!

## Getting Started

### Installation

#### Git
    git clone  https://github.com/jamesjohnmcguire/CommandLineCommands

#### Nuget
    PM> Install-Package DigitalZenWorks.CommandLine.Commands

### Usage:

#### Library and API usage:
Usage is intended to be simple and direct - Process command line arguments and get going with your business.

NEW!

Define a list of valid commands in JSON file, then load the file into the library.  Here is an example of the JSON command definitions file:
```json
[
	{
		"command": "help",
		"description": "Show this information"
	},
	{
		"command": "dbx-to-pst",
		"description": "Migrate dbx files to pst file",
		"options":
		[
			{
				"shortName": "e",
				"longName": "encoding",
				"requiresParameter": true
			}
		],
		"parameters":
		[
			"dbx files path",
			"PST file path"
		]
	},
	{
		"command": "eml-to-pst",
		"description": "Migrate eml files to pst file",
		"options":
		[
			{
				"shortName": "a",
				"longName": "adjust",
				"requiresParameter": false
			},
			{
				"shortName": "c",
				"longName": "close-store",
				"requiresParameter": false
			}
		],
		"parameters":
		[
			"eml files path",
			"PST file path"
		]
	},
	{
		"command": "list-folders",
		"description": "List all sub folders of a given store or folder",
		"options":
		[
			{
				"shortName": "r",
				"longName": "recurse",
				"requiresParameter": false
			}
		],
		"parameters":
		[
			"PST file path",
			"folder path"
		]
	},
	{
		"command": "remove-duplicates",
		"description": "Remove duplicate messages",
		"options":
		[
			{
				"shortName": "n",
				"longName": "dryrun",
				"requiresParameter": false
			},
			{
				"shortName": "s",
				"longName": "flush",
				"requiresParameter": false
			}
		],
		"parameters":
		[
			"PST file path"
		]
	}
]
```

Then in your code somewhere, you can instantiate with something like:
```c#
	commandsSet = new CommandsSet(jsonText);
```

Note: This example taken from: https://github.com/jamesjohnmcguire/DigitalZenWorks.Email.ToolKit

Otherwise, to pragmatically create a list of commands that the application will support, here is an example:
```c#
	IList<Command> commands = new List<Command>();

	Command help = new ("help");
	help.Description = "Show this information";
	commands.Add(help);

	CommandOption encoding = new ("e", "encoding", true);
	IList<CommandOption> options = new List<CommandOption>();
	options.Add(encoding);

	Command someCommand = new (
		"some-command", options, 1, "A Do Something Command");
	commands.Add(someCommand);
```
The third parameter is the minimum amount of required parameters, such as data file path.  The other parameters should be self-explanatory.

Then instantiate a CommandLineInstance object:
```c#
	CommandLineInstance commandLine = new (commands, arguments);
```
If all goes well and the supplied arguments pass validation, the active command object will be available, and you can get on your way:
```c#
	if (commandLine.ValidArguments == true)
	{
		Command command = commandLine.Command;

		switch (command.Name)
		{
			case "some-command":
				DoSomething(command);
				break;
		}
	}
```

Getting the associated additional parameters and options, can be done as follows:
```c#
	string dataFilePath = command.Parameters[0];

	bool dryRun = command.DoesOptionExist("n", "dryrun");
```

## Contributing

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

### Process:

1. Fork the Project
2. Create your Feature Branch (`git checkout -b feature/AmazingFeature`)
3. Commit your Changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the Branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Coding style
Please match the current coding style.  Most notably:
1. One operation per line
2. Use complete English words in variable and method names
3. Attempt to declare variable and method names in a self-documenting manner


## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

James John McGuire - [@jamesmc](https://twitter.com/jamesmc) - jamesjohnmcguire@gmail.com

Project Link: [https://github.com/jamesjohnmcguire/CommandLineCommands](https://github.com/jamesjohnmcguire/CommandLineCommands)
