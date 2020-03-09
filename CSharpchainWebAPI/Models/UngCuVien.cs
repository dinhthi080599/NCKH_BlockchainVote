using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CSharpchainWebAPI.Models
{
    public class UngCuVien
    {
        // string ma_ungcuvien { get; set; }
        public string sHoTen { get; set; }
        public Boolean bGioiTinh { get; set; }
        public string dNgaySinh { get; set; }
        // string email { get; set; }
        public int ma_dotbaucu { get; set; }
        // string sDiachi { get; set; }
        public string themUngCuVien(List<UngCuVien> List_UCV)
        {
            try
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    List_UCV.ForEach(
                        n => {
                            tbl_ungcuvien tbl_ucv = new tbl_ungcuvien()
                            {
                                ma_dotbaucu = n.ma_dotbaucu,
                                sHoten = n.sHoTen,
                                bGioitinh = n.bGioiTinh,
                                dNgaysinh = DateTime.Parse(n.dNgaySinh)
                            };
                            db.tbl_ungcuvien.Add(tbl_ucv);
                        }
                    );
                    db.SaveChanges();
                    return "them_ungcuvien_thanhcong";
                }
            }
            catch (InvalidCastException e)
            {
                return "them_thatbai";
            }
        }
    }
}