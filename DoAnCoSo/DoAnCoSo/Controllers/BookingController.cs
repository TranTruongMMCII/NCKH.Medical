using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
namespace DoAnCoSo.Controllers
{
    public class BookingController : Controller
    {
        // GET: Book

        LinQDataContext data = new LinQDataContext();
        
        public ActionResult SelectSpecialist()
        {
            var list = from khoa in data.KHOAs
                       select khoa;
            return View(list);
        }

        public ActionResult SelectDoctor(int id)
        {
            var list = from bacSi in data.BACSIs
                       where bacSi.MAKHOA == id
                       select bacSi;
                       
            return View(list);
        }

        public ActionResult FillForm()
        {
            return View();
        }
    }
}