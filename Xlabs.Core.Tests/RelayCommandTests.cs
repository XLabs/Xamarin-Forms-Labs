using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.CoreTests
{
	[TestFixture]
	public class RelayCommandTests
	{
		
		[Test]
		public void RelayCommandConstNoTestTests ()
		{
			RelayCommand rc = new RelayCommand(()=>System.Diagnostics.Debug.WriteLine("test"));
			Assert.IsNotNull(rc);
		}
		[Test]
		public void RelayCommandConstWithTestTests ()
		{
			RelayCommand rc = new RelayCommand(()=>System.Diagnostics.Debug.WriteLine("test"),()=>false);
			Assert.IsNotNull(rc);
		}
		[Test]
		public void RelayCommandCantExecute ()
		{
			RelayCommand rc = new RelayCommand(()=>System.Diagnostics.Debug.WriteLine("test"),()=>false);
			Assert.IsFalse(rc.CanExecute(null));
		}
		[Test]
		public void RelayCommandCanExecute ()
		{
			RelayCommand rc = new RelayCommand(()=>System.Diagnostics.Debug.WriteLine("test"),()=>true);
			Assert.IsTrue(rc.CanExecute(null));
		}
		string RelayCommandExecuteNoParameterResult;
		[Test]
		public void RelayCommandExecuteNoParameter ()
		{
			RelayCommand rc = new RelayCommand(()=>RelayCommandExecuteNoParameterResult = "TestSat",()=>true);
			rc.CanExecute(null);
			rc.Execute (null);
			Assert.AreEqual("TestSat",RelayCommandExecuteNoParameterResult);
		}
		string canexectueChangedResult;
		[Test]
		public void RelayCommandCanExecuteChanged()
		{
			RelayCommand rc = new RelayCommand(()=>RelayCommandExecuteNoParameterResult = "TestString",()=>true);
			rc.CanExecuteChanged += (sender, e) => canexectueChangedResult = "TestSat";
			rc.RaiseCanExecuteChanged ();

			Assert.AreEqual("TestSat",canexectueChangedResult);
		}

	}
}


