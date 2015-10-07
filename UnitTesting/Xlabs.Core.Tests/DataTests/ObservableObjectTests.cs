using System;
using XLabs.Data;
using NUnit.Framework;

namespace Xlabs.Core.DataTests
{
	[TestFixture ()]
	public class ObservableObjectTests
	{
		public class TestTarget:ObservableObject
		{
			public int myStringProperty;
			public int MyStringProperty
			{
				get{return myStringProperty;}
				set{myStringProperty=value;
					NotifyPropertyChanged ("MyStringProperty");
				}
			}

			public int myLinqProperty;
			public int MyLinqProperty
			{
				get{return myLinqProperty;}
				set{myLinqProperty=value;
					NotifyPropertyChanged (()=>MyLinqProperty);
				}
			}

			public void CallSetWithString()
			{
				base.SetProperty<int>(ref myStringProperty, 3, "MyStringProperty");
			}

			public void CallSetWithLinq()
			{
				base.SetProperty<int> (ref myLinqProperty, 4, ()=>MyLinqProperty);
			}
		}
		[Test]
		public void OnPropertyChangeFiresOnStringName()
		{
			string propertyThatChanged = "change";
			TestTarget target = new TestTarget();
			target.PropertyChanged += (sender, e) => {
				propertyThatChanged = e.PropertyName;
			};
			target.MyStringProperty = 1;
			Assert.IsTrue (propertyThatChanged == "MyStringProperty");
		}
		[Test]
		public void OnPropertyChangeFiresOnLinqExp()
		{
			string propertyThatChanged = "change";
			TestTarget target = new TestTarget();
			target.PropertyChanged += (sender, e) => {
				propertyThatChanged = e.PropertyName;
			};
			target.MyLinqProperty = 2;
			Assert.IsTrue (propertyThatChanged == "MyLinqProperty");
		}
		[Test]
		public void SetFiresOnPropertyChangeOnStringName()
		{
			string propertyThatChanged = "change";
			TestTarget target = new TestTarget();
			target.PropertyChanged += (sender, e) => {
				propertyThatChanged = e.PropertyName;
			};
			target.CallSetWithString();
			Assert.IsTrue (propertyThatChanged == "MyStringProperty");
		}

		[Test]
		public void SetFiresOnPropertyChangeOnLinqExp()
		{
			string propertyThatChanged = "change";
			TestTarget target = new TestTarget();
			target.PropertyChanged += (sender, e) => {
				propertyThatChanged = e.PropertyName;
			};
			target.CallSetWithLinq (); 
			Assert.IsTrue (propertyThatChanged == "MyLinqProperty");
		}
	}
}

