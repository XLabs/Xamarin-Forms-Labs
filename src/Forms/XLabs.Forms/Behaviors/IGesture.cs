namespace XLabs.Forms.Behaviors
{
	/// <summary>
	/// Interface implmenented to consume gestures
	/// analagous to ICommand
	/// </summary>
	public interface IGesture
	{
		/// <summary>
		/// Execute the gesture
		/// </summary>
		/// <param name="result">The <see cref="GestureResult"/></param>
		/// <param name="param">the user supplied paramater</param>
		void ExecuteGesture(GestureResult result, object param);
		/// <summary>
		/// Checks to see if the gesture should execute
		/// </summary>
		/// <param name="result">The <see cref="GestureResult"/></param>
		/// <param name="param">The user supplied parameter</param>
		/// <returns>True to execute the gesture, False otherwise</returns>
		bool CanExecuteGesture(GestureResult result, object param);
	}
}
