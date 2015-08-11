namespace Labs.Tests.WP8.Tests
{
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SecureStorageTests;
    using XLabs.Platform.Services;
    using XLabs.Serialization;
    using XLabs.Serialization.ServiceStack;

    [TestClass]
    public class WpSecureStorage : SecureStorageTests
    {
        #region Overrides of SecureStorageTests

        protected override ISecureStorage Storage
        {
            get { return new SecureStorage();}
        }

        protected override IByteSerializer Serializer
        {
            get { return new JsonSerializer(); }
        }

        #endregion
    }
}