#if WINDOWS_PHONE
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace IocTests
{
    using TinyIoC;
    using XLabs.Ioc;
    using XLabs.Ioc.TinyIOC;

    [TestFixture()]
    public class TinyIocResolveTests : ResolveTests
    {
        protected override IResolver GetEmptyResolver()
        {
            return new TinyResolver(new TinyIoCContainer());
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
            return new TinyContainer(new TinyIoCContainer());
        }
    }
}
