namespace XLabs.Forms.Validation
{
    /// <summary>
    /// The order that predicates are evaluated in
    /// </summary>
    /// Element created at 08/11/2014,2:37 PM by Charles
    internal enum PredicatePriority
    {
        /// <summary>High Priority, No type conversion or processing required</summary>
        /// Element created at 08/11/2014,2:37 PM by Charles
        High=0,

        /// <summary>Medium Priority Type conversions and evaluation</summary>
        /// Element created at 08/11/2014,2:37 PM by Charles
        Medium=2,
        /// <summary>
        /// Low priority anything with regular expressions
        /// </summary>
        Low=3,
        /// <summary>
        /// User Priority Callback and Pattern
        /// </summary>
        User=4
    }
}