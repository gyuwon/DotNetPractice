using Owin;

namespace SelfHostedNancy
{
    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.UseNancy();
        }
    }
}
