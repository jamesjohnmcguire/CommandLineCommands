/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineTests.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

[assembly: System.CLSCompliant(true)]

namespace DigitalZenWorks.CommandLine.Commands.Tests
{
	using System.Collections.Generic;
	using System.IO;
	using DigitalZenWorks.CommandLine.Commands;
	using DigitalZenWorks.Common.Utilities;
	using NUnit.Framework;
	using NUnit.Framework.Internal;

	/// <summary>
	/// Test class.
	/// </summary>
	internal sealed class CommandLineTests
	{
		private List<Command> commands;
		private CommandsSet commandsSet;

		/// <summary>
		/// One time set up method.
		/// </summary>
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			commands = [];

			Command help = new ("help");
			help.Description = "Show this information";
			commands.Add(help);

			CommandOption option = new ("e", "encoding");
			List<CommandOption> options = [option];

			Command commandOne = new (
				"command-one",
				options,
				1,
				"A command with an option that has an option.");
			commands.Add(commandOne);

			Command commandTwo = new (
				"command-two",
				null,
				1,
				"A command with no options");
			commands.Add(commandTwo);

			CommandOption dryRun = new ("n", "dryrun");
			options = [dryRun];

			Command commandThree = new (
				"command-three",
				options,
				0,
				"A command with no parameters");
			commands.Add(commandThree);

			Command commandFour = new (
				"command-four",
				null,
				2,
				"A command with 2 minimum required parameters");
			commands.Add(commandFour);

			Command commandFive = new (
				"command-five",
				null,
				4,
				"A command with 4 minimum required parameters");
			commands.Add(commandFive);

			CommandOption flush = new ("s", "flush");
			options = [dryRun, flush];

			Command commandSix = new (
				"command-six",
				options,
				0,
				"A command with multiple options");
			commands.Add(commandSix);

			CommandOption encoding = new ("e", "encoding", true);
			options = [encoding];

			Command commandSeven = new (
				"command-seven",
				options,
				1,
				"A command with an option that has a value.");
			commands.Add(commandSeven);

			commandsSet = new CommandsSet(commands);
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
		/// Sanity test.
		/// </summary>
		[Test]
		public void SanityTest()
		{
			Assert.Pass();
		}

		/// <summary>
		/// Command Set from JSON text test.
		/// </summary>
		[Test]
		public void CommandSetFromJsonTest()
		{
			string tempPath = Path.GetTempPath();
			string path = Path.Combine(tempPath, "Sample.json");

			bool result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.CommandLine.Commands.Tests.Sample.json",
				path);
			Assert.That(result, Is.True);

			string jsonText = File.ReadAllText(path);

			CommandsSet commandsSet;
			Assert.That(
				() =>
				commandsSet = new CommandsSet(jsonText),
				Throws.Nothing);
		}

		/// <summary>
		/// Command Set from JSON file test.
		/// </summary>
		[Test]
		public void CommandSetFromJsonFileTest()
		{
			string tempPath = Path.GetTempPath();
			string path = Path.Combine(tempPath, "Sample.json");

			bool result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.CommandLine.Commands.Tests.Sample.json",
				path);
			Assert.That(result, Is.True);

