/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineArguments.cs" company="James John McGuire">
// Copyright © 2022 - 2024 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

[assembly: CLSCompliant(true)]

namespace DigitalZenWorks.CommandLine.Commands
{
	/// <summary>
	/// Delegate to infer a command.
	/// </summary>
	/// <param name="argument">The first argument to check upon.</param>
	/// <param name="commands">The list of allowed commands.</param>
	/// <returns>The inferred command, if any.</returns>
	public delegate Command InferCommand(
		string argument, IList<Command> commands);

	/// <summary>
	/// Represents a set of command line arguments.
	/// </summary>
	public class CommandLineArguments
	{
		private static readonly ILog Log = LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly string[] arguments;
		private readonly CommandsSet commands;
		private readonly IList<Command> commandsList;
		private readonly InferCommand inferCommand;
		private readonly bool validArguments;

		private Command command;
		private string commandName;
		private string errorMessage;
		private string invalidOption;
		private bool useLog;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineArguments"/> class.
		/// </summary>
		/// <param name="commands">A list of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		public CommandLineArguments(
			IList<Command> commands, string[] arguments)
		{
			this.commandsList = commands;
			this.commands = new CommandsSet(commands);
			this.arguments = arguments;

			validArguments = ValidateArguments();
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineArguments"/> class.
		/// </summary>
		/// <param name="commands">The set of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		public CommandLineArguments(CommandsSet commands, string[] arguments)
		{
			this.commands = commands;
			this.arguments = arguments;

			validArguments = ValidateArguments();
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineArguments"/> class.
		/// </summary>
		/// <param name="commands">A list of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		/// <param name="inferCommand">The infer command delegate.</param>
		public CommandLineArguments(
			IList<Command> commands,
			string[] arguments,
			InferCommand inferCommand)
			: this(commands, arguments)
		{
			this.inferCommand = inferCommand;
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineArguments"/> class.
		/// </summary>
		/// <param name="commands">The set of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		/// <param name="inferCommand">The infer command delegate.</param>
		public CommandLineArguments(
			CommandsSet commands,
			string[] arguments,
			InferCommand inferCommand)
		{
			this.inferCommand = inferCommand;

			this.commands = commands;
			this.arguments = arguments;

			validArguments = ValidateArguments();
		}

		/// <summary>
		/// Gets the active command.
		/// </summary>
		/// <value>The active command.</value>
		public Command Command { get { return command; } }

		/// <summary>
		/// Gets the error message, if any.
		/// </summary>
		/// <value>The error message, if any.</value>
		public string ErrorMessage { get { return errorMessage; } }

		/// <summary>
		/// Gets or sets the usage statement.
		/// </summary>
		/// <value>The usage statement.</value>
		public string UsageStatement { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether indicates whether to use
		/// logging functionality.
		/// </summary>
		/// <value>A value indicating whether to use logging functionality.
		/// </value>
		public bool UseLog
		{
			get { return useLog; } set { useLog = value; }
		}

		/// <summary>
		/// Gets a value indicating whether a value indicating whether the
		/// arguments are valid or not.
		/// </summary>
		/// <value>A value indicating whether the arguments are valid
		/// or not.</value>
		public bool ValidArguments { get { return validArguments; } }

		/// <summary>
		/// Show help message.
		/// </summary>
		/// <param name="title">An optional title to display.</param>
		public void ShowHelp(string title = null)
		{
			if (!string.IsNullOrWhiteSpace(title))
			{
				Output(title);
				Output(string.Empty);
			}

			Output("Usage:");
			Output(UsageStatement);
			Output(string.Empty);

			int commandMaximumLength = 0;
			int descriptionMaximumLength = 0;

			foreach (Command command in commandsList)
			{
				commandMaximumLength = GetMaximumLength(
					commandMaximumLength, command.Name);
				descriptionMaximumLength = GetMaximumLength(
					descriptionMaximumLength, command.Description);
			}

			Command help = commandsList.SingleOrDefault(x => x.Name == "help");

			commandsList.Remove(help);

			IOrderedEnumerable<Command> sortedCommands =
				commandsList.OrderBy(x => x.Name);

			foreach (Command command in sortedCommands)
			{
				string options = string.Empty;
				bool first = true;

				if (command.Options != null)
				{
					foreach (CommandOption option in command.Options)
					{
						if (first == true)
						{
							first = false;
						}
						else
						{
							options += Environment.NewLine;

							int paddingAmount = commandMaximumLength +
									descriptionMaximumLength + 2;
							string padding = string.Empty;
							padding = padding.PadRight(paddingAmount, ' ');
							options += padding;
						}

						string optionMessage = string.Format(
							CultureInfo.InvariantCulture,
							"-{0}, --{1}",
							option.ShortName,
							option.LongName);
						options += optionMessage;
					}
				}

				string message = string.Format(
					CultureInfo.InvariantCulture,
					"{0} {1} {2}",
					command.Name.PadRight(commandMaximumLength, ' '),
					command.Description.PadRight(
						descriptionMaximumLength, ' '),
					options);
				Output(message);
			}

			if (help != null)
			{
				string helpMessage = string.Format(
					CultureInfo.InvariantCulture,
					"{0} {1}",
					help.Name.PadRight(commandMaximumLength, ' '),
					help.Description.PadRight(
						descriptionMaximumLength, ' '));
				Output(helpMessage);
			}
		}

		private static int GetMaximumLength(
			int previousMaximumLength, string text)
		{
			int maximumLength = Math.Max(previousMaximumLength, text.Length);

			return maximumLength;
		}

		private static bool IsHelpCommend(string command)
		{
			bool isHelp = false;

			string[] helpCommands = ["-?", "-h", "--help"];

			if (helpCommands.Contains(command))
			{
				isHelp = true;
			}

			return isHelp;
		}

		private bool AreOptionsValid(
			Command validatedCommand, out IList<CommandOption> commandOptions)
		{
			bool areOptionsValid = true;

			commandOptions = GetOptions();

			foreach (CommandOption option in commandOptions)
			{
				bool isValid = IsValidOption(validatedCommand, option);

				if (isValid == false)
				{
					areOptionsValid = false;
					break;
				}
			}

			return areOptionsValid;
		}

		private IList<CommandOption> GetOptions()
		{
			IList<CommandOption> options = [];

			for (int index = 0; index < arguments.Length; index++)
			{
				string argument = arguments[index];

#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
				if (argument.StartsWith('-'))
#else
				if (argument.StartsWith("-"))
#endif
				{
					CommandOption option = new ();

					string optionName = argument.TrimStart('-');

					if (argument.StartsWith("--", StringComparison.Ordinal))
					{
						option.LongName = optionName;
					}
					else
					{
						option.ShortName = optionName;
					}

					option.ArgumentIndex = index;

					options.Add(option);
				}
			}

			return options;
		}

		private IList<string> GetParameters(Command command)
		{
			IList<string> parameters = [];

			for (int index = 0; index < arguments.Length; index++)
			{
				string argument = arguments[index];

				if (argument.Equals(
					command.Name, StringComparison.OrdinalIgnoreCase))
				{
					continue;
				}

#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
				if (!argument.StartsWith('-'))
#else
				if (!argument.StartsWith("-"))
#endif
				{
					parameters.Add(argument);
				}
			}

			return parameters;
		}

		private bool IsValidOption(
			Command command, CommandOption option)
		{
			bool isValid = false;

			foreach (CommandOption validOption in command.Options)
			{
				if ((option.LongName != null && option.LongName.Equals(
						validOption.LongName, StringComparison.Ordinal)) ||
					(option.ShortName != null && option.ShortName.Equals(
						validOption.ShortName, StringComparison.Ordinal)))
				{
					if (validOption.RequiresParameter == true)
					{
						// Subtract one for the needed parameter afterwards.
						if (option.ArgumentIndex < arguments.Length - 1)
						{
							option.Parameter =
								arguments[option.ArgumentIndex + 1];

							isValid = true;
							break;
						}
					}
					else
					{
						isValid = true;
						break;
					}
				}
			}

			if (isValid == false)
			{
				if (!string.IsNullOrWhiteSpace(option.LongName))
				{
					invalidOption = option.LongName;
				}
				else
				{
					invalidOption = option.ShortName;
				}
			}

			return isValid;
		}

		private bool ValidateArguments()
		{
			bool areValid = false;
			bool isValidCommand = false;
			Command validatedCommand = null;
			IList<CommandOption> commandOptions = null;
			IList<string> parameters = null;

			if (arguments == null || arguments.Length < 1)
			{
				errorMessage = "There are no aguments given.";
			}
			else
			{
				commandName = arguments[0];

				foreach (Command validCommand in commands.Commands)
				{
					if (commandName.Equals(
						validCommand.Name, StringComparison.Ordinal))
					{
						validatedCommand = validCommand;
						isValidCommand = true;
						break;
					}
				}

				if (isValidCommand == false && inferCommand != null)
				{
					Command inferredCommand =
						inferCommand(commandName, commands.Commands);

					if (inferredCommand != null)
					{
						validatedCommand = inferredCommand;
						isValidCommand = true;
					}
				}

				bool isHelpCommand = IsHelpCommend(commandName);

				if (isValidCommand == false && isHelpCommand == true)
				{
					IList<Command> commandList = commands.Commands;

					Command help =
						commandList.SingleOrDefault(x => x.Name == "help");
					validatedCommand = help;
					isValidCommand = true;
					areValid = true;
				}

				if (isValidCommand == false)
				{
					errorMessage = "Unknown command.";
				}
				else if (isHelpCommand == false)
				{
					bool areOptionsValid =
						AreOptionsValid(validatedCommand, out commandOptions);

					if (areOptionsValid == false)
					{
						errorMessage = string.Format(
							CultureInfo.InvariantCulture,
							"Unknown option:{0}.",
							invalidOption);
					}
					else
					{
						parameters = GetParameters(validatedCommand);

						if (parameters.Count >=
							validatedCommand.ParameterCount)
						{
							areValid = true;
						}
						else
						{
							errorMessage = "Incorrect amount of parameters.";
						}
					}
				}

				if (areValid == true)
				{
					command = new (
						validatedCommand.Name, commandOptions, parameters);
				}
			}

			return areValid;
		}

		private void Output(string message)
		{
			if (useLog == true)
			{
				Log.Info(message);
			}
			else
			{
				Console.WriteLine(message);
			}
		}
	}
}
