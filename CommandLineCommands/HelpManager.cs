/////////////////////////////////////////////////////////////////////////////
// <copyright file="HelpManager.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

#nullable enable

namespace DigitalZenWorks.CommandLine.Commands
{
	using System;
	using System.Globalization;
	using System.Xml.Linq;

	/// <summary>
	/// The help manager class.
	/// </summary>
	public class HelpManager
	{
		private readonly CommandsSet commandSet;

		// For 'Command' length.
		private int commandColumnLength = 7;

		private int descriptionColumnLength;

		private int optionsColumnLength;

		private int parametersColumnLength;

		/// <summary>
		/// Initializes a new instance of the
		/// <see cref="HelpManager"/> class.
		/// </summary>
		/// <param name="commandSet">The command set.</param>
		public HelpManager(CommandsSet commandSet)
		{
			this.commandSet = commandSet;

			if (commandSet != null)
			{
				foreach (Command command in commandSet.Commands)
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
		}

		/// <summary>
		/// Get the help header text.
		/// </summary>
		/// <param name="title">The title of the application.</param>
		/// <returns>The help header text.</returns>
		public string GetHelpHeaderText(string title)
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

		private static int GetOptionsMaximumLength(
			int currentColumnLength, Command command)
		{
			int optionsColumnLength = currentColumnLength;

			if (command.Options != null)
			{
				foreach (CommandOption option in command.Options)
				{
					string optionName = option.LongName;

					optionsColumnLength = Math.Max(
						option.LongName.Length, optionsColumnLength);
				}
			}

			return optionsColumnLength;
		}

		private static int GetParametersMaximumLength(
			int currentColumnLength, Command command)
		{
			int parametersColumnLength = currentColumnLength;

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

		private static string PadColumn(string column, int columnLength)
		{
			int paddingLength =
				Math.Max(column.Length, columnLength);

			// For column separator.
			paddingLength++;

			column = column.PadRight(paddingLength);

			return column;
		}

		private string GetHeaderColumns()
		{
			string columnName = "Command";
			string headerColumns = columnName;
			bool addPadding = false;
			string columnText;

			if (descriptionColumnLength > 0 || optionsColumnLength > 0 ||
				parametersColumnLength > 0)
			{
				headerColumns = PadColumn(headerColumns, commandColumnLength);

				if (descriptionColumnLength > 0)
				{
					columnName = "Description";

					if (optionsColumnLength > 0 || parametersColumnLength > 0)
					{
						addPadding = true;

						// Compensate for the header name too.
						descriptionColumnLength =
							Math.Max(columnName.Length, descriptionColumnLength);
					}

					columnText = GetPaddedColumn(
						columnName, descriptionColumnLength, addPadding);
					headerColumns += columnText;
				}

				if (optionsColumnLength > 0)
				{
					columnName = "Options";

					if (parametersColumnLength > 0)
					{
						columnText = GetPaddedColumn(
							columnName, optionsColumnLength, true);
						headerColumns += columnText;

						headerColumns += "Parameters";
					}
					else
					{
						headerColumns += "Options";
					}
				}
				else if (parametersColumnLength > 0)
				{
					headerColumns += "Parameters";
				}
			}

			return headerColumns;
		}

		private string GetHeaderNameColumn()
		{
			string nameColumn = "Command";

			return nameColumn;
		}

		private int GetOptionsMaximumLength(Command command)
		{
			if (command.Options != null)
			{
				foreach (CommandOption option in command.Options)
				{
					string optionName = option.LongName;

					optionsColumnLength = Math.Max(
						option.LongName.Length, optionsColumnLength);
				}
			}

			return optionsColumnLength;
		}

		private string AddParametersHeaderColumn(string headerColumns)
		{
			string text = string.Empty;

			if (parametersColumnLength > 0)
			{
				headerColumns = PadColumn(
					headerColumns, optionsColumnLength);

				headerColumns += "Parameters";
			}

			return headerColumns;
		}

		private string GetPaddedColumn(
			string columnText, int columnLength, bool addPadding)
		{
			if (addPadding == true)
			{
				columnText = PadColumn(columnText, columnLength);
			}

			return columnText;
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
