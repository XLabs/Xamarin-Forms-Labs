#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace SerializationTests
{
    using XLabs.Serialization;

    //[TestFixture()]
    //public class CanSerializeTestsSystemJson : CanSerializerTests
    //{
    //    protected override ISerializer Serializer
    //    {
    //        get { return new SystemJsonSerializer(); }
    //    }
    //}
}