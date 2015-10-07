using System;
using NUnit.Framework;
using XLabs.Exceptions;
using System.Collections.Generic;

namespace Xlabs.Core.ExceptionTests
{
	[TestFixture]
	public class NoDataTemplateMatchExceptionTests
	{
		public class firstparam{}
		public class secondparam{}
		[Test]
		public void ConstructorSetsMessage()
		{
			var target = new NoDataTemplateMatchException (typeof(firstparam),new List<Type>{typeof(secondparam)});
			Assert.IsNotNullOrEmpty( target.Message);
		}
		[Test]
		public void ConstructorAssignsAttemptedMatch()
		{
			var target = new NoDataTemplateMatchException (typeof(firstparam),new List<Type>{typeof(secondparam)});
			Assert.AreEqual (target.AttemptedMatch, typeof(firstparam));
		}
		[Test]
		public void ConstructorAssignsTypesExamined()
		{
			var target = new NoDataTemplateMatchException (typeof(firstparam),new List<Type>{typeof(secondparam)});
			Assert.IsNotEmpty(target.TypesExamined);
		}
		[Test]
		public void ConstructorAssignsTypeNamesExamined()
		{
			var target = new NoDataTemplateMatchException (typeof(firstparam),new List<Type>{typeof(secondparam)});
			Assert.Contains("secondparam",target.TypeNamesExamined);
		}

	}
}

