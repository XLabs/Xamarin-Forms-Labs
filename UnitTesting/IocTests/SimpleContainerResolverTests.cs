#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace IocTests
{
    using XLabs.Ioc;

    [TestFixture()]
    public class SimpleContainerResolverTests : ResolveTests
    {
        protected override IResolver GetEmptyResolver()
        {
            return new SimpleContainer().GetResolver();
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
            return new SimpleContainer();
        }
    }
}
