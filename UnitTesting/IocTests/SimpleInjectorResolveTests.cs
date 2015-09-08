#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace IocTests
{
    using SimpleInjector;
    using XLabs.Ioc;
    using XLabs.Ioc.SimpleInjectorContainer;

    [TestFixture()]
    public class SimpleInjectorResolveTests : ResolveTests
    {
        protected override IResolver GetEmptyResolver()
        {
            return new SimpleInjectorResolver(new Container());
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
            return new SimpleInjectorContainer();
        }
    }
}
