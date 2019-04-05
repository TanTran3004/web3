using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVCDungcuhoctap.Models;

namespace MVCDungcuhoctap.Models
{
    public class Giohang
    {
        dbQLDCHTDataContext data = new dbQLDCHTDataContext();

        public int iMaDungCu { get; set; }
        public string sTenDungCu { get; set; }
        public string sAnhbia { get; set; }
        public Double dDongia { get; set; }
        public int iSoluong { get; set; }
        public Double dThanhtien {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int MaDungCu)
        {
            iMaDungCu = MaDungCu;
            DUNGCU dungcu = data.DUNGCUs.Single(n=>n.Madungcu==iMaDungCu);
            sTenDungCu = dungcu.Tendungcu;
            sAnhbia = dungcu.Anhbia;
            dDongia = double.Parse(dungcu.Giaban.ToString());
            iSoluong = 1;
        }

    }
}