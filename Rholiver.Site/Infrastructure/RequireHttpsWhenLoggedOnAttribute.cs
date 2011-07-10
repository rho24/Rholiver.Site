using System;
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

            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
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
                if (!filterContext.HttpContext.Request.IsSecureConnection) {
                    return;
                }

                if (string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"],
                                  "http",
                                  StringComparison.InvariantCultureIgnoreCase)) {
                    return;
                }

                RedirectToHttp(filterContext);
            }
        }

        void RedirectToHttp(AuthorizationContext filterContext) {
            var url = filterContext.HttpContext.Request.Url;
            var newUrl = "http://{0}{1}{2}".Fmt(url.Host,url.IsDefaultPort ? "": ":" + url.Port, filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectResult(newUrl);
        }
    }
}