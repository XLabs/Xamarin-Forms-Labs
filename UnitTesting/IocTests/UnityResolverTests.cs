//#if !__IOS__

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
    using XLabs.Ioc.Unity;

    [TestFixture()]
    public class UnityResolverTests : ResolveTests
    {

        protected override IResolver GetEmptyResolver()
        {
            return GetEmptyContainer().GetResolver();
            //return new UnityResolver(new Microsoft.Practices.Unity.UnityContainer());
        }

        protected override IDependencyContainer GetEmptyContainer()
        {
            return new UnityDependencyContainer();
        }
    }
}
//#endif
