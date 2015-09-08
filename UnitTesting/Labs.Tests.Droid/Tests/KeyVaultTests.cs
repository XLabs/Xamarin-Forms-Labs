namespace Labs.Tests.Droid.Tests
{
    using System.Text;
    using NUnit.Framework;
    using SecureStorageTests;
    using XLabs.Platform.Services;
    using XLabs.Serialization;
    using XLabs.Serialization.ServiceStack;

    [TestFixture]
    public class KeyVaultTests : SecureStorageTests
    {
        #region Overrides of SecureStorageTests

        protected override ISecureStorage Storage
        {
            get { return new KeyVaultStorage(Encoding.UTF8.GetChars(Encoding.UTF8.GetBytes("PassW0rd"))); }
        }

        protected override IByteSerializer Serializer
        {
            get { return new JsonSerializer(); }
        }

        #endregion
    }
}