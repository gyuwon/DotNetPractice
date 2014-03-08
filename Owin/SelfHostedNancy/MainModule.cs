using Nancy;

namespace SelfHostedNancy
{
    public class MainModule : NancyModule
    {
        public MainModule()
        {
            this.Get["/"] = _ => "Hello Nancy";
        }
    }
}
