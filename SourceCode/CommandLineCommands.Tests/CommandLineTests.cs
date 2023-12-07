/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandLineTests.cs" company="James John McGuire">
// Copyright © 2022 - 2023 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

using DigitalZenWorks.CommandLine.Commands;
using NUnit.Framework;
using System;
using System.Collections.Generic;

[assembly: CLSCompliant(true)]

namespace CommandLineCommands.Tests
{
	/// <summary>
	/// Test class.
	/// </summary>
	public class CommandLineTests
	{
		private IList<Command> commands;

		/// <summary>
		/// One time set up method.
		/// </summary>
		[OneTimeSetUp]
		public void OneTimeSetUp()
		{
			commands = new List<Command>();

			Command help = new ("help");
			help.Description = "Show this information";
			commands.Add(help);

			CommandOption option = new ("e", "encoding");
			IList<CommandOption> options = new List<CommandOption>();
			options.Add(option);

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
			options = new List<CommandOption>();
			options.Add(dryRun);

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
			options = new List<CommandOption>();
			options.Add(dryRun);
			options.Add(flush);

			Command commandSix = new (
				"command-six",
				options,
				0,
				"A command with multiple options");
			commands.Add(commandSix);

			CommandOption encoding = new ("e", "encoding", true);
			options = new List<CommandOption>();
			options.Add(encoding);

			Command commandSeven = new (
				"command-seven",
				options,
				1,
				"A command with an option that has a value.");
			commands.Add(commandSeven);
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
		/// Get option fail test.
		/// </summary>
		[Test]
		public void GetOptionFailTest()
		{
			CommandOption encoding = new ("e", "encoding", true);
			encoding.Parameter = "utf8";
			IList<CommandOption> options = new List<CommandOption>();
			options.Add(encoding);

			IList<string> parameters = new List<string>();
			parameters.Add("%USERPROFILE%");

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
			IList<CommandOption> options = new List<CommandOption>();
			options.Add(encoding);

			IList<string> parameters = new List<string>();
			parameters.Add("%USERPROFILE%");

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
			string[] arguments = { "help" };

			CommandLineArguments commandLine = new (commands, arguments);

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
			string[] arguments = { "command-one", "%USERPROFILE%" };

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;

			Assert.That(0, Is.EqualTo(options.Count));
		}

		/// <summary>
		/// Option simple no parameter test.
		/// </summary>
		[Test]
		public void OptionSimpleNoParameterTest()
		{
			string[] arguments = { "command-three" };

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-three"));

			IList<CommandOption> options = command.Options;

			Assert.That(0, Is.EqualTo(options.Count));
		}

		/// <summary>
		/// Option simple no parameter with option test.
		/// </summary>
		[Test]
		public void OptionSimpleNoParameterWithOptionTest()
		{
			string[] arguments = { "command-three", "-n" };

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-three"));

			IList<CommandOption> options = command.Options;

			Assert.That(1, Is.EqualTo(options.Count));
		}

		/// <summary>
		/// Option simple fail no parameter test.
		/// </summary>
		[Test]
		public void OptionSimpleFailNoParameterTest()
		{
			string[] arguments = { "command-one", "-e" };

			CommandLineArguments commandLine = new (commands, arguments);

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
			string[] arguments = { "command-one", "-e", "%USERPROFILE%" };

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;

			Assert.That(options.Count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option simple short option last test.
		/// </summary>
		[Test]
		public void OptionSimpleShortOptionLastTest()
		{
			string[] arguments = { "command-one", "%USERPROFILE%", "-e" };

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;

			Assert.That(options.Count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option simple long option first test.
		/// </summary>
		[Test]
		public void OptionSimpleLongOptionFirstTest()
		{
			string[] arguments =
			{
				"command-one", "--encoding", "%USERPROFILE%"
			};

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;

			Assert.That(options.Count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option simple long option last test.
		/// </summary>
		[Test]
		public void OptionSimpleLongOptionLastTest()
		{
			string[] arguments =
			{
				"command-one", "%USERPROFILE%", "--encoding"
			};

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-one"));

			IList<CommandOption> options = command.Options;

			Assert.That(options.Count, Is.GreaterThan(0));
		}

		/// <summary>
		/// Option with required parameter first test.
		/// </summary>
		[Test]
		public void OptionWithdRequiredParameterFirstTest()
		{
			string[] arguments =
			{
				"command-seven", "--encoding", "utf8", "%USERPROFILE%"
			};

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-seven"));

			IList<CommandOption> options = command.Options;

			Assert.That(options.Count, Is.GreaterThan(0));

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
			{
				"command-seven", "%USERPROFILE%", "--encoding", "utf8"
			};

			CommandLineArguments commandLine = new (commands, arguments);

			Assert.That(commandLine.ValidArguments, Is.True);

			Command command = commandLine.Command;
			Assert.That(command, Is.Not.Null);

			Assert.That(command.Name, Is.EqualTo("command-seven"));

			IList<CommandOption> options = command.Options;

			Assert.That(options.Count, Is.GreaterThan(0));

			CommandOption option = options[0];
			string parameter = option.Parameter;

			Assert.That(parameter, Is.EqualTo("utf8"));
		}
	}
}
