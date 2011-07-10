using System;
using System.Diagnostics;
using System.Web;

namespace Rholiver.Site.Infrastructure
{
    public static class RequestExtensions
    {
        public static bool SafeIsSecureConnection(this HttpRequestBase request) {
            if (request.Headers["X-Forwarded-Proto"] == null)
                return request.IsSecureConnection;

            return string.Equals(request.Headers["X-Forwarded-Proto"], "https",
                                 StringComparison.InvariantCultureIgnoreCase);
        }

        public static Uri SafeUrl(this HttpRequestBase request) {
            if (request.Headers["X-Forwarded-Proto"] == null)
                return request.Url;

            var cleanUrl = request.Url.GetComponents(UriComponents.AbsoluteUri & ~UriComponents.Port,
                                                     UriFormat.UriEscaped);

            Trace.TraceInformation("SafeUrl - clean url '{0}'", cleanUrl);

            return new Uri(cleanUrl);
        }
    }
}