			CommandsSet commandsSet = new ();
			Assert.That(() => commandsSet.JsonFromFile(path), Throws.Nothing);
		}

		/// <summary>
		/// Get help text test.
		/// </summary>
		[Test]
		public void GetHelpTextTest()
		{
			string tempPath = Path.GetTempPath();
			string path = Path.Combine(tempPath, "Sample.json");

			bool result = FileUtils.CreateFileFromEmbeddedResource(
				"DigitalZenWorks.CommandLine.Commands.Tests.Sample.json",
				path);
			Assert.That(result, Is.True);

			string jsonText = File.ReadAllText(path);

			CommandsSet commandsSet = new (jsonText);

			string helpMessage =
				commandsSet.GetHelp("CommandLine Commands Tests");

			Assert.That(helpMessage, Is.Not.Empty);
		}

		/// <summary>
		/// Get option fail test.
		/// </summary>
		[Test]
		public void GetOptionFailTest()
		{
			CommandOption encoding = new ("e", "encoding", true);
			encoding.Parameter = "utf8";
			List<CommandOption> options = [encoding];

			List<string> parameters = ["%USERPROFILE%"];

			Command command = new ("some-command", options, parameters);

			CommandOption optionFound = command.GetOption("n", "dryrun");

			Assert.That(optionFound, Is.Null);
		}

		/// <summary>
		/// Get option success test.
		/// </summary>
		[Test]
		public void GetOptionSuccessTest()
		{
			CommandOption encoding = new ("e", "encoding", true);
			encoding.Parameter = "utf8";
			List<CommandOption> options = [encoding];

			List<string> parameters = ["%USERPROFILE%"];

			Command command = new ("some-command", options, parameters);

			CommandOption optionFound = command.GetOption("e", "encoding");

			Assert.That(optionFound, Is.Not.Null);

			string encodingName = optionFound.Parameter;

			Assert.That(encodingName, Is.EqualTo("utf8"));
		}

		/// <summary>
		/// Help test.
		/// </summary>
		[Test]
		public void HelpTest()
		{
			string[] arguments = ["help"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("help"));
		}

		/// <summary>
		/// Option simple no option test.
		/// </summary>
		[Test]
		public void OptionSimpleNoOptionTest()
		{
			string[] arguments = ["command-one", "%USERPROFILE%"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.EqualTo(0));
		}

		/// <summary>
		/// Option simple no parameter test.
		/// </summary>
		[Test]
		public void OptionSimpleNoParameterTest()
		{
			string[] arguments = ["command-three"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-three"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.EqualTo(0));
		}

		/// <summary>
		/// Option simple no parameter with option test.
		/// </summary>
		[Test]
		public void OptionSimpleNoParameterWithOptionTest()
		{
			string[] arguments = ["command-three", "-n"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-three"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.EqualTo(1));
		}

		/// <summary>
		/// Option simple fail no parameter test.
		/// </summary>
		[Test]
		public void OptionSimpleFailNoParameterTest()
		{
			string[] arguments = ["command-one", "-e"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.False);

			Command command = commandLine.Command;
			Assert.That(command, Is.Null);
		}

		/// <summary>
		/// Option simple short option first test.
		/// </summary>
		[Test]
		public void OptionSimpleShortOptionFirstTest()
		{
			string[] arguments = ["command-one", "-e", "%USERPROFILE%"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option simple short option last test.
		/// </summary>
		[Test]
		public void OptionSimpleShortOptionLastTest()
		{
			string[] arguments = ["command-one", "%USERPROFILE%", "-e"];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option simple long option first test.
		/// </summary>
		[Test]
		public void OptionSimpleLongOptionFirstTest()
		{
			string[] arguments =
			[
				"command-one", "--encoding", "%USERPROFILE%"
			];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option simple long option last test.
		/// </summary>
		[Test]
		public void OptionSimpleLongOptionLastTest()
		{
			string[] arguments =
			[
				"command-one", "%USERPROFILE%", "--encoding"
			];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option with required parameter first test.
		/// </summary>
		[Test]
		public void OptionWithdRequiredParameterFirstTest()
		{
			string[] arguments =
			[
				"command-seven", "--encoding", "utf8", "%USERPROFILE%"
			];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-seven"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.GreaterThan(0));

			CommandOption option = options[0];
			string parameter = option.Parameter;

			Assert.That(parameter, Is.EqualTo("utf8"));
		}

		/// <summary>
		/// Option with required parameter last test.
		/// </summary>
		[Test]
		public void OptionWithdRequiredParameterLastTest()
		{
			string[] arguments =
			[
				"command-seven", "%USERPROFILE%", "--encoding", "utf8"
			];

			CommandLineInstance commandLine = new (commandsSet, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-seven"));

			IList<CommandOption> options = command.Options;
			int count = options.Count;

			Assert.That(count, Is.GreaterThan(0));

			CommandOption option = options[0];
			string parameter = option.Parameter;

			Assert.That(parameter, Is.EqualTo("utf8"));
		}
	}
}
