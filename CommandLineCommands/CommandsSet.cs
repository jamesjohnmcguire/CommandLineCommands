/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandsSet.cs" company="James John McGuire">
// Copyright © 2022 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.CommandLine.Commands
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using Common.Logging;
	using Newtonsoft.Json;

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
		/// <returns>A list of commands.</returns>
		public IList<Command> JsonFromFile(string filePath)
		{
			IList<Command> commands;

			bool exists = File.Exists(filePath);

			if (exists == true)
			{
				string jsonText = File.ReadAllText(filePath);

				commands =
					JsonConvert.DeserializeObject<IList<Command>>(jsonText);
				this.commands = commands;
			}
			else
			{
				throw new FileNotFoundException("File Not Found.");
			}

			return commands;
		}

		/// <summary>
		/// Gets the complete help message.
		/// </summary>
		/// <param name="title">An optional title to display.</param>
		/// <returns>The complete help message.</returns>
		public string GetHelp(string title = null)
		{
			HelpManager helpManager = new (title, commands);

			string message = helpManager.HelpText;

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
