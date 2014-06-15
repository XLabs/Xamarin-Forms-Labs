using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyIoC;

namespace XForms.Toolkit.Services.TinyIOC
{
    public class TinyResolver : IResolver
    {
        private readonly TinyIoCContainer container;

        public TinyResolver(TinyIoCContainer container)
        {
            this.container = container;
        }

        #region IResolver Members

        public T Resolve<T>() where T : class
        {
            return this.container.Resolve<T>();
        }

        public object Resolve(Type type)
        {
            return this.container.Resolve(type);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return this.container.ResolveAll<T>();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.container.ResolveAll(type);
        }

        #endregion
    }
}
