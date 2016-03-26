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
    using XLabs.Serialization.MsgPack;
    using MsgPack.Serialization;

    [TestFixture()]
    public class CanSerializerTestsMsgPackCli : CanSerializerTests
    {
        protected override ISerializer Serializer => new MsgPackSerializer(new SerializationContext());
    }
}
