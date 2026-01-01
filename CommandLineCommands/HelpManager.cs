/////////////////////////////////////////////////////////////////////////////
// <copyright file="HelpManager.cs" company="James John McGuire">
// Copyright © 2022 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.CommandLine.Commands
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;

	/// <summary>
	/// The help manager class.
	/// </summary>
	public class HelpManager
	{
		private readonly IList<Command> commands;
		private readonly string title;

		// Use 7 for 'Command' length.
		private int commandColumnLength = 7;

		private int descriptionColumnLength;

		private string? helpText;

		private int optionsColumnLength;

		private int parametersColumnLength;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="HelpManager"/> class.
		/// </summary>
		/// <param name="applicationTitle">The application title.</param>
		/// <param name="commands">The commands list.</param>
		public HelpManager(string applicationTitle, IList<Command> commands)
		{
			this.title = applicationTitle;
			this.commands = commands;

			if (commands != null)
			{
				GetColumnLengths(commands);
			}
		}

		/// <summary>
		/// Gets the command informationhelp text.
		/// </summary>
		public string CommandsInformation
		{
			get
			{
				string commandsInformation = string.Empty;

				if (commands != null)
				{
					foreach (Command command in commands)
					{
						commandsInformation += GetCommandInformation(command);
					}
				}

				return commandsInformation;
			}
		}

		/// <summary>
		/// Gets the help header text.
		/// </summary>
		/// <returns>The help header text.</returns>
		public string HelpHeaderText
		{
			get
			{
				string headerText = string.Format(
					CultureInfo.InvariantCulture,
					"{0}{1}{1}Usage:{1}",
					title,
					Environment.NewLine);

				headerText += GetHeaderColumns();
				headerText += Environment.NewLine;

				return headerText;
			}
		}

		/// <summary>
		/// Gets the help text associated with this instance.
		/// </summary>
		public string HelpText
		{
			get
			{
				helpText ??= HelpHeaderText + CommandsInformation;

				return helpText;
			}
		}

		private static string GetCommandLineParameter(
			Command command, int lineIndex)
		{
			string lineParameter = string.Empty;

			if (command.Parameters.Count > lineIndex)
			{
				string rawParameter = command.Parameters[lineIndex];
				lineParameter = string.Format(
					CultureInfo.InvariantCulture,
					"<{0}>",
					rawParameter);
			}

			return lineParameter;
		}

		private static string PadColumn(string column, int columnLength)
		{
			int paddingLength =
				Math.Max(column.Length, columnLength);

			// For column separator.
			paddingLength++;

			column = column.PadRight(paddingLength);

			return column;
		}

		private string GetColumnDescription(Command command)
		{
			string columnText = string.Empty;

			if (descriptionColumnLength > 0)
			{
				columnText = command.Description;

				if ((command.Options.Count > 0 ||
					command.Parameters.Count > 0) &&
					(optionsColumnLength > 0 || parametersColumnLength > 0))
				{
					columnText = PadColumn(
						command.Description, descriptionColumnLength);
				}
			}

			return columnText;
		}

		private void GetColumnLengths(IList<Command> commandsList)
		{
			foreach (Command command in commandsList)
			{
				string description = command.Description;
				string name = command.Name;

				commandColumnLength =
					Math.Max(name.Length, commandColumnLength);

				if (description != null)
				{
					descriptionColumnLength = Math.Max(
						description.Length, descriptionColumnLength);
				}

				optionsColumnLength = GetOptionsMaximumLength(command);

				parametersColumnLength =
					GetParametersMaximumLength(command);
			}
		}

		private string GetColumnName(Command command)
		{
			string columnText = command.Name;

			if (descriptionColumnLength > 0 || optionsColumnLength > 0 ||
				parametersColumnLength > 0)
			{
				columnText = PadColumn(columnText, commandColumnLength);
			}

			return columnText;
		}

		private string GetColumnOption(Command command, int optionIndex)
		{
			string columnText;

			CommandOption option = command.Options[optionIndex];

			string optionMessage = string.Format(
				CultureInfo.InvariantCulture,
				"-{0}, --{1}",
				option.ShortName,
				option.LongName);

			if (option.RequiresParameter == true)
			{
				optionMessage += " <option>";
			}

			if (command.Parameters.Count > 0 && parametersColumnLength > 0)
			{
				columnText = PadColumn(optionMessage, optionsColumnLength);
			}
			else
			{
				columnText = optionMessage;
			}

			return columnText;
		}

		private string GetCommandInformation(Command command)
		{
			string commandInformation = string.Empty;

			int linesCount = 1;
			int optionalsCount = Math.Max(
				command.Options.Count, command.Parameters.Count);
			linesCount = Math.Max(linesCount, optionalsCount);

			for (int index = 0; index < linesCount; index++)
			{
				string line = GetCommandLine(command, index);
				commandInformation += line + Environment.NewLine;
			}

			return commandInformation;
		}

		private string GetCommandLine(Command command, int lineIndex)
		{
			string line = string.Empty;

			if (lineIndex == 0)
			{
				line = GetColumnName(command);
				line += GetColumnDescription(command);
			}
			else
			{
				line = PadColumn(line, commandColumnLength);
				line += PadColumn(line, descriptionColumnLength);
			}

			string lineOption = GetCommandLineOption(command, lineIndex);
			line += lineOption;

			string lineParameter = GetCommandLineParameter(command, lineIndex);
			line += lineParameter;

			return line;
		}

		private string GetCommandLineOption(Command command, int lineIndex)
		{
			string lineOption = string.Empty;

			if (command.Options.Count > lineIndex)
			{
				lineOption = GetColumnOption(command, lineIndex);
			}
			else if (command.Parameters.Count > lineIndex &&
				optionsColumnLength > 0)
			{
				lineOption = PadColumn(lineOption, optionsColumnLength);
			}

			return lineOption;
		}

		private string GetHeaderColumnDescription()
		{
			string columnHeaderText = string.Empty;

			if (descriptionColumnLength > 0)
			{
				columnHeaderText = "Description";

				if (optionsColumnLength > 0 || parametersColumnLength > 0)
				{
					// Compensate for the header name too.
					descriptionColumnLength = Math.Max(
						columnHeaderText.Length,
						descriptionColumnLength);

					columnHeaderText =
						PadColumn(columnHeaderText, descriptionColumnLength);
				}
			}

			return columnHeaderText;
		}

		private string GetHeaderColumnName()
		{
			string columnHeaderText = "Command";

			if (descriptionColumnLength > 0 || optionsColumnLength > 0 ||
				parametersColumnLength > 0)
			{
				columnHeaderText =
					PadColumn(columnHeaderText, commandColumnLength);
			}

			return columnHeaderText;
		}

		private string GetHeaderColumnOptions()
		{
			string columnHeaderText = string.Empty;

			if (optionsColumnLength > 0)
			{
				columnHeaderText = "Options";

				if (parametersColumnLength > 0)
				{
					columnHeaderText =
						PadColumn(columnHeaderText, optionsColumnLength);
				}
			}

			return columnHeaderText;
		}

		private string GetHeaderColumnParameters()
		{
			string columnHeaderText = string.Empty;

			if (parametersColumnLength > 0)
			{
				columnHeaderText = "Parameters";
			}

			return columnHeaderText;
		}

		private string GetHeaderColumns()
		{
			string columnText = GetHeaderColumnName();
			string headerColumns = columnText;

			columnText = GetHeaderColumnDescription();
			headerColumns += columnText;

			columnText = GetHeaderColumnOptions();
			headerColumns += columnText;

			columnText = GetHeaderColumnParameters();
			headerColumns += columnText;

			return headerColumns;
		}

		private int GetOptionsMaximumLength(Command command)
		{
			if (command.Options != null)
			{
				foreach (CommandOption option in command.Options)
				{
					string optionName = option.LongName;

					// Add 6, for additional formatting and short name.
					int actualOptionLength = optionName.Length + 6;

					if (option.RequiresParameter == true)
					{
						// Add 9, for <option> text.
						actualOptionLength += 9;
					}

					optionsColumnLength = Math.Max(
						actualOptionLength, optionsColumnLength);
				}
			}

			return optionsColumnLength;
		}

		private int GetParametersMaximumLength(Command command)
		{
			if (command.Parameters != null)
			{
				foreach (string parameter in command.Parameters)
				{
					parametersColumnLength = Math.Max(
						parameter.Length, parametersColumnLength);
				}
			}

			return parametersColumnLength;
		}
	}
}
