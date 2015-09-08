#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace IocTests
{
    using Autofac;
    using XLabs.Ioc;
    using XLabs.Ioc.Autofac;

    [TestFixture()]
    public class AutofacResolveTests : ResolveTests
    {
        protected override IResolver GetEmptyResolver()
        {
            return new AutofacResolver(new ContainerBuilder().Build());
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
            return new AutofacContainer(new ContainerBuilder().Build());
        }
    }
}
