using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;

namespace DoAnCoSo.Controllers.Login
{
    public class LoginController : Controller
    {
        // GET: Login

        LinQDataContext data = new LinQDataContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection collection)
        {
            var userID = collection.Get("userID");
            var userPASS = collection.Get("userPASS");

            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(a => a.ID == userID && a.PASSWORD == userPASS);

            if (tk == null)
                ViewBag.Noti = "Sai tài khoản hoặc mật khẩu";
            else
            {
                Session["sessionAdmin"] = tk;
                Session["userName"] = tk.HOTEN;
                return RedirectToAction("Index", "Admin");
            }

            return View();
        }
    }
}