using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCDungcuhoctap.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCDungcuhoctap.Controllers
{
    public class DUNGCUController : Controller
    {
        // GET: DUNGCU
        dbQLDCHTDataContext data = new dbQLDCHTDataContext();
        private List<DUNGCU> Laydungcumoi(int count)
        {
            return data.DUNGCUs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 5;
            int pageNum = (page ?? 1);

            var dungcumoi = Laydungcumoi(15);
            return View(dungcumoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult Chude()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult SPTheochude(int id, int ? page)
        {
            int pageSize = 9;
            int pagenum = (page ?? 1);
            var dungcu = from dc in data.DUNGCUs where dc.MaCD == id select dc;
            return View(dungcu.ToPagedList(pagenum,pageSize));
        }
        public ActionResult Details(int id)
        {
            var dungcu = from dc in data.DUNGCUs
                         where dc.Madungcu == id
                         select dc;
            return View(dungcu.Single());

        }
        public ActionResult Thongtin()
        {
            return View();
        }
        public ActionResult Thanhtoan()
        {
            return View();
        }
        public ActionResult Diachi()
        {
            return View();
        }
    }
}