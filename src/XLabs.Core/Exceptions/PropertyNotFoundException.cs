
namespace XLabs.Exceptions
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Thrown when a property could not be found on a Type
	/// </summary>
	/// Element created at 07/11/2014,3:35 AM by Charles
	public class PropertyNotFoundException : Exception
	{

		/// <summary>
		/// Hide any possible default constructor
		/// Redundant I know, but it costs nothing
		/// and communicates the design intent to
		/// other developers.
		/// </summary>
		private PropertyNotFoundException() { }

		/// <summary>
		/// Constructs the exception and passes a meaningful
		/// message to the base Exception
		/// </summary>
		/// <param name="etype">The Type that was inspected.</param>
		/// <param name="property">The property Name.</param>
		/// <param name="available">The available properties found on the Type.</param>
		/// <param name="caller">The method that attempted to find the property.</param>
		/// Element created at 07/11/2014,3:36 AM by Charles
		public PropertyNotFoundException(Type etype, string property,IEnumerable<string>available,[CallerMemberName] string caller=null )
			: base(string.Format("{0} could not find the property [{1}] on type [{2}]",caller, property, etype.Name))
		{
			InspectedType = etype;
			InspectedTypeName = etype.Name;
			PropertyName = property;
			AvailableProperties=new List<string>(available);
		}

		/// <summary>Gets the type of the inspected object.</summary>
		/// <value>The type of the inspected.</value>
		/// Element created at 07/11/2014,3:37 AM by Charles
		public Type InspectedType { get; private set; }

		/// <summary>Gets the type name of the inspected object.</summary>
		/// <value>The name of the inspected type.</value>
		/// Element created at 07/11/2014,3:37 AM by Charles
		public string InspectedTypeName { get; private set; }

		/// <summary>Gets the name of the property.</summary>
		/// <value>The name of the property.</value>
		/// Element created at 07/11/2014,3:37 AM by Charles
		public string PropertyName { get; private set; }

		/// <summary>Gets the available properties.</summary>
		/// <value>The available properties.</value>
		/// Element created at 07/11/2014,3:37 AM by Charles
		public List<string> AvailableProperties { get; private set; }
	}
}
