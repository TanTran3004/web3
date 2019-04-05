using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDungcuhoctap.Models;

namespace MVCDungcuhoctap.Controllers
{
    public class USERController : Controller
    {
        dbQLDCHTDataContext data = new dbQLDCHTDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["PassDN"];
            var matkhaunl = collection["PassNL"];
            var email = collection["EmailKH"];
            var diachi = collection["DiachiKH"];
            var dienthoai = collection["DienthoaiKH"];
            var ngaysinh = String.Format("{0:MM/dd/YYYY}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Vui lòng nhập họ tên";
            }
            else if (String.IsNullOrEmpty(tendn))
                {
                ViewData["Loi2"] = "Vui lòng nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
                {
                ViewData["Loi3"] = "Vui lòng nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunl))
                {
                ViewData["Loi4"] = "Vui lòng nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(email))
                {
                ViewData["Loi5"] = "Vui lòng nhập email";
            }
            if (String.IsNullOrEmpty(diachi))
                {
                ViewData["Loi6"] = "Vui lòng nhập địa chỉ";
            }
            if (String.IsNullOrEmpty(dienthoai))
                {
                ViewData["Loi7"] = "Vui lòng nhập điện thoại";
            }
            else
            {
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();              
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["PassDN"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Vui lòng nhập tài khoản";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {                    
                    ViewBag.ThongBao = "Đăng nhập thành công, mời bạn tiếp tục";
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Giohang", "Giohang");
                    
                }
                else
                    ViewBag.ThongBao = "Tài khoản hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}
