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
		private readonly string title = "CommandLine Commands Tests";

		private IList<Command> commandsSetFull;

		private IList<Command> commandsSetMinimal;

		private IList<Command> commandsSetNoDescription;

		private IList<Command> commandsSetNoOptions;

		private IList<Command> commandsSetNoParameters;

		private IList<Command> commandsSetVerySimple;

		private string topHeaderText;

		/// <summary>
		/// One time set up method.
		/// </summary>
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			topHeaderText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}{1}{1}Usage:{1}",
				title,
				Environment.NewLine);

			commandsSetMinimal = GetCommandSetMinimal();
			commandsSetVerySimple = GetCommandSetVerySimple();
			commandsSetNoDescription = GetCommandSetNoDescription();
			commandsSetNoOptions = GetCommandSetNoOptions();
			commandsSetNoParameters = GetCommandSetNoParameters();
			commandsSetFull = GetCommandSetFull();
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
		/// Get command information full set test.
		/// </summary>
		[Test]
		public void GetCommandInformationFullSet()
		{
			string expected = GetExpectedCommandsFullSet();

			HelpManager helpManager = new (title, commandsSetFull);
			string actual = helpManager.CommandsInformation;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get command information minimal test.
		/// </summary>
		[Test]
		public void GetCommandInformationMinimal()
		{
			string expected = GetExpectedCommandsMinimal();

			HelpManager helpManager = new (title, commandsSetMinimal);
			string actual = helpManager.CommandsInformation;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get command information no description test.
		/// </summary>
		[Test]
		public void GetCommandInformationNoDescription()
		{
			string expected = GetExpectedCommandsNoDescription();

			HelpManager helpManager = new (title, commandsSetNoDescription);
			string actual = helpManager.CommandsInformation;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get command information no options test.
		/// </summary>
		[Test]
		public void GetCommandInformationNoOptions()
		{
			string expected = GetExpectedCommandsNoOptions();

			HelpManager helpManager = new (title, commandsSetNoOptions);
			string actual = helpManager.CommandsInformation;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get command information no parameters test.
		/// </summary>
		[Test]
		public void GetCommandInformationNoParameters()
		{
			string expected = GetExpectedCommandsNoParameters();

			HelpManager helpManager = new (title, commandsSetNoParameters);
			string actual = helpManager.CommandsInformation;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get command information minimal test.
		/// </summary>
		[Test]
		public void GetCommandInformationVerySimple()
		{
			string expected = GetExpectedCommandsVerySimple();

			HelpManager helpManager = new (title, commandsSetVerySimple);
			string actual = helpManager.CommandsInformation;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text full set test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextFullSet()
		{
			string expected = GetHeaderTextFullSet();

			HelpManager helpManager = new (title, commandsSetFull);
			string actual = helpManager.HelpHeaderText;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text minimal test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextMinimal()
		{
			string expected = GetHeaderTextMinimal();

			HelpManager helpManager = new (title, commandsSetMinimal);
			string actual = helpManager.HelpHeaderText;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text no description test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextNoDescription()
		{
			string expected = GetHeaderTextNoDescription();

			HelpManager helpManager = new (title, commandsSetNoDescription);
			string actual = helpManager.HelpHeaderText;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text no option test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextNoOptions()
		{
			string expected = GetHeaderTextNoOptions();

			HelpManager helpManager = new (title, commandsSetNoOptions);
			string actual = helpManager.HelpHeaderText;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text no option test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextNoParameters()
		{
			string expected = GetHeaderTextNoParameters();

			HelpManager helpManager = new (title, commandsSetNoParameters);
			string actual = helpManager.HelpHeaderText;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help header text very simple test.
		/// </summary>
		[Test]
		public void GetHelpHeaderTextVerySimple()
		{
			string expected = GetHeaderTextVerySimple();

			HelpManager helpManager = new (title, commandsSetVerySimple);
			string actual = helpManager.HelpHeaderText;

			Assert.That(actual, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help text full set test.
		/// </summary>
		[Test]
		public void GetHelpTextFullSet()
		{
			string headerText = GetHeaderTextFullSet();
			string commandsText = GetExpectedCommandsFullSet();

			string expected = headerText + commandsText;

			HelpManager helpManager = new (title, commandsSetFull);

			string helpText = helpManager.HelpText;

			Assert.That(helpText, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help text minimal test.
		/// </summary>
		[Test]
		public void GetHelpTextMinimal()
		{
			string headerText = GetHeaderTextMinimal();
			string commandsText = GetExpectedCommandsMinimal();

			string expected = headerText + commandsText;

			HelpManager helpManager = new (title, commandsSetMinimal);

			string helpText = helpManager.HelpText;

			Assert.That(helpText, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help text no description test.
		/// </summary>
		[Test]
		public void GetHelpTextNoDescription()
		{
			string headerText = GetHeaderTextNoDescription();
			string commandsText = GetExpectedCommandsNoDescription();

			string expected = headerText + commandsText;

			HelpManager helpManager = new (title, commandsSetNoDescription);

			string helpText = helpManager.HelpText;

			Assert.That(helpText, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help text no options test.
		/// </summary>
		[Test]
		public void GetHelpTextNoOptions()
		{
			string headerText = GetHeaderTextNoOptions();
			string commandsText = GetExpectedCommandsNoOptions();

			string expected = headerText + commandsText;

			HelpManager helpManager = new (title, commandsSetNoOptions);

			string helpText = helpManager.HelpText;

			Assert.That(helpText, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help text no parameters test.
		/// </summary>
		[Test]
		public void GetHelpTextNoParameters()
		{
			string headerText = GetHeaderTextNoParameters();
			string commandsText = GetExpectedCommandsNoParameters();

			string expected = headerText + commandsText;

			HelpManager helpManager = new (title, commandsSetNoParameters);

			string helpText = helpManager.HelpText;

			Assert.That(helpText, Is.EqualTo(expected));
		}

		/// <summary>
		/// Get help text very simple test.
		/// </summary>
		[Test]
		public void GetHelpTextVerySimple()
		{
			string headerText = GetHeaderTextVerySimple();
			string commandsText = GetExpectedCommandsVerySimple();

			string expected = headerText + commandsText;

			HelpManager helpManager = new (title, commandsSetVerySimple);

			string helpText = helpManager.HelpText;

			Assert.That(helpText, Is.EqualTo(expected));
		}

		private static List<Command> GetCommandSetMinimal()
		{
			List<Command> commands = [];

			Command command = new ("help");
			command.Description = "Show this information";
			commands.Add(command);

			return commands;
		}

		private static List<Command> GetCommandSetNoDescription()
		{
			List<Command> commands = [];

			Command command = new ("help");
			commands.Add(command);

			command = new ("check");
			List<string> parameters = ["file path"];
			command.Parameters = parameters;

			commands.Add(command);

			return commands;
		}

		private static string GetExpectedCommandsFullSet()
		{
			string command1 = "help            Show this information";
			string command2 = "check           Check file for some text     " +
				"                file path";
			string command3 = "convert         Convert file for some reason " +
				"                input file path" + Environment.NewLine +
				"                                                     " +
				"        output file path";
			string command4 = "command-options A command with only options  " +
				"-s, --something" + Environment.NewLine +
				"                                             -o, --other";
			string command5 = "encode          A command to encode          " +
				"-e, --encoding  UTF-8";

			string commandsText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}{5}{1}{5}{2}{5}{3}{5}{4}{5}",
				command1,
				command2,
				command3,
				command4,
				command5,
				Environment.NewLine);

			return commandsText;
		}

		private static string GetExpectedCommandsMinimal()
		{
			string commandsText = string.Format(
				CultureInfo.InvariantCulture,
				"help    Show this information{0}",
				Environment.NewLine);

			return commandsText;
		}

		private static string GetExpectedCommandsNoDescription()
		{
			string commandsText = string.Format(
				CultureInfo.InvariantCulture,
				"help    {0}check   file path{0}",
				Environment.NewLine);

			return commandsText;
		}

		private static string GetExpectedCommandsNoOptions()
		{
			string command1 = "help    Show this information";
			string command2 = "check   Check file for some text     " +
				"file path";
			string command3 = "convert Convert file for some reason " +
				"input file path" + Environment.NewLine +
				"                                     output file path";

			string commandsText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}{3}{1}{3}{2}{3}",
				command1,
				command2,
				command3,
				Environment.NewLine);

			return commandsText;
		}

		private static string GetExpectedCommandsNoParameters()
		{
			string command1 = "help            Show this information";
			string command2 = "command-options A command with only options " +
				"-s, --something" + Environment.NewLine +
				"                                            -o, --other";

			string commandsText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}{2}{1}{2}",
				command1,
				command2,
				Environment.NewLine);

			return commandsText;
		}

		private static string GetExpectedCommandsVerySimple()
		{
			string command1 = "help    Show this information";
			string command2 = "check   Check file for some text file path";

			string commandsText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}{2}{1}{2}",
				command1,
				command2,
				Environment.NewLine);

			return commandsText;
		}

		private List<Command> GetCommandSetFull()
		{
			List<Command> commands = [.. commandsSetNoOptions];

			CommandOption option1 = new ("s", "something");
			CommandOption option2 = new ("o", "other");
			List<CommandOption> options = [option1, option2];

			Command command = new (
				"command-options",
				options,
				0,
				"A command with only options");
			commands.Add(command);

			option1 = new ("e", "encoding");
			options = [option1];

			string[] parameters = ["UTF-8"];

			command = new (
				"encode",
				options,
				1,
				"A command to encode");
			command.Parameters = parameters;
			commands.Add(command);

			return commands;
		}

		private List<Command> GetCommandSetNoOptions()
		{
			List<Command> commands = [.. commandsSetVerySimple];

			Command convert = new ("convert");
			convert.Description = "Convert file for some reason";

			string[] parameters = ["input file path", "output file path"];
			convert.Parameters = parameters;

			commands.Add(convert);

			return commands;
		}

		private List<Command> GetCommandSetNoParameters()
		{
			List<Command> commands = [.. commandsSetMinimal];

			CommandOption option1 = new ("s", "something");
			CommandOption option2 = new ("o", "other");
			List<CommandOption> options = [option1, option2];

			Command command = new (
				"command-options",
				options,
				0,
				"A command with only options");
			commands.Add(command);

			return commands;
		}

		private List<Command> GetCommandSetVerySimple()
		{
			List<Command> commands = [.. commandsSetMinimal];

			Command command = new ("check");
			command.Description = "Check file for some text";

			List<string> parameters = ["file path"];
			command.Parameters = parameters;

			commands.Add(command);

			return commands;
		}

		private string GetHeaderTextFullSet()
		{
			string buffer = "{0}Command         Description" +
				"                  Options         Parameters{1}";

			string headerText = string.Format(
				CultureInfo.InvariantCulture,
				buffer,
				topHeaderText,
				Environment.NewLine);

			return headerText;
		}

		private string GetHeaderTextMinimal()
		{
			string headerText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Description{1}",
				topHeaderText,
				Environment.NewLine);

			return headerText;
		}

		private string GetHeaderTextNoDescription()
		{
			string headerText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Parameters{1}",
				topHeaderText,
				Environment.NewLine);

			return headerText;
		}

		private string GetHeaderTextNoOptions()
		{
			string headerText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Description                  Parameters{1}",
				topHeaderText,
				Environment.NewLine);

			return headerText;
		}

		private string GetHeaderTextNoParameters()
		{
			string headerText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command         Description                 Options{1}",
				topHeaderText,
				Environment.NewLine);

			return headerText;
		}

		private string GetHeaderTextVerySimple()
		{
			string headerText = string.Format(
				CultureInfo.InvariantCulture,
				"{0}Command Description              Parameters{1}",
				topHeaderText,
				Environment.NewLine);

			return headerText;
		}
	}
}
