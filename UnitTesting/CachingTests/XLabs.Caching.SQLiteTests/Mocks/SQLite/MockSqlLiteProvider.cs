using System;
using SQLite.Net.Interop;

namespace XLabs.Caching.SQLiteTests.Mocks
{
	public class MockSqlLiteProvider:ISQLitePlatform
	{
		#region ISQLitePlatform implementation

		public ISQLiteApi SQLiteApi {
			get {
				return new Mocks.MockSQLiteApi ();
			}
		}

		public IStopwatchFactory StopwatchFactory {
			get {
				throw new NotImplementedException ();
			}
		}

		public IReflectionService ReflectionService {
			get {
				return new MockReflectionService ();
			}
		}

		public IVolatileService VolatileService {
			get {
				throw new NotImplementedException ();
			}
		}

		#endregion

		public MockSqlLiteProvider ()
		{
		}
	}
}

