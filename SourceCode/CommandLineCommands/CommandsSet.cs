/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandsSet.cs" company="James John McGuire">
// Copyright © 2022 - 2024 James John McGuire. All Rights Reserved.
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
	/// Represents a set of commands.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the
	/// <see cref="CommandsSet"/> class.
	/// </remarks>
	/// <param name="commands">A list of valid commands.</param>
	public class CommandsSet(IList<Command> commands)
	{
		private static readonly ILog Log = LogManager.GetLogger(
			System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		private readonly IList<Command> commands = commands;

		private bool useLog;

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
