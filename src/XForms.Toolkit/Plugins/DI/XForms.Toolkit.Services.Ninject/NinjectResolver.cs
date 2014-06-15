using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace XForms.Toolkit.Services.Ninject
{
    public class NinjectResolver : IResolver
    {
        private readonly IKernel kernel;

        public NinjectResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        #region IResolver Members

        public T Resolve<T>() where T : class
        {
            return this.kernel.Get<T>();
        }

        public object Resolve(Type type)
        {
            return this.kernel.Get(type);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return this.kernel.GetAll<T>();
        }

        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.kernel.GetAll(type);
        }
        #endregion
    }
}
