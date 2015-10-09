using System;
using SQLite.Net;

namespace XLabs.Caching.SQLiteTests.Mocks
{
	public class MockBlobSerializer:IBlobSerializer
	{
		public MockBlobSerializer ()
		{
		}

		#region IBlobSerializer implementation

		public byte[] Serialize<T> (T obj)
		{
			throw new NotImplementedException ();
		}

		public object Deserialize (byte[] data, Type type)
		{
			throw new NotImplementedException ();
		}

		public bool CanDeserialize (Type type)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

