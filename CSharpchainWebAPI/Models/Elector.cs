using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CSharpchainWebAPI.Models
{
    public partial class Elector
    {
        public long ma_ungcuvien { get; set; }
        public string sHoten { get; set; }
        public string bGioitinh { get; set; }
        public string dNgaysinh { get; set; }
        public string sEmail { get; set; }
        public string sDiachi { get; set; }
        public string sGhichu { get; set; }
        public long ma_dotbaucu { get; set; }

        public List<Elector> getElector()
        {
            List<Elector> elecList = null;
            using (var db = new admin_voteEntities())
            {
                elecList = db.tbl_ungcuvien
                            .Select(s => new Elector()
                            {
                                ma_dotbaucu = (long)s.ma_dotbaucu,
                                sHoten = s.sHoten,
                                bGioitinh = s.bGioitinh == true ? "Nam" : "Nữ",
                                dNgaysinh = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dNgaysinh).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dNgaysinh).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dNgaysinh),
                                sEmail = s.sEmail,
                                sDiachi = s.sDiachi,
                                sGhichu = s.sGhichu,
                                ma_ungcuvien = s.ma_ungcuvien
                            }).ToList<Elector>();
            }
            return elecList;
        }
        public List<Elector> getElectorbyId(int i)
        {
            List<Elector> ElectorList = null;
            using (var db = new admin_voteEntities())
            {
                ElectorList = db.tbl_ungcuvien
                            .Select(s => new Elector()
                            {
                                ma_dotbaucu = (long)s.ma_dotbaucu,
                                sHoten = s.sHoten,
                                bGioitinh = s.bGioitinh == true ? "Nam" : "Nữ",
                                dNgaysinh = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dNgaysinh).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dNgaysinh).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dNgaysinh),
                                sEmail = s.sEmail,
                                sDiachi = s.sDiachi,
                                sGhichu = s.sGhichu,
                                ma_ungcuvien = s.ma_ungcuvien
                            }).Where(s => s.ma_dotbaucu == i).ToList<Elector>();
            }
            return ElectorList;
        }

        public Elector getElectorbyIdE(int i)
        {
            Elector Elector = null;
            using (var db = new admin_voteEntities())
            {
                Elector = db.tbl_ungcuvien
                            .Select(s => new Elector()
                            {
                                ma_dotbaucu = (long)s.ma_dotbaucu,
                                sHoten = s.sHoten,
                                bGioitinh = s.bGioitinh == true ? "Nam" : "Nữ",
                                dNgaysinh = SqlFunctions.Replicate("0", 2 - SqlFunctions.DateName("dd", s.dNgaysinh).Trim().Length)
                                            + SqlFunctions.DateName("dd", s.dNgaysinh).Trim()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.Replicate("0", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart()
                                            + SqlFunctions.Replicate("/", 2 - SqlFunctions.StringConvert((double)s.dNgaysinh.Value.Month).TrimStart().Length)
                                            + SqlFunctions.DateName("year", s.dNgaysinh),
                                sEmail = s.sEmail,
                                sDiachi = s.sDiachi,
                                sGhichu = s.sGhichu,
                                ma_ungcuvien = s.ma_ungcuvien
                            }).Where(s => s.ma_ungcuvien == i).FirstOrDefault<Elector>();
            }
            return Elector;
        }

        public bool createNewElector(string sHoten, string bGioitinh, string dNgaysinh, string sEmail, string sDiachi, string ma_dotbaucu, string sGhichu)
        {
            try
            {
                using (admin_voteEntities db = new admin_voteEntities())
                {
                    var electorDB = db.Set<tbl_ungcuvien>();
                    electorDB.Add(new tbl_ungcuvien
                    {
                        sHoten = sHoten,
                        dNgaysinh = DateTime.ParseExact(dNgaysinh, "dd/mm/yyyy", CultureInfo.InvariantCulture),
                        bGioitinh = bGioitinh == "Nam" ? true : false,
                        sEmail = sEmail,
                        sDiachi = sDiachi,
                        sGhichu = sGhichu,
                        ma_dotbaucu = long.Parse(ma_dotbaucu)
                    });
                    db.SaveChanges();
                }
            }
            catch (InvalidCastException e)
            {
                return false;
            }
            return true;
        }
    }
}