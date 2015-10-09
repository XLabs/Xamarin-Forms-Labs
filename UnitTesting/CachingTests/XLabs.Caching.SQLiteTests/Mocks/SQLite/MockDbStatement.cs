using System;
using SQLite.Net.Interop;

namespace XLabs.Caching.SQLiteTests
{
	public class MockDbStatement:IDbStatement
	{
		public MockDbStatement ()
		{
		}

		public string Query{get;set;}

		public int ResultsStepCount{get;set;}
	}
}

