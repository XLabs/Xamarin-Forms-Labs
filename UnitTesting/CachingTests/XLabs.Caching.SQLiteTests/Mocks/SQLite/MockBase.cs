using System;
using System.Collections.Generic;

namespace XLabs.Caching.SQLiteTests
{
	public class MockBase
	{
		public static List<string> CreateStatementsExecuted;
		public static List<string> DeleteStatementsExecuted;
		public static List<string> InsertStatementsExecuted;
		public static List<string> UpdateStatementsExecuted;
		public static List<string> SelectStatementsExecuted;
		public static List<string> DropStatementsExecuted;
		public static List<string> InsertOrReplaceStatementsExecuted;
		public static List<string> ActionsTaken;
		static MockBase()
		{
			CreateStatementsExecuted = new List<string>();
			DeleteStatementsExecuted = new List<string>();
			InsertStatementsExecuted = new List<string>();
			UpdateStatementsExecuted = new List<string>();
			SelectStatementsExecuted = new List<string>();
			DropStatementsExecuted = new List<string>();

			ActionsTaken = new List<string>();
		}
	}
}

