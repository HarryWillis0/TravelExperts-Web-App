using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using TravelExperts_Web_App.Models;

namespace TravelExperts_Web_App.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                UserName = User.Identity.GetUserName()
                // TO DO GET MODEL VALUES

            };
            return View(model);
        }

        //
        // GET: /Manage/ChangeUserName
        /// <summary>
        /// Serve change user name page
        /// </summary>
        /// @author - Harry
        public ActionResult ChangeUserName()
        {
            return View();
        }

        /// <summary>
        /// Serve change user name post
        /// </summary>
        /// <param name="model">form data</param>
        /// @author - Harry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeUserName(ChangeUserNameViewModel model)
        {
            // TO DO - IMPLEMENT
            throw new NotImplementedException();
        }

        //
        // GET: /Manage/ChangeHomePhoneNumber
        /// <summary>
        /// Serve change home phone number page
        /// </summary>
        /// @author - Harry
        public ActionResult ChangeHomePhone()
        {
            return View();
        }

        /// <summary>
        /// Serve change Home phone number post
        /// </summary>
        /// <param name="model">form data</param>
        /// @author - Harry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeHomePhone(ChangeHomePhoneViewModel model)
        {
            // TO DO - IMPLEMENT
            throw new NotImplementedException();
        }

        //
        // GET: /Manage/ChangeBusPhone
        /// <summary>
        /// Serve change business phone number page
        /// </summary>
        /// @author - Harry
        public ActionResult ChangeBusPhone()
        {
            return View();
        }

        /// <summary>
        /// Serve change business phone number post
        /// </summary>
        /// <param name="model">form data</param>
        /// @author - Harry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeBusPhone(ChangeBusPhoneViewModel model)
        {
            // TO DO - IMPLEMENT
            throw new NotImplementedException();
        }

        //
        // GET: /Manage/ChangeAddress
        /// <summary>
        /// Serve change address page
        /// </summary>
        /// @author - Harry
        public ActionResult ChangeAddress()
        {
            return View();
        }

        /// <summary>
        /// Serve change address post
        /// </summary>
        /// <param name="model">form data</param>
        /// @author - Harry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAddress(ChangeAddressViewModel model)
        {
            // TO DO - IMPLEMENT
            throw new NotImplementedException();
        }

        //
        // GET: /Manage/ChangeEmail
        /// <summary>
        /// Serve change email page
        /// </summary>
        /// @author - Harry
        public ActionResult ChangeEmail()
        {
            return View();
        }

        /// <summary>
        /// Serve change email post
        /// </summary>
        /// <param name="model">form data</param>
        /// @author - Harry
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeEmail(ChangeEmailViewModel model)
        {
            // TO DO - IMPLEMENT
            throw new NotImplementedException();
        }


        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}