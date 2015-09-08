using Xamarin.Forms;

namespace XLabs.Forms.Validation
{
    /// <summary>
    ///     A bindable object that performs validation and
    ///     allows the setting of properties based on
    ///     validation results.
    ///     A Validator must be put in the ResourceDictionary
    ///     As it does not inherit form VisualElement.
    /// </summary>
    /// Element created at 07/11/2014,6:09 AM by Charles
    public class Validator : BindableObject
    {
        #region Static Fields

        /// <summary>The Set of Validations</summary>
        /// Element created at 07/11/2014,12:00 PM by Charles
        public static BindableProperty SetsProperty =
            BindableProperty.Create<Validator, ValidationSets>(x => x.Sets, default(ValidationSets));

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="Validator" /> class.
        /// </summary>
        /// Element created at 07/11/2014,6:10 AM by Charles
        public Validator()
        {
            Sets = new ValidationSets();
        }

        #endregion

        #region Public Properties

        /// <summary>Gets or sets the list of ValiationSets.</summary>
        /// <value>The sets.</value>
        /// Element created at 07/11/2014,6:11 AM by Charles
        public ValidationSets Sets
        {
            get { return (ValidationSets)GetValue(SetsProperty); }
            set { SetValue(SetsProperty, value); }
        }

        #endregion
    }
}