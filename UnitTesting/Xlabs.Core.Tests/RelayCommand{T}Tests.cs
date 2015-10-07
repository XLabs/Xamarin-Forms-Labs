using System;
using NUnit.Framework;

namespace Xlabs.CoreTests
{
	[TestFixture]
	public class RelayCommand_T_Tests
	{
		[Test]
		public void RelayCommandConstNoTestTests ()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>System.Diagnostics.Debug.WriteLine("test"));
			Assert.IsNotNull(rc);
		}
		[Test]
		public void RelayCommandConstWithTestTests ()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>System.Diagnostics.Debug.WriteLine("test"),(s)=>false);
			Assert.IsNotNull(rc);
		}
		[Test]
		public void RelayCommandCantExecute ()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>System.Diagnostics.Debug.WriteLine("test"),(s)=>false);
			Assert.IsFalse(rc.CanExecute(null));
		}
		[Test]
		public void RelayCommandCanExecute ()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>System.Diagnostics.Debug.WriteLine("test"),(s)=>true);
			Assert.IsTrue(rc.CanExecute(null));
		}
		string RelayCommandExecuteNoParameterResult;
		[Test]
		public void RelayCommandExecuteNoParameter ()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>RelayCommandExecuteNoParameterResult = "TestSat",(s)=>true);
			rc.CanExecute(null);
			rc.Execute (null);
			Assert.AreEqual("TestSat",RelayCommandExecuteNoParameterResult);
		}
		string RelayCommandExecuteWithParameterResult;
		[Test]
		public void RelayCommandExecuteWithParameter ()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>RelayCommandExecuteNoParameterResult = s,(s)=>true);
			rc.CanExecute("TestSat");
			rc.Execute ("TestSat");
			Assert.AreEqual("TestSat",RelayCommandExecuteNoParameterResult);
		}

		string canexectueChangedResult;
		[Test]
		public void RelayCommandCanExecuteChanged()
		{
			XLabs.RelayCommand<string> rc = new XLabs.RelayCommand<string>((s)=>RelayCommandExecuteNoParameterResult = "TestString",(s)=>true);
			rc.CanExecuteChanged += (sender, e) => canexectueChangedResult = "TestSat";
			rc.RaiseCanExecuteChanged ();

			Assert.AreEqual("TestSat",canexectueChangedResult);
		}
	}
}

