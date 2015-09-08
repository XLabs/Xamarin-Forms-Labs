namespace Labs.Tests.iOS
{
    using NUnit.Framework;

    [TestFixture]
    public class DeviceTests
    {
        [TestFixtureSetUp]
        public void Initialize()
        {
            

        }

        [TestFixtureTearDown]
        public void ShutDown()
        {
            
        }

        [Test]
        public void Test1()
        {
            //var device = new Pad(4, 4);

            //var actual = device.Version;
            //var expected = Pad.iPadVersion.iPadMini2GWiFi;

            //Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Fail()
        {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }
    }
}