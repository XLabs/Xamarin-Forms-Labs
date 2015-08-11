namespace Labs.Tests.iOS.Tests
{
    using NUnit.Framework;
    using SecureStorageTests;
    using XLabs.Platform.Services;
    using XLabs.Serialization;
    using XLabs.Serialization.ServiceStack;

    [TestFixture]
    public class SecKeyTests : SecureStorageTests
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