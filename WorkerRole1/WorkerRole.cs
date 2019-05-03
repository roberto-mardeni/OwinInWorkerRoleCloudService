using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private IDisposable _httpApp = null;
        private IDisposable _httpsApp = null;

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole entry point called", "Information");

            while (true)
            {
                Thread.Sleep(10000);
                Trace.TraceInformation("Working", "Information");
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;

            var httpEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HttpEndpoint"];
            string httpBaseUri = String.Format("{0}://{1}",
                httpEndpoint.Protocol, httpEndpoint.IPEndpoint);

            Trace.TraceInformation(String.Format("Starting OWIN HTTP at {0}", httpBaseUri),
                "Information");

            _httpApp = WebApp.Start<Startup>(new StartOptions(url: httpBaseUri));

            var httpsEndpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["HttpsEndpoint"];
            string httpsBaseUri = String.Format("{0}://{1}",
                httpsEndpoint.Protocol, httpsEndpoint.IPEndpoint);

            Trace.TraceInformation(String.Format("Starting OWIN HTTPS at {0}", httpsBaseUri),
                "Information");

            _httpsApp = WebApp.Start<Startup>(new StartOptions(url: httpsBaseUri));

            return base.OnStart();
        }

        public override void OnStop()
        {
            if (_httpApp != null)
            {
                _httpApp.Dispose();
            }
            if (_httpsApp != null)
            {
                _httpsApp.Dispose();
            }
            base.OnStop();
        }
    }
}