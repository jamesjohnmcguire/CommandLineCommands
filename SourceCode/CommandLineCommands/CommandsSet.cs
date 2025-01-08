/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandsSet.cs" company="James John McGuire">
// Copyright © 2022 - 2024 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace DigitalZenWorks.CommandLine.Commands
{
	/// <summary>
	/// Represents a set of commands.
	/// </summary>
	public class CommandsSet
	{
		private static readonly ILog Log = LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private IList<Command> commands;

		private bool useLog;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandsSet"/> class.
		/// </summary>
		public CommandsSet()
		{
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandsSet"/> class.
		/// </summary>
		/// <param name="commands">A list of valid commands.</param>
		public CommandsSet(IList<Command> commands)
		{
			this.commands = commands;
		}

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="CommandsSet"/> class.
		/// </summary>
		/// <param name="commandsJson">A list of valid commands in
		/// JSON format.</param>
		public CommandsSet(string commandsJson)
		{
			IList<Command> commands =
				JsonConvert.DeserializeObject<IList<Command>>(commandsJson);
			this.commands = commands;
		}

		/// <summary>
		/// Gets the list of commands.
		/// </summary>
		/// <value>The list of commands.</value>
		public IList<Command> Commands { get => commands; }

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
			get { return useLog; }
			set { useLog = value; }
		}

		/// <summary>
		/// Read JSON commands from a file.
		/// </summary>
		/// <param name="filePath">The file to read from.</param>
		/// <exception cref="FileNotFoundException">Thrown if file is
		/// not found.</exception>
		public void JsonFromFile(string filePath)
		{
			bool exists = File.Exists(filePath);

			if (exists == true)
			{
				string jsonText = File.ReadAllText(filePath);

				IList<Command> commands =
					JsonConvert.DeserializeObject<IList<Command>>(jsonText);
				this.commands = commands;
			}
			else
			{
				throw new FileNotFoundException("File Not Found.");
			}
		}

		/// <summary>
		/// Gets the complete help message.
		/// </summary>
		/// <param name="title">An optional title to display.</param>
		/// <returns>The complete help message.</returns>
		public string GetHelp(string title = null)
		{
			string message = string.Empty;

			StringBuilder buffer = new ();

			if (!string.IsNullOrWhiteSpace(title))
			{
				buffer.AppendLine(title);
				buffer.AppendLine(string.Empty);
			}

			buffer.AppendLine("Usage:");
			buffer.AppendLine(UsageStatement);
			buffer.AppendLine(string.Empty);

			int commandMaximumLength = 0;
			int descriptionMaximumLength = 0;

			foreach (Command command in commands)
			{
				commandMaximumLength = GetMaximumLength(
					commandMaximumLength, command.Name);
				descriptionMaximumLength = GetMaximumLength(
					descriptionMaximumLength, command.Description);
			}

			Command help = commands.SingleOrDefault(x => x.Name == "help");
			commands.Remove(help);

			IOrderedEnumerable<Command> sortedCommands =
				commands.OrderBy(x => x.Name);

			foreach (Command command in sortedCommands)
			{
				string commandText = GetCommandText(
					command, commandMaximumLength, descriptionMaximumLength);
				buffer.AppendLine(commandText);
			}

			if (help != null)
			{
				string helpMessage = string.Format(
					CultureInfo.InvariantCulture,
					"{0} {1}",
					help.Name.PadRight(commandMaximumLength, ' '),
					help.Description.PadRight(
						descriptionMaximumLength, ' '));
				buffer.AppendLine(helpMessage);
			}

			message = buffer.ToString();

			return message;
		}

		/// <summary>
		/// Show help message.
		/// </summary>
		/// <param name="title">An optional title to display.</param>
		public void ShowHelp(string title = null)
		{
			string helpMessage = GetHelp(title);

			Output(helpMessage);
		}

		private static string GetCommandText(
			Command command,
			int commandMaximumLength,
			int descriptionMaximumLength)
		{
			string commandText = string.Empty;

			string options = string.Empty;
			bool first = true;

			if (command.Options != null)
			{
				foreach (CommandOption option in command.Options)
				{
					options += GetOptionText(
						option,
						commandMaximumLength,
						descriptionMaximumLength,
						first);

					first = false;
				}
			}

			string paddedName = new (' ', commandMaximumLength);

			if (command.Name != null)
			{
				paddedName = command.Name.PadRight(commandMaximumLength, ' ');
			}

			string paddedDescription = new (' ', descriptionMaximumLength);

			if (command.Description != null)
			{
				paddedDescription = command.Description.PadRight(
					descriptionMaximumLength, ' ');
			}

			commandText = string.Format(
				CultureInfo.InvariantCulture,
				"{0} {1} {2}",
				paddedName,
				paddedDescription,
				options);

			return commandText;
		}

		private static int GetMaximumLength(
			int previousMaximumLength, string text)
		{
			int maximumLength = previousMaximumLength;

			if (text != null)
			{
				maximumLength = Math.Max(previousMaximumLength, text.Length);
			}

			return maximumLength;
		}

		private static string GetOptionText(
			CommandOption option,
			int commandMaximumLength,
			int descriptionMaximumLength,
			bool first)
		{
			string optionText = string.Empty;

			if (first == false)
			{
				optionText += Environment.NewLine;

				string padding = string.Empty;

				int paddingAmount = commandMaximumLength +
						descriptionMaximumLength + 2;
				padding = padding.PadRight(paddingAmount, ' ');
				optionText += padding;
			}

			string optionMessage = string.Format(
				CultureInfo.InvariantCulture,
				"-{0}, --{1}",
				option.ShortName,
				option.LongName);

			optionText += optionMessage;

			return optionText;
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
