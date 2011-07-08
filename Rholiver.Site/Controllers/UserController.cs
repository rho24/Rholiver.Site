using System;
using System.Web.Mvc;
using System.Web.Security;
using DotNetOpenAuth.Messaging;
using DotNetOpenAuth.OpenId;
using DotNetOpenAuth.OpenId.RelyingParty;
using Rholiver.Site.Infrastructure;

namespace Rholiver.Site.Controllers
{
    [RequireHttps]
    public class UserController : Controller
    {
        public OpenIdRelyingParty OpenIdService { get; set; }
        IUserProvider UserProvider { get; set; }

        public UserController(OpenIdRelyingParty openIdService, IUserProvider userProvider) {
            OpenIdService = openIdService;
            UserProvider = userProvider;
        }

        public ActionResult Logout() {
            FormsAuthentication.SignOut();
            return Redirect("/");
        }

        public ActionResult Login(string returnUrl) {
            // Stage 1: display login form to user
            ViewBag.ReturnUrl = returnUrl;
            return View("Login");
        }

        [ValidateInput(false)]
        public ActionResult Authenticate(string openidIdentifyer, string returnUrl) {
            var response = OpenIdService.GetResponse();
            if (response == null) {
                // Stage 2: user submitting Identifier
                Identifier id;
                if (Identifier.TryParse(openidIdentifyer, out id)) {
                    try {
                        var authenticateRequest = OpenIdService.CreateRequest(openidIdentifyer);

                        return authenticateRequest.RedirectingResponse.AsActionResult();
                    }
                    catch (ProtocolException ex) {
                        ViewBag.Message = ex.Message;
                        return View("Login");
                    }
                }
                else {
                    ViewBag.Message = "Invalid identifier";
                    return View("Login");
                }
            }
            else {
                // Stage 3: OpenID Provider sending assertion response
                switch (response.Status) {
                    case AuthenticationStatus.Authenticated:

                        var user = UserProvider.GetUser(response.ClaimedIdentifier);

                        if (user == null) {
                            ViewBag.Message = "Id not recognised";
                            return View("Login");
                        }

                        FormsAuthentication.SetAuthCookie(user.NickName, false);
                        if (!string.IsNullOrEmpty(returnUrl)) {
                            return Redirect(returnUrl);
                        }
                        else {
                            return RedirectToAction("Index", "Home");
                        }
                    case AuthenticationStatus.Canceled:
                        ViewBag.Message = "Canceled at provider";
                        return View("Login");
                    case AuthenticationStatus.Failed:
                        ViewBag.Message = response.Exception.Message;
                        return View("Login");
                }
            }
            return new EmptyResult();
        }
    }
}