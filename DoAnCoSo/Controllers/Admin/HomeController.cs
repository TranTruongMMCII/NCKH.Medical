//using DoAnCoSo.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;

//namespace DoAnCoSo.Controllers.Admin
//{
//    public class HomeController : Controller
//    {
//        // GET: Home
//        public ActionResult Index()
//        {
//            return View();
//        }
//        [HttpGet]
//        public ActionResult Register()
//        {
//            return View();
//        }
//        public ActionResult Register(FormCollection collect, TAIKHOAN tk)
//        {
//            var hoten = collect["username"];
//            var password = collect["password"];
//            var nhaplaimatkhau = collect["repassword"];
//            if (String.IsNullOrEmpty(hoten))
//            {
//                ViewData["Loi1"] = "Tên đăng nhập không thể bỏ trống";
//            }
//            else if (String.IsNullOrEmpty(password))
//            {
//                ViewData["Loi3"] = "Mật khẩu không thể bỏ trống";
//            }
//            else if (String.IsNullOrEmpty(nhaplaimatkhau))
//            {
//                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
//            }
//            else
//            {
//                tk.HOTEN = hoten;
//                tk.PASSWORD = password;
//                data.TAIKHOANs.InsertOnSubmit(tk);
//                data.SubmitChanges();
//                return RedirectToAction("login");
//            }
//            return View();
//        }
//        [HttpGet]
//        public ActionResult Login()
//        {
//            return View();
//        }
//        // Phan dang nhap
//        public ActionResult Login(FormCollection collect)
//        {
//            var hoten = collect["username"];
//            var matkhau = collect["password"];
//            if (String.IsNullOrEmpty(hoten))
//            {
//                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
//            }
//            else if (String.IsNullOrEmpty(matkhau))
//            {
//                ViewData["Loi2"] = "Phải nhập mật khẩu";
//            }
//            else
//            {
//                TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.HOTEN == hoten && n.MATKHAU == matkhau);
//                if (tk != null)
//                {
//                    Session["Tenthanhvien"] = tk.HOTEN;
//                    Session["ID"] = tk.ID;
//                    return RedirectToAction("Index", "Home");
//                }
//                else
//                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
//            }
//            return View();
//        }
//        public ActionResult Complete()
//        {
//            return View();
//        }
//        [HttpGet]
//    }
//}