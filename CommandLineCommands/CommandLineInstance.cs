/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineInstance.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

[assembly: System.CLSCompliant(true)]

namespace DigitalZenWorks.CommandLine.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;

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
	public class CommandLineInstance
	{
		private readonly string commandName;

		private string[] arguments;
		private Command command;
		private CommandsSet commands;
		private string errorMessage;
		private InferCommand inferCommand;
		private bool useLog;
		private bool validArguments;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineInstance"/> class.
		/// </summary>
		/// <param name="commandsList">A list of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		public CommandLineInstance(
			IList<Command> commandsList, string[] arguments)
		{
			CommandsSet commands = new (commandsList);
			Initialize(commands, arguments);
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineInstance"/> class.
		/// </summary>
		/// <param name="commands">The set of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		public CommandLineInstance(CommandsSet commands, string[] arguments)
		{
			Initialize(commands, arguments);
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineInstance"/> class.
		/// </summary>
		/// <param name="commands">A list of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		/// <param name="inferCommand">The infer command delegate.</param>
		public CommandLineInstance(
			IList<Command> commands,
			string[] arguments,
			InferCommand inferCommand)
			: this(commands, arguments)
		{
			this.inferCommand = inferCommand;
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandLineInstance"/> class.
		/// </summary>
		/// <param name="commands">The set of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		/// <param name="inferCommand">The infer command delegate.</param>
		public CommandLineInstance(
			CommandsSet commands,
			string[] arguments,
			InferCommand inferCommand)
		{
			this.inferCommand = inferCommand;

			Initialize(commands, arguments);
		}

		/// <summary>
		/// Gets the active command.
		/// </summary>
		/// <value>The active command.</value>
		public Command Command
		{
			get { return command; }
		}

		/// <summary>
		/// Gets the error message, if any.
		/// </summary>
		/// <value>The error message, if any.</value>
		public string ErrorMessage
		{
			get { return errorMessage; }
		}

		/// <summary>
		/// Gets or sets the inferred command delegate.
		/// </summary>
		/// <value>The inferred command delegate.</value>
		public InferCommand InferCommand
		{
			get { return inferCommand; }
			set { inferCommand = value; }
		}

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
		public bool ValidArguments
		{
			get { return validArguments; }
		}

		/// <summary>
		/// Show help message.
		/// </summary>
		/// <param name="title">An optional title to display.</param>
		public void ShowHelp(string title = null)
		{
			commands.ShowHelp(title);
		}

		private static CommandOption GetCommandOption(
			IList<CommandOption> options,
			string optionName,
			bool isLongFormOption)
		{
			CommandOption option = null;

			optionName = optionName.TrimStart('-');

			foreach (CommandOption commandOption in options)
			{
				if (isLongFormOption == true &&
					commandOption.LongName == optionName)
				{
					option = commandOption;
					break;
				}
				else if (commandOption.ShortName == optionName)
				{
					option = commandOption;
					break;
				}
			}

			return option;
		}

		private static string GetOptionName(
			string argument, bool isLongFormOption)
		{
			string optionName;

			if (isLongFormOption == true)
			{
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
				optionName = argument[2..];
#else
				optionName = argument.Substring(2);
#endif

			}
			else
			{
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
				optionName = argument[1..];
#else
				optionName = argument.Substring(1);
#endif
			}

			return optionName;
		}

		private static bool IsHelpCommand(string[] arguments)
		{
			bool isHelp = false;

			if (arguments != null && arguments.Length > 0)
			{
				string command = arguments[0];

				if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
				{
					isHelp = true;
				}
				else
				{
					string[] helpCommands = ["-?", "-h", "--help"];

					isHelp = arguments.Any(argument => helpCommands.Contains(
						argument, StringComparer.OrdinalIgnoreCase));
				}
			}

			return isHelp;
		}

		private static bool IsLongFormOption(string argument)
		{
			bool isLongFormOption = false;

			if (argument.StartsWith("--", StringComparison.Ordinal))
			{
				isLongFormOption = true;
			}

			return isLongFormOption;
		}

		private static bool IsOption(string argument)
		{
			bool isOption = false;

#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
			if (argument.StartsWith('-'))
#else
			if (argument.StartsWith("-"))
#endif
			{
				isOption = true;
			}

			return isOption;
		}

		private static bool IsValidOption(
			IList<CommandOption> options, string[] arguments, int index)
		{
			bool isValidOption = false;

			if (arguments.Length > index)
			{
				string argument = arguments[index];

				bool isOption = IsOption(argument);

				if (isOption == true)
				{
					bool isLongFormOption = IsLongFormOption(argument);

					string optionName =
						GetOptionName(argument, isLongFormOption);

					CommandOption option = GetCommandOption(
						options,
						optionName,
						isLongFormOption);

					if (option != null)
					{
						isValidOption = IsValidRequiredParameter(
							option, arguments, index);
					}
				}
			}

			return isValidOption;
		}

		private static bool IsValidRequiredParameter(
			CommandOption option, string[] arguments, int index)
		{
			bool isValidRequiredParameter = false;

			if (option.RequiresParameter == true)
			{
				// Ensure there's a next argument and it's
				// not another option
				int nextIndex = index + 1;

				if (arguments.Length > nextIndex)
				{
					bool isOption = IsOption(arguments[nextIndex]);

					if (isOption == false)
					{
						isValidRequiredParameter = true;
					}
				}
			}
			else
			{
				isValidRequiredParameter = true;
			}

			return isValidRequiredParameter;
		}

		private Command GetCommand(string commandName)
		{
			Command command = null;
			Command commandTemplate = GetCommandTemplate(commandName);

			if (commandTemplate != null)
			{
				IList<CommandOption> options = [];

				command = new Command(
					commandName,
					options,
					commandTemplate.ParameterCount,
					commandTemplate.Description);
			}

			return command;
		}

		private Command GetCommand(string[] arguments)
		{
			Command command = null;
			bool isHelpCommand = IsHelpCommand(arguments);

			if (isHelpCommand == true)
			{
				command = new ("help");
			}
			else
			{
				bool isInferCommand = IsInferCommand();

				if (isInferCommand == true)
				{
					command = inferCommand(commandName, commands.Commands);
				}
				else
				{
					command = GetCommand(arguments[0]);
				}
			}

			return command;
		}

		private Command GetCommandTemplate(string commandName)
		{
			Command command = null;

			foreach (Command commandTemplate in commands.Commands)
			{
				if (commandName.Equals(
					commandTemplate.Name, StringComparison.Ordinal))
				{
					command = commandTemplate;
					break;
				}
			}

			return command;
		}

		private void Initialize(CommandsSet commands, string[] arguments)
		{
			this.commands = commands;
			this.arguments = arguments;

			validArguments = ProcessArguments();
		}

		private bool IsInferCommand()
		{
			bool isInferCommand = false;

			if (inferCommand != null)
			{
				Command inferredCommand =
					inferCommand(commandName, commands.Commands);

				if (inferredCommand != null)
				{
					this.command = inferredCommand;
					isInferCommand = true;
				}
			}

			return isInferCommand;
		}

		private bool PostArgumentsCheck(
			bool argumentsFailed, int paramaterCount, Command command)
		{
			bool validArguments = false;

			if (argumentsFailed == false)
			{
				if (paramaterCount >= command.ParameterCount)
				{
					validArguments = true;
				}
				else
				{
					errorMessage = "Incorrect amount of parameters.";
				}
			}

			return validArguments;
		}

		private bool ProcessArgument(
			string argument,
			Command commandTemplate,
			ref int index,
			ref int paramaterCount)
		{
			bool validArgument = false;

			bool isOption = IsOption(argument);

			if (isOption == true)
			{
				bool isValidOption = IsValidOption(
					commandTemplate.Options, arguments, index);

				if (isValidOption == false)
				{
					errorMessage = string.Format(
						CultureInfo.InvariantCulture,
						"Unknown option:{0}.",
						argument);
				}
				else
				{
					bool isLongFormOption =
						IsLongFormOption(argument);

					CommandOption option = GetCommandOption(
						commandTemplate.Options,
						argument,
						isLongFormOption);

					if (option != null)
					{
						this.command.Options.Add(option);

						if (option.RequiresParameter == true)
						{
							// The next argument is taken.
							index++;
							option.Parameter =
								arguments[index];
						}

						validArgument = true;
					}
				}
			}
			else
			{
				// Assuming remaining arguments as parameters.
				this.command.Parameters.Add(argument);
				paramaterCount++;
				validArgument = true;
			}

			return validArgument;
		}

		private bool ProcessArguments()
		{
			bool validArguments = false;
			int paramaterCount = 0;

			if (arguments == null || arguments.Length < 1)
			{
				errorMessage = "There are no arguments given.";
			}
			else
			{
				Command command = GetCommand(arguments);

				if (command == null)
				{
					errorMessage = "Unknown command.";
				}
				else
				{
					this.command = command;

					bool failed = false;
					Command commandTemplate = GetCommandTemplate(command.Name);

					// Already processed first argument
					for (int index = 1; index < arguments.Length; index++)
					{
						string argument = arguments[index];

						bool validArgument = ProcessArgument(
							argument,
							commandTemplate,
							ref index,
							ref paramaterCount);

						if (validArgument == false)
						{
							failed = true;
							break;
						}
					}

					validArguments =
						PostArgumentsCheck(failed, paramaterCount, command);
				}
			}

			return validArguments;
		}
	}
}
