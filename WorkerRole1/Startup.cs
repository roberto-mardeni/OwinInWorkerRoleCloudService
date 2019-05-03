using Owin;
using System;
using System.Diagnostics;

namespace WorkerRole1
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseHttpsRedirect();

            app.Run(context =>
            {
                Trace.TraceInformation("Processed Request", "Information");

                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync(string.Format("Hello, world: {0}", DateTime.Now));
            });
        }
    }
}
