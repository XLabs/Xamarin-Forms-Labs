using Xamarin.Forms.Labs.Data;

namespace Xamarin.Forms.Labs.Sample.Model
{

    /// <summary>
    /// Define the TestPerson.
    /// </summary>
    public class TestPerson : ObservableObject
    {
        private string _firstName;
        private string _lastName;
        private int _age;
        
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>The age.</value>
        public int Age
        {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return FirstName;
        }
    }
}
