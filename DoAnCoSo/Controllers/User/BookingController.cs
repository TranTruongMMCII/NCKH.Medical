using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnCoSo.Models;
using DoAnCoSo.Generation;
namespace DoAnCoSo.Controllers
{
    public class BookingController : Controller
    {
        // GET: Book

        LinQDataContext data = new LinQDataContext();
        
        public ActionResult SelectSpecialist()
        {
            var specialList = from dt in data.KHOAs
                              select dt;
            return View(specialList);
        }

        public ActionResult SearchDoctor(FormCollection collection)
        {
            var specialistId = collection.Get("specialist");
            var searchResult = from dt in data.BACSIs
                               where dt.MAKHOA.ToString() == specialistId
                               select dt;
            return View(searchResult);
        }

        public ActionResult LoadTimeOfDoctor(int id)
        {           
            var timeOfDoctor = from dt in data.GIOKHAMs
                               where dt.MABS == id
                               select dt;
            return PartialView(timeOfDoctor);
        }

        public ActionResult SelectDoctor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult FillForm(int id)
        {
            var doctorDetail = from dt in data.BACSIs
                               where dt.MABS == id
                               select dt;
            return View(doctorDetail);
        }

        [HttpPost]
        public ActionResult SendBookingForm(FormCollection collection,PHIEUHEN ph)
        {
            try
            {
                var name = collection.Get("name").Trim().ToUpper();
                var gender = collection.Get("gender");
                var dob = string.Format("{0:MM/dd/yyyy}", collection.Get("dob"));
                var address = collection.Get("address").ToUpper();
                var phone = collection.Get("phone").Trim();
                var detail = collection.Get("detail");
                var bookingDate = string.Format("{0:MM/dd/yyyy}", collection.Get("selectdate"));
                var bookingTime = collection.Get("time");
                var doctorID = int.Parse(collection.Get("doctorID"));

                ph.HOTEN = name;
                ph.GIOITINH = gender;
                ph.NAMSINH = DateTime.Parse(dob);
                ph.DIACHI = address;
                ph.SDT = phone;
                ph.GIOKHAM = TimeSpan.Parse(bookingTime);
                ph.NGAYKHAM = DateTime.Parse(bookingDate);
                ph.MOTA = detail;
                ph.MABS =doctorID;
                ph.TRANGTHAI = "Chưa xác thực";
                ph.MAXACTHUC = GenerateRandomString.GetRandomString();

                data.PHIEUHENs.InsertOnSubmit(ph);
                data.SubmitChanges();

                Session["SDT"] = phone;
                Session["Message"] = "Dat lich kham thanh cong" + " " + name +" " + bookingDate + " " + bookingTime;
                return RedirectToAction("Send", "SMS");
                //return Redirect('https://www.baokim.vn/payment/product/version11?business=vunhatanhtuan13%40gmail.com&id=&order_description=%C4%90%E1%BA%B7t+l%E1%BB%8Bch+kh%C3%A1m+b%E1%BB%87nh+&product_name=Kh%C3%A1m+B%E1%BB%87nh&product_price=150000&product_quantity=1&total_amount=150000&url_cancel=http%3A%2F%2Fbenhvien.somee.com%2F&url_detail=http%3A%2F%2Fbenhvien.somee.com%2FBooking%2FFillForm%2F1&url_success=http%3A%2F%2Fbenhvien.somee.com%2FBooking%2FBookingSuccess');
                //return Content("<html><head></head><body><a href ='https://www.google.com/chrome'>Click this URL</a></body></html>");
                //return RedirectToAction("BookingSuccess", "Booking");
            }
            catch (Exception)
            {
                return HttpNotFound();
            }
        }
        //[HttpPost]
        //public ActionResult SendResultForm( int id)
        //{
        //    try
        //    {
        //        KETQUA kq = data.KETQUAs.SingleOrDefault(n => n.MAPHIEU == id);
        //        PHIEUHEN ph = data.PHIEUHENs.SingleOrDefault(m => m.MAPHIEU == id);
        //        var phone = ph.SDT;
        //        var ketqua = kq.KETQUA1;
        //        var chuandoan = kq.CHUANDOAN;
        //        var nen = kq.NEN;
        //        var khongnen = kq.KHONGNEN;
        //        var ngaytaikham = kq.NGAYTAIKHAM;
        //        Session["SDT"] = phone;
        //        Session["Message"] = "Bệnh viện gửi bạn kết quả khám bệnh" + " " + ketqua + " " + chuandoan + " " + nen + " " + khongnen + " " + ngaytaikham;
        //        return RedirectToAction("Send", "SMS");
        //    }
        //    catch (Exception)
        //    {
        //        return HttpNotFound();
        //    }
        //  }

        public ActionResult BookingSuccess()
        {
            return View();
        }
        public ActionResult ThanhToan()
        {
            return View();
        }
    }
}