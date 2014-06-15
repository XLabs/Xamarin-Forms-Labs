using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector;

namespace XForms.Toolkit.Services.SimpleContainer
{
    public class SimpleInjectorResolver : IResolver
    {
        private readonly Container container;

        public SimpleInjectorResolver(Container container)
        {
            this.container = container;
        }

        #region IResolver Members

        public T Resolve<T>() where T : class
        {
            return this.container.GetInstance<T>();
        }

        public object Resolve(Type type)
        {
            return this.container.GetInstance(type);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return this.container.GetAllInstances<T>();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.container.GetAllInstances(type);
        }

        #endregion
    }
}
