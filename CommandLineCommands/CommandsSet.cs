/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandsSet.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
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

		private int commandMaximumLength;
		private IList<Command> commands;
		private int descriptionMaximumLength;
		private int optionsMaximumLength;
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

			GetMaximumLengths();

			Command help = commands.SingleOrDefault(x => x.Name == "help");
			commands.Remove(help);

			string headerText = GetHelpHeader(title);
			buffer.AppendLine(headerText);

			IOrderedEnumerable<Command> sortedCommands =
				commands.OrderBy(x => x.Name);

			foreach (Command command in sortedCommands)
			{
				string commandText = GetCommandText(
					command,
					commandMaximumLength,
					descriptionMaximumLength,
					optionsMaximumLength);
				buffer.AppendLine(commandText);
			}

			if (help != null)
			{
				string helpMessage = GetFormattedCommandText(
					help.Name,
					help.Description,
					string.Empty,
					commandMaximumLength,
					0);
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
			int descriptionMaximumLength,
			int optionsMaximumLength)
		{
			string optionsParametersText = GetOptionsParametersText(
				command.Options,
				command.Parameters,
				commandMaximumLength,
				descriptionMaximumLength,
				optionsMaximumLength);

			string commandText = GetFormattedCommandText(
				command.Name,
				command.Description,
				optionsParametersText,
				commandMaximumLength,
				descriptionMaximumLength);

			return commandText;
		}

		private static string GetFormattedCommandText(
			string name,
			string description,
			string optionsParametersText,
			int commandMaximumLength,
			int descriptionMaximumLength)
		{
			string paddedName = new (' ', commandMaximumLength);

			if (name != null)
			{
				paddedName = name.PadRight(commandMaximumLength, ' ');
			}

			string paddedDescription = new (' ', descriptionMaximumLength);

			if (description != null)
			{
				paddedDescription = description.PadRight(
					descriptionMaximumLength, ' ');
			}

			string commandText = string.Format(
				CultureInfo.InvariantCulture,
				"{0} {1} {2}",
				paddedName,
				paddedDescription,
				optionsParametersText);

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

		private static int GetMaximumLength(
			int previousMaximumLength, IList<CommandOption> options)
		{
			int maximumLength = previousMaximumLength;

			foreach (CommandOption option in options)
			{
				int optionLength = 0;

				if (option.LongName != null)
				{
					optionLength = option.LongName.Length;

					// 2 for the dashes, 1 for space
					optionLength += 3;
				}

				if (option.ShortName != null)
				{
					optionLength += option.ShortName.Length;

					// 1 for the dash, 1 for space
					optionLength += 2;
				}

				maximumLength =
					Math.Max(maximumLength, optionLength);
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

				string padding = GetPadding(
					commandMaximumLength, descriptionMaximumLength);
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

		private static string GetOptionsParametersText(
			IList<CommandOption> options,
			IList<string> parameters,
			int commandMaximumLength,
			int descriptionMaximumLength,
			int optionsMaximumLength)
		{
			string text = string.Empty;
			bool first = true;

			int lines = Math.Max(options.Count, parameters.Count);

			for (int index = 0; index < lines; index++)
			{
				string optionText = string.Empty;
				string parameterText = string.Empty;

				if (options.Count > index)
				{
					CommandOption option = options[index];

					optionText = GetOptionText(
						option,
						commandMaximumLength,
						descriptionMaximumLength,
						first);
				}

				if (parameters.Count > index)
				{
					string parameter = parameters[index];

					parameterText = GetParameterText(
						parameter,
						commandMaximumLength,
						descriptionMaximumLength,
						optionsMaximumLength + 1,
						optionText.Length,
						first);
				}

				text += optionText + parameterText;
				first = false;
			}

			return text;
		}

		private static string GetPadding(
			int commandMaximumLength, int descriptionMaximumLength)
		{
			int paddingAmount = commandMaximumLength +
					descriptionMaximumLength + 2;

			string padding = new (' ', paddingAmount);

			return padding;
		}

		private static string GetPadding(
			int commandMaximumLength,
			int descriptionMaximumLength,
			int optionsMaximumLength)
		{
			int paddingAmount = commandMaximumLength +
					descriptionMaximumLength + optionsMaximumLength + 2;

			string padding = new (' ', paddingAmount);

			return padding;
		}

		private static string GetParameterText(
			string parameter,
			int commandMaximumLength,
			int descriptionMaximumLength,
			int optionsMaximumLength,
			int optionLength,
			bool first)
		{
			string parameterText = string.Empty;

			string padding = string.Empty;

			if (first == false)
			{
				parameterText += Environment.NewLine;

				padding = GetPadding(
					commandMaximumLength,
					descriptionMaximumLength,
					optionsMaximumLength);
			}
			else
			{
				int paddingAmount =
					optionsMaximumLength - optionLength;
				padding = padding.PadRight(paddingAmount, ' ');
			}

			parameterText += padding;
			parameterText += parameter;

			return parameterText;
		}

		private string GetHelpHeader(string title)
		{
			StringBuilder buffer = new ();

			if (!string.IsNullOrWhiteSpace(title))
			{
				buffer.AppendLine(title);
				buffer.AppendLine(string.Empty);
			}

			buffer.AppendLine("Usage:");
			string usage = GetFormattedCommandText(
				"Command",
				"Description",
				"Options",
				commandMaximumLength,
				descriptionMaximumLength);

			int paddingAmount =
				optionsMaximumLength - 6;
			string padding = new (' ', paddingAmount);
			usage += padding + "Parameters";

			buffer.AppendLine(usage);
			buffer.AppendLine(string.Empty);

			string headerText = buffer.ToString();

			return headerText;
		}

		private void GetMaximumLengths()
		{
			foreach (Command command in commands)
			{
				commandMaximumLength = GetMaximumLength(
					commandMaximumLength, command.Name);
				descriptionMaximumLength = GetMaximumLength(
					descriptionMaximumLength, command.Description);
				optionsMaximumLength = GetMaximumLength(
					optionsMaximumLength, command.Options);
			}
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
