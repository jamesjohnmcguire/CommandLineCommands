/////////////////////////////////////////////////////////////////////////////
// <copyright file="Command.cs" company="James John McGuire">
// Copyright © 2022 - 2026 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.CommandLine.Commands
{
	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	/// <summary>
	/// Represents a command line command.
	/// </summary>
	public class Command
	{
		private string description;
		private string name;
		private IList<CommandOption> options = [];
		private IList<string> parameters = [];
		private int requiredParameterCount;

		/// <summary>
		/// Initializes a new instance of the <see cref="Command"/> class.
		/// </summary>
		[Obsolete("Command constructor with no arguments is deprecated, " +
			"please use Command constructor with arguments instead.")]
		public Command()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Command"/> class.
		/// </summary>
		/// <param name="name">The command name.</param>
		public Command(string name)
		{
			string variableName = nameof(name);

#if NET7_0_OR_GREATER
			ArgumentException.ThrowIfNullOrEmpty(name, variableName);
#else
			if (string.IsNullOrEmpty(name))
			{
				string message = "Value cannot be null or empty.";
				ArgumentException exception =
					new ArgumentException(message, variableName);
				throw exception;
			}
#endif

			this.name = name;

			options ??= [];
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Command"/> class.
		/// </summary>
		/// <param name="name">The command name.</param>
		/// <param name="options">The command options.</param>
		/// <param name="parameters">The command parameters.</param>
		public Command(
			string name,
			IList<CommandOption> options,
			IList<string> parameters)
			: this(name)
		{
			this.options = options;
			this.parameters = parameters;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Command"/> class.
		/// </summary>
		/// <param name="name">The command name.</param>
		/// <param name="options">The command options.</param>
		/// <param name="requiredParameterCount">The command required parameter
		/// count.</param>
		/// <param name="description">The command description.</param>
		public Command(
			string name,
			IList<CommandOption> options,
			int requiredParameterCount,
			string description)
			: this(name)
		{
			this.options = options;
			this.requiredParameterCount = requiredParameterCount;
			this.description = description;
		}

#pragma warning disable CA2227
		/// <summary>
		/// Gets or sets the command description.
		/// </summary>
		/// <value>The command description.</value>
		public string Description
		{
			get { return description; }
			set { description = value; }
		}

		/// <summary>
		/// Gets or sets the command name.
		/// </summary>
		/// <value>The command name.</value>
		[JsonProperty("command")]
		public string Name
		{
			get { return name; }
			set { name = value; }
		}

		/// <summary>
		/// Gets or sets the command options.
		/// </summary>
		/// <value>The command options.</value>
		public IList<CommandOption> Options
		{
			get { return options; }
			set { options = value; }
		}

		/// <summary>
		/// Gets or sets the command parameter count.
		/// </summary>
		/// <value>The command parameter count.</value>
		public int ParameterCount
		{
			get { return requiredParameterCount; }
			set { requiredParameterCount = value; }
		}

		/// <summary>
		/// Gets or sets the command parameters.
		/// </summary>
		/// <value>The command parameters.</value>
		public IList<string> Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}
#pragma warning restore CA2227

		/// <summary>
		/// Does option exist.
		/// </summary>
		/// <param name="shortName">The short name to search for.</param>
		/// <param name="longName">The long name to search for.</param>
		/// <returns>A value indicating whether the option exists
		/// or not.</returns>
		public bool DoesOptionExist(string shortName, string longName)
		{
			bool optionExists = false;

			CommandOption optionFound = GetOption(shortName, longName);

			if (optionFound != null)
			{
				optionExists = true;
			}

			return optionExists;
		}

		/// <summary>
		/// Get option.
		/// </summary>
		/// <param name="shortName">The short name to search for.</param>
		/// <param name="longName">The long name to search for.</param>
		/// <returns>The found option, if it exists.</returns>
		public CommandOption GetOption(string shortName, string longName)
		{
			List<CommandOption> optionsList = [.. options];

			CommandOption option = optionsList.Find(option =>
				(option.ShortName != null &&
				option.ShortName.Equals(
					shortName, StringComparison.Ordinal)) ||
				(option.LongName != null &&
				option.LongName.Equals(
					longName, StringComparison.Ordinal)));

			return option;
		}
	}
}
