namespace XLabs.Data
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Runtime.CompilerServices;

	/// <summary>
	/// Base class enabling INotifyPropertyChanged implementation and methods for setting property values.
	/// </summary>
	public abstract class ObservableObject : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when property is changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <param name="propertyName">The name of the property to raise the PropertyChanged event for.</param>
		protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName = null)
		{
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Raises the PropertyChanged event.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <param name="propertyExpression">The lambda expression of the property to raise the PropertyChanged event for.</param>
		protected virtual void NotifyPropertyChanged<T>(Expression<Func<T>> propertyExpression)
		{
			string propertyName = GetPropertyName(propertyExpression);
			OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// The property changed event invoker.
		/// </summary>
		/// <param name="e">
		/// The event arguments.
		/// </param>
		protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
		{
			var eventHandler = PropertyChanged;
			if (eventHandler != null)
			{
				eventHandler(this, e);
			}
		}

		/// <summary>
		/// Changes the property if the value is different and raises the PropertyChanged event.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <param name="storage">Reference to current value.</param>
		/// <param name="value">New value to be set.</param>
		/// <param name="propertyExpression">The lambda expression of the property to raise the PropertyChanged event for.</param>
		/// <returns><c>true</c> if new value, <c>false</c> otherwise.</returns>
		protected bool SetProperty<T>(ref T storage, T value, Expression<Func<T>> propertyExpression)
		{
			var propertyName = GetPropertyName(propertyExpression);
			return SetProperty<T>(ref storage, value, propertyName);
		}

		/// <summary>
		/// Changes the property if the value is different and raises the PropertyChanged event.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <param name="storage">Reference to current value.</param>
		/// <param name="value">New value to be set.</param>
		/// <param name="propertyName">The name of the property to raise the PropertyChanged event for.</param>
		/// <returns><c>true</c> if new value, <c>false</c> otherwise.</returns>
		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(storage, value))
			{
				return false;
			}

			storage = value;
			this.NotifyPropertyChanged(propertyName);
			return true;
		}

		/// <summary>
		/// Gets property name from expression.
		/// </summary>
		/// <param name="propertyExpression">
		/// The property expression.
		/// </param>
		/// <typeparam name="T">
		/// </typeparam>
		/// <returns>
		/// The <see cref="string"/>.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// Throws an exception if expression is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// Expression should be a member access lambda expression
		/// </exception>
		private string GetPropertyName<T>(Expression<Func<T>> propertyExpression)
		{
			if (propertyExpression == null)
			{
				throw new ArgumentNullException("propertyExpression");
			}

			if (propertyExpression.Body.NodeType != ExpressionType.MemberAccess)
			{
				throw new ArgumentException("Should be a member access lambda expression", "propertyExpression");
			}

			var memberExpression = (MemberExpression)propertyExpression.Body;
			return memberExpression.Member.Name;
		}
	}
}
