using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Generation;
using DoAnCoSo.Models;
using PagedList;
using PagedList.Mvc;


namespace DoAnCoSo.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin

        LinQDataContext data = new LinQDataContext();

        public bool IsLogined()
        {
            if (Session["sessionAdmin"] == null)
                return false;
            else
                return true;
        }

        public ActionResult Index()
        {
            if (IsLogined() == true)
                return View();
            else
                return RedirectToAction("Index", "Login");
        }

        public ActionResult DepartmentManager(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var khoa = from kh in data.KHOAs
                       select kh;
            return View(khoa.ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult EditDepartment(int id)
        {
            KHOA kh = data.KHOAs.SingleOrDefault(n => n.MAKHOA == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        public ActionResult EditDepartment(KHOA kh)
        {
            KHOA tmp = data.KHOAs.SingleOrDefault(n => n.MAKHOA == kh.MAKHOA);
            data.KHOAs.DeleteOnSubmit(tmp);
            data.KHOAs.InsertOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("DepartmentManager", "admin");
        }

        public ActionResult DeleteDepartment(int id, string btnDelete)
        {
            KHOA kh = data.KHOAs.FirstOrDefault(n => n.MAKHOA == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            if(btnDelete == "Xóa")
            {
                var bacsi = (from bs in data.BACSIs where bs.MAKHOA == id select bs.MABS).ToList();

                foreach (int i in bacsi)
                {
                    var phieu = (from ph in data.PHIEUHENs where ph.MABS == i select ph.MAPHIEU).ToList();
                    foreach (int j in phieu)
                    {
                        PHIEUHEN pHIEUHEN = data.PHIEUHENs.SingleOrDefault(n => n.MAPHIEU == j);
                        KETQUA kETQUA = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == j);
                        data.PHIEUHENs.DeleteOnSubmit(pHIEUHEN);
                        data.KETQUAs.DeleteOnSubmit(kETQUA);
                    }

                    var giokham = (from gk in data.GIOKHAMs where gk.MABS == i select gk).ToList();
                    foreach (var j in giokham)
                    {
                        data.GIOKHAMs.DeleteOnSubmit(j);
                    }

                    BACSI bACSI = data.BACSIs.SingleOrDefault(k => k.MABS == i);
                    data.BACSIs.DeleteOnSubmit(bACSI);
                }
                data.KHOAs.DeleteOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("DepartmentManager");
            }

            return View(kh);
        }
        [HttpGet]
        public ActionResult CreateDepartment()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateDepartment(KHOA khoa)
        {
            data.KHOAs.InsertOnSubmit(khoa);
            data.SubmitChanges();
            return RedirectToAction("DepartmentManager", "admin");
        }

        // Doctor //

        public ActionResult DoctorManager(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var bacsi = from bs in data.BACSIs
                        select bs;
            return View(bacsi.ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult EditDoctor(int id)
        {
            BACSI bs = data.BACSIs.SingleOrDefault(n => n.MABS == id);
            if (bs == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bs);
        }

        //public ActionResult EditDoctor(BACSI bs)
        //{
        //    BACSI tmp = data.BACSIs.SingleOrDefault(n => n.MABS == bs.MABS);

        //    data.BACSIs.InsertOnSubmit(bs);
        //    data.SubmitChanges();
        //    return RedirectToAction("DoctorManager", "admin");
        //}

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditDoctor(BACSI bs, HttpPostedFileBase fileUpload)
        {
            BACSI tmp = data.BACSIs.SingleOrDefault(n => n.MABS == bs.MABS);
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh đại diện";
                //UpdateModel(bs);
                //data.SubmitChanges();
                bs.HINHANH = tmp.HINHANH;
                data.BACSIs.DeleteOnSubmit(tmp);
                data.BACSIs.InsertOnSubmit(bs);
                data.SubmitChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    bs.HINHANH = fileName;
                    data.BACSIs.DeleteOnSubmit(tmp);
                    data.BACSIs.InsertOnSubmit(bs);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("DoctorManager");
        }
        public ActionResult DeleteDoctor(int id, string btnDelete)
        {
            BACSI bs = data.BACSIs.SingleOrDefault(n => n.MABS == id);
            if (bs == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //KHOA kh = data.KHOAs.FirstOrDefault(n => n.MAKHOA == id);
            //while (kh != null)
            //{
            //    data.KHOAs.DeleteOnSubmit(kh);
            //    data.SubmitChanges();
            //    kh = data.KHOAs.FirstOrDefault(n => n.MAKHOA == id);
            //}
            if(btnDelete == "Xóa")
            {
                KETQUA kq = data.KETQUAs.FirstOrDefault(n => n.MAPHIEU == id);
                while (kq != null)
                {
                    data.KETQUAs.DeleteOnSubmit(kq);
                    data.SubmitChanges();
                    kq = data.KETQUAs.FirstOrDefault(n => n.MAPHIEU == id);
                }
                PHIEUHEN ph = data.PHIEUHENs.FirstOrDefault(n => n.MABS == id);
                while (ph != null)
                {
                    data.PHIEUHENs.DeleteOnSubmit(ph);
                    data.SubmitChanges();
                    ph = data.PHIEUHENs.FirstOrDefault(n => n.MABS == id);
                }
                GIOKHAM gk = data.GIOKHAMs.FirstOrDefault(n => n.MABS == id);
                while (gk != null)
                {
                    data.GIOKHAMs.DeleteOnSubmit(gk);
                    data.SubmitChanges();
                    gk = data.GIOKHAMs.FirstOrDefault(n => n.MABS == id);
                }
                data.BACSIs.DeleteOnSubmit(bs);
                data.SubmitChanges();
                return RedirectToAction("DoctorManager");
            }

            return View(bs);
        }
        [HttpGet]
        public ActionResult CreateDoctor()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult CreateDoctor(BACSI bacsi)
        //{
        //    data.BACSIs.InsertOnSubmit(bacsi);
        //    data.SubmitChanges();
        //    return RedirectToAction("DoctorManager", "admin");
        //}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateDoctor(BACSI bs, HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh đại diện";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    bs.HINHANH = fileName;
                    data.BACSIs.InsertOnSubmit(bs);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("DoctorManager");
        }
        [HttpGet]
        public ActionResult CreateAdmin()
        {
            return View();
        }
        public ActionResult CreateAdmin(TAIKHOAN tk)
        {
            data.TAIKHOANs.InsertOnSubmit(tk);
            data.SubmitChanges();
            return RedirectToAction("AdminManager", "Admin");
        }
        public ActionResult DeleteAdmin(string id, string btnDelete)
        {
            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.ID == id);
            if (tk == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            if(btnDelete == "Xóa")
            {
                data.TAIKHOANs.DeleteOnSubmit(tk);
                data.SubmitChanges();
                return RedirectToAction("AdminManager");
            }

            return View(tk);
        }
        [HttpGet]
        public ActionResult EditAdmin(string id)
        {
            TAIKHOAN tk = data.TAIKHOANs.SingleOrDefault(n => n.ID == id);
            return View(tk);
        }
        public ActionResult EditAdmin(TAIKHOAN tk)
        {
            TAIKHOAN tmp = data.TAIKHOANs.SingleOrDefault(n => n.ID == tk.ID);
            data.TAIKHOANs.DeleteOnSubmit(tmp);
            data.TAIKHOANs.InsertOnSubmit(tk);
            data.SubmitChanges();
            return RedirectToAction("AdminManager");
        }
        public ActionResult AdminManager(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var taikhoan = from tk in data.TAIKHOANs
                           select tk;
            return View(taikhoan.ToPagedList(pageNum, pageSize));
        }
        [HttpGet]
        public ActionResult CreateResult()
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult CreateResult(KETQUA kq)
        //{
        //    data.KETQUAs.InsertOnSubmit(kq);
        //    data.SubmitChanges();
        //    return RedirectToAction("ResultManager", "Admin");
        //}
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateResult(KETQUA kq, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2)
        {
            if (fileUpload1 == null && fileUpload2 == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh kết quả bệnh nhân";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName1 = Path.GetFileName(fileUpload1.FileName);
                    var fileName2 = Path.GetFileName(fileUpload2.FileName);
                    var path1 = Path.Combine(Server.MapPath("~/Content/Images"), fileName1);
                    var path2 = Path.Combine(Server.MapPath("~/Content/Images"), fileName2);
                    if (System.IO.File.Exists(path1) || System.IO.File.Exists(path2))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload1.SaveAs(path1);
                        fileUpload2.SaveAs(path2);
                    }
                    kq.HINHANH1 = fileName1;
                    kq.HINHANH2 = fileName2;
                    data.KETQUAs.InsertOnSubmit(kq);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("ResultManager");
        }
        public ActionResult DeleteResult(int id, string btnDelete)
        {
            KETQUA kq = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == id);
            if (kq == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            if(btnDelete == "Xóa")
            {
                data.KETQUAs.DeleteOnSubmit(kq);
                data.SubmitChanges();
                return RedirectToAction("ResultManager");
            }

            return View(kq);
        }
        [HttpGet]
        public ActionResult EditResult(int id)
        {
            KETQUA kq = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == id);
            return View(kq);
        }
        //public ActionResult EditResult(KETQUA kq)
        //{
        //    KETQUA tmp = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == kq.MAPHIEU);
        //    data.KETQUAs.DeleteOnSubmit(tmp);
        //    data.KETQUAs.InsertOnSubmit(kq);
        //    data.SubmitChanges();
        //    return RedirectToAction("ResultManager");
        //}
        public ActionResult ResultManager(int? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);
            var ketqua = from tk in data.KETQUAs
                         select tk;
            return View(ketqua.ToPagedList(pageNum, pageSize));
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditResult(KETQUA kq, HttpPostedFileBase fileUpload1, HttpPostedFileBase fileUpload2)
        {
            KETQUA tmp = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == kq.MAPHIEU);
            if (fileUpload1 == null && fileUpload2 == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh kết quả bệnh nhân";
                return View();
                //kq.HINHANH1 = tmp.HINHANH1;
                //kq.HINHANH2 = tmp.HINHANH2;
                //data.KETQUAs.DeleteOnSubmit(tmp);
                //data.KETQUAs.InsertOnSubmit(kq);
                //data.SubmitChanges();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName1 = Path.GetFileName(fileUpload1.FileName);
                    var fileName2 = Path.GetFileName(fileUpload2.FileName);
                    var path1 = Path.Combine(Server.MapPath("~/Content/Images"), fileName1);
                    var path2 = Path.Combine(Server.MapPath("~/Content/Images"), fileName2);
                    if (System.IO.File.Exists(path1) || System.IO.File.Exists(path2))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUpload1.SaveAs(path1);
                        fileUpload2.SaveAs(path2);
                    }
                    kq.HINHANH1 = fileName1;
                    kq.HINHANH2 = fileName2;
                    data.KETQUAs.DeleteOnSubmit(tmp);
                    data.KETQUAs.InsertOnSubmit(kq);
                    data.SubmitChanges();
                }
            }
            return RedirectToAction("ResultManager");
        }


        [HttpGet]
        public ActionResult EditBooking(int id)
        {
            PHIEUHEN ph = data.PHIEUHENs.SingleOrDefault(n => n.MAPHIEU == id);
            return View(ph);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditBooking(PHIEUHEN ph)
        {
            PHIEUHEN tmp = data.PHIEUHENs.SingleOrDefault(n => n.MAPHIEU == ph.MAPHIEU);
            data.PHIEUHENs.DeleteOnSubmit(tmp);
            data.PHIEUHENs.InsertOnSubmit(ph);
            data.SubmitChanges();
            return RedirectToAction("BookingManager");
        }
        public ActionResult BookingManager(int? page)
        {
            int pageSize = 7;
            int pageNum = (page ?? 1);
            var phieuhen = from ph in data.PHIEUHENs
                           select ph;

            return View(phieuhen.ToPagedList(pageNum, pageSize));
        }

        public ActionResult DeleteBooking(int id, string btnDelete)
        {
            PHIEUHEN ph = data.PHIEUHENs.SingleOrDefault(n => n.MAPHIEU == id);
            if (ph == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            if (btnDelete == "Xóa")
            {
                KETQUA kq = data.KETQUAs.FirstOrDefault(n => n.MAPHIEU == id);
                while (kq != null)
                {
                    data.KETQUAs.DeleteOnSubmit(kq);
                    data.SubmitChanges();
                    kq = data.KETQUAs.FirstOrDefault(n => n.MAPHIEU == id);
                }
                data.PHIEUHENs.DeleteOnSubmit(ph);
                data.SubmitChanges();
                return RedirectToAction("BookingManager");
            }
            return View(ph);
        }

        [HttpGet]
        public ActionResult CreateBooking()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateBooking(PHIEUHEN phieuhen)
        {
            phieuhen.MAXACTHUC = GenerateRandomString.GetRandomString();
            data.PHIEUHENs.InsertOnSubmit(phieuhen);
            data.SubmitChanges();
            return RedirectToAction("BookingManager");
        }

        // Lịch Khám BS
        public ActionResult TimeManager(int? page)
        {
            int pageSize = 10;
            int pageNum = (page ?? 1);
            var giokham = from gk in data.GIOKHAMs
                          select gk;
            return View(giokham.ToPagedList(pageNum, pageSize));
        }

        [HttpGet]
        public ActionResult EditTime(int id)
        {
            GIOKHAM gk = data.GIOKHAMs.SingleOrDefault(n => n.MABS == id);
            return View(gk);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditTime(GIOKHAM gk)
        {
            GIOKHAM tmp = data.GIOKHAMs.SingleOrDefault(n => n.MABS == gk.MABS);
            data.GIOKHAMs.DeleteOnSubmit(tmp);
            data.GIOKHAMs.InsertOnSubmit(gk);
            data.SubmitChanges();
            return RedirectToAction("TimeManager");
        }
        [HttpGet]
        public ActionResult DeleteTime(int id, string btnDelete)
        {
            GIOKHAM gk = data.GIOKHAMs.SingleOrDefault(n => n.MABS == id);
            if (gk == null)
            {
                Response.StatusCode = 404;
                return null;
            }
           
            if(btnDelete == "Xóa")
            {
                data.GIOKHAMs.DeleteOnSubmit(gk);
                data.SubmitChanges();
                return RedirectToAction("TimeManager");
            }
            return View(gk);
        }

        [HttpGet]
        public ActionResult CreateTime()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTime(GIOKHAM giokham)
        {
            data.GIOKHAMs.InsertOnSubmit(giokham);
            data.SubmitChanges();
            return RedirectToAction("TimeManager");
        }

        public ActionResult AcceptBooking(int id)
        {
            var query = data.PHIEUHENs.SingleOrDefault(a => a.MAPHIEU == id);
            query.TRANGTHAI = "Đã xác thực";
            data.SubmitChanges();
            return RedirectToAction("BookingManager", "Admin");
        }
        [HttpGet]
        public ActionResult SendResultForm(int id)
        {
            KETQUA kq = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == id);
            PHIEUHEN ph = data.PHIEUHENs.SingleOrDefault(m => m.MAPHIEU == id);
            var phone = ph.SDT;
            var ketqua = kq.KETQUA1;
            var chuandoan = kq.CHUANDOAN;
            var nen = kq.NEN;
            var khongnen = kq.KHONGNEN;
            var ngaytaikham = kq.NGAYTAIKHAM;
            Session["SDT2"] = phone;
            Session["Message2"] = "Bệnh viện gửi bạn kết quả khám bệnh. " + "Kết quả: " + ketqua + " Chuẩn đoán: " + chuandoan + " Nên: " + nen + " Không nên: " + khongnen + " Ngày tái khám: " + ngaytaikham;
            return RedirectToAction("Send2", "SMS");
        }
        //public ActionResult ggggg()
        //{
        //    DateTime today2 = DateTime.Now;
        //    string Str1 = today2.ToString("yyyy-MM-dd");
        //    PHIEUHEN maphieu = data.PHIEUHENs.SingleOrDefault(n => n.NGAYKHAM.ToString() == Str1);


        //    return View();
        //}
    }
}