/////////////////////////////////////////////////////////////////////////////
// <copyright file="HelpManagerTests.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.CommandLine.Commands.Tests
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using NUnit.Framework;

	/// <summary>
	/// The help manager tests class.
	/// </summary>
	internal sealed class HelpManagerTests
	{
		private CommandsSet commandsSetFull;

		private CommandsSet commandsSetMinimal;

		private CommandsSet commandSetNoDescription;

		private CommandsSet commandsSetNoOptions;

		private CommandsSet commandsSetNoParameters;

		private CommandsSet commandsSetVerySimple;

		private string topHeaderText;

		/// <summary>
		/// One time set up method.
		/// </summary>
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			string title = "CommandLine Commands Tests";

			topHeaderText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}{1}{1}Usage:{1}",
				title,
				Environment.NewLine);

			commandsSetFull = GetCommandSetFull();
			commandsSetMinimal = GetCommandSetMinimal();
			commandSetNoDescription = GetCommandSetNoDescription();
			commandsSetNoOptions = GetCommandSetNoOptions();
			commandsSetNoParameters = GetCommandSetNoParameters();
			commandsSetVerySimple = GetCommandSetVerySimple();
		}

		/// <summary>
		/// One time tear down method.
		/// </summary>
		[OneTimeTearDown]
		public void OneTimeTearDown()
		{
		}

		/// <summary>
		/// Set up method.
		/// </summary>
		[SetUp]
		public void Setup()
		{
		}

		/// <summary>
		/// Get help header text no option test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextFullSet()
		{
			string buffer = "{0}Command         Description" +
				"                  Options   Parameters{1}";

			string expected = string.Format(
				CultureInfo.InvariantCulture,
				buffer,
				topHeaderText,
				Environment.NewLine);

			HelpManager helpManager = new (commandsSetFull);
			string actual = helpManager.GetHelpHeaderText(
				"CommandLine Commands Tests");

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text minimal test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextMinimal()
		{
			string expected = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Description{1}",
				topHeaderText,
				Environment.NewLine);

			HelpManager helpManager = new (commandsSetMinimal);
			string actual = helpManager.GetHelpHeaderText(
				"CommandLine Commands Tests");

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text no description test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextNoDescription()
		{
			string expected = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Parameters{1}",
				topHeaderText,
				Environment.NewLine);

			HelpManager helpManager = new (commandSetNoDescription);
			string actual = helpManager.GetHelpHeaderText(
				"CommandLine Commands Tests");

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text no option test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextNoOptions()
		{
			string expected = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Description                  Parameters{1}",
				topHeaderText,
				Environment.NewLine);

			HelpManager helpManager = new (commandsSetNoOptions);
			string actual = helpManager.GetHelpHeaderText(
				"CommandLine Commands Tests");

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text no option test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextNoParameters()
		{
			string expected = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command         Description                 Options{1}",
				topHeaderText,
				Environment.NewLine);

			HelpManager helpManager = new (commandsSetNoParameters);
			string actual = helpManager.GetHelpHeaderText(
				"CommandLine Commands Tests");

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text very simple test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextVerySimple()
		{
			string expected = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Description              Parameters{1}",
				topHeaderText,
				Environment.NewLine);

			HelpManager helpManager = new (commandsSetVerySimple);
			string actual = helpManager.GetHelpHeaderText(
				"CommandLine Commands Tests");

			Assert.That(actual, Is.EqualTo(expected));
		}

		private CommandsSet GetCommandSetFull()
		{
			CommandsSet commandsSetMinimal = GetCommandSetNoParameters();

			List<Command> commands =
				new List<Command>(commandsSetMinimal.Commands);

			Command convert = new("convert");
			convert.Description = "Convert file for some reason";

			string[] parameters = ["input file path", "output file path"];
			convert.Parameters = parameters;

			commands.Add(convert);

			CommandsSet commandsSet = new CommandsSet(commands);

			return commandsSet;
		}

		private CommandsSet GetCommandSetMinimal()
		{
			List<Command> commands = [];

			Command command = new ("help");
			command.Description = "Show this information";
			commands.Add(command);

			CommandsSet commandsSet = new CommandsSet(commands);

			return commandsSet;
		}

		private CommandsSet GetCommandSetNoDescription()
		{
			List<Command> commands = [];

			Command command = new ("help");
			commands.Add(command);

			command = new ("check");
			List<string> parameters = ["file path"];
			command.Parameters = parameters;

			commands.Add(command);

			CommandsSet commandsSet = new CommandsSet(commands);

			return commandsSet;
		}

		private CommandsSet GetCommandSetNoOptions()
		{
			CommandsSet commandsSetMinimal = GetCommandSetVerySimple();

			List<Command> commands =
				new List<Command>(commandsSetMinimal.Commands);

			Command convert = new ("convert");
			convert.Description = "Convert file for some reason";

			string[] parameters = ["input file path", "output file path"];
			convert.Parameters = parameters;

			commands.Add(convert);

			CommandsSet commandsSet = new CommandsSet(commands);

			return commandsSet;
		}

		private CommandsSet GetCommandSetNoParameters()
		{
			CommandsSet commandsSetMinimal = GetCommandSetMinimal();

			List<Command> commands =
				new List<Command>(commandsSetMinimal.Commands);

			CommandOption option1 = new ("s", "something");
			CommandOption option2 = new ("o", "other");
			List<CommandOption> options = [option1, option2];

			Command command = new (
				"command-options",
				options,
				0,
				"A command with only options");
			commands.Add(command);

			CommandsSet commandsSet = new CommandsSet(commands);

			return commandsSet;
		}

		private CommandsSet GetCommandSetVerySimple()
		{
			CommandsSet commandsSetMinimal = GetCommandSetMinimal();

			List<Command> commands =
				new List<Command>(commandsSetMinimal.Commands);

			Command command = new ("check");
			command.Description = "Check file for some text";

			List<string> parameters = ["file path"];
			command.Parameters = parameters;

			commands.Add(command);

			CommandsSet commandsSet = new CommandsSet(commands);

			return commandsSet;
		}
	}
}
