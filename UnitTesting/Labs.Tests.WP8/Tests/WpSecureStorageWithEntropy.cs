namespace Labs.Tests.WP8.Tests
{
    using System.Text;
    using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
    using SecureStorageTests;
    using XLabs.Platform.Services;
    using XLabs.Serialization;
    using XLabs.Serialization.ServiceStack;

    [TestClass]
    public class WpSecureStorageWithEntropy : SecureStorageTests
    {
        #region Overrides of SecureStorageTests

        protected override ISecureStorage Storage
        {
            get { return new SecureStorage(Encoding.UTF8.GetBytes("PassW0rd")); }
        }

        protected override IByteSerializer Serializer
        {
            get { return new JsonSerializer(); }
        }

        #endregion
    }
}