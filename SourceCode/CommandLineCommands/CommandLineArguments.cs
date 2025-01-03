/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineArguments.cs" company="James John McGuire">
// Copyright © 2022 - 2024 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;

namespace DigitalZenWorks.CommandLine.Commands
{
	/// <summary>
	/// Represents a set of command line arguments.
	/// </summary>
	[Obsolete("CommandLineArguments is deprecated, " +
		"please use CommandLineInstance instead.")]
	public class CommandLineArguments
	{
		private readonly CommandLineInstance commandLineInstance;

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
			CommandsSet commandsSet = new (commands);

			commandLineInstance = new (commandsSet, arguments);
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
			this.commandLineInstance.InferCommand = inferCommand;
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
		/// <param name="commands">The set of valid commands.</param>
		/// <param name="arguments">The array of command line
		/// arguments.</param>
		/// <param name="inferCommand">The infer command delegate.</param>
		public CommandLineArguments(
			CommandsSet commands,
			string[] arguments,
			InferCommand inferCommand)
			: this(commands, arguments)
		{
			this.commandLineInstance.InferCommand = inferCommand;
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
		public string ErrorMessage
		{
			get { return commandLineInstance.ErrorMessage; }
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
	}
}
