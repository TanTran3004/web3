using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDungcuhoctap.Models;

namespace MVCDungcuhoctap.Controllers
{
    public class GiohangController : Controller
    {
        dbQLDCHTDataContext data = new dbQLDCHTDataContext();
        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang==null)
            {
                lstGiohang = new List<Giohang>();
                Session["Giohang"] = lstGiohang;
            }
                return lstGiohang;
        }

        public ActionResult ThemGiohang(int iMaDC, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang dungcu = lstGiohang.Find(n=>n.iMaDungCu==iMaDC);
            if(dungcu==null)
            {
                dungcu = new Giohang(iMaDC);
                lstGiohang.Add(dungcu);
                return Redirect(strURL);
            }else
            {
                dungcu.iSoluong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Giohang> lstGiohang = Session["Giohang"] as List<Giohang>;
            if(lstGiohang!=null)
            {
                iTongSoLuong = lstGiohang.Sum(n=>n.iSoluong);
            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<Giohang> lstGiohang = Session["GioHang"] as List<Giohang>;
            if(lstGiohang!=null)
            {
                iTongTien = lstGiohang.Sum(n => n.dThanhtien);
            }
            return iTongTien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            if(lstGiohang.Count ==0)
            {
                return RedirectToAction("Index","DUNGCU");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int iMaDC)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang dungcu = lstGiohang.SingleOrDefault(n=>n.iMaDungCu==iMaDC);
            if(dungcu!=null)
            {
                lstGiohang.RemoveAll(n => n.iMaDungCu == iMaDC);
                return RedirectToAction("GioHang");
            }
            if(lstGiohang.Count ==0)
            {
                return RedirectToAction("Index","DUNGCU");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapnhatGiohang(int iMaDC, FormCollection f)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang dungcu = lstGiohang.SingleOrDefault(n =>n.iMaDungCu == iMaDC);
            if (dungcu != null)
            {
                dungcu.iSoluong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            return RedirectToAction("Index", "DUNGCU");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Dangnhap", "USER");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "HomePage");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();

            return View(lstGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = Laygiohang();
            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["Ngaygiao"]);
            ddh.Ngaygiao = DateTime.Parse(ngaygiao);
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.Madungcu = item.iMaDungCu;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhandonhang");
        }
        public ActionResult Xacnhandonhang()
        {
            return View();
        }
    }
}