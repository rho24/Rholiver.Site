using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.RelyingParty;
using System.Web.Security;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId.Extensions.SimpleRegistration;

namespace Rholiver.Site.Controllers
{
    public class UserController : Controller
    {
        private static OpenIdRelyingParty openid = new OpenIdRelyingParty();

        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("/User/Login?ReturnUrl=Index");
            }

            return View("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Login()
        {
            // Stage 1: display login form to user
            return View("Login");
        }

        [ValidateInput(false)]
        public ActionResult Authenticate(string openidIdentifyer, string returnUrl)
        {
            var response = openid.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openidIdentifyer, out id))
                {
                    try
                    {
                        var authenticateRequest = openid.CreateRequest(openidIdentifyer);
                        authenticateRequest.AddExtension(new ClaimsRequest {
                            FullName = DemandLevel.Require,
                            Email = DemandLevel.Require,
                            Nickname = DemandLevel.Require
                        });

                        return authenticateRequest.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewData["Message"] = ex.Message;
                        return View("Login");
                    }
                }
                else
                {
                    ViewData["Message"] = "Invalid identifier";
                    return View("Login");
                }
            }
            else
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:

                        var userInfo = response.GetExtension<ClaimsResponse>();

                        //Session["FriendlyIdentifier"] = response.FriendlyIdentifierForDisplay;
                        FormsAuthentication.SetAuthCookie(userInfo.Email, false);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    case AuthenticationStatus.Canceled:
                        ViewData["Message"] = "Canceled at provider";
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        ViewData["Message"] = response.Exception.Message;
                        return View("Login");
                }
            }
            return new EmptyResult();
        }

    }
}
