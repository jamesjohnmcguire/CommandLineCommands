/////////////////////////////////////////////////////////////////////////////
// <copyright file="CommandColumn.cs" company="James John McGuire">
// Copyright © 2022 - 2025 James John McGuire. All Rights Reserved.
// </copyright>
/////////////////////////////////////////////////////////////////////////////

namespace DigitalZenWorks.CommandLine.Commands
{
	/// <summary>
	/// Represents a command column type.
	/// </summary>
	public enum CommandColumn
	{
		/// <summary>
		/// Unknown column type.
		/// </summary>
		Unknown,

		/// <summary>
		/// Command column type.
		/// </summary>
		Command,

		/// <summary>
		/// Description column type.
		/// </summary>
		Description,

		/// <summary>
		/// Option column type.
		/// </summary>
		Option,

		/// <summary>
		/// Parameter column type.
		/// </summary>
		Parameter,
	}
}
