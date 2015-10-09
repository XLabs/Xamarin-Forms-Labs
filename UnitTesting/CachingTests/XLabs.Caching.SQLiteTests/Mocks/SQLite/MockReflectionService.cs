using System;
using SQLite.Net.Interop;
using System.Collections.Generic;
using System.Reflection;

namespace XLabs.Caching.SQLiteTests.Mocks
{
	public class MockReflectionService:IReflectionService
	{
		public MockReflectionService ()
		{
		}

		#region IReflectionService implementation

		public System.Collections.Generic.IEnumerable<System.Reflection.PropertyInfo> GetPublicInstanceProperties (Type mappedType)
		{
			return mappedType.GetRuntimeProperties ();
		}

		public object GetMemberValue (object obj, System.Linq.Expressions.Expression expr, System.Reflection.MemberInfo member)
		{
			throw new NotImplementedException ();
		}

		#endregion
	}
}

