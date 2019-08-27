using ImageGallary.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DynamicImageGallary.Controllers
{
    public class AuthenticationController : Controller
    {
        #region Signin method
        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignIn(UserDetail user)
        {
            using (DBContext db = new DBContext())
            {
                var email = user.Email;
                var passsword = user.Password;

                if (db.UserDetails.Any(x => x.Email.Equals(user.Email, StringComparison.Ordinal) && x.Password.Equals(user.Password, StringComparison.Ordinal)))
                {
                    UserDetail userDetail = db.UserDetails.Include(e => e.Usertype).Single(x => x.Email == user.Email);

                    Session["UserEmail"] = userDetail.Email;
                    Session["UserRole"] = userDetail.UsertypeId;
                    Session["UserId"] = userDetail.Id;
                    FormsAuthentication.SetAuthCookie(userDetail.Email, false);
                    FormsAuthentication.SetAuthCookie(Convert.ToString(userDetail.Usertype), false);

                    if (userDetail.UsertypeId == 1)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else if (userDetail.UsertypeId == 2)
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            ModelState.AddModelError("", "Invalid email and password");
            return View();
        }
        #endregion

        #region Signup method
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                userDetail.UsertypeId = 2;
                userDetail.RegistrationDate = DateTime.Now;
                using (DBContext db = new DBContext())
                {
                    db.UserDetails.Add(userDetail);
                    db.SaveChanges();
                    return RedirectToAction("SignIn");
                }
            }
            return View();
        }
        #endregion

        #region "Sign out"
        /// <summary>Represents an event that is raised when the sign-out operation is complete.</summary>
        /// <returns></returns>
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Session.Abandon();
            HttpContext.Session.Clear();
            HttpContext.Session.RemoveAll();
            return RedirectToAction("SignIn");
        }
        #endregion

        #region "Error"
        /// <summary>Errors this instance.</summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        #endregion
    }
}