using MVCDungcuhoctap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity;
namespace MVCDungcuhoctap.Controllers
{
    public class AdminController : Controller
    {


        dbQLDCHTDataContext db = new dbQLDCHTDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DUNGCU(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.SACHes.ToList());
            return View(db.DUNGCUs.ToList().OrderBy(n => n.Madungcu).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";

            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        } 
        [HttpGet]
        public ActionResult ThemmoiDungCu()
        {
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenCD), "MaCD", "TenCD");
            ViewBag.MaHSX = new SelectList(db.HANGSANXUATs.ToList().OrderBy(n => n.TenHSX), "MaHSX", "TenHSX");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemmoiDungCu(DUNGCU dc, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenCD), "MaCD", "TenCD");
            ViewBag.MaHSX = new SelectList(db.HANGSANXUATs.ToList().OrderBy(n => n.TenHSX), "MaHSX", "TenHSX");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/ImageSRC"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    dc.Anhbia = fileName;
                    //Luu vao CSDL
                    db.DUNGCUs.InsertOnSubmit(dc);
                    db.SubmitChanges();
                }
                return RedirectToAction("DungCu");
            }
        }
        //Hiển thị sản phẩm
        public ActionResult Chitietdungcu(int id)
        {
            //Lay ra doi tuong sach theo ma
            DUNGCU dc = db.DUNGCUs.SingleOrDefault(n => n.Madungcu == id);
            ViewBag.Madungcu = dc.Madungcu;
            if (dc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dc);
        }

        //Xóa sản phẩm
        [HttpGet]
        public ActionResult Xoadungcu(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            DUNGCU dc = db.DUNGCUs.SingleOrDefault(n => n.Madungcu == id);
            ViewBag.Madungcu = dc.Madungcu;
            if (dc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dc);
        }

        [HttpPost, ActionName("Xoadungcu")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            DUNGCU dc = db.DUNGCUs.SingleOrDefault(n => n.Madungcu == id);
            ViewBag.Madungcu = dc.Madungcu;
            if (dc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.DUNGCUs.DeleteOnSubmit(dc);
            db.SubmitChanges();
            return RedirectToAction("DungCu");
        }
        //Chinh sửa sản phẩm
        [HttpGet]
        public ActionResult Suadungcu(int id)
        {
            //Lay ra doi tuong sach theo ma
            DUNGCU dc = db.DUNGCUs.SingleOrDefault(n => n.Madungcu == id);
            ViewBag.Madungcu = dc.Madungcu;
            if (dc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao dropdownList
            //Lay ds tu tabke chu de, sắp xep tang dan trheo ten chu de, chon lay gia tri Ma CD, hien thi thi Tenchude
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenCD), "MaCD", "TenCD", dc.MaCD);
            ViewBag.MaHSX = new SelectList(db.HANGSANXUATs.ToList().OrderBy(n => n.TenHSX), "MaHSX", "TenHSX", dc.MaHSX);
            return View(dc);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suadungcu(DUNGCU dc, HttpPostedFileBase fileUpload)
        {
            //Dua du lieu vao dropdownload
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenCD), "MaCD", "TenCD");
            ViewBag.MaHSX = new SelectList(db.HANGSANXUATs.ToList().OrderBy(n => n.TenHSX), "MaHSX", "TenHSX");
            //Kiem tra duong dan file
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Them vao CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten fie, luu y bo sung thu vien using System.IO;
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/ImageSRC"), fileName);
                    //Kiem tra hình anh ton tai chua?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Luu hinh anh vao duong dan
                        fileUpload.SaveAs(path);
                    }
                    dc.Anhbia = fileName;
                    //Luu vao CSDL   
                    UpdateModel(dc);
                    db.SubmitChanges();

                }
                return RedirectToAction("dc");
            }
        }

    }

}
