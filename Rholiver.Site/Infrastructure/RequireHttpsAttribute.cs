using System;
using System.Web.Mvc;
using Rholiver.Site.Infrastructure;
using RequireHttpsAttributeBase = System.Web.Mvc.RequireHttpsAttribute;

namespace Rholiver.Site
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class RequireHttpsAttribute : RequireHttpsAttributeBase
    {
        public override void OnAuthorization(AuthorizationContext filterContext) {
            if (filterContext == null) {
                throw new ArgumentNullException("filterContext");
            }

            if (filterContext.HttpContext.Request.SafeIsSecureConnection())
                return;

            if (filterContext.HttpContext.Request.IsLocal) {
                return;
            }

            HandleNonHttpsRequest(filterContext);
        }
    }
}