using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using XForms.Toolkit.Services;

namespace XForms.Toolkit.Services.Autofac
{
    public class AutofacResolver : IResolver
    {
        private readonly IContainer container;

        public AutofacResolver(IContainer container)
        {
            this.container = container;
        }

        #region IResolver Members

        public T Resolve<T>() where T : class
        {
            return this.container.ResolveOptional<T>();
        }

        public object Resolve(Type type)
        {
            return this.container.ResolveOptional(type);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return new[] { this.Resolve<T>() };
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return new[] { this.Resolve(type) };
        }

        #endregion
    }
}
