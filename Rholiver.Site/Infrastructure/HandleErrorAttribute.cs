using System;
using System.Web;
using System.Web.Mvc;
using Elmah;

namespace Rholiver.Site.Infrastructure
{
    public class HandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);
            if (context.ExceptionHandled)
                RaiseErrorSignal(context.Exception);
        }

        private static void RaiseErrorSignal(Exception e)
        {
            var context = HttpContext.Current;
            ErrorSignal.FromContext(context).Raise(e, context);
        }
    }
}