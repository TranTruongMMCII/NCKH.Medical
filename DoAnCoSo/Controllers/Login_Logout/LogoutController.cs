using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers.Login_Logout
{
    public class LogoutController : Controller
    {
        // GET: Logout
        public ActionResult Index()
        {
            Session["sessionAdmin"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}