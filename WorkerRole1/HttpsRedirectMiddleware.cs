using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public enum RedirectStatusCode
    {
        Permanent = 301,
        SeeOther = 303,
        Temporary = 307,
    }

    public class HttpsRedirectMiddleware : OwinMiddleware
    {
        private readonly int httpsPort;
        private readonly RedirectStatusCode statusCode;

        public HttpsRedirectMiddleware(OwinMiddleware next, int httpsPort, RedirectStatusCode statusCode) : base(next)
        {
            this.httpsPort = httpsPort;
            this.statusCode = statusCode;
        }

        public override async Task Invoke(IOwinContext context)
        {
            var protoHeader = context.Request.Headers["X-Forwarded-Proto"]?.ToString();
            if (context.Request.IsSecure || (protoHeader != null && protoHeader.ToLower().Equals("https")))
            {
                await Next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)statusCode;
                var location = httpsPort == 443
                    ? $"https://{context.Request.Uri.Host}{context.Request.Uri.PathAndQuery}"
                    : $"https://{context.Request.Uri.Host}:{httpsPort}{context.Request.Uri.PathAndQuery}";
                context.Response.Redirect(location);
            }
        }
    }

    public static class HttpsRedirectMiddlewareExtensions
    {
        public static IAppBuilder UseHttpsRedirect(this IAppBuilder builder, int httpsPort = 443, RedirectStatusCode statusCode = RedirectStatusCode.Permanent)
        {
            return builder.Use<HttpsRedirectMiddleware>(httpsPort, statusCode);
        }
    }
}
