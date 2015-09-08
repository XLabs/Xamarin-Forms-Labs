namespace XLabs.Forms.Exceptions
{
	using System;
	using System.Runtime.CompilerServices;

	using Xamarin.Forms;

	/// <summary>
	/// Thrown when an invalid bindable object has been passed to a callback
	/// </summary>
	public class InvalidBindableException : Exception
	{

		/// <summary>
		/// Hide any possible default constructor
		/// Redundant I know, but it costs nothing
		/// and communicates the design intent to
		/// other developers.
		/// </summary>
		private InvalidBindableException() { }

		/// <summary>
		/// Constructs the exception and passes a meaningful
		/// message to the base Exception
		/// </summary>
		/// <param name="bindable">The bindable object that was passed</param>
		/// <param name="expected">The expected type</param>
		/// <param name="name">The calling methods name, uses [CallerMemberName]</param>
		public InvalidBindableException(BindableObject bindable, Type expected,[CallerMemberName]string name=null) 
			: base(string.Format("Invalid bindable passed to {0} expected a {1} received a {2}", name, expected.Name, bindable.GetType().Name))
		{
		}

		/// <summary>
		/// The bindable object that was passed
		/// </summary>
		public BindableObject IncorrectBindableObject { get; set; }
		/// <summary>
		/// The expected type of the bindable object
		/// </summary>
		public Type ExpectedType { get; set; }
	}
}
