#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

using System;

namespace SecureStorageTests
{
    using XLabs.Platform.Services;
    using XLabs.Serialization;

    [TestFixture]
    public abstract class SecureStorageTests
    {
        private const string Key = "test";
        private const string Value = "value";
        private const string NewValue = "NewValue";

        protected abstract ISecureStorage Storage { get; }
        protected abstract IByteSerializer Serializer { get; }

        [Test]
        public void CanStore()
        {
            this.Storage.Store(Key, this.Serializer.SerializeToBytes(Value));
            Assert.IsTrue(this.Storage.Contains(Key));
        }

        [Test]
        public void CanRetrieve()
        {
            try
            {
                this.Storage.Store(Key, this.Serializer.SerializeToBytes(Value));
                Assert.IsTrue(this.Storage.Contains(Key));
            }
            catch (Exception ex)
            {
                Assert.Inconclusive(ex.Message);
            }


            var value = this.Serializer.Deserialize<string>(this.Storage.Retrieve(Key));
            Assert.AreEqual(value, Value);
        }

        [Test]
        public void CanReplace()
        {
            string value;

            try
            {
                this.Storage.Store(Key, this.Serializer.SerializeToBytes(Value));
                value = this.Serializer.Deserialize<string>(this.Storage.Retrieve(Key));
                Assert.AreEqual(value, Value);
            }
            catch (Exception ex)
            {
                Assert.Inconclusive(ex.Message);
            }

            this.Storage.Store(Key, this.Serializer.SerializeToBytes(NewValue));
            value = this.Serializer.Deserialize<string>(this.Storage.Retrieve(Key));

            Assert.AreEqual(value, NewValue);
        }

        [Test]
        public void CanDelete()
        {
            try
            {
                this.Storage.Store(Key, this.Serializer.SerializeToBytes(Value));
                Assert.IsTrue(this.Storage.Contains(Key));
            }
            catch (Exception ex)
            {
                Assert.Inconclusive(ex.Message);
            }

            this.Storage.Delete(Key);
            Assert.IsFalse(this.Storage.Contains(Key));
        }

        [Test]
        public void DoesNotThrowOnEmptyDelete()
        {
            Exception exception = null;
            if (this.Storage.Contains(Key))
            {
                this.Storage.Delete(Key);
            }

            try
            {
                this.Storage.Delete(Key);
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNull(exception, "Should not throw if key doesn't exist");
        }
    }
}
