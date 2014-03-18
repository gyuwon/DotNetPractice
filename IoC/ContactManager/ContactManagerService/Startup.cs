using System.Web.Http;
using ContactManager.Models;
using Ninject;
using Owin;

namespace ContactManager
{
    public class Startup
    {
        public static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<IRepository>().To<ContactManagerContext>();
            return kernel;
        }

        public void Configuration(IAppBuilder builder)
        {
            var config = new HttpConfiguration();
            config.DependencyResolver = new NinjectDependencyResolver(CreateKernel());
            config.Routes.MapHttpRoute(
                name: "WebApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional });
            builder.UseWebApi(config);
        }
    }
}
