using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using Owin;
using Thrift.Protocol;
using Thrift.Transport;

namespace ThriftServer.Http
{
    public class ThriftHttpHandler : THttpHandler
    {
        public ThriftHttpHandler(Func<IDictionary<string, object>, Task> next)
            : base(new ContactsService.Processor(new ContactsServiceHandler()))
        {
        }

        public Task Invoke(IDictionary<string, object> environment)
        {
            return Task.Factory.StartNew(() =>
            {
                Stream request = environment["owin.RequestBody"] as Stream;
                Stream response = environment["owin.ResponseBody"] as Stream;
                this.ProcessRequest(request, response);
            });
        }
    }

    public class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(typeof(ThriftHttpHandler));
        }
    }
}
