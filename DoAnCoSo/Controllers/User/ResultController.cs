using DoAnCoSo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnCoSo.Controllers
{
    public class ResultController : Controller
    {
        // GET: Result

        LinQDataContext data = new LinQDataContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendRequest(FormCollection collection)
        {
            var maXacThuc = collection.Get("maXacThuc").ToUpper();
            var sdt = collection.Get("sdt");

            var result = data.PHIEUHENs.Select(a => a).Where(a => a.SDT == sdt && a.MAXACTHUC == maXacThuc);

            return View(result);
        }

        public ActionResult UserInfo(int id)
        {
            var result = from dt in data.PHIEUHENs
                         where dt.MAPHIEU == id
                         select dt;
            return PartialView(result);
        }

        public ActionResult Detail(int id)
        {
            var result = from dt in data.KETQUAs
                         where dt.MAPHIEU == id
                         select dt;
            return PartialView(result);
        }

        public ActionResult LoadImage(int id)
        {
            var result = from dt in data.KETQUAs
                         where dt.MAPHIEU == id
                         select dt;
            return PartialView(result);
        }
    }
}