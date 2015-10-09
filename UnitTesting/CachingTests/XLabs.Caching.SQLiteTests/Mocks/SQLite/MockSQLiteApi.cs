using System;
using SQLite.Net.Interop;
using System.Collections.Generic;
using SQLite.Net.Attributes;

namespace XLabs.Caching.SQLiteTests.Mocks
{
	//Class expected by the API for most data action tests
	public class MockSQLiteData
	{
		public MockSQLiteData()
		{
		}
		
		[PrimaryKey]
		public int Id{ get; set; }
		public string Name{ get; set; }
	}
	public class MockSQLiteApi:MockBase,ISQLiteApi
	{

		public MockSQLiteApi ()
		{
		}

		#region ISQLiteApi implementation

		public Result Open (byte[] filename, out IDbHandle db, int flags, IntPtr zvfs)
		{
			ActionsTaken.Add(string.Concat(filename.ToString(), "-Opened"));
			db = new MockDbHandle ();

			CreateStatementsExecuted = new List<string> ();
			DeleteStatementsExecuted = new List<string> ();
			InsertOrReplaceStatementsExecuted = new List<string> ();
			InsertStatementsExecuted = new List<string> ();
			UpdateStatementsExecuted = new List<string> ();
			DropStatementsExecuted = new List<string> ();
			SelectStatementsExecuted=  new List<string> ();


			return Result.OK;
		}

		public ExtendedResult ExtendedErrCode (IDbHandle db)
		{
			throw new NotImplementedException ();
		}

		public int LibVersionNumber ()
		{
			throw new NotImplementedException ();
		}

		public Result EnableLoadExtension (IDbHandle db, int onoff)
		{
			throw new NotImplementedException ();
		}

		public Result Close (IDbHandle db)
		{
			ActionsTaken.Add("Close");
			return Result.Done;
		}

		public Result Initialize ()
		{
			throw new NotImplementedException ();
		}

		public Result Shutdown ()
		{
			throw new NotImplementedException ();
		}

		public Result Config (ConfigOption option)
		{
			throw new NotImplementedException ();
		}

		public Result BusyTimeout (IDbHandle db, int milliseconds)
		{
			ActionsTaken.Add ("BusyTimeoutSet");
			return Result.Done;
		}

		public int Changes (IDbHandle db)
		{
			ActionsTaken.Add ("Changes");
			return 1;
		}

		public IDbStatement Prepare2 (IDbHandle db, string query)
		{
			ActionsTaken.Add ("Prepare2");
			return new MockDbStatement(){Query = query};
		}
		public Result Step (IDbStatement stmt)
		{
			string query = ((MockDbStatement)stmt).Query; 
			if (query.StartsWith ("Create", StringComparison.InvariantCultureIgnoreCase))
				CreateStatementsExecuted.Add (query);
			else if (query.StartsWith ("Delete", StringComparison.InvariantCultureIgnoreCase))
				DeleteStatementsExecuted.Add (query);
			else if (query.StartsWith ("Insert or replace", StringComparison.InvariantCultureIgnoreCase))
				InsertOrReplaceStatementsExecuted.Add (query);
			else if (query.StartsWith ("Insert", StringComparison.InvariantCultureIgnoreCase))
				InsertStatementsExecuted.Add (query);
			else if (query.StartsWith ("Update", StringComparison.InvariantCultureIgnoreCase))
				UpdateStatementsExecuted.Add (query);
			else if (query.StartsWith ("Drop", StringComparison.InvariantCultureIgnoreCase))
				DropStatementsExecuted.Add (query);
			else if (query.StartsWith ("Select", StringComparison.InvariantCultureIgnoreCase))
				SelectStatementsExecuted.Add (query);
			else
				throw new NotImplementedException (query);
			
			ActionsTaken.Add ("Step");
			if (((MockDbStatement)stmt).Query.StartsWith ("select") && ((MockDbStatement)stmt).ResultsStepCount == 0) {
				((MockDbStatement)stmt).ResultsStepCount++;
				return Result.Row;
			} else {
				((MockDbStatement)stmt).ResultsStepCount = 0;
				return Result.Done;
			}
		}

		public Result Reset (IDbStatement stmt)
		{
			return Result.Done;
		}

		public Result Finalize (IDbStatement stmt)
		{
			ActionsTaken.Add ("Finalize");
			return Result.Done;
		}

		public long LastInsertRowid (IDbHandle db)
		{
			throw new NotImplementedException ();
		}

		public string Errmsg16 (IDbHandle db)
		{
			ActionsTaken.Add ("Errmsg16");
			return "Errmsg16";
		}

		public int BindParameterIndex (IDbStatement stmt, string name)
		{
			throw new NotImplementedException ();
		}

		public int BindNull (IDbStatement stmt, int index)
		{
			ActionsTaken.Add ("BindNull");
			return 0;		
		}

		public int BindInt (IDbStatement stmt, int index, int val)
		{
			ActionsTaken.Add ("BindInt");
			return 0;
		}

		public int BindInt64 (IDbStatement stmt, int index, long val)
		{
			ActionsTaken.Add ("BindInt64");
			return 0;
		}

		public int BindDouble (IDbStatement stmt, int index, double val)
		{
			ActionsTaken.Add ("BindDouble");
			return 0;
		}

		public int BindText16 (IDbStatement stmt, int index, string val, int n, IntPtr free)
		{
			ActionsTaken.Add ("BindText16");
			return 0;
		}

		public int BindBlob (IDbStatement stmt, int index, byte[] val, int n, IntPtr free)
		{
			ActionsTaken.Add ("BindBlob");
			return 0;
		}

		public int ColumnCount (IDbStatement stmt)
		{
			return 2;
		}

		public string ColumnName16 (IDbStatement stmt, int index)
		{
			return index == 0 ? "Id" : "Name";
		}

		public ColType ColumnType (IDbStatement stmt, int index)
		{
			switch (index) {
			case 0:
				return ColType.Integer;

			case 1:
				return ColType.Text;
			default:
				throw new NotSupportedException ();

			}
		}

		public int ColumnInt (IDbStatement stmt, int index)
		{
			return 0;
		}

		public long ColumnInt64 (IDbStatement stmt, int index)
		{
			throw new NotImplementedException ();
		}

		public double ColumnDouble (IDbStatement stmt, int index)
		{
			throw new NotImplementedException ();
		}

		public string ColumnText16 (IDbStatement stmt, int index)
		{
			return "XLabs";
		}

		public byte[] ColumnBlob (IDbStatement stmt, int index)
		{
			throw new NotImplementedException ();
		}

		public int ColumnBytes (IDbStatement stmt, int index)
		{
			throw new NotImplementedException ();
		}

		public byte[] ColumnByteArray (IDbStatement stmt, int index)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

