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

            if (filterContext.ActionDescriptor.GetCustomAttributes(typeof (RequireHttpsAttribute), true).Length != 0 ||
                filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof (RequireHttpsAttribute),
                                                                                        true).Length != 0)
                return;

            if (filterContext.HttpContext.User.Identity.IsAuthenticated) {
                if (filterContext.HttpContext.Request.SafeIsSecureConnection())
                    return;

                if (filterContext.HttpContext.Request.IsLocal) {
                    return;
                }

                HandleNonHttpsRequest(filterContext);
            }
            else {
                if (!filterContext.HttpContext.Request.SafeIsSecureConnection())
                    return;

                RedirectToHttp(filterContext);
            }
        }

        void RedirectToHttp(AuthorizationContext filterContext) {
            var url = filterContext.HttpContext.Request.SafeUrl();
            var newUrl = "http://{0}{1}{2}".Fmt(url.Host, url.IsDefaultPort ? "" : ":" + url.Port,
                                                filterContext.HttpContext.Request.RawUrl);
            filterContext.Result = new RedirectResult(newUrl);
        }
    }
}