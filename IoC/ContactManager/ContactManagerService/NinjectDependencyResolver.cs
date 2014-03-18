using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Web.Http.Dependencies;
using Ninject;

namespace ContactManager
{
    public class NinjectDependencyScope : IDependencyScope
    {
        private IKernel _kernel;

        public NinjectDependencyScope(IKernel kernel)
        {
            Contract.Assert(kernel != null);

            this._kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return this._kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this._kernel.GetAll(serviceType);
        }

        public void Dispose()
        {
            this._kernel = null;
        }
    }

    public class NinjectDependencyResolver : NinjectDependencyScope, IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
            : base(kernel)
        {
            this._kernel = kernel;
        }

        public IDependencyScope BeginScope()
        {
            return new NinjectDependencyScope(this._kernel);
        }
    }
}
