using System.Collections.ObjectModel;

namespace XLabs.Forms.Validation
{
	/// <summary>
	///     A Validation Set succeds or fails entirely
	///     If it succeeds then all Valid properties from
	///     the actions are applied.  If it fails
	///     then all InValid properties are applied
	/// </summary>
	/// Element created at 07/11/2014,6:11 AM by Charles
	public class ValidationSets : ObservableCollection<RuleSet>
	{
	}
}