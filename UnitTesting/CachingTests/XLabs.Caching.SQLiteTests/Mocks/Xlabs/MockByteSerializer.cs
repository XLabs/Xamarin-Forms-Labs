using System;
using XLabs.Serialization;

namespace XLabs.Caching.SQLiteTests.Mocks
{
	public class MockByteSerializer:IByteSerializer
	{
		#region IByteSerializer implementation

		public byte[] SerializeToBytes<T> (T obj)
		{
			return new byte[]{ 0x00 , 0xff };
			throw new NotImplementedException ();
		}

		public T Deserialize<T> (byte[] data)
		{
			return default(T);
		}

		public object Deserialize (byte[] data, Type type)
		{
			throw new NotImplementedException ();
		}

		#endregion

		public MockByteSerializer ()
		{
		}
	}
}

