/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineArguments.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DigitalZenWorks.CommandLine.Commands
{
	/// <summary>
	/// Represents a set of command line arguments.
	/// </summary>
	public class CommandLineArguments
	{
		private static readonly ILog Log = LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly string[] arguments;
		private readonly CommandLineInstance commandLineInstance;
		private readonly CommandsSet commands;
		private readonly IList<Command> commandsList;
		private readonly InferCommand inferCommand;

		private string errorMessage;
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

			commandLineInstance = new (this.commands, arguments);
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

			commandLineInstance = new (commands, arguments);
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
		}

		/// <summary>
		/// Gets the active command.
		/// </summary>
		/// <value>The active command.</value>
		public Command Command { get { return commandLineInstance.Command; } }

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
		public bool ValidArguments
		{
			get { return commandLineInstance.ValidArguments; }
		}

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
