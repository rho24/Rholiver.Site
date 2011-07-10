using System;
using System.Diagnostics;
using System.Web.Mvc;
using RequireHttpsAttributeBase = System.Web.Mvc.RequireHttpsAttribute;

namespace Rholiver.Site.Infrastructure
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequireHttpsWhenLoggedOnAttribute : RequireHttpsAttributeBase
    {
        public override void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }
            Trace.TraceInformation("RequireHttpsWhenLoggedOnAttribute.OnAuthorization()");
            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                Trace.TraceInformation("Authenticated");
                if (filterContext.HttpContext.Request.IsSecureConnection) {
                    return;
                }

                if (string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"],
                                  "https",
                                  StringComparison.InvariantCultureIgnoreCase)) {
                    return;
                }

                if (filterContext.HttpContext.Request.IsLocal) {
                    return;
                }

                HandleNonHttpsRequest(filterContext);
            }
            else {
                Trace.TraceInformation("Not authenticated");
                if (!filterContext.HttpContext.Request.IsSecureConnection) {
                    Trace.TraceInformation("Not secure connection");
                    return;
                }

                if (string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"],
                                  "http",
                                  StringComparison.InvariantCultureIgnoreCase)) {
                    Trace.TraceInformation("X-Forwarded-Proto = http");
                    return;
                }

                RedirectToHttp(filterContext);
            }
        }

        void RedirectToHttp(AuthorizationContext filterContext) {
            Trace.TraceInformation("RedirectToHttp");
            var url = filterContext.HttpContext.Request.Url;
            var newUrl = "http://{0}{1}{2}".Fmt(url.Host,url.IsDefaultPort ? "": ":" + url.Port, filterContext.HttpContext.Request.RawUrl);
            Trace.TraceInformation("New url = '{0}'", newUrl);
            filterContext.Result = new RedirectResult(newUrl);
        }
    }
}