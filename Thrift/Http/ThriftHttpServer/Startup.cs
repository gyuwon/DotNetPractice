using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Owin;
using Thrift.Transport;

namespace ThriftServer.Http
{
    public class ThriftHttpHandler : THttpHandler
    {
        public ThriftHttpHandler(Func<IDictionary<string, object>, Task> next)
            : base(new ContactsService.Processor(new ContactsServiceHandler()))
        {
        }

        public Task Invoke(IDictionary<string, dynamic> env)
        {
            Stream req = env["owin.RequestBody"];
            Stream res = env["owin.ResponseBody"];
            return Task.Factory.StartNew(() => this.ProcessRequest(req, res));
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
