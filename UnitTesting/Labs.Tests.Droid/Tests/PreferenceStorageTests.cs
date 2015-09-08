namespace Labs.Tests.Droid.Tests
{
    using NUnit.Framework;
    using SecureStorageTests;
    using XLabs.Platform.Services;
    using XLabs.Serialization;
    using XLabs.Serialization.ServiceStack;

    [TestFixture]
    public class PreferenceStorageTests : SecureStorageTests
    {
        #region Overrides of SecureStorageTests

        protected override ISecureStorage Storage
        {
            get { return new SharedPreferencesStorage("PassW0rd"); }
        }

        protected override IByteSerializer Serializer
        {
            get { return new JsonSerializer(); }
        }

        #endregion
    }
}