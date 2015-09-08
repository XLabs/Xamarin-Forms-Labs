namespace XLabs.Forms.Validation
{
    /// <summary>
    /// The result of a validation (a single rule or an entire validationset)
    /// </summary>
    /// Element created at 08/11/2014,12:57 PM by Charles
    public enum RuleResult
    {
        /// <summary>Empty value, we should never see it in the wild</summary>
        /// Element created at 08/11/2014,12:57 PM by Charles
        Unknown=0,
        /// <summary>The validation was successful</summary>
        /// Element created at 08/11/2014,12:58 PM by Charles
        ValidationSuccess,

        /// <summary>The validation failed</summary>
        /// Element created at 08/11/2014,12:58 PM by Charles
        ValidationFailure,

        /// <summary>
        /// There was no change from the last validation attempt
        /// </summary>
        /// Element created at 08/11/2014,12:58 PM by Charles
        ValidationNoChange
    }
}