namespace XLabs.Exceptions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// exception thrown when a template cannot
	/// be found for a supplied type
	/// </summary>
	public class NoDataTemplateMatchException : Exception
	{
		/// <summary>
		/// Hide any possible default constructor
		/// Redundant I know, but it costs nothing
		/// and communicates the design intent to
		/// other developers.
		/// </summary>
		private NoDataTemplateMatchException(){}

		/// <summary>
		/// Constructs the exception and passses a meaningful
		/// message to the base Exception
		/// </summary>
		/// <param name="tomatch">The type that a match was attempted for</param>
		/// <param name="candidates">All types examined during the match process</param>
		public NoDataTemplateMatchException(Type tomatch, List<Type> candidates) :
			base(string.Format("Could not find a template for type [{0}]",tomatch.Name))
		{
			AttemptedMatch = tomatch;
			TypesExamined = candidates;
			TypeNamesExamined = TypesExamined.Select(x => x.Name).ToList();
		}

		/// <summary>
		/// The type that a match was attempted for
		/// </summary>
		public Type AttemptedMatch { get; set; }
		/// <summary>
		/// A list of all types that were examined
		/// </summary>
		public List<Type> TypesExamined { get; set; }
		/// <summary>
		/// A List of the names of all examined types (Simple name only)
		/// </summary>
		public List<string> TypeNamesExamined { get; set; }
	}
}
