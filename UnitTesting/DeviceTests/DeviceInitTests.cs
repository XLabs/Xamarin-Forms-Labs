using System;
using System.Collections.Generic;
using System.Text;

#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

//#if WINDOWS_PHONE
//using PlatformDevice = Xamarin.Forms.Labs.WindowsPhoneDevice;
//using DeviceCapabilities = Xamarin.Forms.Labs.DeviceCapabilities;
//#elif __IOS__
//using PlatformDevice = Xamarin.Forms.Labs.AppleDevice;
//#elif __ANDROID__
//using PlatformDevice = Xamarin.Forms.Labs.AndroidDevice;
//#endif

namespace DeviceTests
{
//    [TestFixture()]
//    public class DeviceInitTests
//    {
//        [Test]
//        public void CanInit()
//        {
//            Assert.IsNotNull(PlatformDevice.CurrentDevice);
//        }

//        [Test]
//        public void HasDisplay()
//        {
//            Assert.IsNotNull(PlatformDevice.CurrentDevice.Display);
//        }

//        [Test]
//        public void HasBattery()
//        {
//            Assert.IsNotNull(PlatformDevice.CurrentDevice.Battery);
//        }

//        [Test]
//        public void HasMediaPicker()
//        {
//#if WINDOWS_PHONE
//            if (DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.ID_CAP_ISV_CAMERA))
//            {
//                Assert.IsNotNull(PlatformDevice.CurrentDevice.MediaPicker);
//            }
//            else
//            {
//                Assert.ThrowsException<UnauthorizedAccessException>(() => PlatformDevice.CurrentDevice.MediaPicker);
//                Assert.Inconclusive("Unable to initialize MediaPicker since {0} has not been defined in app manifest.", DeviceCapabilities.Capability.ID_CAP_ISV_CAMERA);
//            }
//#else
//            Assert.IsNotNull(PlatformDevice.CurrentDevice.MediaPicker);
//#endif
//        }

//        [Test]
//        public void HasName()
//        {
//            Assert.IsFalse(string.IsNullOrEmpty(PlatformDevice.CurrentDevice.Name));
//        }

//        [Test]
//        public void HasHardwareVersion()
//        {
//            Assert.IsFalse(string.IsNullOrEmpty(PlatformDevice.CurrentDevice.HardwareVersion));
//        }

//        [Test]
//        public void HasId()
//        {
//#if WINDOWS_PHONE
//            if (DeviceCapabilities.IsEnabled(DeviceCapabilities.Capability.ID_CAP_IDENTITY_DEVICE))
//            {
//                Assert.IsFalse(string.IsNullOrEmpty(PlatformDevice.CurrentDevice.Id));
//            }
//            else
//            {
//                Assert.ThrowsException<UnauthorizedAccessException>(() => PlatformDevice.CurrentDevice.Id);
//                Assert.Inconclusive("Unable to get device ID since {0} has not been defined in app manifest.", Xamarin.Forms.Labs.DeviceCapabilities.Capability.ID_CAP_IDENTITY_DEVICE);
//            }
//#else
//            Assert.IsFalse(string.IsNullOrEmpty(PlatformDevice.CurrentDevice.Id));
//#endif
//        }

//        [Test]
//        public void HasManufacturer()
//        {
//            Assert.IsFalse(string.IsNullOrEmpty(PlatformDevice.CurrentDevice.Manufacturer));
//        }

//        [Test]
//        public void HasFirmwareVersion()
//        {
//            Assert.IsFalse(string.IsNullOrEmpty(PlatformDevice.CurrentDevice.FirmwareVersion));
//        }
//    }
}
