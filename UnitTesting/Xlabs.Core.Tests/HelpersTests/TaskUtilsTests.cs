using System;
using NUnit.Framework;
using XLabs;

namespace Xlabs.Core.HelpersTests
{
	[TestFixture]
	public class TaskUtilsTests
	{
		class dataClass{}
		[Test]
		public void TaskFromResultTest ()
		{
			var taskUtils = TaskUtils.TaskFromResult<dataClass>(new dataClass());
			Assert.IsNotNull (taskUtils);
		}
	}
}

