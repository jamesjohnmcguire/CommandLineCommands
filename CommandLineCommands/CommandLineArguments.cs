/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineArguments.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.CommandLine.Commands
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Represents a set of command line arguments.
	/// </summary>
	[Obsolete("CommandLineArguments is deprecated, " +
		"please use CommandLineInstance instead.")]
	public class CommandLineArguments
	{
		private readonly CommandLineInstance commandLineInstance;

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
			commandLineInstance = new (commands, arguments);
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
			commandLineInstance.InferCommand = inferCommand;
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
			commandLineInstance.InferCommand = inferCommand;
		}

		/// <summary>
		/// Gets the active command.
		/// </summary>
		/// <value>The active command.</value>
		public Command Command
		{
			get { return commandLineInstance.Command; }
		}

		/// <summary>
		/// Gets the error message, if any.
		/// </summary>
		/// <value>The error message, if any.</value>
		public string ErrorMessage
		{
			get { return commandLineInstance.ErrorMessage; }
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
			get { return commandLineInstance.ValidArguments; }
		}

		/// <summary>
		/// Show help message.
		/// </summary>
		/// <param name="title">An optional title to display.</param>
		public void ShowHelp(string title = null)
		{
			commandLineInstance.ShowHelp(title);
		}
	}
}
