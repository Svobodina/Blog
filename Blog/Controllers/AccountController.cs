namespace Blog.Controllers
{
    using Entities;
    using Models;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly BlogDBEntities _context = new BlogDBEntities();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && IsValidUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, true);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Неправильное имя пользователя или пароль.");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private bool IsValidUser(string login, string password)
        {
            return _context.Users.Any(u => u.Login == login && u.Password == password);
        }
    }
}
