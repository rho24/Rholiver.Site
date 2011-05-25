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
using Rholiver.Site.Infrastructure;

namespace Rholiver.Site.Controllers
{
    public class UserController : Controller
    {
        public OpenIdRelyingParty OpenIdService { get; set; }
        private IUserProvider UserProvider { get; set; }

        public UserController(OpenIdRelyingParty openIdService, IUserProvider userProvider)
        {
            OpenIdService = openIdService;
            UserProvider = userProvider;
        }

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
            var response = OpenIdService.GetResponse();
            if (response == null)
            {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openidIdentifyer, out id))
                {
                    try
                    {
                        var authenticateRequest = OpenIdService.CreateRequest(openidIdentifyer);

                        return authenticateRequest.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex)
                    {
                        ViewBag.Message = ex.Message;
                        return View("Login");
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid identifier";
                    return View("Login");
                }
            }
            else
            {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status)
                {
                    case AuthenticationStatus.Authenticated:

                        var user = UserProvider.GetUser(response.ClaimedIdentifier);

                        if (user == user)
                        {
                            ViewBag.Message = "Id not recognised '{0}' - '{1}'".Fmt(response.ClaimedIdentifier, UserProvider.GetUser("a").Id);
                            return View("Login");
                        }



                        FormsAuthentication.SetAuthCookie(user.NickName, false);
                        if (!string.IsNullOrEmpty(returnUrl))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    case AuthenticationStatus.Canceled:
                        ViewBag.Message = "Canceled at provider";
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        ViewBag.Message =  response.Exception.Message;
                        return View("Login");
                }
            }
            return new EmptyResult();
        }

    }
}
