#if WINDOWS_PHONE
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using TestFixture = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestClassAttribute;
using Test = Microsoft.VisualStudio.TestPlatform.UnitTestFramework.TestMethodAttribute;
#else
using NUnit.Framework;
#endif

namespace IocTests
{
    using System;
    using System.Linq;
    using XLabs.Ioc;

    [TestFixture()]
    public abstract class ResolveTests
    {
        protected abstract IResolver GetEmptyResolver();

        protected abstract IDependencyContainer GetEmptyContainer();

        [Test]
        public void CanResolveWithLambda()
        {
            var container = this.GetEmptyContainer();

            container.Register<IDependencyContainer>(t => container);

            var resolver = container.GetResolver();

            var c2 = resolver.Resolve<IDependencyContainer>();

            Assert.AreEqual(container, c2);
        }

        [Test]
        public void ReturnsNullIfNotRegisteredForGeneric()
        {
            var empty = this.GetEmptyResolver().Resolve<IDependencyContainer>();

            Assert.IsNull(empty);
        }

        [Test]
        public void ReturnsNullIfNotRegisteredForType()
        {
            var empty = this.GetEmptyResolver().Resolve(typeof(IDependencyContainer));

            Assert.IsNull(empty);
        }

        [Test]
        public void ReturnsEmptyIEnumerableIfNotRegisteredForGeneric()
        {
            var empty = this.GetEmptyResolver().ResolveAll<IDependencyContainer>();
            Assert.IsNotNull(empty);
            Assert.IsTrue(!empty.Any());
        }

        [Test]
        public void ReturnsEmptyIEnumerableIfNotRegisteredForType()
        {
            var empty = this.GetEmptyResolver().ResolveAll(typeof(IDependencyContainer));
            Assert.IsNotNull(empty);
            Assert.IsTrue(!empty.Any());
        }

        [Test]
        public void DoesNotHideExceptionsForSingleGeneric()
        {
            var container = this.GetEmptyContainer();

            container.Register<IDependencyContainer, InvalidClass>();

            var resolver = container.GetResolver();

            Exception exception = null;

            try
            {
                resolver.Resolve<IDependencyContainer>();
            }
            catch (Exception ex)
            {
                exception = ex;
            }

            Assert.IsNotNull(exception);

            while (!(exception is NotImplementedException))
            {
                exception = exception.InnerException;
                Assert.IsNotNull(exception, "Exception did not contain the original exception.");
            }
        }

        [Test]
        public void RegisterGeneric()
        {
            var container = this.GetEmptyContainer();

            container.Register(typeof(IFoo<>), typeof(Foo<>));

            var resolver = container.GetResolver();

            var genericInt = resolver.Resolve<IFoo<int>>();

            Assert.IsNotNull(genericInt);

            //Assert.IsTrue(genericInt is IFoo<int>);

        }

        [Test]
        public void CanInjectCtorDependenciesAutomatically()
        {
            var container = this.GetEmptyContainer();

            container.Register<IService, Service>().Register<IMyService, MyServiceNoDefaultCtor>();

            var resolver = container.GetResolver();

            var service = resolver.Resolve<IMyService>();

            Assert.IsNotNull(service);
            Assert.IsNotNull(service.Service);
            Assert.IsTrue(service.Service is Service);
        }

        [Test]
        public void RegisterMultipleGeneric()
        {
            var container = this.GetEmptyContainer();

            container.Register<IService, Service>().Register<IService, Service1>().Register<IService, Service2>();

            var resolver = container.GetResolver();

            var services = resolver.ResolveAll<IService>().DefaultIfEmpty(null).ToList();

            Assert.IsNotNull(services);
            Assert.IsTrue(services.Count() == 3);
        }

        [Test]
        public void RegisterMultipleType()
        {
            var container = this.GetEmptyContainer();

            container.Register<IService>(typeof(Service)).Register<IService>(typeof(Service1)).Register<IService>(typeof(Service2));

            var resolver = container.GetResolver();

            var services = resolver.ResolveAll(typeof(IService)).DefaultIfEmpty(null).ToList();

            Assert.IsNotNull(services);
            Assert.IsTrue(services.Count() == 3);
        }

        public class InvalidClass : IDependencyContainer
        {
            public InvalidClass()
            {
                throw new NotImplementedException();
            }

            #region IDependencyContainer Members

            public IResolver GetResolver()
            {
                throw new NotImplementedException();
            }

            public IDependencyContainer Register<T>(T instance) where T : class
            {
                throw new NotImplementedException();
            }

            public IDependencyContainer Register<T, TImpl>()
                where T : class
                where TImpl : class, T
            {
                throw new NotImplementedException();
            }

            public IDependencyContainer RegisterSingle<T, TImpl>()
                where T : class
                where TImpl : class, T
            {
                throw new NotImplementedException();
            }

            public IDependencyContainer Register<T>(Type type) where T : class
            {
                throw new NotImplementedException();
            }

            public IDependencyContainer Register(Type type, Type impl)
            {
                throw new NotImplementedException();
            }

            public IDependencyContainer Register<T>(Func<IResolver, T> func) where T : class
            {
                throw new NotImplementedException();
            }

            #endregion
        }

        public interface IFoo<T> { };
        public class Foo<T> : IFoo<T> { };
    }
}
