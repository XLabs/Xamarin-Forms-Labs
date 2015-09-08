using System;

#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

//#if WINDOWS_PHONE
//using PlatformDevice = Xamarin.Forms.Labs.WindowsPhoneDevice;
//#elif __IOS__
//using PlatformDevice = Xamarin.Forms.Labs.AppleDevice;
//#elif __ANDROID__
//using PlatformDevice = XLabs..AndroidDevice;
//#endif

namespace DeviceTests
{
    [TestFixture()]
    public class DeviceInfoTests
    {
        
    }
}

